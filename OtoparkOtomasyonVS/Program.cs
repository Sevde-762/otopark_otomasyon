using System;
using MySql.Data.MySqlClient;

namespace OtoparkBackend
{
    class Program
    {
        // veritabani baglanti bilgileri
        private static string connectionString = "Server=localhost;Port=3306;Database=mydb;Uid=root;Pwd=Sevde.762;";

        static void Main(string[] args)
        {
            bool devamEt = true;

            while (devamEt)
            {
                Console.Clear();
                Console.WriteLine("========================================"); // giriş ekranı tasarımı
                Console.WriteLine("   OTOPARK OTOMASYON SİSTEMİ   ");
                Console.WriteLine("========================================");
                Console.WriteLine("[1] Musteri Kayit");
                Console.WriteLine("[2] Arac Kayit (Plaka Kontrollü)");
                Console.WriteLine("[3] Arac Giris");
                Console.WriteLine("[4] Arac Cikis ve Odeme");
                Console.WriteLine("[5] Otopark Doluluk Durumu");
                Console.WriteLine("[6] Musteri Arac Listesi");
                Console.WriteLine("[7] Plakaya Gore Gecmis Sorgula");
                Console.WriteLine("[0] Cikis");
                Console.WriteLine("========================================");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();

                switch (secim)
                {
                    case "1": MusteriKayit(); break;
                    case "2": AracKayit(); break;
                    case "3": AracGiris(); break;
                    case "4": AracCikisVeOdeme(); break;
                    case "5": OtoparkDoluluk(); break;
                    case "6": MusteriAracListesi(); break;
                    case "7": PlakaSorgula(); break;
                    case "0":
                        devamEt = false;
                        Console.WriteLine("Çıkış yapılıyor... İYİ GÜNLER DİLERİZ");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyiniz.");
                        Bekle();
                        break;
                }
            }
        }

        // 1 numara seçilirse müşteri kaydı yapılır 
        static void MusteriKayit()
        {
            Console.Clear();
            Console.WriteLine("--- [1] MÜŞERİ KAYIT ---");

            Console.Write("Ad: ");
            string ad = Console.ReadLine();

            Console.Write("Soyad: ");
            string soyad = Console.ReadLine();

            Console.Write("Telefon: ");
            string telefon = Console.ReadLine();

            Console.Write("Eposta: ");
            string eposta = Console.ReadLine();

            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) || string.IsNullOrEmpty(telefon)) // degerlerin bos olup olmadigini kontrol ediyoruz
            {
                Console.WriteLine("[HATA] Ad soyad ve telefon bos birakilamaz!");
                Bekle();
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString)) // veritabanina baglanmak icin using blogu kullanarak otomatik olarak baglantiyi kapatmasini sagliyoruz(using blogu ile)
            {
                try //bu kod bloğu için yapay zekadan yardım alınmıştır.
                {
                    con.Open();

                    // Aynı telefn daha önce kayıtlı mı diye kontrol ediyoruz
                    string kontrol = "SELECT musteri_id FROM musteriler WHERE telefon = @telefon LIMIT 1"; // SQL sorgusu ile telefon numarasının daha önce kayıtlı olup olmadığını kontrol ediyoruz. LIMIT 1 ifadesi, sadece ilk eşleşmeyi döndürür 
                    using (MySqlCommand cmd = new MySqlCommand(kontrol, con)) //kontrol sorgusunu çalıştırmak için MySqlCommand nesnesi oluşturuyoruz ve bağlantıyı sağlıyoruz
                    {
                        cmd.Parameters.AddWithValue("@telefon", telefon);
                        if (cmd.ExecuteScalar() != null) // ExecuteScalar() metodu, sorgudan dönen ilk değeri alır. Eğer telefon numarası zaten kayıtlıysa, null olmayan bir değer dönecektir. Bu durumda kullanıcıya uyarı mesajı gösterilir ve kayıt işlemi engellenir.
                        {
                            Console.WriteLine("[UYARI] Bu telefon numarası zaten kayıtlı! Kayıt engellendi.");
                            Bekle();
                            return;
                        }
                    }

                    // Müşteri Ekleme işlemi için stored procedure kullanıyoruz. sp_musteri_ekle prosedürü, müşteri bilgilerini alır ve veritabanına ekler
                    using (MySqlCommand cmd = new MySqlCommand("sp_musteri_ekle", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure; // komut türünü stored procedure olarak belirtiyoruz sql çağrısı olarak değil 
                        cmd.Parameters.AddWithValue("p_adi", ad);
                        cmd.Parameters.AddWithValue("p_soyadi", soyad);
                        cmd.Parameters.AddWithValue("p_telefon", telefon);
                        cmd.Parameters.AddWithValue("p_eposta", eposta);
                        cmd.ExecuteNonQuery(); // SQL kodunu direkt yazmak yerine veritabanındaki hazır prodedure'ü çağırarak müşteri ekleme işlemini gerçekleştiriyoruz. 
                    }

                    Console.WriteLine("[BASARILI] Musteri basariyla kaydedildi!");
                }
                catch (Exception ex) // try bloğu içinde hata olursa burası devreye giriyor 
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // 2 numara seçilirse araç kaydı yapılır. Plaka kontrolü yapılır, müşteri ID'si doğrulanır ve araç kaydedilir. Plaka zaten kayıtlıysa uyarı verilir ve kayıt engellenir.
        static void AracKayit()
        {
            Console.Clear();
            Console.WriteLine("--- [2] ARAC KAYIT ---");

            Console.Write("Plaka: ");
            string plaka = Console.ReadLine().Trim().ToUpper(); // kullanıcı boşluk bırakıp küçük harf girse bile palaka formatı haline getirilip sıkıntı çıkmması için

            Console.Write("Ara. Türünü giriniz: ");
            string aracTuru = Console.ReadLine();

            Console.Write("Musteri ID: ");
            string musteriIdStr = Console.ReadLine();

            if (!int.TryParse(musteriIdStr, out int musteriId)) //Tryparse ile integer dönüştürüldü
            {
                Console.WriteLine("[HATA] Geçerli bir müşteri ID giriniz!");
                Bekle();
                return;
            }

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // müşteri var mı
                    string musteriKontrol = "SELECT musteri_id FROM musteriler WHERE musteri_id = @id LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(musteriKontrol, con))
                    {
                        cmd.Parameters.AddWithValue("@id", musteriId);
                        if (cmd.ExecuteScalar() == null) // Sql'i çalıştırıp tek sonuç döndürür
                        {
                            Console.WriteLine("[HATA] Bu ID'li müşteri bulunamadı!");
                            Bekle();
                            return;
                        }
                    }

                    // Plaka kontrolü - aynı plaka ile ikinci araç kaydı engelleniyor
                    string plakaKontrol = "SELECT araclar_id FROM araclar WHERE plaka = @plaka LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(plakaKontrol, con))
                    {
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        if (cmd.ExecuteScalar() != null)
                        {
                            Console.WriteLine("[ENGELLENDİ] Bu plaka zaten kayıtlı! İkinci araç kaydı engellendi.");
                            Bekle();
                            return;
                        }
                    }

                    // aracı veritabanına ekleme
                    string ekle = "INSERT INTO araclar (plaka, arac_turu, musteri_id) VALUES (@plaka, @tur, @musteriId); SELECT LAST_INSERT_ID();";
                    using (MySqlCommand cmd = new MySqlCommand(ekle, con))
                    {
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        cmd.Parameters.AddWithValue("@tur", aracTuru);
                        cmd.Parameters.AddWithValue("@musteriId", musteriId);
                        int yeniId = Convert.ToInt32(cmd.ExecuteScalar()); // ExecuteScalar() ile eklenen aracın ID'sini alıyoruz. LAST_INSERT_ID() fonksiyonu, son eklenen kaydın ID'sini döndürür. Bu sayede yeni aracın ID'sini öğrenip kullanıcıya gösterebiliyoruz.
                        Console.WriteLine($"[BASARILI] Arac kaydedildi! Arac ID: {yeniId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // [3] arac giris - INSERT sonrasi trigger otomatik kapasiteyi azaltir
        static void AracGiris()
        {
            Console.Clear();
            Console.WriteLine("--- [3] ARAÇ GİRİŞİ ---");

            Console.Write("Plaka: ");
            string plaka = Console.ReadLine().Trim().ToUpper();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // arac sistemde kayitli mi
                    string aracSorgu = "SELECT araclar_id FROM araclar WHERE plaka = @plaka LIMIT 1";
                    int aracId = 0;
                    using (MySqlCommand cmd = new MySqlCommand(aracSorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        object sonuc = cmd.ExecuteScalar();
                        if (sonuc == null)
                        {
                            Console.WriteLine("[HATA] Bu plaka sistemde kayitli degil! Once Arac Kayit yapiniz.");
                            Bekle();
                            return;
                        }
                        aracId = Convert.ToInt32(sonuc);
                    }

                    // arac zaten iceride mi
                    string icerideKontrol = "SELECT islem_id FROM giris_cikislar WHERE araclar_id = @id AND durum = 'İçeride' LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(icerideKontrol, con))
                    {
                        cmd.Parameters.AddWithValue("@id", aracId);
                        if (cmd.ExecuteScalar() != null)
                        {
                            Console.WriteLine("[UYARI] Bu arac zaten otopark icerisinde bulunuyor!");
                            Bekle();
                            return;
                        }
                    }

                    // bos park alanlarini listele
                    Console.WriteLine("\nBos Park Alanlari:");
                    Console.WriteLine("-------------------------------");
                    string alanSorgu = "SELECT alan_id, alan_adi, bos_kapasite FROM parkalanlari WHERE bos_kapasite > 0";
                    using (MySqlCommand cmd = new MySqlCommand(alanSorgu, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader()) //SQL'den gelen kodu satır satır okumaya yarıyor
                        {
                            bool bosAlanVar = false;
                            while (reader.Read())
                            {
                                bosAlanVar = true;
                                Console.WriteLine($"  ID: {reader["alan_id"]}  Adi: {reader["alan_adi"]}  Bos: {reader["bos_kapasite"]}"); // içlerimdeki konutları yazdırıyor
                            }
                            if (!bosAlanVar)
                            {
                                Console.WriteLine("  Tum park alanlari dolu!");
                                Bekle();
                                return;
                            }
                        }
                    }

                    Console.Write("Alan ID Seciniz: ");
                    int alanId = Convert.ToInt32(Console.ReadLine()); // String'den integer dönüşümü yaptık

                    // giris kaydi olustur
                    DateTime girisZamani = DateTime.Now; //Şu anki tarih ve saati alır 
                    string girisSorgu = "INSERT INTO giris_cikislar (araclar_id, alan_id, giris_zamani, durum) VALUES (@aracId, @alanId, @zaman, 'İçeride')";
                    using (MySqlCommand cmd = new MySqlCommand(girisSorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@aracId", aracId);
                        cmd.Parameters.AddWithValue("@alanId", alanId);
                        cmd.Parameters.AddWithValue("@zaman", girisZamani);
                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"[BASARILI] Giris kaydedildi! Giris Zamani: {girisZamani:dd.MM.yyyy HH:mm:ss}");
                        Console.WriteLine("(Trigger park alani kapasitesini otomatik guncelledi)");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // [4] arac cikis - sp_odeme_al stored procedure ve trigger burada devreye giriyor
        static void AracCikisVeOdeme()
        {
            Console.Clear();
            Console.WriteLine("--- [4] ARAÇ ÇIKIŞI VE ÖDEME ---");

            Console.Write("Plaka: ");
            string plaka = Console.ReadLine().Trim().ToUpper();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    // araci ve giris bilgisini getir
                    string sorgu = @"SELECT gc.islem_id, gc.giris_zamani, pa.alan_adi
                                     FROM giris_cikislar gc
                                     JOIN araclar a ON gc.araclar_id = a.araclar_id
                                     JOIN parkalanlari pa ON gc.alan_id = pa.alan_id
                                     WHERE a.plaka = @plaka AND gc.durum = 'İçeride' LIMIT 1";

                    int islemId = 0;
                    DateTime girisZamani = DateTime.MinValue;

                    using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                Console.WriteLine("[UYARI] Bu plakali arac su an otopark icerisinde degil!");
                                Bekle();
                                return;
                            }
                            islemId = Convert.ToInt32(reader["islem_id"]);
                            girisZamani = Convert.ToDateTime(reader["giris_zamani"]);
                            Console.WriteLine($"Park Alani : {reader["alan_adi"]}");
                        }
                    }

                    // sure ve ucret hesapla (25 TL/saat, minimum 1 saat)
                    DateTime cikisZamani = DateTime.Now; // 
                    double saat = Math.Ceiling((cikisZamani - girisZamani).TotalHours);
                    if (saat < 1) saat = 1;
                    decimal tutar = (decimal)saat * 25;

                    Console.WriteLine($"Giris  : {girisZamani:dd.MM.yyyy HH:mm:ss}");
                    Console.WriteLine($"Cikis  : {cikisZamani:dd.MM.yyyy HH:mm:ss}");
                    Console.WriteLine($"Sure   : {saat} saat");
                    Console.WriteLine($"Ucret  : {tutar} TL");
                    Console.Write("Odeme onaylaniyor mu? (E/H): ");

                    if (Console.ReadLine().Trim().ToUpper() != "E")
                    {
                        Console.WriteLine("Islem iptal edildi.");
                        Bekle();
                        return;
                    }

                    // durum Tamamlandi yapilinca trg_arac_cikis_sonrasi_kapasite_artir triggeri otomatik calisir
                    string guncelle = "UPDATE giris_cikislar SET cikis_zamani = @cikis, durum = 'Tamamlandi' WHERE islem_id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(guncelle, con))
                    {
                        cmd.Parameters.AddWithValue("@cikis", cikisZamani);
                        cmd.Parameters.AddWithValue("@id", islemId);
                        cmd.ExecuteNonQuery();
                    }

                    // sp_odeme_al stored procedure ile odeme kaydi olusturuyoruz
                    using (MySqlCommand cmd = new MySqlCommand("sp_odeme_al", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_tutar", tutar);
                        cmd.Parameters.AddWithValue("p_islem_id", islemId);
                        cmd.ExecuteNonQuery();
                    }

                    Console.WriteLine($"[BASARILI] Cikis tamamlandi ve odeme kaydedildi! Odenen: {tutar} TL");
                    Console.WriteLine("(Trigger park alani kapasitesini otomatik guncelledi)");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // [5] otopark doluluk - view_otopark_durumu view'i kullaniliyor
        static void OtoparkDoluluk()
        {
            Console.Clear();
            Console.WriteLine("--- [5] OTOPARK DOLULUK DURUMU ---");
            Console.WriteLine("view_otopark_durumu view'i uzerinden veriler cekiliyor...\n");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string sorgu = "SELECT * FROM view_otopark_durumu";
                    using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine(string.Format("{0,-15} | {1,-16} | {2,-8}", "Park Alani", "Toplam Kapasite", "Bos Yer"));
                            Console.WriteLine("-------------------------------------------");
                            while (reader.Read())
                            {
                                Console.WriteLine(string.Format("{0,-15} | {1,-16} | {2,-8}",
                                    reader["Park Alani"],
                                    reader["Toplam Kapasite"],
                                    reader["Bos Yer"]));
                            }
                            Console.WriteLine("\n[BASARILI] View basariyla listelendi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // [6] musteri arac listesi - view_musteri_arac_listesi view'i kullaniliyor
        static void MusteriAracListesi()
        {
            Console.Clear();
            Console.WriteLine("--- [6] MUSTERI ARAC LİSTESİ ---");
            Console.WriteLine("view_musteri_arac_listesi view'i uzerinden veriler cekiliyor...\n");

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string sorgu = "SELECT * FROM view_musteri_arac_listesi";
                    using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine(string.Format("{0,-12} | {1,-12} | {2,-15} | {3,-12}", "Ad", "Soyad", "Plaka", "Arac Turu"));
                            Console.WriteLine("----------------------------------------------------------");
                            while (reader.Read())
                            {
                                Console.WriteLine(string.Format("{0,-12} | {1,-12} | {2,-15} | {3,-12}",
                                    reader["Ad"], reader["Soyad"], reader["Plaka"], reader["Arac Turu"]));
                            }
                            Console.WriteLine("\n[BASARILI] View basariyla listelendi.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // [7] plakaya gore gecmis - idx_arac_plaka indexi bu sorguda performans sagliyor
        static void PlakaSorgula()
        {
            Console.Clear();
            Console.WriteLine("--- [7] PLAKAYA GORE GECMIS SORGULAMA ---");

            Console.Write("Plaka: ");
            string plaka = Console.ReadLine().Trim().ToUpper();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    string sorgu = @"SELECT gc.islem_id, gc.giris_zamani, gc.cikis_zamani, gc.durum, pa.alan_adi, o.tutar
                                     FROM giris_cikislar gc
                                     JOIN araclar a ON gc.araclar_id = a.araclar_id
                                     JOIN parkalanlari pa ON gc.alan_id = pa.alan_id
                                     LEFT JOIN odemeler o ON gc.islem_id = o.islem_id
                                     WHERE a.plaka = @plaka
                                     ORDER BY gc.giris_zamani DESC";

                    using (MySqlCommand cmd = new MySqlCommand(sorgu, con))
                    {
                        cmd.Parameters.AddWithValue("@plaka", plaka);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("[BILGI] Bu plakaya ait kayit bulunamadi.");
                                Bekle();
                                return;
                            }

                            Console.WriteLine($"\n{plaka} Plakalı Arac - Islem Gecmisi:");
                            Console.WriteLine("--------------------------------------------");

                            while (reader.Read())
                            {
                                string cikis = reader["cikis_zamani"] == DBNull.Value
                                    ? "Henuz cikmadi"
                                    : Convert.ToDateTime(reader["cikis_zamani"]).ToString("dd.MM.yyyy HH:mm:ss");

                                string odeme = reader["tutar"] == DBNull.Value
                                    ? "Odeme bekleniyor"
                                    : reader["tutar"].ToString() + " TL";

                                Console.WriteLine($"Islem No  : {reader["islem_id"]}");
                                Console.WriteLine($"Durum     : {reader["durum"]}");
                                Console.WriteLine($"Alan      : {reader["alan_adi"]}");
                                Console.WriteLine($"Giris     : {Convert.ToDateTime(reader["giris_zamani"]):dd.MM.yyyy HH:mm:ss}");
                                Console.WriteLine($"Cikis     : {cikis}");
                                Console.WriteLine($"Odeme     : {odeme}");
                                Console.WriteLine("--------------------------------------------");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[HATA]: " + ex.Message);
                }
            }
            Bekle();
        }

        // ana menuye donmek icin bekleme
        static void Bekle()
        {
            Console.WriteLine("\nAna menuye donmek icin bir tusa basin...");
            Console.ReadKey();
        }
    }
}