using Microsoft.AspNetCore.Components.Authorization;
using PIoT2020.COMMON.Interfaces;
using PIoT2020.Shared.Modelos;
using PIoT2020.Shared.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using PIoT2020.Shared;

namespace PIoT2020.Client.Auth
{
    public class ServicioDeAutenticacion : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        private readonly HttpClient HttpClient;
        public ServicioDeAutenticacion(HttpClient HttpClient)
        {
            this.HttpClient = HttpClient;
        }
        public override async Task<AuthenticationState>
            GetAuthenticationStateAsync()
        {
            var UserInfo = await HttpClient.GetJsonAsync<UserInfo>("Usuario/Logger");
            ClaimsIdentity Identity;
            if (UserInfo.IsAuthenticated)
            {
                Identity = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, UserInfo.Name),
                    }, "serverauth");
            }
            else
            {
                Identity = new ClaimsIdentity();
            }


            return new AuthenticationState(new ClaimsPrincipal(Identity));
        }
    }
}
