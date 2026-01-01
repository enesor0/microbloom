using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using microbloom.Entities;

namespace microbloom.Data
{
    public class KariyerDBContext : IdentityDbContext<AppUser>
    {
        public KariyerDBContext(DbContextOptions<KariyerDBContext> options) : base(options)
        {
        }
    public DbSet<Company> Companies { get; set; } = default!;
    public DbSet<ContentCategory> ContentCategories { get; set; } = default!;
    public DbSet<ContentArticle> ContentArticles { get; set; } = default!;
    public DbSet<JobPosting> JobPostings { get; set; } = default!;
    public DbSet<JobApplication> JobApplications { get; set; } = default!;


    public DbSet<University> Universities { get; set; } = default!;
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<CvSample> CvSamples { get; set; } = default!;
    public DbSet<Message> Messages { get; set; } = default!;
    public DbSet<MentorshipApplication> MentorshipApplications { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AppUser>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Employees)
                .HasForeignKey(u => u.CompanyId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<JobPosting>()
                .HasOne(jp => jp.Company)
                .WithMany(c => c.JobPostings)
                .HasForeignKey(jp => jp.CompanyId);

            builder.Entity<JobApplication>()
                .HasOne(ja => ja.JobPosting)
                .WithMany()
                .HasForeignKey(ja => ja.JobPostingId);
            builder.Entity<JobApplication>()
                .HasOne(ja => ja.AppUser)
                .WithMany(u => u.Applications)
                .HasForeignKey(ja => ja.AppUserId);
            builder.Entity<ContentCategory>()
                .HasMany(c => c.Articles)
                .WithOne(a => a.ContentCategory)
                .HasForeignKey(a => a.ContentCategoryId);


            builder.Entity<Department>()
        .HasOne(d => d.University)
    .WithMany(u => u.Departments)
        .HasForeignKey(d => d.UniversityId)
 .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<MentorshipApplication>()
                .HasOne(m => m.Mentee)
                .WithMany()
                .HasForeignKey(m => m.MenteeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MentorshipApplication>()
                .HasOne(m => m.Mentor)
                .WithMany()
                .HasForeignKey(m => m.MentorId)
                .OnDelete(DeleteBehavior.Restrict);




            builder.Entity<ContentCategory>().HasData(
               new { Id = 1, Title = "Üniversiteye Hazırlık", Slug = "universiteye-hazirlik" },
                          new { Id = 2, Title = "İlk İşim ve Profesyonel Hayat", Slug = "profesyonel-hayat" }
                );


            builder.Entity<ContentArticle>().HasData(

    new
    {
        Id = 1,
        Title = "Üniversite Seçimi Rehberi",
        Slug = "universite-secimi-rehberi",
        Summary = "Üniversite seçerken nelere dikkat etmelisiniz? Şehir, bölüm, taban puan ve kariyer hedeflerinize göre doğru tercih nasıl yapılır?",
        Content = @"# &#x1F393; Üniversite Seçimi Rehberi

Üniversite seçimi, hayatınızın en önemli kararlarından biridir. İşte doğru tercih yapmanız için ipuçları:

## &#x1F4D6; 1. Bölüm Seçimi
- &#x2714; İlgi alanlarınızı ve yeteneklerinizi değerlendirin
- &#x2714; Bölümün kariyer olanaklarını araştırın
- &#x2714; Sektördeki iş imkanlarını inceleyin

## &#x1F3EB; 2. Üniversite Kriterleri
- &#x1F4DA; Akademik kadro kalitesi
- &#x1F3C6; Kampüs imkanları ve sosyal aktiviteler
- &#x1F30D; Uluslararası değişim programları
- &#x1F4BC; Mezun memnuniyeti ve kariyer desteği

## &#x1F3D9; 3. Şehir Seçimi
- &#x1F4B5; Yaşam maliyeti
- &#x1F3AD; Kültürel ve sosyal olanaklar
- &#x1F3E2; İş bulma imkanları
- &#x1F3E0; Ailenize uzaklık

## &#x1F4CA; 4. Taban Puan ve Sıralama
- &#x2705; Gerçekçi hedefler belirleyin
- &#x1F4DD; Yedek tercihlerinizi mutlaka doldurun
- &#x1F4C8; Önceki yılların yerleştirme puanlarını inceleyin",
        ContentCategoryId = 1
    },
      new
      {
          Id = 2,
          Title = "Bölüm Rehberi: Mühendislik",
          Slug = "bolum-rehberi-muhendislik",
          Summary = "Mühendislik bölümleri hakkında her şey: Hangi bölüm size uygun? Kariyer olanakları neler? Mezuniyet sonrası ne yapabilirsiniz?",
          Content = @"# &#x1F4BB; Mühendislik Bölümleri Rehberi

## &#x1F5A5; Bilgisayar Mühendisliği
Yazılım geliştirme, veri bilimi, yapay zeka gibi alanlarda çalışma fırsatı sunar.

**Kariyer Alanları:**
- &#x1F4BB; Yazılım Geliştirici
- &#x1F4CA; Veri Analisti / Data Scientist
- &#x1F3D7; Sistem Mimarı
- &#x2699; DevOps Mühendisi

**Başlangıç Maaşları:** &#x1F4B0; 25.000 - 40.000 TL

## &#x26A1; Elektrik-Elektronik Mühendisliği
Elektronik sistemler, güç sistemleri, telekomünikasyon alanlarında uzmanlaşma.

**Kariyer Alanları:**
- &#x1F50C; Elektronik Tasarım Mühendisi
- &#x1F4A1; Enerji Sistemleri Uzmanı
- &#x1F4E1; Telekomünikasyon Mühendisi

## &#x2699; Endüstri Mühendisliği
Üretim süreçlerinin optimizasyonu, lojistik ve proje yönetimi.

**Kariyer Alanları:**
- &#x1F4C8; Proje Yöneticisi
- &#x1F69A; Lojistik Uzmanı
- &#x1F504; Süreç Geliştirme Mühendisi",
          ContentCategoryId = 1
      },
          new
          {
              Id = 3,
              Title = "Burs ve Mali Destek İmkanları",
              Slug = "burs-mali-destek",
              Summary = "Üniversite eğitiminiz için burs ve mali destek alma yolları. Hangi kuruluşlar burs veriyor? Başvuru şartları neler?",
              Content = @"# &#x1F4B0; Burs ve Mali Destek Rehberi

## &#x1F3DB; Devlet Bursu
- &#x1F393; YÖK bursu
- &#x1F3E0; Kredi Yurtlar Kurumu (KYK)
- &#x1F3C6; Başarı bursu

## &#x1F3E2; Özel Kuruluş Bursları
- &#x1F4DA; Vakıf üniversiteleri tam burs programları
- &#x1F4BC; Özel sektör şirket bursları (TÜBİTAK, TÜSİAD)
- &#x1F3DB; Belediye bursları

## &#x1F4DD; Başvuru İpuçları
1. &#x1F4C5; Başvuru tarihlerini takip edin
2. &#x1F4C4; Gereken belgeleri önceden hazırlayın
3. &#x270D; Motivasyon mektubunuza özen gösterin
4. &#x1F4E8; Birden fazla burs programına başvurun

## &#x1F517; Önemli Linkler
- turkiye.gov.tr/kyk-ogrenci-kredisi
- yok.gov.tr",
              ContentCategoryId = 1
          },


      new
      {
          Id = 4,
          Title = "CV Hazırlama Rehberi",
          Slug = "cv-hazirlama-rehberi",
          Summary = "Profesyonel bir CV nasıl hazırlanır? İşverenin dikkatini çekecek CV örnekleri ve ipuçları.",
          Content = @"# &#x1F4C4; CV Hazırlama Rehberi

## &#x2705; CV'de Olması Gerekenler

### 1. &#x1F464; Kişisel Bilgiler
- &#x1F4DD; Ad Soyad
- &#x1F4DE; İletişim Bilgileri (Telefon, E-posta)
- &#x1F517; LinkedIn Profili
- &#x1F4BB; GitHub (yazılımcılar için)

### 2. &#x1F4AC; Özet
2-3 cümlelik kısa bir özet ile kendinizi tanıtın.

**Örnek:** ""Bilgisayar Mühendisliği mezunu, 2 yıllık web geliştirme deneyimi. React ve Node.js teknolojilerinde uzman.""

### 3. &#x1F393; Eğitim
- &#x1F3DB; Üniversite adı ve bölüm
- &#x1F4C5; Mezuniyet tarihi
- &#x1F4CA; Not ortalaması (3.00 üzerindeyse)

### 4. &#x1F4BC; İş Deneyimi
- &#x1F3E2; Şirket adı ve pozisyon
- &#x1F4C6; Çalışma tarihleri
- &#x2705; Görev ve başarılarınız
- &#x1F6E0; Kullandığınız teknolojiler

### 5. &#x1F680; Projeler
- &#x1F4BB; Kişisel veya okul projeleri
- &#x1F310; Açık kaynak katkılarınız

### 6. &#x2B50; Beceriler
- &#x1F4DA; Programlama dilleri
- &#x1F527; Araçlar ve teknolojiler
- &#x1F30D; Yabancı dil seviyeleri

## &#x1F4A1; CV Hazırlama İpuçları
- &#x1F4C4; Maksimum 2 sayfa olmalı
- &#x1F3AF; Özgeçmişinizi her pozisyon için özelleştirin
- &#x1F4CA; Somut başarılarınızı sayılarla destekleyin
- &#x2705; Yazım hatalarından kaçının",
          ContentCategoryId = 2
      },
      new
      {
          Id = 5,
          Title = "İş Görüşmesine Hazırlık",
          Slug = "is-gorusmesine-hazirlik",
          Summary = "İş görüşmesinde başarılı olmanın püf noktaları. Sık sorulan sorular ve nasıl cevaplanır?",
          Content = @"# &#x1F3AF; İş Görüşmesine Hazırlık

## &#x1F4C5; Görüşme Öncesi
1. &#x1F50D; Şirket hakkında araştırma yapın
2. &#x1F4DD; Pozisyon tanımını detaylı inceleyin
3. &#x1F4AC; Kendinizi tanıtma pratiği yapın
4. &#x1F454; Şık ve profesyonel giyinin

## &#x2753; Sık Sorulan Sorular

### ""Kendinizden bahseder misiniz?""
- &#x23F1; Kısa ve öz olun
- &#x1F393; Eğitim ve deneyimlerinize değinin
- &#x1F3AF; Neden bu pozisyona uygun olduğunuzu vurgulayın

### ""Güçlü yönleriniz neler?""
- &#x1F4AA; Pozisyonla ilgili güçlü yönlerinizi seçin
- &#x1F4A1; Somut örneklerle destekleyin

### ""Zayıf yönleriniz?""
- &#x1F91D; Dürüst olun ama kendinizi kötülemeyin
- &#x1F4C8; Nasıl geliştirmeye çalıştığınızı anlatın

### ""5 yıl sonra kendinizi nerede görüyorsunuz?""
- &#x1F680; Kariyer hedeflerinizden bahsedin
- &#x1F3E2; Şirketle birlikte büyümek istediğinizi belirtin

## &#x1F4E7; Görüşme Sonrası
- &#x1F64F; Teşekkür e-postası gönderin
- &#x23F0; Geri dönüş süresini sorun
- &#x1F9D8; Sabırlı olun",
          ContentCategoryId = 2
      },
       new
       {
           Id = 6,
           Title = "Staj ve İş Bulma Stratejileri",
           Slug = "staj-is-bulma",
           Summary = "İlk stajınızı veya işinizi nasıl bulursunuz? Hangi platformları kullanmalısınız? Networking nasıl yapılır?",
           Content = @"# &#x1F680; Staj ve İş Bulma Stratejileri

## &#x1F310; İş Arama Platformları
1. **LinkedIn** - &#x1F465; Profesyonel networking
2. **Kariyer.net** - &#x1F4BC; İş ilanları
3. **SecretCV** - &#x1F575; Anonim başvuru
4. **GitHub Jobs** - &#x1F4BB; Yazılım pozisyonları
5. **AngelList** - &#x1F680; Startup'lar

## &#x1F91D; Networking İpuçları
- &#x1F4C8; LinkedIn profilinizi güncel tutun
- &#x1F3AA; Sektör etkinliklerine katılın
- &#x1F393; Üniversite mezunları ağınızı kullanın
- &#x1F468;&#x1F3EB; Mentorluk programlarına başvurun

## &#x1F4BC; Staj Başvurusu
- **Ne zaman başvurmalı?** 
  &#x2600; Yazın staj için 3-4 ay önce başlayın
  
- **Başvuru mektubu yazın**
  &#x270D; Neden o şirkette çalışmak istediğinizi açıklayın

- **Portföy hazırlayın**
  &#x1F4BB; GitHub projeleri, kişisel web sitesi

## &#x1F3AF; İlk İş İçin İpuçları
- &#x1F4B0; Maaş beklentinizi araştırın
- &#x1F3E2; Şirket kültürüne dikkat edin
- &#x1F4C8; Gelişim fırsatlarını değerlendirin
- &#x23F3; İlk işinizde 1-2 yıl kalın",
           ContentCategoryId = 2
       }
      );




            builder.Entity<University>().HasData(

           new University { Id = 1, Name = "İstanbul Üniversitesi", City = "İstanbul", IsStateUniversity = true, WebSite = "https://istanbul.edu.tr" },
       new University { Id = 2, Name = "İstanbul Teknik Üniversitesi", City = "İstanbul", IsStateUniversity = true, WebSite = "https://itu.edu.tr" },
     new University { Id = 3, Name = "Boğaziçi Üniversitesi", City = "İstanbul", IsStateUniversity = true, WebSite = "https://boun.edu.tr" },
                new University { Id = 4, Name = "Marmara Üniversitesi", City = "İstanbul", IsStateUniversity = true, WebSite = "https://marmara.edu.tr" },
  new University { Id = 5, Name = "Yıldız Teknik Üniversitesi", City = "İstanbul", IsStateUniversity = true, WebSite = "https://yildiz.edu.tr" },
       new University { Id = 6, Name = "Galatasaray Üniversitesi", City = "İstanbul", IsStateUniversity = true, WebSite = "https://gsu.edu.tr" },


          new University { Id = 7, Name = "Ankara Üniversitesi", City = "Ankara", IsStateUniversity = true, WebSite = "https://ankara.edu.tr" },
        new University { Id = 8, Name = "Orta Doğu Teknik Üniversitesi", City = "Ankara", IsStateUniversity = true, WebSite = "https://metu.edu.tr" },
      new University { Id = 9, Name = "Hacettepe Üniversitesi", City = "Ankara", IsStateUniversity = true, WebSite = "https://hacettepe.edu.tr" },
                new University { Id = 10, Name = "Gazi Üniversitesi", City = "Ankara", IsStateUniversity = true, WebSite = "https://gazi.edu.tr" },
      new University { Id = 11, Name = "Ankara Yıldırım Beyazıt Üniversitesi", City = "Ankara", IsStateUniversity = true, WebSite = "https://ybu.edu.tr" },


             new University { Id = 12, Name = "Ege Üniversitesi", City = "İzmir", IsStateUniversity = true, WebSite = "https://ege.edu.tr" },
    new University { Id = 13, Name = "Dokuz Eylül Üniversitesi", City = "İzmir", IsStateUniversity = true, WebSite = "https://deu.edu.tr" },
 new University { Id = 14, Name = "İzmir Yüksek Teknoloji Enstitüsü", City = "İzmir", IsStateUniversity = true, WebSite = "https://iyte.edu.tr" },
            new University { Id = 15, Name = "İzmir Katip Çelebi Üniversitesi", City = "İzmir", IsStateUniversity = true, WebSite = "https://ikc.edu.tr" },


    new University { Id = 16, Name = "Erciyes Üniversitesi", City = "Kayseri", IsStateUniversity = true, WebSite = "https://erciyes.edu.tr" },
        new University { Id = 17, Name = "Selçuk Üniversitesi", City = "Konya", IsStateUniversity = true, WebSite = "https://selcuk.edu.tr" },
             new University { Id = 18, Name = "Atatürk Üniversitesi", City = "Erzurum", IsStateUniversity = true, WebSite = "https://atauni.edu.tr" },
        new University { Id = 19, Name = "Çukurova Üniversitesi", City = "Adana", IsStateUniversity = true, WebSite = "https://cu.edu.tr" },
     new University { Id = 20, Name = "Akdeniz Üniversitesi", City = "Antalya", IsStateUniversity = true, WebSite = "https://akdeniz.edu.tr" },
new University { Id = 21, Name = "Ondokuz Mayıs Üniversitesi", City = "Samsun", IsStateUniversity = true, WebSite = "https://omu.edu.tr" },
 new University { Id = 22, Name = "Karadeniz Teknik Üniversitesi", City = "Trabzon", IsStateUniversity = true, WebSite = "https://ktu.edu.tr" },
                new University { Id = 23, Name = "Uludağ Üniversitesi", City = "Bursa", IsStateUniversity = true, WebSite = "https://uludag.edu.tr" },
             new University { Id = 24, Name = "Anadolu Üniversitesi", City = "Eskişehir", IsStateUniversity = true, WebSite = "https://anadolu.edu.tr" },
        new University { Id = 25, Name = "Pamukkale Üniversitesi", City = "Denizli", IsStateUniversity = true, WebSite = "https://pau.edu.tr" },
              new University { Id = 26, Name = "Fırat Üniversitesi", City = "Elazığ", IsStateUniversity = true, WebSite = "https://firat.edu.tr" },
           new University { Id = 27, Name = "Süleyman Demirel Üniversitesi", City = "Isparta", IsStateUniversity = true, WebSite = "https://sdu.edu.tr" },
     new University { Id = 28, Name = "Gaziantep Üniversitesi", City = "Gaziantep", IsStateUniversity = true, WebSite = "https://gantep.edu.tr" },
                new University { Id = 29, Name = "Sakarya Üniversitesi", City = "Sakarya", IsStateUniversity = true, WebSite = "https://sakarya.edu.tr" },
            new University { Id = 30, Name = "Kocaeli Üniversitesi", City = "Kocaeli", IsStateUniversity = true, WebSite = "https://kocaeli.edu.tr" },


      new University { Id = 31, Name = "Koç Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://ku.edu.tr" },
          new University { Id = 32, Name = "Sabancı Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://sabanciuniv.edu" },
          new University { Id = 33, Name = "Bahçeşehir Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://bahcesehir.edu.tr" },
         new University { Id = 34, Name = "İstanbul Bilgi Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://bilgi.edu.tr" },
       new University { Id = 35, Name = "Özyeğin Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://ozyegin.edu.tr" },
  new University { Id = 36, Name = "Kadir Has Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://khas.edu.tr" },
         new University { Id = 37, Name = "Yeditepe Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://yeditepe.edu.tr" },
         new University { Id = 38, Name = "İstanbul Ticaret Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://ticaret.edu.tr" },
           new University { Id = 39, Name = "İstanbul Kültür Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://iku.edu.tr" },
   new University { Id = 40, Name = "Işık Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://isikun.edu.tr" },


    new University { Id = 41, Name = "Bilkent Üniversitesi", City = "Ankara", IsStateUniversity = false, WebSite = "https://bilkent.edu.tr" },
    new University { Id = 42, Name = "Atılım Üniversitesi", City = "Ankara", IsStateUniversity = false, WebSite = "https://atilim.edu.tr" },
    new University { Id = 43, Name = "Başkent Üniversitesi", City = "Ankara", IsStateUniversity = false, WebSite = "https://baskent.edu.tr" },
             new University { Id = 44, Name = "Çankaya Üniversitesi", City = "Ankara", IsStateUniversity = false, WebSite = "https://cankaya.edu.tr" },
    new University { Id = 45, Name = "TOBB Ekonomi ve Teknoloji Üniversitesi", City = "Ankara", IsStateUniversity = false, WebSite = "https://etu.edu.tr" },


        new University { Id = 46, Name = "İzmir Ekonomi Üniversitesi", City = "İzmir", IsStateUniversity = false, WebSite = "https://ieu.edu.tr" },
           new University { Id = 47, Name = "Yaşar Üniversitesi", City = "İzmir", IsStateUniversity = false, WebSite = "https://yasar.edu.tr" },


            new University { Id = 48, Name = "Özyeğin Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://ozyegin.edu.tr" },
     new University { Id = 49, Name = "TED Üniversitesi", City = "Ankara", IsStateUniversity = false, WebSite = "https://tedu.edu.tr" },
          new University { Id = 50, Name = "MEF Üniversitesi", City = "İstanbul", IsStateUniversity = false, WebSite = "https://mef.edu.tr" }
            );




            builder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Google",
                    Description = "Global teknoloji lideri.",
                    LogoUrl = "google.png"
                },
                new Company
                {
                    Id = 2,
                    Name = "Microsoft",
                    Description = "Yazılım ve bulut çözümleri.",
                    LogoUrl = "microsoft.png"
                }
            );


            builder.Entity<JobPosting>().HasData(
                new JobPosting
                {
                    Id = 1,
                    Title = "Kıdemli .NET Geliştiricisi",
                    Description = "ASP.NET Core ve Azure konusunda deneyimli...",
                    Location = "İstanbul",
                    PostedDate = DateTime.UtcNow,
                    IsActive = true,
                    CompanyId = 2 // Microsoft
                },
                new JobPosting
                {
                    Id = 2,
                    Title = "Frontend Geliştirici (React)",
                    Description = "React ve TypeScript bilen...",
                    Location = "Ankara",
                    PostedDate = DateTime.UtcNow,
                    IsActive = true,
                    CompanyId = 1 // Google
                },
                new JobPosting
                {
                    Id = 3,
                    Title = "DevOps Mühendisi",
                    Description = "CI/CD süreçlerine hakim...",
                    Location = "İstanbul",
                    PostedDate = DateTime.UtcNow,
                    IsActive = true,
                    CompanyId = 2 // Microsoft
                }


            );


            builder.Entity<Department>().HasData(

    new Department { Id = 1, Name = "Hukuk", ScoreType = "EA", LastYearBaseScore = 520.5, LastYearBaseRanking = 1850, UniversityId = 1 },
    new Department { Id = 2, Name = "Tıp", ScoreType = "SAY", LastYearBaseScore = 545.2, LastYearBaseRanking = 850, UniversityId = 1 },
    new Department { Id = 3, Name = "İşletme", ScoreType = "EA", LastYearBaseScore = 495.3, LastYearBaseRanking = 3200, UniversityId = 1 },
    new Department { Id = 4, Name = "Psikoloji", ScoreType = "EA", LastYearBaseScore = 485.7, LastYearBaseRanking = 4500, UniversityId = 1 },
    new Department { Id = 5, Name = "İktisat", ScoreType = "EA", LastYearBaseScore = 488.5, LastYearBaseRanking = 4200, UniversityId = 1 },
    new Department { Id = 6, Name = "Siyaset Bilimi", ScoreType = "EA", LastYearBaseScore = 482.3, LastYearBaseRanking = 4650, UniversityId = 1 },
    new Department { Id = 7, Name = "Tarih", ScoreType = "SOZ", LastYearBaseScore = 475.8, LastYearBaseRanking = 5100, UniversityId = 1 },
    new Department { Id = 8, Name = "Türk Dili ve Edebiyatı", ScoreType = "SOZ", LastYearBaseScore = 472.5, LastYearBaseRanking = 5350, UniversityId = 1 },
    new Department { Id = 9, Name = "Felsefe", ScoreType = "SOZ", LastYearBaseScore = 468.9, LastYearBaseRanking = 5650, UniversityId = 1 },
    new Department { Id = 10, Name = "Sosyoloji", ScoreType = "EA", LastYearBaseScore = 478.5, LastYearBaseRanking = 4850, UniversityId = 1 },
    new Department { Id = 11, Name = "Matematik", ScoreType = "SAY", LastYearBaseScore = 485.2, LastYearBaseRanking = 4450, UniversityId = 1 },
    new Department { Id = 12, Name = "Fizik", ScoreType = "SAY", LastYearBaseScore = 478.7, LastYearBaseRanking = 4850, UniversityId = 1 },
    new Department { Id = 13, Name = "Kimya", ScoreType = "SAY", LastYearBaseScore = 475.5, LastYearBaseRanking = 5050, UniversityId = 1 },
    new Department { Id = 14, Name = "Biyoloji", ScoreType = "SAY", LastYearBaseScore = 482.8, LastYearBaseRanking = 4550, UniversityId = 1 },
    new Department { Id = 15, Name = "İstatistik", ScoreType = "SAY", LastYearBaseScore = 472.5, LastYearBaseRanking = 5350, UniversityId = 1 },


    new Department { Id = 16, Name = "Bilgisayar Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 525.5, LastYearBaseRanking = 1250, UniversityId = 2 },
    new Department { Id = 17, Name = "Elektrik-Elektronik Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 520.3, LastYearBaseRanking = 1580, UniversityId = 2 },
    new Department { Id = 18, Name = "Makine Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 515.8, LastYearBaseRanking = 2100, UniversityId = 2 },
    new Department { Id = 19, Name = "İnşaat Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 512.4, LastYearBaseRanking = 2450, UniversityId = 2 },
    new Department { Id = 20, Name = "Endüstri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 518.7, LastYearBaseRanking = 1850, UniversityId = 2 },
    new Department { Id = 21, Name = "Kimya Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 505.8, LastYearBaseRanking = 2850, UniversityId = 2 },
    new Department { Id = 22, Name = "Makine Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 508.5, LastYearBaseRanking = 2650, UniversityId = 2 },
    new Department { Id = 23, Name = "Metalurji ve Malzeme Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 498.7, LastYearBaseRanking = 3450, UniversityId = 2 },
    new Department { Id = 24, Name = "Gıda Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 495.5, LastYearBaseRanking = 3650, UniversityId = 2 },
    new Department { Id = 25, Name = "Tekstil Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 488.8, LastYearBaseRanking = 4050, UniversityId = 2 },
    new Department { Id = 26, Name = "Çevre Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 492.5, LastYearBaseRanking = 3850, UniversityId = 2 },
    new Department { Id = 27, Name = "Jeoloji Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 485.7, LastYearBaseRanking = 4350, UniversityId = 2 },
    new Department { Id = 28, Name = "Maden Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 482.5, LastYearBaseRanking = 4550, UniversityId = 2 },
    new Department { Id = 29, Name = "Petrol ve Doğal Gaz Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 495.8, LastYearBaseRanking = 3650, UniversityId = 2 },
    new Department { Id = 30, Name = "Uçak Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 512.5, LastYearBaseRanking = 2450, UniversityId = 2 },
    new Department { Id = 31, Name = "Gemi İnşaatı ve Gemi Makineleri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 502.8, LastYearBaseRanking = 3150, UniversityId = 2 },
    new Department { Id = 32, Name = "Harita Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 488.5, LastYearBaseRanking = 4050, UniversityId = 2 },
    new Department { Id = 33, Name = "Jeofizik Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 485.3, LastYearBaseRanking = 4350, UniversityId = 2 },
    new Department { Id = 34, Name = "Matematik Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 505.7, LastYearBaseRanking = 2950, UniversityId = 2 },
    new Department { Id = 35, Name = "Mimarlık", ScoreType = "SAY", LastYearBaseScore = 508.8, LastYearBaseRanking = 2750, UniversityId = 2 },


    new Department { Id = 36, Name = "Bilgisayar Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 530.2, LastYearBaseRanking = 980, UniversityId = 3 },
    new Department { Id = 37, Name = "Elektrik-Elektronik Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 525.8, LastYearBaseRanking = 1280, UniversityId = 3 },
    new Department { Id = 38, Name = "Endüstri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 522.5, LastYearBaseRanking = 1450, UniversityId = 3 },
    new Department { Id = 39, Name = "Makine Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 518.7, LastYearBaseRanking = 1850, UniversityId = 3 },
    new Department { Id = 40, Name = "İnşaat Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 515.5, LastYearBaseRanking = 2050, UniversityId = 3 },
    new Department { Id = 41, Name = "İşletme", ScoreType = "EA", LastYearBaseScore = 495.5, LastYearBaseRanking = 3200, UniversityId = 3 },
    new Department { Id = 42, Name = "Ekonomi", ScoreType = "EA", LastYearBaseScore = 490.8, LastYearBaseRanking = 3800, UniversityId = 3 },
    new Department { Id = 43, Name = "Uluslararası İlişkiler", ScoreType = "EA", LastYearBaseScore = 492.5, LastYearBaseRanking = 3550, UniversityId = 3 },
    new Department { Id = 44, Name = "Siyaset Bilimi ve Uluslararası İlişkiler", ScoreType = "EA", LastYearBaseScore = 488.8, LastYearBaseRanking = 3950, UniversityId = 3 },
    new Department { Id = 45, Name = "Psikoloji", ScoreType = "EA", LastYearBaseScore = 485.3, LastYearBaseRanking = 4500, UniversityId = 3 },
    new Department { Id = 46, Name = "Sosyoloji", ScoreType = "EA", LastYearBaseScore = 478.7, LastYearBaseRanking = 4850, UniversityId = 3 },
    new Department { Id = 47, Name = "Tarih", ScoreType = "SOZ", LastYearBaseScore = 475.5, LastYearBaseRanking = 5050, UniversityId = 3 },
    new Department { Id = 48, Name = "Felsefe", ScoreType = "SOZ", LastYearBaseScore = 472.8, LastYearBaseRanking = 5350, UniversityId = 3 },
    new Department { Id = 49, Name = "Matematik", ScoreType = "SAY", LastYearBaseScore = 505.8, LastYearBaseRanking = 2950, UniversityId = 3 },
    new Department { Id = 50, Name = "Fizik", ScoreType = "SAY", LastYearBaseScore = 502.5, LastYearBaseRanking = 3150, UniversityId = 3 },
    new Department { Id = 51, Name = "Kimya", ScoreType = "SAY", LastYearBaseScore = 498.7, LastYearBaseRanking = 3450, UniversityId = 3 },
    new Department { Id = 52, Name = "Moleküler Biyoloji ve Genetik", ScoreType = "SAY", LastYearBaseScore = 508.5, LastYearBaseRanking = 2750, UniversityId = 3 },
    new Department { Id = 53, Name = "Çeviribilim", ScoreType = "SOZ", LastYearBaseScore = 478.5, LastYearBaseRanking = 4850, UniversityId = 3 },


    new Department { Id = 54, Name = "Hukuk", ScoreType = "EA", LastYearBaseScore = 510.5, LastYearBaseRanking = 2350, UniversityId = 4 },
    new Department { Id = 55, Name = "Tıp", ScoreType = "SAY", LastYearBaseScore = 535.8, LastYearBaseRanking = 1150, UniversityId = 4 },
    new Department { Id = 56, Name = "Diş Hekimliği", ScoreType = "SAY", LastYearBaseScore = 518.7, LastYearBaseRanking = 1950, UniversityId = 4 },
    new Department { Id = 57, Name = "Eczacılık", ScoreType = "SAY", LastYearBaseScore = 512.5, LastYearBaseRanking = 2450, UniversityId = 4 },
    new Department { Id = 58, Name = "İşletme", ScoreType = "EA", LastYearBaseScore = 485.2, LastYearBaseRanking = 4200, UniversityId = 4 },
    new Department { Id = 59, Name = "İktisat", ScoreType = "EA", LastYearBaseScore = 475.6, LastYearBaseRanking = 5100, UniversityId = 4 },
    new Department { Id = 60, Name = "Uluslararası İlişkiler", ScoreType = "EA", LastYearBaseScore = 482.8, LastYearBaseRanking = 4550, UniversityId = 4 },
    new Department { Id = 61, Name = "Siyaset Bilimi ve Kamu Yönetimi", ScoreType = "EA", LastYearBaseScore = 478.5, LastYearBaseRanking = 4850, UniversityId = 4 },
    new Department { Id = 62, Name = "İletişim", ScoreType = "EA", LastYearBaseScore = 472.5, LastYearBaseRanking = 5350, UniversityId = 4 },
    new Department { Id = 63, Name = "Radyo, Televizyon ve Sinema", ScoreType = "EA", LastYearBaseScore = 475.8, LastYearBaseRanking = 5050, UniversityId = 4 },
    new Department { Id = 64, Name = "Hemşirelik", ScoreType = "SAY", LastYearBaseScore = 485.5, LastYearBaseRanking = 4350, UniversityId = 4 },
    new Department { Id = 65, Name = "Fizyoterapi ve Rehabilitasyon", ScoreType = "SAY", LastYearBaseScore = 488.7, LastYearBaseRanking = 4050, UniversityId = 4 },
    new Department { Id = 66, Name = "Beslenme ve Diyetetik", ScoreType = "SAY", LastYearBaseScore = 482.5, LastYearBaseRanking = 4550, UniversityId = 4 },
    new Department { Id = 67, Name = "Bankacılık ve Finans", ScoreType = "EA", LastYearBaseScore = 468.8, LastYearBaseRanking = 5750, UniversityId = 4 },
    new Department { Id = 68, Name = "Turizm İşletmeciliği", ScoreType = "EA", LastYearBaseScore = 465.5, LastYearBaseRanking = 6050, UniversityId = 4 },
    new Department { Id = 69, Name = "Sosyal Hizmet", ScoreType = "EA", LastYearBaseScore = 472.8, LastYearBaseRanking = 5350, UniversityId = 4 },


    new Department { Id = 70, Name = "Bilgisayar Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 518.4, LastYearBaseRanking = 1750, UniversityId = 5 },
    new Department { Id = 71, Name = "Elektrik-Elektronik Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 512.8, LastYearBaseRanking = 2450, UniversityId = 5 },
    new Department { Id = 72, Name = "İnşaat Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 505.2, LastYearBaseRanking = 2850, UniversityId = 5 },
    new Department { Id = 73, Name = "Makine Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 508.7, LastYearBaseRanking = 2650, UniversityId = 5 },
    new Department { Id = 74, Name = "Endüstri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 510.5, LastYearBaseRanking = 2550, UniversityId = 5 },
    new Department { Id = 75, Name = "Kimya Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 498.8, LastYearBaseRanking = 3450, UniversityId = 5 },
    new Department { Id = 76, Name = "Gıda Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 492.5, LastYearBaseRanking = 3850, UniversityId = 5 },
    new Department { Id = 77, Name = "Harita Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 485.7, LastYearBaseRanking = 4350, UniversityId = 5 },
    new Department { Id = 78, Name = "Jeodezi ve Fotogrametri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 482.5, LastYearBaseRanking = 4550, UniversityId = 5 },
    new Department { Id = 79, Name = "Çevre Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 488.8, LastYearBaseRanking = 4050, UniversityId = 5 },
    new Department { Id = 80, Name = "Matematik", ScoreType = "SAY", LastYearBaseScore = 478.5, LastYearBaseRanking = 4850, UniversityId = 5 },
    new Department { Id = 81, Name = "Fizik", ScoreType = "SAY", LastYearBaseScore = 475.8, LastYearBaseRanking = 5050, UniversityId = 5 },
    new Department { Id = 82, Name = "Kimya", ScoreType = "SAY", LastYearBaseScore = 472.5, LastYearBaseRanking = 5350, UniversityId = 5 },
    new Department { Id = 83, Name = "Mimarlık", ScoreType = "SAY", LastYearBaseScore = 498.5, LastYearBaseRanking = 3450, UniversityId = 5 },
    new Department { Id = 84, Name = "Şehir ve Bölge Planlama", ScoreType = "SAY", LastYearBaseScore = 492.7, LastYearBaseRanking = 3850, UniversityId = 5 },


    new Department { Id = 85, Name = "Hukuk", ScoreType = "EA", LastYearBaseScore = 515.3, LastYearBaseRanking = 2150, UniversityId = 6 },
    new Department { Id = 86, Name = "İşletme", ScoreType = "EA", LastYearBaseScore = 492.5, LastYearBaseRanking = 3450, UniversityId = 6 },
    new Department { Id = 87, Name = "İktisat", ScoreType = "EA", LastYearBaseScore = 488.7, LastYearBaseRanking = 3950, UniversityId = 6 },
    new Department { Id = 88, Name = "Uluslararası İlişkiler", ScoreType = "EA", LastYearBaseScore = 488.9, LastYearBaseRanking = 3900, UniversityId = 6 },
    new Department { Id = 89, Name = "Siyaset Bilimi", ScoreType = "EA", LastYearBaseScore = 485.5, LastYearBaseRanking = 4250, UniversityId = 6 },
    new Department { Id = 90, Name = "Sosyoloji", ScoreType = "EA", LastYearBaseScore = 478.8, LastYearBaseRanking = 4750, UniversityId = 6 },
    new Department { Id = 91, Name = "Matematik", ScoreType = "SAY", LastYearBaseScore = 482.5, LastYearBaseRanking = 4550, UniversityId = 6 },
    new Department { Id = 92, Name = "Bilgisayar ve Bilişim Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 505.8, LastYearBaseRanking = 2950, UniversityId = 6 },
    new Department { Id = 93, Name = "Endüstri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 502.5, LastYearBaseRanking = 3150, UniversityId = 6 },
    new Department { Id = 94, Name = "İletişim", ScoreType = "EA", LastYearBaseScore = 475.8, LastYearBaseRanking = 5050, UniversityId = 6 },
    new Department { Id = 95, Name = "Felsefe", ScoreType = "SOZ", LastYearBaseScore = 472.5, LastYearBaseRanking = 5350, UniversityId = 6 },
    new Department { Id = 96, Name = "Tarih", ScoreType = "SOZ", LastYearBaseScore = 468.9, LastYearBaseRanking = 5650, UniversityId = 6 },


    new Department { Id = 97, Name = "Hukuk", ScoreType = "EA", LastYearBaseScore = 522.8, LastYearBaseRanking = 1650, UniversityId = 7 },
    new Department { Id = 98, Name = "Tıp", ScoreType = "SAY", LastYearBaseScore = 542.3, LastYearBaseRanking = 920, UniversityId = 7 },
    new Department { Id = 99, Name = "Diş Hekimliği", ScoreType = "SAY", LastYearBaseScore = 522.5, LastYearBaseRanking = 1650, UniversityId = 7 },
    new Department { Id = 100, Name = "Eczacılık", ScoreType = "SAY", LastYearBaseScore = 510.2, LastYearBaseRanking = 3200, UniversityId = 7 },
    new Department { Id = 101, Name = "Veterinerlik", ScoreType = "SAY", LastYearBaseScore = 495.7, LastYearBaseRanking = 4850, UniversityId = 7 },
    new Department { Id = 102, Name = "Siyasal Bilgiler", ScoreType = "EA", LastYearBaseScore = 505.8, LastYearBaseRanking = 2850, UniversityId = 7 },
    new Department { Id = 103, Name = "İletişim", ScoreType = "EA", LastYearBaseScore = 488.5, LastYearBaseRanking = 3950, UniversityId = 7 },
    new Department { Id = 104, Name = "Dil ve Tarih-Coğrafya Fakültesi - Tarih", ScoreType = "SOZ", LastYearBaseScore = 482.8, LastYearBaseRanking = 4550, UniversityId = 7 },
    new Department { Id = 105, Name = "Türk Dili ve Edebiyatı", ScoreType = "SOZ", LastYearBaseScore = 478.5, LastYearBaseRanking = 4850, UniversityId = 7 },
    new Department { Id = 106, Name = "Arkeoloji", ScoreType = "SOZ", LastYearBaseScore = 475.7, LastYearBaseRanking = 5050, UniversityId = 7 },
    new Department { Id = 107, Name = "Coğrafya", ScoreType = "SOZ", LastYearBaseScore = 472.5, LastYearBaseRanking = 5350, UniversityId = 7 },
    new Department { Id = 108, Name = "Felsefe", ScoreType = "SOZ", LastYearBaseScore = 468.8, LastYearBaseRanking = 5650, UniversityId = 7 },
    new Department { Id = 109, Name = "Psikoloji", ScoreType = "EA", LastYearBaseScore = 492.5, LastYearBaseRanking = 3550, UniversityId = 7 },
    new Department { Id = 110, Name = "Sosyoloji", ScoreType = "EA", LastYearBaseScore = 485.8, LastYearBaseRanking = 4150, UniversityId = 7 },
    new Department { Id = 111, Name = "Antropoloji", ScoreType = "EA", LastYearBaseScore = 478.7, LastYearBaseRanking = 4750, UniversityId = 7 },
    new Department { Id = 112, Name = "Hemşirelik", ScoreType = "SAY", LastYearBaseScore = 488.5, LastYearBaseRanking = 4050, UniversityId = 7 },
    new Department { Id = 113, Name = "Ebelik", ScoreType = "SAY", LastYearBaseScore = 482.7, LastYearBaseRanking = 4550, UniversityId = 7 },
    new Department { Id = 114, Name = "Fizik Tedavi ve Rehabilitasyon", ScoreType = "SAY", LastYearBaseScore = 492.8, LastYearBaseRanking = 3850, UniversityId = 7 },
    new Department { Id = 115, Name = "Beslenme ve Diyetetik", ScoreType = "SAY", LastYearBaseScore = 485.5, LastYearBaseRanking = 4350, UniversityId = 7 },
    new Department { Id = 116, Name = "Ziraat Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 475.8, LastYearBaseRanking = 5050, UniversityId = 7 },


    new Department { Id = 117, Name = "Bilgisayar Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 528.7, LastYearBaseRanking = 1100, UniversityId = 8 },
    new Department { Id = 118, Name = "Elektrik-Elektronik Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 522.5, LastYearBaseRanking = 1450, UniversityId = 8 },
    new Department { Id = 119, Name = "Makine Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 515.3, LastYearBaseRanking = 2150, UniversityId = 8 },
    new Department { Id = 120, Name = "Endüstri Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 518.2, LastYearBaseRanking = 1850, UniversityId = 8 },
    new Department { Id = 121, Name = "İnşaat Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 512.5, LastYearBaseRanking = 2450, UniversityId = 8 },
    new Department { Id = 122, Name = "Kimya Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 508.8, LastYearBaseRanking = 2750, UniversityId = 8 },
    new Department { Id = 123, Name = "Metalurji ve Malzeme Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 505.7, LastYearBaseRanking = 2950, UniversityId = 8 },
    new Department { Id = 124, Name = "Maden Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 498.5, LastYearBaseRanking = 3450, UniversityId = 8 },
    new Department { Id = 125, Name = "Jeoloji Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 495.8, LastYearBaseRanking = 3650, UniversityId = 8 },
    new Department { Id = 126, Name = "Petrol ve Doğal Gaz Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 505.5, LastYearBaseRanking = 2950, UniversityId = 8 },
    new Department { Id = 127, Name = "Çevre Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 502.8, LastYearBaseRanking = 3150, UniversityId = 8 },
    new Department { Id = 128, Name = "Havacılık ve Uzay Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 515.8, LastYearBaseRanking = 2050, UniversityId = 8 },
    new Department { Id = 129, Name = "Gıda Mühendisliği", ScoreType = "SAY", LastYearBaseScore = 498.7, LastYearBaseRanking = 3450, UniversityId = 8 },
    new Department { Id = 130, Name = "Fizik", ScoreType = "SAY", LastYearBaseScore = 508.5, LastYearBaseRanking = 2750, UniversityId = 8 },
    new Department { Id = 131, Name = "Kimya", ScoreType = "SAY", LastYearBaseScore = 505.8, LastYearBaseRanking = 2950, UniversityId = 8 },
    new Department { Id = 132, Name = "Matematik", ScoreType = "SAY", LastYearBaseScore = 510.5, LastYearBaseRanking = 2550, UniversityId = 8 },
    new Department { Id = 133, Name = "İstatistik", ScoreType = "SAY", LastYearBaseScore = 498.8, LastYearBaseRanking = 3450, UniversityId = 8 },
    new Department { Id = 134, Name = "Biyoloji", ScoreType = "SAY", LastYearBaseScore = 502.5, LastYearBaseRanking = 3150, UniversityId = 8 },
    new Department { Id = 135, Name = "Moleküler Biyoloji ve Genetik", ScoreType = "SAY", LastYearBaseScore = 508.7, LastYearBaseRanking = 2750, UniversityId = 8 },
    new Department { Id = 136, Name = "Mimarlık", ScoreType = "SAY", LastYearBaseScore = 512.8, LastYearBaseRanking = 2450, UniversityId = 8 },
    new Department { Id = 137, Name = "Şehir ve Bölge Planlama", ScoreType = "SAY", LastYearBaseScore = 505.5, LastYearBaseRanking = 2950, UniversityId = 8 },
    new Department { Id = 138, Name = "İşletme", ScoreType = "EA", LastYearBaseScore = 505.8, LastYearBaseRanking = 2850, UniversityId = 8 },
    new Department { Id = 139, Name = "Ekonomi", ScoreType = "EA", LastYearBaseScore = 502.5, LastYearBaseRanking = 3050, UniversityId = 8 },
    new Department { Id = 140, Name = "Uluslararası İlişkiler", ScoreType = "EA", LastYearBaseScore = 498.8, LastYearBaseRanking = 3350, UniversityId = 8 },
  new Department { Id = 141, Name = "Psikoloji", ScoreType = "EA", LastYearBaseScore = 495.7, LastYearBaseRanking = 3550, UniversityId = 8 }
    );
     }
    }
}