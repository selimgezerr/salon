# ProRandevu

ProRandevu kuaför ve güzellik salonları için hazırlanmış bir randevu ve mücteri takip uygulamasıdır. Proje .NET 7 ve WPF kullanarak geliştirilmiştir ve SQLite veritabanı kullanır.

## Kurulum
1. .NET 7 SDK yükleyin.
2. Depoyu klonlayın ve bağımlılıkları yüklemek için `dotnet restore` çalıştırın.
3. Veritabanını oluşturmak ve demo verileri eklemek için `dotnet run --project ProRandevu.Seeder` komutunu çalıştırın.
4. Uygulamayı derlemek için `dotnet build` kullanabilirsiniz. Windows ortamında WPF uygulamasını çalıştırmak için `dotnet run --project ProRandevu.App` komutunu çalıştırın.

## Kullanım
Program açıldığında veritabanı boş ise otomatik olarak Örnek veriler eklenir. Sol menüden mücteri, randevu, hizmet, personel ve diğer modüllere geçiş yapabilirsiniz. Ayarlar ekranından tema ve yedekleme yolunu düzenleyebilirsiniz. Lisans anahtarı girilmediyse uygulama uyarı mesajı gösterir ancak temel fonksiyonlar çalışmaya devam eder.
