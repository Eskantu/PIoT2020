using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIoT2020.COMMON.Entidades;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.DAL;
using PIoT2020.Shared.Modelos;
using Microsoft.AspNetCore.Blazor.Http;
using System.Net;
using Microsoft.AspNetCore.Components;
using PIoT2020.Shared.Tools;

namespace PIoT2020.Server.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class UsuarioController : GenericController<Usuario>
    {
        private static UserInfo LoggedOutUser;
        private string baseAddres;
        public UsuarioController() : base(new GenericRepository<Usuario>())
        {
            LoggedOutUser = new UserInfo { IsAuthenticated = false, Name = "No autenticado" };
            
        }

        [HttpGet("Logger")]
        public UserInfo GetUser()
        {
            
            if (User.Identity.IsAuthenticated)
            {

                return new UserInfo
                {
                    Name = User.Identity.Name,
                    IsAuthenticated = true, Role=User.Claims.Where(claim=>claim.Type==ClaimTypes.Role).SingleOrDefault().Value, IdUsuario= User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).SingleOrDefault().Value
                };
            }
            else
            {
                return LoggedOutUser;
            }
        }

        [HttpGet("SignIn")]
        public async Task<RedirectResult> SignIn(string username, string passowrd)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passowrd))
            {
                return Redirect("");
            }
            Usuario usuario = _genericRepository.Query(user => user.Password == passowrd && user.UsuarioName == username).SingleOrDefault();
            if (usuario != null)
            {
                Tools.usuario = usuario;
                //            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                //Request.ApplicationPath.TrimEnd('/') + "/";
                HttpClient httpClient = new HttpClient();
                baseAddres = HttpContext.Request.Scheme +"://"+ HttpContext.Request.Host + "/";
                httpClient.BaseAddress =new Uri(baseAddres);
                var TipoUsuario = await httpClient.GetJsonAsync<TipoUsuario>($"TipoUsuario/{usuario.TipoUsuario}");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,usuario.UsuarioName),
                    new Claim(ClaimTypes.Role, TipoUsuario.Name),
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id)
                };

                var authProperties = new AuthenticationProperties { IsPersistent = false, IssuedUtc=DateTimeOffset.UtcNow, ExpiresUtc=DateTime.UtcNow.AddMinutes(30) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                if (TipoUsuario.Name == "Administrador")
                {
                    return Redirect("/DashboardAdministrador");
                }
                if (TipoUsuario.Name == "General")
                {
                    return Redirect("/DashboardGeneral");
                }
            }
            return Redirect("/");
        }

        [HttpGet("signout")]
        public async Task<RedirectResult> SignOut()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}