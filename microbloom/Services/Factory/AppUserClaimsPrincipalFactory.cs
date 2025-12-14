using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using microbloom.Entities;

namespace microbloom.Services.Factory
{
    // KRITIK: UserClaimsPrincipalFactory<AppUser, IdentityRole> kullanılmalı ki roller claim'lere eklensin!
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, IdentityRole>
    {
        public AppUserClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            if (!string.IsNullOrEmpty(user.FirstName))
            {
                identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
            }

            if (user.CompanyId.HasValue)
            {
                identity.AddClaim(new Claim("CompanyId", user.CompanyId.Value.ToString()));
                
                // Note: We might need to load Company navigation property if not loaded.
                // Assuming it might NOT be loaded, we check if it is null.
                if (user.Company != null)
                {
                     identity.AddClaim(new Claim("CompanyName", user.Company.Name ?? ""));
                }
            }

            return identity;
        }
    }
}
