using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims; 

namespace ActiveDirectoryTokenBased
{
    public class ADAuthorizeServiceProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return base.ValidateClientAuthentication(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { " " });

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "ICG"))
            {
                bool IsValid = pc.ValidateCredentials(context.UserName, context.Password);
                if (!IsValid)
                {
                    context.SetError("invalid_grant", "Incorrect userid/password");
                    return base.GrantResourceOwnerCredentials(context); 
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            context.Validated(identity);
            return base.GrantResourceOwnerCredentials(context);
        }





    }
}