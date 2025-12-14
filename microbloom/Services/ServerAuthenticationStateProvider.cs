using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using microbloom.Entities;

namespace microbloom.Services
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ServerAuthenticationStateProvider> _logger;

        public ServerAuthenticationStateProvider(
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<ServerAuthenticationStateProvider> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            return Task.FromResult(new AuthenticationState(user));
        }

        public async Task SignOutAsync()
        {
             NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}
