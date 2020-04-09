using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using PIoT2020.Client.Auth;

namespace PIoT2020.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, ServicioDeAutenticacion>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }


        public void Configure(IComponentsApplicationBuilder app)
        {
            
            
            app.AddComponent<App>("app");
        }
    }
}
