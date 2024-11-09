using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Orquesta.WEB.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            await Task.Delay(3000);
            var anonimous = new ClaimsIdentity();
            var oapUser = new ClaimsIdentity(new List<Claim>
            {
                new Claim("FirstName", "Super"),
                new Claim("LastName","Admin"),
                new Claim(ClaimTypes.Name,"kevin@gmail.com"),
                new Claim(ClaimTypes.Role, "Admin")
            },
                authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(oapUser)));
        }
    }
}
