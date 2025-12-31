using Microsoft.AspNetCore.Identity;
using microbloom.DTOs;
using microbloom.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace microbloom.Services.Implementations
{
    public class ServerAuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<AppUser> _userManager;

        public ServerAuthService(HttpClient httpClient, UserManager<AppUser> userManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var user = new AppUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new AuthResult { Successful = false, Error = errors };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "JobSeeker");
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    return new AuthResult { Successful = false, Error = $"Rol atanirken hata: {errors}" };
                }

                return new AuthResult { Successful = true };
            }
            catch (Exception ex)
            {
                return new AuthResult { Successful = false, Error = $"Kayit hatasi: {ex.Message}" };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                Console.WriteLine($"ServerAuthService.LoginAsync called for: {loginDto.Email}");
                Console.WriteLine($"HttpClient BaseAddress: {_httpClient.BaseAddress}");

                var response = await _httpClient.PostAsJsonAsync("api/account/login", loginDto);

                Console.WriteLine($"API Response Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error Response: {error}");
                    return new AuthResult
                    {
                        Successful = false,
                        Error = error.Contains("Giris bilgileri") ? error : "E-posta veya sifre hatali."
                    };
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Success Response: {responseContent}");

                return new AuthResult { Successful = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ServerAuthService.LoginAsync: {ex}");
                return new AuthResult
                {
                    Successful = false,
                    Error = $"Giris hatasi: {ex.Message}"
                };
            }
        }

        public async Task LogoutAsync()
        {
            try
            {
                await _httpClient.PostAsync("api/account/logout", null);
            }
            catch (Exception)
            {
            }
        }
    }
}