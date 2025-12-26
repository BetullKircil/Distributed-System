# E-Commerce Distributed System Implementation
## Proje Amacı ve Kapsamı (Project Goal)
Bu proje, Mikroservis Mimarisi (Microservices Architecture), Dağıtık Sistemler (Distributed Systems) ve DevOps süreçleri konusundaki yetkinliklerimi geliştirmek amacıyla geliştirdiğim örnek bir E-Ticaret backend projesidir.

Projenin temel amacı ticari bir ürün ortaya koymak değil; modern yazılım mimarilerinde karşılaşılan veri tutarlılığı, servisler arası iletişim, performans optimizasyonu ve konteynerizasyon gibi teknik zorlukları pratik ederek çözmektir.

## Mimari Tasarım ve Akış (Architecture)
Sistem, monolitik bir yapıyı parçalamak yerine, "Database-per-Service" deseniyle, her biri kendi sorumluluğuna sahip izole servislerden oluşacak şekilde tasarlanmıştır.

Aşağıdaki diyagramda, bir ürün verisinin nasıl işlendiği, Redis (Cache) ve PostgreSQL (Database) arasındaki karar mekanizması (Decision Flow) ve servis içi mantık detaylandırılmıştır.

<img width="1280" height="413" alt="image" src="https://github.com/user-attachments/assets/c9124bd9-f1e1-49f0-8b6b-c80d061c4c3f" />

## Servis İletişimi ve Desenler (Communication Patterns)
Bu projede iki farklı iletişim yöntemi hibrit olarak kullanılmıştır:
### 1. Senkron İletişim (Synchronous - HTTP/REST)
Kullanıcının anında cevap beklediği durumlarda (Örn: Ürün listesini getirme) kullanılır.

Örnek: Product Service üzerinde uygulanan Cache-Aside Pattern.

İstek gelir -> Redis kontrol edilir.

Veri varsa (Hit) -> Döner.

Veri yoksa (Miss) -> DB'den çekilir, Redis'e yazılır ve döner.

### 2. Asenkron İletişim (Asynchronous - Event-Driven)
Bir işlemin sonucunun diğer servisleri tetiklediği ancak kullanıcının bekletilmemesi gereken durumlarda kullanılır.

Örnek: Sipariş oluşturulduğunda (OrderCreatedEvent), RabbitMQ kuyruğuna bir mesaj bırakılır. Notification Service bu mesajı dinler (Consumer) ve kullanıcıya bildirim gönderir. Bu sayede Sipariş servisi çöken bir bildirim servisi yüzünden hata vermez.

## Altyapı ve Orkestrasyon (Infrastructure)
Proje, Infrastructure as Code (IaC) prensibiyle tasarlanmıştır. Tüm veritabanları, kuyruk yapıları ve API'lar Docker konteynerleri olarak çalışır.

<img width="1216" height="208" alt="image" src="https://github.com/user-attachments/assets/ed18a806-d081-451c-a14f-3c1032c61253" />


<img width="1202" height="299" alt="image" src="https://github.com/user-attachments/assets/0d1952a7-5918-4fe1-b65e-9b9ea6d3b809" />



Aşağıda görüldüğü üzere, her servisin veritabanı birbirinden ayrılmıştır (Isolation), bu da bir servisteki veritabanı hatasının diğerlerini etkilemesini engeller.

## Proje Yapısı
Kod tabanı, sürdürülebilirlik ve temiz kod (Clean Code) prensipleri gözetilerek src (kaynak kod) ve deployment (altyapı) olarak ayrılmıştır.

<img width="772" height="419" alt="image" src="https://github.com/user-attachments/assets/570029a9-b525-4ec2-bb4c-2dab38d3e1c2" />



Docker compose ile tüm containerlar için orkestrasyon yapabiliyoruz. Aşağıdaki görselde tüm servisler ayağa kaldırılıyor. 
<img width="1280" height="1150" alt="image" src="https://github.com/user-attachments/assets/39d4efa8-b103-492c-b3c6-56a56dcbd75f" />








