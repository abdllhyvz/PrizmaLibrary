# PrizmaLibrary

Projeyi çalıştırmadan önce LibraryManagementService(backend tarafı) altındaki appsettings.json dosyasında bulunan connectionstring e kendi veritabanı kullanıcı adınızı ve şifrenizi yazmayı unutmayınız.

Herhangi bir migration yapmanıza gerek yoktur projeyi çalıştırdığınızda otomatik migration yapıp veritabanını oluşturacaktır.

Ayrıca proje çalıştıktan sonra otomatik olarak 1 admin ve 1 kütüphane görevlisi hesabı ve gerekli roller eklenecektir.

Admin hesabı kullanıcı adı: admin@prizma.com şifre: Admin.1234
Görevli hesabı kullanıcı adı: officer@prizma.com şifre: Officer.1234
Roller: Admin, Officer ve User.
