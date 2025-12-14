using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using microbloom.Entities;

namespace microbloom.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<KariyerDBContext>();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("Employer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Employer"));
            }

            if (!await roleManager.RoleExistsAsync("JobSeeker"))
            {
                await roleManager.CreateAsync(new IdentityRole("JobSeeker"));
            }

            string companyEmail = "company@microsoft.com";
            var companyUser = await userManager.FindByEmailAsync(companyEmail);

            if (companyUser == null)
            {
                var microsoftCompany = await dbContext.Companies!.FindAsync(2);

                if (microsoftCompany != null)
                {
                    companyUser = new AppUser
                    {
                        UserName = companyEmail,
                        Email = companyEmail,
                        FirstName = "Bill",
                        LastName = "Gates (Test)",
                        EmailConfirmed = true,
                        CompanyId = microsoftCompany.Id
                    };

                    var result = await userManager.CreateAsync(companyUser, "Company123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(companyUser, "Employer");
                    }
                }
            }

            // Test normal kullanıcısı oluştur
            string userEmail = "user@test.com";
            var normalUser = await userManager.FindByEmailAsync(userEmail);

            if (normalUser == null)
            {
                normalUser = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Normal",
                    LastName = "Kullanıcı (Test)",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(normalUser, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "JobSeeker");
                }
            }
        }


        public static async Task SeedRandomDataAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<KariyerDBContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // 1. Random Employers & Companies (20 tane)
            if (await dbContext.Companies.CountAsync() < 10)
            {
                var companySuffixes = new[] { "Tech", "Systems", "Solutions", "Soft", "Global", "Group", "Holdings", "Partners", "Labs", "Interactive", "Digital", "Media", "Construction", "Health", "Energy" };
                var companyPrefixes = new[] { "Alpha", "Beta", "Omega", "Delta", "Nova", "Terra", "Cyber", "Blue", "Red", "Green", "Star", "Sky", "Ocean", "Peak", "Summit", "Apex", "Zenith", "Quantum", "Future", "Smart" };

                var random = new Random();

                for (int i = 0; i < 20; i++)
                {
                    var name = $"{companyPrefixes[random.Next(companyPrefixes.Length)]} {companySuffixes[random.Next(companySuffixes.Length)]}";
                    if (await dbContext.Companies.AnyAsync(c => c.Name == name)) continue;

                    var company = new Company
                    {
                        Name = name,
                        Description = $"{name} sektörün öncü firmalarından biridir. Yenilikçi çözümler sunuyoruz.",
                        LogoUrl = "default-company.png"
                    };

                    dbContext.Companies.Add(company);
                    await dbContext.SaveChangesAsync(); // ID almak için kaydet

                    // Şirket Kullanıcısı (Employer)
                    var email = $"info@{name.Replace(" ", "").ToLower()}.com";
                    if (await userManager.FindByEmailAsync(email) == null)
                    {
                        var employerUser = new AppUser
                        {
                            UserName = email,
                            Email = email,
                            FirstName = "HR",
                            LastName = "Manager",
                            EmailConfirmed = true,
                            CompanyId = company.Id,
                            Title = "İşe Alım Uzmanı"
                        };

                        var result = await userManager.CreateAsync(employerUser, "Company123!");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(employerUser, "Employer");
                        }
                    }

                    // 2. Jobs (En az 2 ilan)
                    var jobTitles = new[] { "Yazılım Mühendisi", "Proje Yöneticisi", "Satış Temsilcisi", "Muhasebe Uzmanı", "İK Asistanı", "Frontend Developer", "Backend Developer", "DevOps Engineer", "Pazarlama Uzmanı", "Grafik Tasarımcı", "İş Analisti", "Müşteri Temsilcisi" };
                    var cities = new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya", "Kocaeli", "remote" };

                    for (int j = 0; j < 3; j++) // Her şirkete 3 ilan
                    {
                        var title = jobTitles[random.Next(jobTitles.Length)];
                        var job = new JobPosting
                        {
                            CompanyId = company.Id,
                            Title = title,
                            Description = $"Aradığımız nitelikler: {title} pozisyonunda en az 2 yıl deneyim. İletişim becerileri güçlü. Takım çalışmasına yatkın.",
                            Location = cities[random.Next(cities.Length)],
                            IsActive = true,
                            PostedDate = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                        };
                        dbContext.JobPostings.Add(job);
                    }
                }
                await dbContext.SaveChangesAsync();
            }

            // 3. Random JobSeekers
            if (await userManager.GetUsersInRoleAsync("JobSeeker").ContinueWith(t => t.Result.Count) < 5)
            {
                 var firstNames = new[] { "Ahmet", "Mehmet", "Ayşe", "Fatma", "Mustafa", "Zeynep", "Can", "Elif", "Emre", "Selin", "Burak", "Gamze", "Murat", "Ebru" };
                 var lastNames = new[] { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Yıldırım", "Öztürk", "Aydın", "Özdemir", "Arslan", "Doğan" };
                
                 var random = new Random();

                 for(int i=0; i<15; i++)
                 {
                     var fName = firstNames[random.Next(firstNames.Length)];
                     var lName = lastNames[random.Next(lastNames.Length)];
                     var email = $"{fName.ToLower()}.{lName.ToLower()}{random.Next(100,999)}@test.com";

                     if (await userManager.FindByEmailAsync(email) == null)
                     {
                         var user = new AppUser
                         {
                             UserName = email,
                             Email = email,
                             FirstName = fName,
                             LastName = lName,
                             Title = "İş Arayan",
                             Bio = "Kariyerimde yeni fırsatlar arıyorum.",
                             EmailConfirmed = true
                         };

                         var result = await userManager.CreateAsync(user, "User123!");
                         if (result.Succeeded)
                         {
                             await userManager.AddToRoleAsync(user, "JobSeeker");
                         }
                     }
                 }
                 // No need for SaveChanges here as UserManager handles it
             }

            // 4. Content Seeding (Guides)
            if (!await dbContext.ContentCategories.AnyAsync())
            {
                // -- Kategori 1: Üniversiteye Hazırlık --
                var uniCategory = new ContentCategory
                {
                    Title = "Üniversiteye Hazırlık",
                    Slug = "universiteye-hazirlik"
                    // Description and Icon removed as they don't exist in Entity
                };
                dbContext.ContentCategories.Add(uniCategory);
                await dbContext.SaveChangesAsync();

                var uniArticles = new[]
                {
                    new ContentArticle
                    {
                        Title = "Doğru Üniversite Tercihi Nasıl Yapılır?",
                        Slug = "universite-secimi-rehberi",
                        Summary = "Geleceğinizi şekillendirecek üniversite ve bölüm tercihini yaparken dikkat etmeniz gereken 10 altın kural.",
                        Content = @"<h1 class='display-6 text-primary mb-4'>🎓 Üniversite Seçimi Rehberi</h1>
<p class='lead'>Üniversite tercihi, kariyer yolculuğunuzun en önemli kavşaklarından biridir. Puanınızın yetmesinden ziyade, ilgi ve yeteneklerinize uygun bir bölüm seçmek sizi başarıya götürür.</p>
<h2 class='h4 text-secondary mt-4 mb-3'>1. Kendinizi Tanıyın</h2>
<p>Öncelikle hangi alanlara ilginiz olduğunu ve hangi konularda yetenekli olduğunuzu belirleyin. Sözel, sayısal veya eşit ağırlık alanlarından hangisinde kendinizi daha rahat hissediyorsunuz?</p>
<h2 class='h4 text-secondary mt-4 mb-3'>2. Kampüs Olanaklarını Araştırın</h2>
<p>Seçeceğiniz üniversitenin kütüphane, laboratuvar, sosyal kulüpler ve yurt imkanlarını detaylıca inceleyin.</p>
<blockquote>
'Başarı, hazırlık ve fırsatın buluştuğu yerdedir.' - Bobby Unser
</blockquote>
<h3 class='h5 text-muted mt-3 mb-2'>Şehir Faktörü</h3>
<p>Üniversitenin bulunduğu şehir, öğrencilik hayatınızı doğrudan etkiler. Büyükşehirlerin sunduğu staj imkanları ile küçük şehirlerin sakinliği arasında bir tercih yapın.</p>",
                        ContentCategoryId = uniCategory.Id
                    },
                    new ContentArticle
                    {
                        Title = "Mühendislik Fakültesi Bölüm Rehberi",
                        Slug = "bolum-rehberi-muhendislik",
                        Summary = "Bilgisayar, Endüstri, Makine... Hangi mühendislik dalı size göre? Detaylı analiz.",
                        Content = @"<h1 class='display-6 text-primary mb-4'>💻 Mühendislik Bölümleri Rehberi</h1>
<p>Mühendislik, problem çözme yeteneğine dayalı, analitik düşünmeyi gerektiren geniş bir alandır.</p>
<ul class='list-unstyled ms-3'>
<li><strong>Bilgisayar Mühendisliği:</strong> Yazılım ve donanım sistemleri üzerine odaklanır.</li>
<li><strong>Endüstri Mühendisliği:</strong> Sistem verimliliği ve süreç optimizasyonu ile ilgilenir.</li>
<li><strong>Makine Mühendisliği:</strong> Mekanik sistemlerin tasarımı ve üretimi temelidir.</li>
</ul>
<h2 class='h4 text-secondary mt-4 mb-3'>Geleceğin Mühendislik Dalları</h2>
<p>Yapay zeka mühendisliği ve enerji sistemleri mühendisliği son yılların en popüler alanları arasında.</p>",
                        ContentCategoryId = uniCategory.Id
                    }
                };
                dbContext.ContentArticles.AddRange(uniArticles);


                // -- Kategori 2: İlk İşim ve Profesyonel Hayat --
                var careerCategory = new ContentCategory
                {
                    Title = "İlk İşim ve Profesyonel Hayat",
                    Slug = "profesyonel-hayat"
                };
                dbContext.ContentCategories.Add(careerCategory);
                await dbContext.SaveChangesAsync();

                var careerArticles = new[]
                {
                     new ContentArticle
                    {
                        Title = "Etkili CV Hazırlama Teknikleri",
                        Slug = "etkili-cv-hazirlama",
                        Summary = "İK uzmanlarının dikkatini çekecek, ATS uyumlu ve profesyonel bir CV nasıl oluşturulur?",
                        Content = @"<h1 class='display-6 text-primary mb-4'>📄 CV Hazırlama Rehberi</h1>
<p class='lead'>CV'niz sizin vitrininizdir. İlk izlenimi oluşturmak için sadece 6 saniyeniz var.</p>
<h2 class='h4 text-secondary mt-4 mb-3'>Yapılması Gerekenler</h2>
<ul class='list-unstyled ms-3'>
<li><strong>Kısa ve Özdür:</strong> Deneyimlerinizi maddeler halinde özetleyin.</li>
<li><strong>Ters Kronolojik Sıra:</strong> En son deneyiminizden başlayın.</li>
<li><strong>Başarı Odaklı Olun:</strong> Sadece görev tanımını değil, başardığınız somut sonuçları yazın.</li>
</ul>
<h3 class='h5 text-muted mt-3 mb-2'>Hobiler Kısmı Gerekli mi?</h3>
<p>Eğer iş ile ilgiliyse veya liderlik vasıflarınızı gösteriyorsa evet, aksi takdirde yer kaplamasına gerek yok.</p>",
                        ContentCategoryId = careerCategory.Id
                    },
                    new ContentArticle
                    {
                        Title = "Mülakatlarda En Çok Sorulan Sorular",
                        Slug = "mulakat-sorulari",
                        Summary = "'Güçlü ve zayıf yönleriniz nedir?' sorusuna nasıl cevap verilmeli? Mülakat simülasyonu.",
                        Content = @"<h1 class='display-6 text-primary mb-4'>🗣️ Mülakat Hazırlığı</h1>
<p>Mülakatlara hazırlıklı gitmek, stresi yönetmenin en iyi yoludur. İşte klasik sorular ve ipuçları:</p>
<code>
Soru: Neden şirketimizi tercih ettiniz?
Cevap: Şirketinizin vizyonu ve projeleri kariyer hedeflerimle örtüşüyor...
</code>
<h2 class='h4 text-secondary mt-4 mb-3'>STAR Tekniği</h2>
<p>Sorulara cevap verirken Durum (Situation), Görev (Task), Eylem (Action) ve Sonuç (Result) yapısını kullanın.</p>",
                        ContentCategoryId = careerCategory.Id
                    }
                };
                dbContext.ContentArticles.AddRange(careerArticles);

                await dbContext.SaveChangesAsync();
            }

            // 6. Department Seeding - Ensure each university has 30+ departments
            try
            {
                var universities = await dbContext.Universities.Include(u => u.Departments).ToListAsync();

                // Standard department list
                var standardDepts = new[]
                {
                    ("Bilgisayar Mühendisliği", "SAY"), ("Elektrik-Elektronik Mühendisliği", "SAY"),
                    ("Makine Mühendisliği", "SAY"), ("İnşaat Mühendisliği", "SAY"),
                    ("Endüstri Mühendisliği", "SAY"), ("Kimya Mühendisliği", "SAY"),
                    ("Biyomedikal Mühendisliği", "SAY"), ("Yazılım Mühendisliği", "SAY"),
                    ("Mekatronik Mühendisliği", "SAY"), ("Çevre Mühendisliği", "SAY"),
                    ("Gıda Mühendisliği", "SAY"), ("Mimarlık", "SAY"),
                    ("Şehir ve Bölge Planlama", "SAY"), ("İç Mimarlık", "EA"),
                    ("Tıp", "SAY"), ("Diş Hekimliği", "SAY"), ("Eczacılık", "SAY"),
                    ("Hemşirelik", "SAY"), ("Fizyoterapi ve Rehabilitasyon", "SAY"),
                    ("Beslenme ve Diyetetik", "SAY"), ("Veterinerlik", "SAY"),
                    ("Hukuk", "EA"), ("İşletme", "EA"), ("İktisat", "EA"),
                    ("Maliye", "EA"), ("Uluslararası İlişkiler", "EA"),
                    ("Siyaset Bilimi", "EA"), ("Kamu Yönetimi", "EA"),
                    ("Psikoloji", "EA"), ("Sosyoloji", "EA"), ("Felsefe", "SOZ"),
                    ("Tarih", "SOZ"), ("Türk Dili ve Edebiyatı", "SOZ"),
                    ("İngiliz Dili ve Edebiyatı", "DİL"), ("Almanca Öğretmenliği", "DİL"),
                    ("Matematik", "SAY"), ("Fizik", "SAY"), ("Kimya", "SAY"),
                    ("Biyoloji", "SAY"), ("Moleküler Biyoloji ve Genetik", "SAY"),
                    ("İstatistik", "SAY"), ("Ekonometri", "EA"),
                    ("Grafik Tasarım", "EA"), ("Görsel İletişim Tasarımı", "EA"),
                    ("Radyo, Televizyon ve Sinema", "EA"), ("Gazetecilik", "EA"),
                    ("Halkla İlişkiler ve Reklamcılık", "EA"), ("İletişim Tasarımı", "EA"),
                    ("Turizm İşletmeciliği", "EA"), ("Gastronomi ve Mutfak Sanatları", "EA"),
                    ("Spor Yönetimi", "EA"), ("Antrenörlük Eğitimi", "EA"),
                    ("Müzik", "ÖZEL"), ("Resim", "ÖZEL"), ("Sahne Sanatları", "ÖZEL"),
                    ("Havacılık Yönetimi", "EA"), ("Uluslararası Ticaret ve Finans", "EA"),
                    ("Lojistik Yönetimi", "EA"), ("Bankacılık ve Finans", "EA"),
                    ("Muhasebe ve Finansal Yönetim", "EA"), ("İnsan Kaynakları Yönetimi", "EA"),
                    ("Yönetim Bilişim Sistemleri", "EA"), ("E-Ticaret ve Pazarlama", "EA")
                };

                var random = new Random(42); // Fixed seed for consistent results

                // Top Turkish universities by name for realistic rankings
                var topUniNames = new Dictionary<string, int>
                {
                    { "Koç", 1 }, { "Bilkent", 2 }, { "Orta Doğu Teknik", 3 }, { "ODTÜ", 3 },
                    { "Boğaziçi", 4 }, { "İstanbul Teknik", 5 }, { "İTÜ", 5 },
                    { "TOBB", 6 }, { "Sabancı", 7 }, { "Hacettepe", 8 },
                    { "Galatasaray", 9 }, { "Ankara", 10 }, { "İstanbul", 11 },
                    { "Yıldız Teknik", 12 }, { "Marmara", 13 }, { "Ege", 14 },
                    { "Dokuz Eylül", 15 }, { "Gazi", 16 }, { "Erciyes", 17 }
                };

                foreach (var uni in universities)
                {
                    var existingDeptNames = uni.Departments?.Select(d => d.Name).ToHashSet() ?? new HashSet<string>();
                    var deptsToAdd = new List<Department>();
                    var targetCount = 30 + random.Next(0, 10); // 30-39 departments

                    // Determine university tier based on name
                    int uniTier = 50; // Default tier
                    foreach (var (name, tier) in topUniNames)
                    {
                        if (uni.Name != null && uni.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                        {
                            uniTier = tier;
                            break;
                        }
                    }

                    foreach (var (deptName, scoreType) in standardDepts)
                    {
                        if (existingDeptNames.Contains(deptName)) continue;
                        if (deptsToAdd.Count + existingDeptNames.Count >= targetCount) break;

                        // Base ranking calculation based on university tier and department type
                        int baseRanking = scoreType switch
                        {
                            "SAY" => 500 + (uniTier * 800) + random.Next(0, 500),
                            "EA" => 1000 + (uniTier * 1000) + random.Next(0, 800),
                            "SOZ" => 2000 + (uniTier * 1200) + random.Next(0, 1000),
                            "DİL" => 3000 + (uniTier * 1500) + random.Next(0, 1200),
                            _ => 5000 + (uniTier * 800) + random.Next(0, 1000)
                        };

                        // İlk 10 üniversite için çok daha iyi sıralamalar
                        if (uniTier <= 5) baseRanking = Math.Max(100, baseRanking - 3000);
                        else if (uniTier <= 10) baseRanking = Math.Max(500, baseRanking - 1500);

                        // Taban puan hesaplama (sıralamadan türetilmiş)
                        double baseScore = Math.Max(300, 600 - (baseRanking * 0.005) + random.NextDouble() * 20);

                        deptsToAdd.Add(new Department
                        {
                            Name = deptName,
                            ScoreType = scoreType,
                            LastYearBaseScore = baseScore,
                            LastYearBaseRanking = baseRanking,
                            UniversityId = uni.Id
                        });
                    }

                    if (deptsToAdd.Count > 0)
                    {
                        dbContext.Departments.AddRange(deptsToAdd);
                    }
                }

                await dbContext.SaveChangesAsync();
                Console.WriteLine($"Department seeding completed. Total departments: {await dbContext.Departments.CountAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Department seeding error: {ex.Message}");
            }

            // 5. Update Existing Content to HTML (Fixing Markdown issue)
            // 5. Force Update Specific Articles to ensure Rich HTML (Fixes "Ugly" content persistence)
            try 
            {
                var articles = await dbContext.ContentArticles.ToListAsync();
                
                foreach (var article in articles)
                {
                    if (article.Slug == "universite-secimi-rehberi")
                    {
                        article.Content = @"<h1 class='display-6 text-primary mb-4'>🎓 Üniversite Seçimi Rehberi</h1>
<p class='lead text-dark'>Üniversite tercihi, hayatınızın en önemli kararlarından biridir. Doğru tercih yaparak geleceğinizi şekillendirin.</p>

<div class='alert alert-info border-0 mb-4'>
  <i class='bi bi-lightbulb-fill me-2'></i> <strong>Önemli:</strong> Sadece puanınızın yettiği bölümü değil, ilgi ve yeteneklerinize uygun bölümü seçin!
</div>

<h2 class='h4 text-secondary mt-4 mb-3'><i class='bi bi-1-circle-fill me-2 text-primary'></i>Kendinizi Tanıyın</h2>
<p>Tercih yapmadan önce şu soruları kendinize sorun:</p>
<ul class='list-group list-group-flush bg-transparent mb-4'>
  <li class='list-group-item bg-transparent'><i class='bi bi-check2 text-success me-2'></i>Hangi konularda yetenekliyim?</li>
  <li class='list-group-item bg-transparent'><i class='bi bi-check2 text-success me-2'></i>Hangi dersleri severek çalışıyorum?</li>
  <li class='list-group-item bg-transparent'><i class='bi bi-check2 text-success me-2'></i>5-10 yıl sonra kendimi nerede görüyorum?</li>
  <li class='list-group-item bg-transparent'><i class='bi bi-check2 text-success me-2'></i>Hangi çalışma ortamında mutlu olurum?</li>
</ul>

<h2 class='h4 text-secondary mt-4 mb-3'><i class='bi bi-2-circle-fill me-2 text-primary'></i>Üniversite Kriterleri</h2>
<div class='row g-3 my-3'>
  <div class='col-md-4'><div class='card border-0 shadow-sm p-3 h-100 text-center'><i class='bi bi-geo-alt fs-2 text-primary'></i><h6 class='mt-2'>Şehir</h6><small class='text-muted'>Büyükşehir mi, Anadolu mu?</small></div></div>
  <div class='col-md-4'><div class='card border-0 shadow-sm p-3 h-100 text-center'><i class='bi bi-building fs-2 text-success'></i><h6 class='mt-2'>Kampüs</h6><small class='text-muted'>Sosyal olanaklar, yurt, ulaşım</small></div></div>
  <div class='col-md-4'><div class='card border-0 shadow-sm p-3 h-100 text-center'><i class='bi bi-people fs-2 text-warning'></i><h6 class='mt-2'>Akademik Kadro</h6><small class='text-muted'>Öğretim üyesi sayısı ve kalitesi</small></div></div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'><i class='bi bi-3-circle-fill me-2 text-primary'></i>Şehir Faktörü</h2>
<p>Üniversitenin bulunduğu şehir, öğrencilik hayatınızı doğrudan etkiler:</p>
<table class='table table-hover'>
  <thead><tr><th>Büyükşehir</th><th>Küçük Şehir</th></tr></thead>
  <tbody>
    <tr><td>✅ Staj imkanları fazla</td><td>✅ Yaşam maliyeti düşük</td></tr>
    <tr><td>✅ Sosyal hayat canlı</td><td>✅ Daha sakin ortam</td></tr>
    <tr><td>❌ Kira ve ulaşım pahalı</td><td>❌ İş fırsatları sınırlı</td></tr>
  </tbody>
</table>

<h2 class='h4 text-secondary mt-4 mb-3'><i class='bi bi-4-circle-fill me-2 text-primary'></i>Tercih Stratejisi</h2>
<ol>
  <li class='mb-2'><strong>İlk 5 tercih:</strong> Gerçekten gitmek istediğiniz bölümler</li>
  <li class='mb-2'><strong>6-15. tercihler:</strong> Puanınıza uygun alternatifler</li>
  <li class='mb-2'><strong>16-24. tercihler:</strong> Güvenlik tercihleri</li>
</ol>

<blockquote class='blockquote border-start border-primary border-4 ps-3 my-4'>
  <p class='mb-0'>'Başarı, hazırlık ve fırsatın buluştuğu yerdedir.'</p>
  <footer class='blockquote-footer mt-2'>Bobby Unser</footer>
</blockquote>";
                    }
                    else if (article.Slug == "bolum-rehberi-muhendislik")
                    {
                        article.Content = @"<h1 class='display-6 text-primary mb-4'>💻 Mühendislik Bölümleri Rehberi</h1>
<p class='lead text-dark'>Mühendislik, problem çözme ve yaratıcılığın buluştuğu, geleceği şekillendiren disiplindir.</p>

<div class='alert alert-warning border-0 mb-4'>
  <i class='bi bi-star-fill me-2'></i> <strong>2024'ün en çok tercih edilen mühendislikleri:</strong> Bilgisayar, Yazılım ve Yapay Zeka Mühendisliği
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>📊 Mühendislik Dalları Karşılaştırması</h2>
<div class='row g-4 my-4'>
  <div class='col-md-6'>
    <div class='card h-100 border-0 shadow-sm'>
      <div class='card-header bg-primary text-white'><i class='bi bi-laptop me-2'></i>Bilgisayar Mühendisliği</div>
      <div class='card-body'>
        <p><strong>Ne yapar:</strong> Yazılım geliştirme, sistem tasarımı, yapay zeka</p>
        <p><strong>İş imkanları:</strong> Yazılım şirketleri, bankalar, teknoloji firmaları</p>
        <p><strong>Ortalama maaş:</strong> 35.000 - 100.000 TL</p>
        <span class='badge bg-success'>Yüksek İstihdam</span>
      </div>
    </div>
  </div>
  <div class='col-md-6'>
    <div class='card h-100 border-0 shadow-sm'>
      <div class='card-header bg-success text-white'><i class='bi bi-gear me-2'></i>Endüstri Mühendisliği</div>
      <div class='card-body'>
        <p><strong>Ne yapar:</strong> Süreç optimizasyonu, kalite yönetimi, üretim planlaması</p>
        <p><strong>İş imkanları:</strong> Üretim, lojistik, danışmanlık</p>
        <p><strong>Ortalama maaş:</strong> 28.000 - 70.000 TL</p>
        <span class='badge bg-info'>Çok Yönlü</span>
      </div>
    </div>
  </div>
  <div class='col-md-6'>
    <div class='card h-100 border-0 shadow-sm'>
      <div class='card-header bg-secondary text-white'><i class='bi bi-tools me-2'></i>Makine Mühendisliği</div>
      <div class='card-body'>
        <p><strong>Ne yapar:</strong> Mekanik tasarım, üretim, enerji sistemleri</p>
        <p><strong>İş imkanları:</strong> Otomotiv, havacılık, enerji</p>
        <p><strong>Ortalama maaş:</strong> 25.000 - 65.000 TL</p>
        <span class='badge bg-warning text-dark'>Klasik & Köklü</span>
      </div>
    </div>
  </div>
  <div class='col-md-6'>
    <div class='card h-100 border-0 shadow-sm'>
      <div class='card-header bg-danger text-white'><i class='bi bi-lightning me-2'></i>Elektrik-Elektronik Müh.</div>
      <div class='card-body'>
        <p><strong>Ne yapar:</strong> Elektronik devre, enerji sistemleri, haberleşme</p>
        <p><strong>İş imkanları:</strong> Telekomünikasyon, enerji, savunma</p>
        <p><strong>Ortalama maaş:</strong> 28.000 - 75.000 TL</p>
        <span class='badge bg-primary'>Geniş Alan</span>
      </div>
    </div>
  </div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>🚀 Geleceğin Mühendislik Alanları</h2>
<ul class='list-group list-group-flush'>
  <li class='list-group-item d-flex align-items-center'><i class='bi bi-robot fs-4 text-primary me-3'></i><div><strong>Yapay Zeka Mühendisliği</strong><br><small class='text-muted'>Machine Learning, Deep Learning, NLP</small></div></li>
  <li class='list-group-item d-flex align-items-center'><i class='bi bi-wind fs-4 text-success me-3'></i><div><strong>Yenilenebilir Enerji</strong><br><small class='text-muted'>Güneş, rüzgar ve hidroelektrik sistemler</small></div></li>
  <li class='list-group-item d-flex align-items-center'><i class='bi bi-rocket fs-4 text-danger me-3'></i><div><strong>Uzay Mühendisliği</strong><br><small class='text-muted'>Uydu teknolojileri, roket sistemleri</small></div></li>
</ul>";
                    }
                    else if (article.Slug == "burs-mali-destek")
                    {
                        article.Content = @"<h1 class='display-6 text-primary mb-4'>💰 Burs ve Mali Destek İmkanları</h1>
<p class='lead text-dark'>Eğitim masraflarınızı karşılamanın birçok yolu var. Fırsatları kaçırmayın!</p>

<div class='alert alert-success border-0 mb-4'>
  <i class='bi bi-cash-stack me-2'></i> <strong>İpucu:</strong> Birden fazla burs programına başvurun, şansınızı artırın!
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>🏛️ Devlet Bursları</h2>
<div class='row g-3 mb-4'>
  <div class='col-md-6'>
    <div class='card border-0 shadow-sm h-100'>
      <div class='card-body'>
        <h5><i class='bi bi-bank text-primary me-2'></i>KYK Bursu</h5>
        <p class='small'>Kredi ve Yurtlar Kurumu öğrenim kredisi ve burs imkanı</p>
        <ul class='small'>
          <li>Aylık 850 TL burs</li>
          <li>Geri ödemesiz veya düşük faizli kredi</li>
          <li>Her yıl başvuru yenilenir</li>
        </ul>
        <span class='badge bg-primary'>En Yaygın</span>
      </div>
    </div>
  </div>
  <div class='col-md-6'>
    <div class='card border-0 shadow-sm h-100'>
      <div class='card-body'>
        <h5><i class='bi bi-mortarboard text-success me-2'></i>YÖK Bursu</h5>
        <p class='small'>Yükseköğretim Kurulu başarı bursu</p>
        <ul class='small'>
          <li>Başarılı öğrencilere verilir</li>
          <li>GNO şartı vardır</li>
          <li>Aylık 1.000-2.000 TL arası</li>
        </ul>
        <span class='badge bg-success'>Başarı Ödülü</span>
      </div>
    </div>
  </div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>🏢 Özel Kuruluş Bursları</h2>
<table class='table table-hover'>
  <thead class='table-light'>
    <tr><th>Kuruluş</th><th>Burs Miktarı</th><th>Başvuru Dönemi</th></tr>
  </thead>
  <tbody>
    <tr><td><strong>TÜBİTAK</strong></td><td>1.500 - 3.000 TL/ay</td><td>Eylül-Ekim</td></tr>
    <tr><td><strong>TÜSİAD</strong></td><td>2.000 TL/ay</td><td>Ekim-Kasım</td></tr>
    <tr><td><strong>Koç Vakfı</strong></td><td>1.800 TL/ay</td><td>Eylül</td></tr>
    <tr><td><strong>Sabancı Vakfı</strong></td><td>2.500 TL/ay</td><td>Ekim</td></tr>
    <tr><td><strong>İş Bankası</strong></td><td>1.500 TL/ay</td><td>Eylül-Ekim</td></tr>
  </tbody>
</table>

<h2 class='h4 text-secondary mt-4 mb-3'>📋 Burs Başvuru Kontrol Listesi</h2>
<ul class='list-group'>
  <li class='list-group-item'><input class='form-check-input me-2' type='checkbox'>Öğrenci belgesi</li>
  <li class='list-group-item'><input class='form-check-input me-2' type='checkbox'>Transkript</li>
  <li class='list-group-item'><input class='form-check-input me-2' type='checkbox'>Gelir belgesi</li>
  <li class='list-group-item'><input class='form-check-input me-2' type='checkbox'>Nüfus kayıt örneği</li>
  <li class='list-group-item'><input class='form-check-input me-2' type='checkbox'>Niyet mektubu</li>
</ul>";
                    }
                    else if (article.Slug == "cv-hazirlama-rehberi")
                    {
                        article.Content = @"<h1 class='display-6 text-primary mb-4'>📄 CV Hazırlama Rehberi</h1>
<p class='lead text-dark'>Profesyonel bir CV ile işverenlerin dikkatini çekin.</p>

<div class='alert alert-info border-0 mb-4'>
  <i class='bi bi-stopwatch me-2'></i> <strong>Biliyor muydunuz?</strong> İK uzmanları CV'nize ortalama <u>6 saniye</u> bakar!
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>✅ CV'de Olması Gerekenler</h2>
<div class='row g-3 mb-4'>
  <div class='col-md-4'>
    <div class='card border-0 shadow-sm text-center p-3'>
      <i class='bi bi-person-badge fs-1 text-primary'></i>
      <h6 class='mt-2'>Kişisel Bilgiler</h6>
      <small class='text-muted'>Ad-soyad, telefon, e-posta, LinkedIn</small>
    </div>
  </div>
  <div class='col-md-4'>
    <div class='card border-0 shadow-sm text-center p-3'>
      <i class='bi bi-mortarboard fs-1 text-success'></i>
      <h6 class='mt-2'>Eğitim</h6>
      <small class='text-muted'>Üniversite, bölüm, GNO</small>
    </div>
  </div>
  <div class='col-md-4'>
    <div class='card border-0 shadow-sm text-center p-3'>
      <i class='bi bi-briefcase fs-1 text-warning'></i>
      <h6 class='mt-2'>Deneyim</h6>
      <small class='text-muted'>Stajlar, projeler, iş tecrübesi</small>
    </div>
  </div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>🎯 CV Yazım İpuçları</h2>
<ol>
  <li class='mb-2'><strong>Kısa ve öz olun:</strong> 1-2 sayfa yeterli</li>
  <li class='mb-2'><strong>Ters kronolojik sıra:</strong> En son deneyimden başlayın</li>
  <li class='mb-2'><strong>Başarı odaklı yazın:</strong> 'Görev aldım' yerine 'Satışları %20 artırdım'</li>
  <li class='mb-2'><strong>Anahtar kelimeler:</strong> İş ilanındaki kelimeleri kullanın</li>
  <li class='mb-2'><strong>Profesyonel format:</strong> Temiz, okunaklı tasarım</li>
</ol>

<h2 class='h4 text-secondary mt-4 mb-3'>❌ CV'de Olmaması Gerekenler</h2>
<ul class='list-group list-group-flush'>
  <li class='list-group-item text-danger'><i class='bi bi-x-circle me-2'></i>Selfie veya gündelik fotoğraf</li>
  <li class='list-group-item text-danger'><i class='bi bi-x-circle me-2'></i>Kişisel bilgiler (TC no, doğum tarihi)</li>
  <li class='list-group-item text-danger'><i class='bi bi-x-circle me-2'></i>İlgisiz hobiler</li>
  <li class='list-group-item text-danger'><i class='bi bi-x-circle me-2'></i>Yazım hataları</li>
</ul>";
                    }
                    else if (article.Slug == "is-gorusmesine-hazirlik")
                    {
                        article.Content = @"<h1 class='display-6 text-primary mb-4'>🎤 İş Görüşmesine Hazırlık</h1>
<p class='lead text-dark'>Mülakatlara hazırlıklı gitmek başarının anahtarıdır.</p>

<h2 class='h4 text-secondary mt-4 mb-3'>❓ En Çok Sorulan Sorular</h2>
<div class='accordion' id='interviewQuestions'>
  <div class='accordion-item'>
    <h2 class='accordion-header'><button class='accordion-button' type='button'>Kendinizden bahseder misiniz?</button></h2>
    <div class='accordion-body'><strong>İpucu:</strong> 2 dakikada özetleyin: Eğitim + Deneyim + Neden bu iş</div>
  </div>
  <div class='accordion-item'>
    <h2 class='accordion-header'><button class='accordion-button collapsed' type='button'>Zayıf yönleriniz neler?</button></h2>
    <div class='accordion-body'><strong>İpucu:</strong> Geliştirdiğiniz bir zayıf yönden bahsedin: 'Detaylara takılıyordum ama artık önceliklendirme yapıyorum'</div>
  </div>
  <div class='accordion-item'>
    <h2 class='accordion-header'><button class='accordion-button collapsed' type='button'>5 yıl sonra kendinizi nerede görüyorsunuz?</button></h2>
    <div class='accordion-body'><strong>İpucu:</strong> Şirketle büyüme hedefinden bahsedin</div>
  </div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>⭐ STAR Tekniği</h2>
<p>Davranışsal sorulara cevap verirken kullanın:</p>
<div class='row g-2 mb-4'>
  <div class='col-3'><div class='card bg-primary text-white text-center p-2'><strong>S</strong>ituation<br><small>Durum</small></div></div>
  <div class='col-3'><div class='card bg-success text-white text-center p-2'><strong>T</strong>ask<br><small>Görev</small></div></div>
  <div class='col-3'><div class='card bg-warning text-center p-2'><strong>A</strong>ction<br><small>Eylem</small></div></div>
  <div class='col-3'><div class='card bg-danger text-white text-center p-2'><strong>R</strong>esult<br><small>Sonuç</small></div></div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>👔 Mülakat Günü Kontrol Listesi</h2>
<ul class='list-group'>
  <li class='list-group-item'><i class='bi bi-check2 text-success me-2'></i>Profesyonel kıyafet</li>
  <li class='list-group-item'><i class='bi bi-check2 text-success me-2'></i>CV'nin çıktısı (3 adet)</li>
  <li class='list-group-item'><i class='bi bi-check2 text-success me-2'></i>10-15 dakika erken git</li>
  <li class='list-group-item'><i class='bi bi-check2 text-success me-2'></i>Şirket hakkında araştırma yap</li>
  <li class='list-group-item'><i class='bi bi-check2 text-success me-2'></i>Sormak için 2-3 soru hazırla</li>
</ul>";
                    }
                    else if (article.Slug == "staj-is-bulma")
                    {
                        article.Content = @"<h1 class='display-6 text-primary mb-4'>🚀 Staj ve İş Bulma Rehberi</h1>
<p class='lead text-dark'>Kariyerinize güçlü bir başlangıç yapın!</p>

<div class='alert alert-warning border-0 mb-4'>
  <i class='bi bi-clock me-2'></i> <strong>Ne zaman başvurmalı?</strong> Yaz stajları için Ocak-Mart arası başvurun!
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>📍 Staj Bulma Platformları</h2>
<div class='row g-3 mb-4'>
  <div class='col-md-4'>
    <div class='card border-0 shadow-sm p-3 text-center'>
      <i class='bi bi-linkedin fs-1 text-primary'></i>
      <h6 class='mt-2'>LinkedIn</h6>
      <small class='text-muted'>Profesyonel ağ kurma</small>
    </div>
  </div>
  <div class='col-md-4'>
    <div class='card border-0 shadow-sm p-3 text-center'>
      <i class='bi bi-briefcase fs-1 text-success'></i>
      <h6 class='mt-2'>Kariyer.net</h6>
      <small class='text-muted'>Türkiye'nin en büyük iş sitesi</small>
    </div>
  </div>
  <div class='col-md-4'>
    <div class='card border-0 shadow-sm p-3 text-center'>
      <i class='bi bi-mortarboard fs-1 text-warning'></i>
      <h6 class='mt-2'>Üniversite Kariyer Merkezi</h6>
      <small class='text-muted'>Kampüs etkinlikleri ve fuarlar</small>
    </div>
  </div>
</div>

<h2 class='h4 text-secondary mt-4 mb-3'>📅 Staj Başvuru Takvimi</h2>
<table class='table table-bordered'>
  <thead class='table-primary'>
    <tr><th>Dönem</th><th>Hazırlık</th><th>Başvuru</th><th>Staj</th></tr>
  </thead>
  <tbody>
    <tr><td><strong>Yaz Stajı</strong></td><td>Aralık-Ocak</td><td>Şubat-Mart</td><td>Haziran-Ağustos</td></tr>
    <tr><td><strong>Kış Stajı</strong></td><td>Eylül</td><td>Ekim</td><td>Ocak-Şubat</td></tr>
  </tbody>
</table>

<h2 class='h4 text-secondary mt-4 mb-3'>💡 Başvuru İpuçları</h2>
<ol>
  <li class='mb-2'><strong>Önceden araştırma:</strong> Şirketin ne yaptığını öğrenin</li>
  <li class='mb-2'><strong>Özelleştirilmiş CV:</strong> Her başvuru için CV'nizi düzenleyin</li>
  <li class='mb-2'><strong>Motivasyon mektubu:</strong> Neden bu şirkette staj yapmak istediğinizi açıklayın</li>
  <li class='mb-2'><strong>Takip edin:</strong> 1-2 hafta sonra nazik bir follow-up yapın</li>
</ol>

<h2 class='h4 text-secondary mt-4 mb-3'>🎯 Stajda Başarılı Olma</h2>
<ul class='list-group list-group-flush'>
  <li class='list-group-item'><i class='bi bi-star text-warning me-2'></i>Proaktif olun, iş isteyin</li>
  <li class='list-group-item'><i class='bi bi-star text-warning me-2'></i>Sorular sorun, öğrenin</li>
  <li class='list-group-item'><i class='bi bi-star text-warning me-2'></i>Network kurun</li>
  <li class='list-group-item'><i class='bi bi-star text-warning me-2'></i>Geri bildirim isteyin</li>
</ul>";
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Ignore errors during content fix
                Console.WriteLine($"Content fix error: {ex.Message}");
            }

            // 7. CV Samples Seeding
            if (await dbContext.CvSamples.CountAsync() < 5)
            {
                var cvSamples = new List<CvSample>
                {
                    new CvSample
                    {
                        Title = "Yeni Mezun Yazılım Geliştirici CV",
                        Description = "Bilgisayar mühendisliği veya yazılım mühendisliği yeni mezunları için hazırlanmış modern CV şablonu. Proje deneyimi, teknik beceriler ve eğitim bilgileri ön planda.",
                        FileDownloadUrl = "/cv-samples/yeni-mezun-yazilim.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/software-dev.png"
                    },
                    new CvSample
                    {
                        Title = "Deneyimli Yazılım Mühendisi CV",
                        Description = "5+ yıl deneyimli yazılım mühendisleri için profesyonel CV şablonu. Proje liderliği, teknik uzmanlık ve başarılar vurgulanmış.",
                        FileDownloadUrl = "/cv-samples/deneyimli-yazilim-muhendisi.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/senior-dev.png"
                    },
                    new CvSample
                    {
                        Title = "Stajyer / Öğrenci CV",
                        Description = "Üniversite öğrencileri ve staj arayan adaylar için ideal CV şablonu. Akademik başarılar, kulüp aktiviteleri ve kurslar ön planda.",
                        FileDownloadUrl = "/cv-samples/stajyer-ogrenci.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/intern.png"
                    },
                    new CvSample
                    {
                        Title = "Veri Bilimci / Data Scientist CV",
                        Description = "Veri bilimi ve yapay zeka alanında kariyer yapmak isteyenler için özel şablon. Python, ML projeleri ve analitik yetkinlikler vurgulanmış.",
                        FileDownloadUrl = "/cv-samples/veri-bilimci.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/data-scientist.png"
                    },
                    new CvSample
                    {
                        Title = "Frontend Developer CV",
                        Description = "React, Vue.js veya Angular deneyimli frontend geliştiriciler için modern CV şablonu. Portfolio linkleri ve UI/UX projeleri vurgulanmış.",
                        FileDownloadUrl = "/cv-samples/frontend-developer.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/frontend.png"
                    },
                    new CvSample
                    {
                        Title = "Backend Developer CV",
                        Description = "Node.js, .NET, Java veya Python backend geliştiricileri için profesyonel şablon. API tasarımı, veritabanı ve sistem mimarisi deneyimi.",
                        FileDownloadUrl = "/cv-samples/backend-developer.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/backend.png"
                    },
                    new CvSample
                    {
                        Title = "DevOps Mühendisi CV",
                        Description = "CI/CD, Docker, Kubernetes ve bulut teknolojileri uzmanları için CV şablonu. Otomasyon projeleri ve altyapı yönetimi deneyimi.",
                        FileDownloadUrl = "/cv-samples/devops-muhendisi.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/devops.png"
                    },
                    new CvSample
                    {
                        Title = "Mobil Uygulama Geliştirici CV",
                        Description = "iOS (Swift) veya Android (Kotlin) geliştiricileri için özel şablon. App Store/Play Store'da yayınlanan uygulamalar ve kullanıcı metrikleri.",
                        FileDownloadUrl = "/cv-samples/mobil-gelistirici.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/mobile.png"
                    },
                    new CvSample
                    {
                        Title = "Proje Yöneticisi / Scrum Master CV",
                        Description = "Agile metodoloji ve proje yönetimi uzmanları için profesyonel şablon. Takım liderliği, sprint planlama ve başarı metrikleri.",
                        FileDownloadUrl = "/cv-samples/proje-yoneticisi.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/pm.png"
                    },
                    new CvSample
                    {
                        Title = "Siber Güvenlik Uzmanı CV",
                        Description = "Bilgi güvenliği ve siber güvenlik alanında kariyer yapanlar için şablon. Sertifikalar, penetrasyon testleri ve güvenlik projeleri.",
                        FileDownloadUrl = "/cv-samples/siber-guvenlik.pdf",
                        ThumbnailImageUrl = "/images/cv-thumbnails/security.png"
                    }
                };

                dbContext.CvSamples.AddRange(cvSamples);
                await dbContext.SaveChangesAsync();
                Console.WriteLine($"CV Samples seeding completed. Total: {await dbContext.CvSamples.CountAsync()}");
            }
        }
    }
}