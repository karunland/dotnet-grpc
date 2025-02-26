# File Upload Demo with gRPC and .NET

Bu proje, gRPC kullanarak dosya yükleme ve indirme işlemlerini gerçekleştiren bir demo uygulamasıdır. Proje, modern bir mikroservis mimarisi kullanılarak geliştirilmiştir.

## Proje Yapısı

Proje üç ana bileşenden oluşmaktadır:

- **rpc.api**: REST API endpoints ve client tarafı işlemleri
- **rpc.server**: gRPC sunucu implementasyonu ve dosya işlemleri
- **rpc.sdk**: Protokol tanımlamaları ve shared kod

## Özellikler

- Dosya yükleme ve indirme işlemleri
- Çeşitli dosya tiplerini destekleme (resim, video, ses, döküman vb.)
- Güvenli dosya işlemleri
- Base64 encoding/decoding
- Dosya boyutu limiti (10MB)
- Sıkıştırma desteği

## API Endpoints

### Dosya Yükleme
```http
POST /upload
Content-Type: multipart/form-data
```

Parametreler:
- `file`: Yüklenecek dosya
- `fileType`: Dosya tipi (Image, Video, Audio, Document)

### Dosya İndirme
```http
GET /download/{fileName}
```

## Desteklenen Dosya Tipleri

- Text dosyaları (.txt)
- PDF dosyaları (.pdf)
- Microsoft Office dosyaları (.doc, .docx, .xls, .xlsx)
- Resim dosyaları (.png, .jpg, .jpeg, .gif)
- CSV dosyaları (.csv)

## Kurulum

1. Projeyi klonlayın
2. Gerekli NuGet paketlerini yükleyin
3. Projeyi derleyin

## Çalıştırma

1. Önce gRPC sunucusunu başlatın:
```bash
cd rpc.server
dotnet run
```

2. API'yi başlatın:
```bash
cd rpc.api
dotnet run
```

## Güvenlik Özellikleri

- Path traversal koruması
- Dosya tipi doğrulama
- Antiforgery koruması (upload endpoint'i hariç)
- Yetkilendirme ve kimlik doğrulama altyapısı

## Notlar

- Yüklenen dosyalar `uploads` klasöründe saklanır
- Maksimum dosya boyutu 10MB ile sınırlandırılmıştır
- Tüm dosya işlemleri asenkron olarak gerçekleştirilir

## Teknik Detaylar

- .NET 8.0
- gRPC
- Protocol Buffers
- REST API
- Swagger UI desteği 