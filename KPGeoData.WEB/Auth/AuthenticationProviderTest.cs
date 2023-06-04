using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KPGeoData.WEB.Auth
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //await Task.Delay(3000);
            var anonimous = new ClaimsIdentity();
            var zuluUser = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("FirstName", "Luis"),
                    new Claim("LastName", "Núñez"),
                    new Claim(ClaimTypes.Name, "luis@yopmail.com"),
                                        new Claim(ClaimTypes.Role, "Admin")
                },
                                authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(zuluUser)));
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimous)));
        }
    }
}
