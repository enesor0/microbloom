using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace microbloom.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CvSamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileDownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThumbnailImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CvSamples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Universities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsStateUniversity = table.Column<bool>(type: "bit", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Universities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CvUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "JobPostings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPostings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPostings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentArticles_ContentCategories_ContentCategoryId",
                        column: x => x.ContentCategoryId,
                        principalTable: "ContentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoreType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastYearBaseScore = table.Column<double>(type: "float", nullable: false),
                    LastYearBaseRanking = table.Column<int>(type: "int", nullable: false),
                    UniversityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Universities_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "Universities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPostingId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplications_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobApplications_JobPostings_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPostings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Description", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { 1, "Global teknoloji lideri.", "google.png", "Google" },
                    { 2, "Yazılım ve bulut çözümleri.", "microsoft.png", "Microsoft" }
                });

            migrationBuilder.InsertData(
                table: "ContentCategories",
                columns: new[] { "Id", "Slug", "Title" },
                values: new object[,]
                {
                    { 1, "universiteye-hazirlik", "Üniversiteye Hazırlık" },
                    { 2, "profesyonel-hayat", "İlk İşim ve Profesyonel Hayat" }
                });

            migrationBuilder.InsertData(
                table: "Universities",
                columns: new[] { "Id", "City", "IsStateUniversity", "LogoUrl", "Name", "WebSite" },
                values: new object[,]
                {
                    { 1, "İstanbul", true, null, "İstanbul Üniversitesi", "https://istanbul.edu.tr" },
                    { 2, "İstanbul", true, null, "İstanbul Teknik Üniversitesi", "https://itu.edu.tr" },
                    { 3, "İstanbul", true, null, "Boğaziçi Üniversitesi", "https://boun.edu.tr" },
                    { 4, "İstanbul", true, null, "Marmara Üniversitesi", "https://marmara.edu.tr" },
                    { 5, "İstanbul", true, null, "Yıldız Teknik Üniversitesi", "https://yildiz.edu.tr" },
                    { 6, "İstanbul", true, null, "Galatasaray Üniversitesi", "https://gsu.edu.tr" },
                    { 7, "Ankara", true, null, "Ankara Üniversitesi", "https://ankara.edu.tr" },
                    { 8, "Ankara", true, null, "Orta Doğu Teknik Üniversitesi", "https://metu.edu.tr" },
                    { 9, "Ankara", true, null, "Hacettepe Üniversitesi", "https://hacettepe.edu.tr" },
                    { 10, "Ankara", true, null, "Gazi Üniversitesi", "https://gazi.edu.tr" },
                    { 11, "Ankara", true, null, "Ankara Yıldırım Beyazıt Üniversitesi", "https://ybu.edu.tr" },
                    { 12, "İzmir", true, null, "Ege Üniversitesi", "https://ege.edu.tr" },
                    { 13, "İzmir", true, null, "Dokuz Eylül Üniversitesi", "https://deu.edu.tr" },
                    { 14, "İzmir", true, null, "İzmir Yüksek Teknoloji Enstitüsü", "https://iyte.edu.tr" },
                    { 15, "İzmir", true, null, "İzmir Katip Çelebi Üniversitesi", "https://ikc.edu.tr" },
                    { 16, "Kayseri", true, null, "Erciyes Üniversitesi", "https://erciyes.edu.tr" },
                    { 17, "Konya", true, null, "Selçuk Üniversitesi", "https://selcuk.edu.tr" },
                    { 18, "Erzurum", true, null, "Atatürk Üniversitesi", "https://atauni.edu.tr" },
                    { 19, "Adana", true, null, "Çukurova Üniversitesi", "https://cu.edu.tr" },
                    { 20, "Antalya", true, null, "Akdeniz Üniversitesi", "https://akdeniz.edu.tr" },
                    { 21, "Samsun", true, null, "Ondokuz Mayıs Üniversitesi", "https://omu.edu.tr" },
                    { 22, "Trabzon", true, null, "Karadeniz Teknik Üniversitesi", "https://ktu.edu.tr" },
                    { 23, "Bursa", true, null, "Uludağ Üniversitesi", "https://uludag.edu.tr" },
                    { 24, "Eskişehir", true, null, "Anadolu Üniversitesi", "https://anadolu.edu.tr" },
                    { 25, "Denizli", true, null, "Pamukkale Üniversitesi", "https://pau.edu.tr" },
                    { 26, "Elazığ", true, null, "Fırat Üniversitesi", "https://firat.edu.tr" },
                    { 27, "Isparta", true, null, "Süleyman Demirel Üniversitesi", "https://sdu.edu.tr" },
                    { 28, "Gaziantep", true, null, "Gaziantep Üniversitesi", "https://gantep.edu.tr" },
                    { 29, "Sakarya", true, null, "Sakarya Üniversitesi", "https://sakarya.edu.tr" },
                    { 30, "Kocaeli", true, null, "Kocaeli Üniversitesi", "https://kocaeli.edu.tr" },
                    { 31, "İstanbul", false, null, "Koç Üniversitesi", "https://ku.edu.tr" },
                    { 32, "İstanbul", false, null, "Sabancı Üniversitesi", "https://sabanciuniv.edu" },
                    { 33, "İstanbul", false, null, "Bahçeşehir Üniversitesi", "https://bahcesehir.edu.tr" },
                    { 34, "İstanbul", false, null, "İstanbul Bilgi Üniversitesi", "https://bilgi.edu.tr" },
                    { 35, "İstanbul", false, null, "Özyeğin Üniversitesi", "https://ozyegin.edu.tr" },
                    { 36, "İstanbul", false, null, "Kadir Has Üniversitesi", "https://khas.edu.tr" },
                    { 37, "İstanbul", false, null, "Yeditepe Üniversitesi", "https://yeditepe.edu.tr" },
                    { 38, "İstanbul", false, null, "İstanbul Ticaret Üniversitesi", "https://ticaret.edu.tr" },
                    { 39, "İstanbul", false, null, "İstanbul Kültür Üniversitesi", "https://iku.edu.tr" },
                    { 40, "İstanbul", false, null, "Işık Üniversitesi", "https://isikun.edu.tr" },
                    { 41, "Ankara", false, null, "Bilkent Üniversitesi", "https://bilkent.edu.tr" },
                    { 42, "Ankara", false, null, "Atılım Üniversitesi", "https://atilim.edu.tr" },
                    { 43, "Ankara", false, null, "Başkent Üniversitesi", "https://baskent.edu.tr" },
                    { 44, "Ankara", false, null, "Çankaya Üniversitesi", "https://cankaya.edu.tr" },
                    { 45, "Ankara", false, null, "TOBB Ekonomi ve Teknoloji Üniversitesi", "https://etu.edu.tr" },
                    { 46, "İzmir", false, null, "İzmir Ekonomi Üniversitesi", "https://ieu.edu.tr" },
                    { 47, "İzmir", false, null, "Yaşar Üniversitesi", "https://yasar.edu.tr" },
                    { 48, "İstanbul", false, null, "Özyeğin Üniversitesi", "https://ozyegin.edu.tr" },
                    { 49, "Ankara", false, null, "TED Üniversitesi", "https://tedu.edu.tr" },
                    { 50, "İstanbul", false, null, "MEF Üniversitesi", "https://mef.edu.tr" }
                });

            migrationBuilder.InsertData(
                table: "ContentArticles",
                columns: new[] { "Id", "Content", "ContentCategoryId", "Slug", "Summary", "Title" },
                values: new object[,]
                {
                    { 1, "# &#x1F393; Üniversite Seçimi Rehberi\r\n\r\nÜniversite seçimi, hayatınızın en önemli kararlarından biridir. İşte doğru tercih yapmanız için ipuçları:\r\n\r\n## &#x1F4D6; 1. Bölüm Seçimi\r\n- &#x2714; İlgi alanlarınızı ve yeteneklerinizi değerlendirin\r\n- &#x2714; Bölümün kariyer olanaklarını araştırın\r\n- &#x2714; Sektördeki iş imkanlarını inceleyin\r\n\r\n## &#x1F3EB; 2. Üniversite Kriterleri\r\n- &#x1F4DA; Akademik kadro kalitesi\r\n- &#x1F3C6; Kampüs imkanları ve sosyal aktiviteler\r\n- &#x1F30D; Uluslararası değişim programları\r\n- &#x1F4BC; Mezun memnuniyeti ve kariyer desteği\r\n\r\n## &#x1F3D9; 3. Şehir Seçimi\r\n- &#x1F4B5; Yaşam maliyeti\r\n- &#x1F3AD; Kültürel ve sosyal olanaklar\r\n- &#x1F3E2; İş bulma imkanları\r\n- &#x1F3E0; Ailenize uzaklık\r\n\r\n## &#x1F4CA; 4. Taban Puan ve Sıralama\r\n- &#x2705; Gerçekçi hedefler belirleyin\r\n- &#x1F4DD; Yedek tercihlerinizi mutlaka doldurun\r\n- &#x1F4C8; Önceki yılların yerleştirme puanlarını inceleyin", 1, "universite-secimi-rehberi", "Üniversite seçerken nelere dikkat etmelisiniz? Şehir, bölüm, taban puan ve kariyer hedeflerinize göre doğru tercih nasıl yapılır?", "Üniversite Seçimi Rehberi" },
                    { 2, "# &#x1F4BB; Mühendislik Bölümleri Rehberi\r\n\r\n## &#x1F5A5; Bilgisayar Mühendisliği\r\nYazılım geliştirme, veri bilimi, yapay zeka gibi alanlarda çalışma fırsatı sunar.\r\n\r\n**Kariyer Alanları:**\r\n- &#x1F4BB; Yazılım Geliştirici\r\n- &#x1F4CA; Veri Analisti / Data Scientist\r\n- &#x1F3D7; Sistem Mimarı\r\n- &#x2699; DevOps Mühendisi\r\n\r\n**Başlangıç Maaşları:** &#x1F4B0; 25.000 - 40.000 TL\r\n\r\n## &#x26A1; Elektrik-Elektronik Mühendisliği\r\nElektronik sistemler, güç sistemleri, telekomünikasyon alanlarında uzmanlaşma.\r\n\r\n**Kariyer Alanları:**\r\n- &#x1F50C; Elektronik Tasarım Mühendisi\r\n- &#x1F4A1; Enerji Sistemleri Uzmanı\r\n- &#x1F4E1; Telekomünikasyon Mühendisi\r\n\r\n## &#x2699; Endüstri Mühendisliği\r\nÜretim süreçlerinin optimizasyonu, lojistik ve proje yönetimi.\r\n\r\n**Kariyer Alanları:**\r\n- &#x1F4C8; Proje Yöneticisi\r\n- &#x1F69A; Lojistik Uzmanı\r\n- &#x1F504; Süreç Geliştirme Mühendisi", 1, "bolum-rehberi-muhendislik", "Mühendislik bölümleri hakkında her şey: Hangi bölüm size uygun? Kariyer olanakları neler? Mezuniyet sonrası ne yapabilirsiniz?", "Bölüm Rehberi: Mühendislik" },
                    { 3, "# &#x1F4B0; Burs ve Mali Destek Rehberi\r\n\r\n## &#x1F3DB; Devlet Bursu\r\n- &#x1F393; YÖK bursu\r\n- &#x1F3E0; Kredi Yurtlar Kurumu (KYK)\r\n- &#x1F3C6; Başarı bursu\r\n\r\n## &#x1F3E2; Özel Kuruluş Bursları\r\n- &#x1F4DA; Vakıf üniversiteleri tam burs programları\r\n- &#x1F4BC; Özel sektör şirket bursları (TÜBİTAK, TÜSİAD)\r\n- &#x1F3DB; Belediye bursları\r\n\r\n## &#x1F4DD; Başvuru İpuçları\r\n1. &#x1F4C5; Başvuru tarihlerini takip edin\r\n2. &#x1F4C4; Gereken belgeleri önceden hazırlayın\r\n3. &#x270D; Motivasyon mektubunuza özen gösterin\r\n4. &#x1F4E8; Birden fazla burs programına başvurun\r\n\r\n## &#x1F517; Önemli Linkler\r\n- turkiye.gov.tr/kyk-ogrenci-kredisi\r\n- yok.gov.tr", 1, "burs-mali-destek", "Üniversite eğitiminiz için burs ve mali destek alma yolları. Hangi kuruluşlar burs veriyor? Başvuru şartları neler?", "Burs ve Mali Destek İmkanları" },
                    { 4, "# &#x1F4C4; CV Hazırlama Rehberi\r\n\r\n## &#x2705; CV'de Olması Gerekenler\r\n\r\n### 1. &#x1F464; Kişisel Bilgiler\r\n- &#x1F4DD; Ad Soyad\r\n- &#x1F4DE; İletişim Bilgileri (Telefon, E-posta)\r\n- &#x1F517; LinkedIn Profili\r\n- &#x1F4BB; GitHub (yazılımcılar için)\r\n\r\n### 2. &#x1F4AC; Özet\r\n2-3 cümlelik kısa bir özet ile kendinizi tanıtın.\r\n\r\n**Örnek:** \"Bilgisayar Mühendisliği mezunu, 2 yıllık web geliştirme deneyimi. React ve Node.js teknolojilerinde uzman.\"\r\n\r\n### 3. &#x1F393; Eğitim\r\n- &#x1F3DB; Üniversite adı ve bölüm\r\n- &#x1F4C5; Mezuniyet tarihi\r\n- &#x1F4CA; Not ortalaması (3.00 üzerindeyse)\r\n\r\n### 4. &#x1F4BC; İş Deneyimi\r\n- &#x1F3E2; Şirket adı ve pozisyon\r\n- &#x1F4C6; Çalışma tarihleri\r\n- &#x2705; Görev ve başarılarınız\r\n- &#x1F6E0; Kullandığınız teknolojiler\r\n\r\n### 5. &#x1F680; Projeler\r\n- &#x1F4BB; Kişisel veya okul projeleri\r\n- &#x1F310; Açık kaynak katkılarınız\r\n\r\n### 6. &#x2B50; Beceriler\r\n- &#x1F4DA; Programlama dilleri\r\n- &#x1F527; Araçlar ve teknolojiler\r\n- &#x1F30D; Yabancı dil seviyeleri\r\n\r\n## &#x1F4A1; CV Hazırlama İpuçları\r\n- &#x1F4C4; Maksimum 2 sayfa olmalı\r\n- &#x1F3AF; Özgeçmişinizi her pozisyon için özelleştirin\r\n- &#x1F4CA; Somut başarılarınızı sayılarla destekleyin\r\n- &#x2705; Yazım hatalarından kaçının", 2, "cv-hazirlama-rehberi", "Profesyonel bir CV nasıl hazırlanır? İşverenin dikkatini çekecek CV örnekleri ve ipuçları.", "CV Hazırlama Rehberi" },
                    { 5, "# &#x1F3AF; İş Görüşmesine Hazırlık\r\n\r\n## &#x1F4C5; Görüşme Öncesi\r\n1. &#x1F50D; Şirket hakkında araştırma yapın\r\n2. &#x1F4DD; Pozisyon tanımını detaylı inceleyin\r\n3. &#x1F4AC; Kendinizi tanıtma pratiği yapın\r\n4. &#x1F454; Şık ve profesyonel giyinin\r\n\r\n## &#x2753; Sık Sorulan Sorular\r\n\r\n### \"Kendinizden bahseder misiniz?\"\r\n- &#x23F1; Kısa ve öz olun\r\n- &#x1F393; Eğitim ve deneyimlerinize değinin\r\n- &#x1F3AF; Neden bu pozisyona uygun olduğunuzu vurgulayın\r\n\r\n### \"Güçlü yönleriniz neler?\"\r\n- &#x1F4AA; Pozisyonla ilgili güçlü yönlerinizi seçin\r\n- &#x1F4A1; Somut örneklerle destekleyin\r\n\r\n### \"Zayıf yönleriniz?\"\r\n- &#x1F91D; Dürüst olun ama kendinizi kötülemeyin\r\n- &#x1F4C8; Nasıl geliştirmeye çalıştığınızı anlatın\r\n\r\n### \"5 yıl sonra kendinizi nerede görüyorsunuz?\"\r\n- &#x1F680; Kariyer hedeflerinizden bahsedin\r\n- &#x1F3E2; Şirketle birlikte büyümek istediğinizi belirtin\r\n\r\n## &#x1F4E7; Görüşme Sonrası\r\n- &#x1F64F; Teşekkür e-postası gönderin\r\n- &#x23F0; Geri dönüş süresini sorun\r\n- &#x1F9D8; Sabırlı olun", 2, "is-gorusmesine-hazirlik", "İş görüşmesinde başarılı olmanın püf noktaları. Sık sorulan sorular ve nasıl cevaplanır?", "İş Görüşmesine Hazırlık" },
                    { 6, "# &#x1F680; Staj ve İş Bulma Stratejileri\r\n\r\n## &#x1F310; İş Arama Platformları\r\n1. **LinkedIn** - &#x1F465; Profesyonel networking\r\n2. **Kariyer.net** - &#x1F4BC; İş ilanları\r\n3. **SecretCV** - &#x1F575; Anonim başvuru\r\n4. **GitHub Jobs** - &#x1F4BB; Yazılım pozisyonları\r\n5. **AngelList** - &#x1F680; Startup'lar\r\n\r\n## &#x1F91D; Networking İpuçları\r\n- &#x1F4C8; LinkedIn profilinizi güncel tutun\r\n- &#x1F3AA; Sektör etkinliklerine katılın\r\n- &#x1F393; Üniversite mezunları ağınızı kullanın\r\n- &#x1F468;&#x1F3EB; Mentorluk programlarına başvurun\r\n\r\n## &#x1F4BC; Staj Başvurusu\r\n- **Ne zaman başvurmalı?** \r\n  &#x2600; Yazın staj için 3-4 ay önce başlayın\r\n  \r\n- **Başvuru mektubu yazın**\r\n  &#x270D; Neden o şirkette çalışmak istediğinizi açıklayın\r\n\r\n- **Portföy hazırlayın**\r\n  &#x1F4BB; GitHub projeleri, kişisel web sitesi\r\n\r\n## &#x1F3AF; İlk İş İçin İpuçları\r\n- &#x1F4B0; Maaş beklentinizi araştırın\r\n- &#x1F3E2; Şirket kültürüne dikkat edin\r\n- &#x1F4C8; Gelişim fırsatlarını değerlendirin\r\n- &#x23F3; İlk işinizde 1-2 yıl kalın", 2, "staj-is-bulma", "İlk stajınızı veya işinizi nasıl bulursunuz? Hangi platformları kullanmalısınız? Networking nasıl yapılır?", "Staj ve İş Bulma Stratejileri" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "LastYearBaseRanking", "LastYearBaseScore", "Name", "ScoreType", "UniversityId" },
                values: new object[,]
                {
                    { 1, 1850, 520.5, "Hukuk", "EA", 1 },
                    { 2, 850, 545.20000000000005, "Tıp", "SAY", 1 },
                    { 3, 3200, 495.30000000000001, "İşletme", "EA", 1 },
                    { 4, 4500, 485.69999999999999, "Psikoloji", "EA", 1 },
                    { 5, 4200, 488.5, "İktisat", "EA", 1 },
                    { 6, 4650, 482.30000000000001, "Siyaset Bilimi", "EA", 1 },
                    { 7, 5100, 475.80000000000001, "Tarih", "SOZ", 1 },
                    { 8, 5350, 472.5, "Türk Dili ve Edebiyatı", "SOZ", 1 },
                    { 9, 5650, 468.89999999999998, "Felsefe", "SOZ", 1 },
                    { 10, 4850, 478.5, "Sosyoloji", "EA", 1 },
                    { 11, 4450, 485.19999999999999, "Matematik", "SAY", 1 },
                    { 12, 4850, 478.69999999999999, "Fizik", "SAY", 1 },
                    { 13, 5050, 475.5, "Kimya", "SAY", 1 },
                    { 14, 4550, 482.80000000000001, "Biyoloji", "SAY", 1 },
                    { 15, 5350, 472.5, "İstatistik", "SAY", 1 },
                    { 16, 1250, 525.5, "Bilgisayar Mühendisliği", "SAY", 2 },
                    { 17, 1580, 520.29999999999995, "Elektrik-Elektronik Mühendisliği", "SAY", 2 },
                    { 18, 2100, 515.79999999999995, "Makine Mühendisliği", "SAY", 2 },
                    { 19, 2450, 512.39999999999998, "İnşaat Mühendisliği", "SAY", 2 },
                    { 20, 1850, 518.70000000000005, "Endüstri Mühendisliği", "SAY", 2 },
                    { 21, 2850, 505.80000000000001, "Kimya Mühendisliği", "SAY", 2 },
                    { 22, 2650, 508.5, "Makine Mühendisliği", "SAY", 2 },
                    { 23, 3450, 498.69999999999999, "Metalurji ve Malzeme Mühendisliği", "SAY", 2 },
                    { 24, 3650, 495.5, "Gıda Mühendisliği", "SAY", 2 },
                    { 25, 4050, 488.80000000000001, "Tekstil Mühendisliği", "SAY", 2 },
                    { 26, 3850, 492.5, "Çevre Mühendisliği", "SAY", 2 },
                    { 27, 4350, 485.69999999999999, "Jeoloji Mühendisliği", "SAY", 2 },
                    { 28, 4550, 482.5, "Maden Mühendisliği", "SAY", 2 },
                    { 29, 3650, 495.80000000000001, "Petrol ve Doğal Gaz Mühendisliği", "SAY", 2 },
                    { 30, 2450, 512.5, "Uçak Mühendisliği", "SAY", 2 },
                    { 31, 3150, 502.80000000000001, "Gemi İnşaatı ve Gemi Makineleri Mühendisliği", "SAY", 2 },
                    { 32, 4050, 488.5, "Harita Mühendisliği", "SAY", 2 },
                    { 33, 4350, 485.30000000000001, "Jeofizik Mühendisliği", "SAY", 2 },
                    { 34, 2950, 505.69999999999999, "Matematik Mühendisliği", "SAY", 2 },
                    { 35, 2750, 508.80000000000001, "Mimarlık", "SAY", 2 },
                    { 36, 980, 530.20000000000005, "Bilgisayar Mühendisliği", "SAY", 3 },
                    { 37, 1280, 525.79999999999995, "Elektrik-Elektronik Mühendisliği", "SAY", 3 },
                    { 38, 1450, 522.5, "Endüstri Mühendisliği", "SAY", 3 },
                    { 39, 1850, 518.70000000000005, "Makine Mühendisliği", "SAY", 3 },
                    { 40, 2050, 515.5, "İnşaat Mühendisliği", "SAY", 3 },
                    { 41, 3200, 495.5, "İşletme", "EA", 3 },
                    { 42, 3800, 490.80000000000001, "Ekonomi", "EA", 3 },
                    { 43, 3550, 492.5, "Uluslararası İlişkiler", "EA", 3 },
                    { 44, 3950, 488.80000000000001, "Siyaset Bilimi ve Uluslararası İlişkiler", "EA", 3 },
                    { 45, 4500, 485.30000000000001, "Psikoloji", "EA", 3 },
                    { 46, 4850, 478.69999999999999, "Sosyoloji", "EA", 3 },
                    { 47, 5050, 475.5, "Tarih", "SOZ", 3 },
                    { 48, 5350, 472.80000000000001, "Felsefe", "SOZ", 3 },
                    { 49, 2950, 505.80000000000001, "Matematik", "SAY", 3 },
                    { 50, 3150, 502.5, "Fizik", "SAY", 3 },
                    { 51, 3450, 498.69999999999999, "Kimya", "SAY", 3 },
                    { 52, 2750, 508.5, "Moleküler Biyoloji ve Genetik", "SAY", 3 },
                    { 53, 4850, 478.5, "Çeviribilim", "SOZ", 3 },
                    { 54, 2350, 510.5, "Hukuk", "EA", 4 },
                    { 55, 1150, 535.79999999999995, "Tıp", "SAY", 4 },
                    { 56, 1950, 518.70000000000005, "Diş Hekimliği", "SAY", 4 },
                    { 57, 2450, 512.5, "Eczacılık", "SAY", 4 },
                    { 58, 4200, 485.19999999999999, "İşletme", "EA", 4 },
                    { 59, 5100, 475.60000000000002, "İktisat", "EA", 4 },
                    { 60, 4550, 482.80000000000001, "Uluslararası İlişkiler", "EA", 4 },
                    { 61, 4850, 478.5, "Siyaset Bilimi ve Kamu Yönetimi", "EA", 4 },
                    { 62, 5350, 472.5, "İletişim", "EA", 4 },
                    { 63, 5050, 475.80000000000001, "Radyo, Televizyon ve Sinema", "EA", 4 },
                    { 64, 4350, 485.5, "Hemşirelik", "SAY", 4 },
                    { 65, 4050, 488.69999999999999, "Fizyoterapi ve Rehabilitasyon", "SAY", 4 },
                    { 66, 4550, 482.5, "Beslenme ve Diyetetik", "SAY", 4 },
                    { 67, 5750, 468.80000000000001, "Bankacılık ve Finans", "EA", 4 },
                    { 68, 6050, 465.5, "Turizm İşletmeciliği", "EA", 4 },
                    { 69, 5350, 472.80000000000001, "Sosyal Hizmet", "EA", 4 },
                    { 70, 1750, 518.39999999999998, "Bilgisayar Mühendisliği", "SAY", 5 },
                    { 71, 2450, 512.79999999999995, "Elektrik-Elektronik Mühendisliği", "SAY", 5 },
                    { 72, 2850, 505.19999999999999, "İnşaat Mühendisliği", "SAY", 5 },
                    { 73, 2650, 508.69999999999999, "Makine Mühendisliği", "SAY", 5 },
                    { 74, 2550, 510.5, "Endüstri Mühendisliği", "SAY", 5 },
                    { 75, 3450, 498.80000000000001, "Kimya Mühendisliği", "SAY", 5 },
                    { 76, 3850, 492.5, "Gıda Mühendisliği", "SAY", 5 },
                    { 77, 4350, 485.69999999999999, "Harita Mühendisliği", "SAY", 5 },
                    { 78, 4550, 482.5, "Jeodezi ve Fotogrametri Mühendisliği", "SAY", 5 },
                    { 79, 4050, 488.80000000000001, "Çevre Mühendisliği", "SAY", 5 },
                    { 80, 4850, 478.5, "Matematik", "SAY", 5 },
                    { 81, 5050, 475.80000000000001, "Fizik", "SAY", 5 },
                    { 82, 5350, 472.5, "Kimya", "SAY", 5 },
                    { 83, 3450, 498.5, "Mimarlık", "SAY", 5 },
                    { 84, 3850, 492.69999999999999, "Şehir ve Bölge Planlama", "SAY", 5 },
                    { 85, 2150, 515.29999999999995, "Hukuk", "EA", 6 },
                    { 86, 3450, 492.5, "İşletme", "EA", 6 },
                    { 87, 3950, 488.69999999999999, "İktisat", "EA", 6 },
                    { 88, 3900, 488.89999999999998, "Uluslararası İlişkiler", "EA", 6 },
                    { 89, 4250, 485.5, "Siyaset Bilimi", "EA", 6 },
                    { 90, 4750, 478.80000000000001, "Sosyoloji", "EA", 6 },
                    { 91, 4550, 482.5, "Matematik", "SAY", 6 },
                    { 92, 2950, 505.80000000000001, "Bilgisayar ve Bilişim Mühendisliği", "SAY", 6 },
                    { 93, 3150, 502.5, "Endüstri Mühendisliği", "SAY", 6 },
                    { 94, 5050, 475.80000000000001, "İletişim", "EA", 6 },
                    { 95, 5350, 472.5, "Felsefe", "SOZ", 6 },
                    { 96, 5650, 468.89999999999998, "Tarih", "SOZ", 6 },
                    { 97, 1650, 522.79999999999995, "Hukuk", "EA", 7 },
                    { 98, 920, 542.29999999999995, "Tıp", "SAY", 7 },
                    { 99, 1650, 522.5, "Diş Hekimliği", "SAY", 7 },
                    { 100, 3200, 510.19999999999999, "Eczacılık", "SAY", 7 },
                    { 101, 4850, 495.69999999999999, "Veterinerlik", "SAY", 7 },
                    { 102, 2850, 505.80000000000001, "Siyasal Bilgiler", "EA", 7 },
                    { 103, 3950, 488.5, "İletişim", "EA", 7 },
                    { 104, 4550, 482.80000000000001, "Dil ve Tarih-Coğrafya Fakültesi - Tarih", "SOZ", 7 },
                    { 105, 4850, 478.5, "Türk Dili ve Edebiyatı", "SOZ", 7 },
                    { 106, 5050, 475.69999999999999, "Arkeoloji", "SOZ", 7 },
                    { 107, 5350, 472.5, "Coğrafya", "SOZ", 7 },
                    { 108, 5650, 468.80000000000001, "Felsefe", "SOZ", 7 },
                    { 109, 3550, 492.5, "Psikoloji", "EA", 7 },
                    { 110, 4150, 485.80000000000001, "Sosyoloji", "EA", 7 },
                    { 111, 4750, 478.69999999999999, "Antropoloji", "EA", 7 },
                    { 112, 4050, 488.5, "Hemşirelik", "SAY", 7 },
                    { 113, 4550, 482.69999999999999, "Ebelik", "SAY", 7 },
                    { 114, 3850, 492.80000000000001, "Fizik Tedavi ve Rehabilitasyon", "SAY", 7 },
                    { 115, 4350, 485.5, "Beslenme ve Diyetetik", "SAY", 7 },
                    { 116, 5050, 475.80000000000001, "Ziraat Mühendisliği", "SAY", 7 },
                    { 117, 1100, 528.70000000000005, "Bilgisayar Mühendisliği", "SAY", 8 },
                    { 118, 1450, 522.5, "Elektrik-Elektronik Mühendisliği", "SAY", 8 },
                    { 119, 2150, 515.29999999999995, "Makine Mühendisliği", "SAY", 8 },
                    { 120, 1850, 518.20000000000005, "Endüstri Mühendisliği", "SAY", 8 },
                    { 121, 2450, 512.5, "İnşaat Mühendisliği", "SAY", 8 },
                    { 122, 2750, 508.80000000000001, "Kimya Mühendisliği", "SAY", 8 },
                    { 123, 2950, 505.69999999999999, "Metalurji ve Malzeme Mühendisliği", "SAY", 8 },
                    { 124, 3450, 498.5, "Maden Mühendisliği", "SAY", 8 },
                    { 125, 3650, 495.80000000000001, "Jeoloji Mühendisliği", "SAY", 8 },
                    { 126, 2950, 505.5, "Petrol ve Doğal Gaz Mühendisliği", "SAY", 8 },
                    { 127, 3150, 502.80000000000001, "Çevre Mühendisliği", "SAY", 8 },
                    { 128, 2050, 515.79999999999995, "Havacılık ve Uzay Mühendisliği", "SAY", 8 },
                    { 129, 3450, 498.69999999999999, "Gıda Mühendisliği", "SAY", 8 },
                    { 130, 2750, 508.5, "Fizik", "SAY", 8 },
                    { 131, 2950, 505.80000000000001, "Kimya", "SAY", 8 },
                    { 132, 2550, 510.5, "Matematik", "SAY", 8 },
                    { 133, 3450, 498.80000000000001, "İstatistik", "SAY", 8 },
                    { 134, 3150, 502.5, "Biyoloji", "SAY", 8 },
                    { 135, 2750, 508.69999999999999, "Moleküler Biyoloji ve Genetik", "SAY", 8 },
                    { 136, 2450, 512.79999999999995, "Mimarlık", "SAY", 8 },
                    { 137, 2950, 505.5, "Şehir ve Bölge Planlama", "SAY", 8 },
                    { 138, 2850, 505.80000000000001, "İşletme", "EA", 8 },
                    { 139, 3050, 502.5, "Ekonomi", "EA", 8 },
                    { 140, 3350, 498.80000000000001, "Uluslararası İlişkiler", "EA", 8 },
                    { 141, 3550, 495.69999999999999, "Psikoloji", "EA", 8 }
                });

            migrationBuilder.InsertData(
                table: "JobPostings",
                columns: new[] { "Id", "CompanyId", "Description", "IsActive", "Location", "PostedDate", "Title" },
                values: new object[,]
                {
                    { 1, 2, "ASP.NET Core ve Azure konusunda deneyimli...", true, "İstanbul", new DateTime(2025, 12, 10, 20, 46, 52, 591, DateTimeKind.Utc).AddTicks(4971), "Kıdemli .NET Geliştiricisi" },
                    { 2, 1, "React ve TypeScript bilen...", true, "Ankara", new DateTime(2025, 12, 10, 20, 46, 52, 591, DateTimeKind.Utc).AddTicks(4977), "Frontend Geliştirici (React)" },
                    { 3, 2, "CI/CD süreçlerine hakim...", true, "İstanbul", new DateTime(2025, 12, 10, 20, 46, 52, 591, DateTimeKind.Utc).AddTicks(4978), "DevOps Mühendisi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContentArticles_ContentCategoryId",
                table: "ContentArticles",
                column: "ContentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_UniversityId",
                table: "Departments",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_AppUserId",
                table: "JobApplications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobPostingId",
                table: "JobApplications",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_CompanyId",
                table: "JobPostings",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ContentArticles");

            migrationBuilder.DropTable(
                name: "CvSamples");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ContentCategories");

            migrationBuilder.DropTable(
                name: "Universities");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "JobPostings");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
