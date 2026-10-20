# Mergen-GCSS

Model uydu için yer istasyonu yazılımı (Ground Control Station Software). Uydudaki Raspberry Pi ile Wi-Fi üzerinden haberleşir: uydudan gelen telemetriyi TCP ile alır; grafiklerde, tabloda, haritada ve 3B modelde gösterir. Uyduya komut gönderir, uydunun kamera yayınını izler ve kaydeder.

C# ve Windows Forms (.NET Framework 4.7.2) ile yazılmıştır.

![Ana pencere](docs/arayuz.png)
*Ana pencere. Görüntüdeki değerler örnek veridir.*

## Özellikler

- **Giriş ve açılış ekranı:** kullanıcı adı ve şifreyle giriş, ardından açılış ekranı.
- **Telemetri alma (TCP sunucusu):**
  - Wi-Fi bölümündeki *Başlat* düğmesiyle sunucu açılır; uydu bağlanınca paketler sürekli okunur.
  - Her paket ikili (binary) biçimde gelir: paket numarası ve uydu statüsü, hata kodu, gönderme saati, 14 ölçüm değeri, RHRH, IoT verisi ve takım numarası.
- **Grafikler:** basınç 1–2, yükseklik 1–2, irtifa farkı, iniş hızı, sıcaklık, pil gerilimi ve IoT verisi için 9 zaman grafiği.
- **Tablo ve kayıt:** her paket tabloya eklenir (en yeni üstte). Tablo her 10 saniyede masaüstündeki `data.csv` dosyasına kaydedilir.
- **Harita:** GMap.NET ile uydunun GPS konumu haritada gösterilir.
- **3B simülasyon:** OpenTK ile uydu modeli gelen pitch / roll / yaw açılarıyla döner.
- **ARAS (arıza tespit) göstergesi:** hata kodunun 5 biti; hatalı bit kırmızı yanar ve sesli uyarı verilir.
- **Komutlar (uyduya TCP ile):**
  - **Ayrılma:** *Kilitle* ve *Ayır* düğmeleri ayrılma mekanizmasının servo açısını gönderir.
  - **Mekanik filtreleme:** girilen komut doğrulanıp gönderilir.
- **Kamera:** uydudaki kameranın MJPEG yayını izlenir ve masaüstüne MP4 olarak kaydedilir.

## Dosyalar

| Dosya | İçerik |
|---|---|
| `mainForm.cs` | Ana pencere, kontrol düğmeleri, harita, ARAS |
| `TcpServer.cs` | Telemetri sunucusu, paket okuma |
| `GondericiServer.cs` | Uyduya komut gönderme |
| `DataGridViewHandler.cs` | Tablo ve CSV kaydı |
| `ChartHandler.cs` | Grafikler |
| `_3dSim.cs` | 3B uydu modeli |
| `Camera.cs` | MJPEG kamera yayını ve kaydı |
| `loginForm.cs`, `splash.cs` | Giriş ve açılış ekranları |

## Kurulum ve çalıştırma

**Gerekenler:**
- Windows 10 / 11
- Visual Studio 2019 ya da 2022, ".NET masaüstü geliştirme" iş yüküyle (.NET Framework 4.7.2 dahil)
- Harita için internet bağlantısı

**Adımlar:**
1. Depoyu indirin:
   ```
   git clone https://github.com/mergenmodeluydu/Mergen-GCSS.git
   ```
2. `Mergen GCSS.sln` dosyasını Visual Studio ile açın.
3. NuGet paketlerini geri yükleyin: Solution'a sağ tık → *Restore NuGet Packages*.
4. Derleyip çalıştırın (F5) ve giriş yapın.
5. Uydu ile yer istasyonu aynı ağdayken Wi-Fi bölümünde *Başlat*'a basın.

Sunucu, komut ve kamera IP adresleri `mainForm.cs` ve `Camera.cs` içinde tanımlıdır; kendi ağınıza göre değiştirin.

## Kullanılan kütüphaneler

GMap.NET (harita), OpenTK (3B), AForge / Accord (kamera ve kayıt), System.Windows.Forms.DataVisualization (grafikler), MetroFramework ve Guna.UI2 (arayüz).
