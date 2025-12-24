global using microbloom.Data;
global using microbloom.DTOs;
global using microbloom.Entities;
using microbloom.Services;
using microbloom.Services.Implementations;
using microbloom.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

// UTF-8 Encoding
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// ============================================================
// MICROBLOOM - PROJE YAPILANDIRMASI (Entry Point)
// ============================================================
// Bu dosya, uygulamanın Dependency Injection (DI) konteynerini,
// veritabanı bağlantılarını, kimlik doğrulama (Identity) ve 
// HTTP istek hattını (Pipeline) yapılandırır.
// ============================================================

var builder = WebApplication.CreateBuilder(args);

// 1. VERİTABANI BAĞLANTISI (EF Core)
// SQL Server kullanılarak veritabanı bağlamı (Context) havuza eklenir.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<KariyerDBContext>(options =>
    options.UseSqlServer(connectionString));

// 2. KİMLİK DOĞRULAMA (Identity)
// Kullanıcı yönetimi, rol tabanlı yetkilendirme ve parola kuralları burada belirlenir.
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Sunum Notu: Geliştirme kolaylığı için parola kuralları esnek tutulmuştur.
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<KariyerDBContext>()
.AddDefaultTokenProviders()
.AddClaimsPrincipalFactory<microbloom.Services.Factory.AppUserClaimsPrincipalFactory>();

// Identity Cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";
    options.AccessDeniedPath = "/access-denied";
    options.Cookie.IsEssential = true;
});

// HttpContextAccessor
// 3. BAĞIMLILIK ENJEKSİYONU (Dependency Injection - Services)
// Katmanlı mimarinin servisleri (Business Logic) burada Scoped ömrüyle kaydedilir.
// Bu sayede her HTTP isteği için yeni bir servis örneği oluşturulur.
builder.Services.AddScoped<IAuthService, ServerAuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ICvSampleService, CvSampleService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IMessageService, MessageService>();

// HttpContext ve Auth State (Blazor için kritik)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddCascadingAuthenticationState();

// HttpClient with cookie forwarding for API calls
builder.Services.AddScoped(sp => 
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var httpContext = httpContextAccessor.HttpContext;
    string baseAddress = "https://localhost:5001";

    var handler = new HttpClientHandler();
    handler.UseCookies = true;
    handler.CookieContainer = new System.Net.CookieContainer();

    if (httpContext != null)
    {
        try
        {
            baseAddress = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
            
            // Forward authentication cookie to API
            var cookies = httpContext.Request.Cookies;
            foreach (var cookie in cookies)
            {
                handler.CookieContainer.Add(
                    new Uri(baseAddress), 
                    new System.Net.Cookie(cookie.Key, cookie.Value));
            }
        }
        catch { }
    }

    return new HttpClient(handler) { BaseAddress = new Uri(baseAddress) };
});



// Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
    options.DisconnectedCircuitMaxRetained = 100;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
});

// Controllers & Swagger
builder.Services.AddControllers();

// Anti-forgery'yi devre dışı bırak (basit form POST için)
builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRolesAsync(roleManager);
        await DbSeeder.SeedRolesAndAdminAsync(services);
        await DbSeeder.SeedRandomDataAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
     logger.LogError(ex, "Database seeding error");
    }
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
 app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

// Helper Methods
static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "JobSeeker", "Employer", "Admin" };
    foreach (var roleName in roleNames)
    {
    if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
}
}