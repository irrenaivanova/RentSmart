namespace RentSmart.Web.Infrastructure.Factories;

using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using RentSmart.Data;
using RentSmart.Data.Models;
using static RentSmart.Common.GlobalConstants;

public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    private readonly ApplicationDbContext db;

    public CustomUserClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor,
        ApplicationDbContext db)
        : base(userManager, optionsAccessor)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
    {
        var principal = await base.CreateAsync(user);
        var identity = (ClaimsIdentity)principal.Identity;
        await this.db.Entry(user).Reference(u => u.Owner).LoadAsync();
        await this.db.Entry(user).Reference(u => u.Manager).LoadAsync();
        await this.db.Entry(user).Reference(u => u.Renter).LoadAsync();

        if (user.Owner != null)
        {
            identity.AddClaim(new Claim(OwnerClaim, "true"));
        }

        if (user.Manager != null)
        {
            identity.AddClaim(new Claim(ManagerClaim, "true"));
        }

        if (user.Renter != null)
        {
            identity.AddClaim(new Claim(RenterClaim, "true"));
        }

        var roles = await this.UserManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return principal;
    }
}
