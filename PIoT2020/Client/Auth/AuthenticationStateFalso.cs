using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PIoT2020.Client.Auth
{
    public class AuthenticationStateFalso : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        public override async Task<Microsoft.AspNetCore.Components.Authorization.AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity(new List<Claim> {new Claim(ClaimTypes.Name,"Mario") },"prueba");
          await  Task.Delay(3000);
            return await Task.FromResult(new Microsoft.AspNetCore.Components.Authorization.AuthenticationState(new ClaimsPrincipal(identity)));
        }
    }
}
