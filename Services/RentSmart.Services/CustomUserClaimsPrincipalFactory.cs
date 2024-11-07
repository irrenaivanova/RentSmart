namespace RentSmart.Services
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using RentSmart.Data.Models;

    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
            if (user.Owner != null)
            {
                identity.AddClaim(new Claim("IsOwner", "true"));
            }

            if (user.Manager != null)
            {
                identity.AddClaim(new Claim("IsManager", "true"));
            }

            if (user.Renter != null)
            {
                identity.AddClaim(new Claim("IsRenter", "true"));
            }

            return principal;
        }
    }
}
