using System;
using System.Collections.Generic;
using System.Linq;

namespace TurkiyeSehirAnaliz
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var analiz = new SehirAnalizSistemi();

            Console.WriteLine(" TÜRKİYE İLLER ARASI MESAFE ANALİZİ \n");

           
            Console.WriteLine("1. İL ADLARI listesi ");
            analiz.IlAdlariniListele();

           
            Console.WriteLine("\n2. RASTGELE 10 ŞEHİR gezisi");
            analiz.RastgeleOnSehirGez();

           
            Console.WriteLine("\n3. KOMŞU ŞEHİRLER listesi");
            analiz.KomsuSehirleriListele();

           
            Console.WriteLine("\n4. EN UZAK KOMŞU ŞEHİR ÇİFTİ");
            analiz.EnUzakKomsuCiftiniBul();

          
            Console.WriteLine("\n5. İZMİR'E DÜZ HAT UZAKLIKLARi");
            analiz.IzmireUzakliklariListele();

            Console.WriteLine("\n6. RASTGELE ŞEHİRDEN İZMİR'E YOLCULUk");
            analiz.IzmireYolBul();

           
           
        }
    }

    public class SehirAnalizSistemi
    {
        private string[] ilAdlari;
        private int[,] mesafeMatrisi;
        private Dictionary<int, List<int>> komsuSehirler;
        private Dictionary<int, int> izmirDuzHatMesafe;
        private Random random;

        public SehirAnalizSistemi()
        {
            random = new Random();
            IlAdlariniYukle();
            MesafeMatrisiniYukleme();
            KomsuSehirleriYuklemek();
            IzmirDuzHatMesafeleriniYukle();
        }

        private void IlAdlariniYukle()
        {
            ilAdlari = new string[82];
            ilAdlari[1] = "Adana"; ilAdlari[2] = "Adıyaman"; ilAdlari[3] = "Afyonkarahisar";
            ilAdlari[4] = "Ağrı"; ilAdlari[5] = "Amasya"; ilAdlari[6] = "Ankara";
            ilAdlari[7] = "Antalya"; ilAdlari[8] = "Artvin"; ilAdlari[9] = "Aydın";
            ilAdlari[10] = "Balıkesir"; ilAdlari[11] = "Bilecik"; ilAdlari[12] = "Bingöl";
            ilAdlari[13] = "Bitlis"; ilAdlari[14] = "Bolu"; ilAdlari[15] = "Burdur";
            ilAdlari[16] = "Bursa"; ilAdlari[17] = "Çanakkale"; ilAdlari[18] = "Çankırı";
            ilAdlari[19] = "Çorum"; ilAdlari[20] = "Denizli"; ilAdlari[21] = "Diyarbakır";
            ilAdlari[22] = "Edirne"; ilAdlari[23] = "Elazığ"; ilAdlari[24] = "Erzincan";
            ilAdlari[25] = "Erzurum"; ilAdlari[26] = "Eskişehir"; ilAdlari[27] = "Gaziantep";
            ilAdlari[28] = "Giresun"; ilAdlari[29] = "Gümüşhane"; ilAdlari[30] = "Hakkari";
            ilAdlari[31] = "Hatay"; ilAdlari[32] = "Isparta"; ilAdlari[33] = "Mersin";
            ilAdlari[34] = "İstanbul"; ilAdlari[35] = "İzmir"; ilAdlari[36] = "Kars";
            ilAdlari[37] = "Kastamonu"; ilAdlari[38] = "Kayseri"; ilAdlari[39] = "Kırklareli";
            ilAdlari[40] = "Kırşehir"; ilAdlari[41] = "Kocaeli"; ilAdlari[42] = "Konya";
            ilAdlari[43] = "Kütahya"; ilAdlari[44] = "Malatya"; ilAdlari[45] = "Manisa";
            ilAdlari[46] = "Kahramanmaraş"; ilAdlari[47] = "Mardin"; ilAdlari[48] = "Muğla";
            ilAdlari[49] = "Muş"; ilAdlari[50] = "Nevşehir"; ilAdlari[51] = "Niğde";
            ilAdlari[52] = "Ordu"; ilAdlari[53] = "Rize"; ilAdlari[54] = "Sakarya";
            ilAdlari[55] = "Samsun"; ilAdlari[56] = "Siirt"; ilAdlari[57] = "Sinop";
            ilAdlari[58] = "Sivas"; ilAdlari[59] = "Tekirdağ"; ilAdlari[60] = "Tokat";
            ilAdlari[61] = "Trabzon"; ilAdlari[62] = "Tunceli"; ilAdlari[63] = "Şanlıurfa";
            ilAdlari[64] = "Uşak"; ilAdlari[65] = "Van"; ilAdlari[66] = "Yozgat";
            ilAdlari[67] = "Zonguldak"; ilAdlari[68] = "Aksaray"; ilAdlari[69] = "Bayburt";
            ilAdlari[70] = "Karaman"; ilAdlari[71] = "Kırıkkale"; ilAdlari[72] = "Batman";
            ilAdlari[73] = "Şırnak"; ilAdlari[74] = "Bartın"; ilAdlari[75] = "Ardahan";
            ilAdlari[76] = "Iğdır"; ilAdlari[77] = "Yalova"; ilAdlari[78] = "Karabük";
            ilAdlari[79] = "Kilis"; ilAdlari[80] = "Osmaniye"; ilAdlari[81] = "Düzce";
        }

        private void MesafeMatrisiniYukleme()
        {
            mesafeMatrisi = new int[82, 82];
           
            MesafeMatrisiniCSVdenYukle();

        }

        private void MesafeMatrisiniCSVdenYukle()
        {
            
            string dosyaYolu = "ilmesafe.csv";

           

            try
            {
                string[] satirlar = System.IO.File.ReadAllLines(dosyaYolu, System.Text.Encoding.UTF8);// csv dosyasına okudum turkçe karaktereleri duzgun okusun diye utf8 kullandım.



                for (int i = 2; i < satirlar.Length && i < 83; i++)
                {

                    string[] degerler = satirlar[i].Split(',');
                    int ilNo = i - 1;

                    for (int j = 2; j < degerler.Length && j < 83; j++)
                    {
                        int hedefIlNo = j - 1;

                        string deger = degerler[j].Trim();
                        if (string.IsNullOrWhiteSpace(deger) || deger == "-")// kontrol ettim boşul ya da boş deger var ise
                        {
                            mesafeMatrisi[ilNo, hedefIlNo] = 0;
                        }
                        else
                        {

                            if (int.TryParse(deger, out int mesafe))
                            {
                                mesafeMatrisi[ilNo, hedefIlNo] = mesafe;
                            }
                            else
                            {
                                mesafeMatrisi[ilNo, hedefIlNo] = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"csv okuma hatasi {ex.Message}");// hata varsa hata mesajı
               ;
               
            }
        }

        

        private void KomsuSehirleriYuklemek()
        {
            komsuSehirler = new Dictionary<int, List<int>>();

            var komsuData = new Dictionary<string, string>
            {
                {"Adana", "Hatay,Osmaniye,Kahramanmaraş,Kayseri,Niğde,Mersin"},
                {"Adıyaman", "Şanlıurfa,Diyarbakır,Malatya,Kahramanmaraş,Gaziantep"},
                {"Afyonkarahisar", "Isparta,Konya,Eskişehir,Kütahya,Uşak,Denizli,Burdur"},
                {"Ağrı", "Van,Iğdır,Kars,Erzurum,Muş,Bitlis"},
                {"Amasya", "Yozgat,Tokat,Samsun,Çorum"},
                {"Ankara", "Konya,Aksaray,Kırşehir,Kırıkkale,Çankırı,Bolu,Eskişehir"},
                {"Antalya", "Mersin,Karaman,Konya,Isparta,Burdur,Muğla"},
                {"Artvin", "Rize,Erzurum,Ardahan"},
                {"Aydın", "Muğla,Denizli,Manisa,İzmir"},
                {"Balıkesir", "İzmir,Manisa,Kütahya,Bursa,Çanakkale"},
                {"Bilecik", "Kütahya,Eskişehir,Bolu,Sakarya,Bursa"},
                {"Bingöl", "Diyarbakır,Muş,Erzurum,Erzincan,Tunceli,Elazığ"},
                {"Bitlis", "Siirt,Van,Ağrı,Muş,Batman"},
                {"Bolu", "Eskişehir,Ankara,Çankırı,Zonguldak,Düzce,Sakarya,Bilecik"},
                {"Burdur", "Muğla,Antalya,Isparta,Afyonkarahisar,Denizli"},
                {"Bursa", "Balıkesir,Kütahya,Bilecik,Sakarya,Kocaeli,Yalova"},
                {"Çanakkale", "Balıkesir,Tekirdağ,Edirne"},
                {"Çankırı", "Ankara,Kırıkkale,Çorum,Kastamonu,Karabük,Bolu"},
                {"Çorum", "Yozgat,Amasya,Samsun,Sinop,Kastamonu,Çankırı,Kırıkkale"},
                {"Denizli", "Manisa,Aydın,Muğla,Burdur,Afyonkarahisar,Uşak"},
                {"Diyarbakır", "Şanlıurfa,Mardin,Batman,Muş,Bingöl,Elazığ,Adıyaman"},
                {"Edirne", "Tekirdağ,Kırklareli,Çanakkale"},
                {"Elazığ", "Malatya,Tunceli,Bingöl,Diyarbakır"},
                {"Erzincan", "Erzurum,Gümüşhane,Bayburt,Sivas,Tunceli,Bingöl"},
                {"Erzurum", "Kars,Ağrı,Muş,Bingöl,Erzincan,Bayburt,Artvin"},
                {"Eskişehir", "Ankara,Bolu,Bilecik,Bursa,Kütahya,Afyonkarahisar"},
                {"Gaziantep", "Kilis,Şanlıurfa,Adıyaman,Kahramanmaraş,Osmaniye"},
                {"Giresun", "Ordu,Gümüşhane"},
                {"Gümüşhane", "Trabzon,Bayburt,Erzincan,Giresun"},
                {"Hakkari", "Van,Şırnak"},
                {"Hatay", "Adana,Osmaniye,Gaziantep"},
                {"Isparta", "Burdur,Antalya,Konya,Afyonkarahisar"},
                {"Mersin", "Adana,Niğde,Karaman,Antalya"},
                {"İstanbul", "Kocaeli,Tekirdağ"},
                {"İzmir", "Manisa,Aydın,Balıkesir"},
                {"Kars", "Ardahan,Iğdır,Ağrı,Erzurum"},
                {"Kastamonu", "Sinop,Çorum,Çankırı,Karabük,Bartın"},
                {"Kayseri", "Sivas,Kahramanmaraş,Adana,Niğde,Nevşehir,Yozgat"},
                {"Kırklareli", "Edirne,Tekirdağ"},
                {"Kırşehir", "Nevşehir,Ankara,Kırıkkale,Yozgat,Aksaray"},
                {"Kocaeli", "İstanbul,Bursa,Sakarya,Yalova"},
                {"Konya", "Karaman,Mersin,Antalya,Isparta,Afyonkarahisar,Ankara,Aksaray"},
                {"Kütahya", "Manisa,Uşak,Afyonkarahisar,Eskişehir,Bilecik,Bursa,Balıkesir"},
                {"Malatya", "Elazığ,Adıyaman,Kahramanmaraş,Sivas"},
                {"Manisa", "İzmir,Aydın,Denizli,Uşak,Kütahya,Balıkesir"},
                {"Kahramanmaraş", "Gaziantep,Adıyaman,Malatya,Sivas,Kayseri,Adana,Osmaniye"},
                {"Mardin", "Şanlıurfa,Diyarbakır,Batman,Şırnak"},
                {"Muğla", "Aydın,Denizli,Burdur,Antalya"},
                {"Muş", "Bitlis,Ağrı,Erzurum,Bingöl,Diyarbakır"},
                {"Nevşehir", "Niğde,Kayseri,Yozgat,Kırşehir,Aksaray"},
                {"Niğde", "Nevşehir,Kayseri,Adana,Mersin,Aksaray"},
                {"Ordu", "Samsun,Giresun"},
                {"Rize", "Trabzon,Artvin"},
                {"Sakarya", "Kocaeli,Bilecik,Bolu,Düzce,Bursa"},
                {"Samsun", "Ordu,Tokat,Amasya,Çorum,Sinop"},
                {"Siirt", "Batman,Bitlis,Van,Şırnak"},
                {"Sinop", "Kastamonu,Çorum,Samsun"},
                {"Sivas", "Malatya,Kahramanmaraş,Kayseri,Yozgat,Tokat,Erzincan"},
                {"Tekirdağ", "İstanbul,Edirne,Kırklareli,Çanakkale"},
                {"Tokat", "Yozgat,Sivas,Amasya,Samsun,Ordu"},
                {"Trabzon", "Rize,Gümüşhane,Bayburt"},
                {"Tunceli", "Elazığ,Erzincan,Bingöl"},
                {"Şanlıurfa", "Gaziantep,Adıyaman,Diyarbakır,Mardin"},
                {"Uşak", "Manisa,Denizli,Afyonkarahisar,Kütahya"},
                {"Van", "Hakkari,Bitlis,Siirt,Ağrı"},
                {"Yozgat", "Kayseri,Sivas,Tokat,Amasya,Çorum,Kırıkkale,Kırşehir"},
                {"Zonguldak", "Bartın,Karabük,Bolu"},
                {"Aksaray", "Niğde,Nevşehir,Kırşehir,Ankara,Konya"},
                {"Bayburt", "Erzurum,Gümüşhane,Trabzon,Erzincan"},
                {"Karaman", "Konya,Mersin,Antalya"},
                {"Kırıkkale", "Ankara,Çankırı,Çorum,Yozgat,Kırşehir"},
                {"Batman", "Diyarbakır,Siirt,Bitlis,Mardin"},
                {"Şırnak", "Mardin,Siirt,Hakkari"},
                {"Bartın", "Zonguldak,Karabük,Kastamonu"},
                {"Ardahan", "Kars,Artvin"},
                {"Iğdır", "Kars,Ağrı"},
                {"Yalova", "Bursa,Kocaeli"},
                {"Karabük", "Zonguldak,Bartın,Kastamonu,Çankırı,Bolu"},
                {"Kilis", "Gaziantep"},
                {"Osmaniye", "Hatay,Adana,Gaziantep,Kahramanmaraş"},
                {"Düzce", "Bolu,Zonguldak,Sakarya"}
            };

            foreach (var kvp in komsuData)
            {
                int ilNo = Array.IndexOf(ilAdlari, kvp.Key);
                if (ilNo > 0)
                {
                    var komsular = new List<int>();
                    foreach (var komsuAd in kvp.Value.Split(','))
                    {
                        int komsuNo = Array.IndexOf(ilAdlari, komsuAd);
                        if (komsuNo > 0)
                            komsular.Add(komsuNo);
                    }
                    komsuSehirler[ilNo] = komsular;
                }
            }
        }

        private void IzmirDuzHatMesafeleriniYukle()
        {
            
            izmirDuzHatMesafe = new Dictionary<int, int>
            {
                {1, 520}, {2, 710}, {3, 270}, {4, 1380}, {5, 550}, {6, 480},
                {7, 330}, {8, 1250}, {9, 110}, {10, 100}, {11, 310}, {12, 1050},
                {13, 1230}, {14, 390}, {15, 220}, {16, 250}, {17, 130}, {18, 480},
                {19, 560}, {20, 150}, {21, 1020}, {22, 290}, {23, 890}, {24, 950},
                {25, 1080}, {26, 360}, {27, 820}, {28, 840}, {29, 910}, {30, 1580},
                {31, 680}, {32, 260}, {33, 560}, {34, 330}, {35, 0}, {36, 1280},
                {37, 580}, {38, 600}, {39, 370}, {40, 530}, {41, 310}, {42, 380},
                {43, 220}, {44, 790}, {45, 30}, {46, 730}, {47, 1120}, {48, 140},
                {49, 1140}, {50, 570}, {51, 570}, {52, 770}, {53, 1050}, {54, 370},
                {55, 660}, {56, 1180}, {57, 670}, {58, 710}, {59, 320}, {60, 680},
                {61, 980}, {62, 920}, {63, 960}, {64, 180}, {65, 1380}, {66, 620},
                {67, 550}, {68, 520}, {69, 1080}, {70, 470}, {71, 520}, {72, 1090},
                {73, 1340}, {74, 540}, {75, 1340}, {76, 1330}, {77, 290}, {78, 520},
                {79, 770}, {80, 610}, {81, 440}
            };
        }

        public void IlAdlariniListele()
        {
            for (int i = 1; i <= 81; i++)
            {
                Console.WriteLine($"plaka {i:D2}: {ilAdlari[i]}");
            }
        }

        public void RastgeleOnSehirGez()
        {
            var gezilenSehirler = new List<int>();
            int mevcutSehir = random.Next(1, 82);
            gezilenSehirler.Add(mevcutSehir);

            int toplamMesafe = 0;
            Console.WriteLine($"\nbaslangıç sehri: {ilAdlari[mevcutSehir]} (plaka {mevcutSehir})");

            for (int i = 1; i < 10; i++)
            {
                int sonrakiSehir;
                do
                {
                    sonrakiSehir = random.Next(1, 82);
                } while (gezilenSehirler.Contains(sonrakiSehir));

                int mesafe = mesafeMatrisi[mevcutSehir, sonrakiSehir];
                toplamMesafe += mesafe;

                Console.WriteLine($"{i}. {ilAdlari[mevcutSehir]} - {ilAdlari[sonrakiSehir]}: {mesafe} km");

                gezilenSehirler.Add(sonrakiSehir);
                mevcutSehir = sonrakiSehir;
            }

            Console.WriteLine($"\ntoplam Katedilen mesafe: {toplamMesafe} km");
            Console.WriteLine($"Gezilen Şehirler: {string.Join(" - ", gezilenSehirler.Select(s => ilAdlari[s]))}");
        }

        public void KomsuSehirleriListele()
        {
            Console.WriteLine("\ntürkiye'deki iller ve de  komşulari\n");
            foreach (var kvp in komsuSehirler.OrderBy(k => k.Key))
            {
                int ilNo = kvp.Key;
                Console.Write($"{ilNo:D2} - {ilAdlari[ilNo]}: ");
                var komsuAdlar = kvp.Value.Select(k => ilAdlari[k]);
                Console.WriteLine(string.Join(", ", komsuAdlar));
            }
        }

        public void EnUzakKomsuCiftiniBul()
        {
            int maxMesafe = 0;
            int il1 = 0, il2 = 0;

            foreach (var kvp in komsuSehirler)
            {
                int merkez = kvp.Key;
                foreach (int komsu in kvp.Value)
                {
                    int mesafe = mesafeMatrisi[merkez, komsu];
                    if (mesafe > maxMesafe)
                    {
                        maxMesafe = mesafe;
                        il1 = merkez;
                        il2 = komsu;
                    }
                }
            }

            Console.WriteLine($"\nen uzak komşu şehir çifti:");
            Console.WriteLine($"{ilAdlari[il1]} - {ilAdlari[il2]}: {maxMesafe} km");
            Console.WriteLine($"\niki komşu şehir arasındakii mesafe, türkiyedeki tüm komşu");
            Console.WriteLine($"şehir çiftleri arasında en büyük olanı.");
        }

        public void IzmireUzakliklariListele()
        {
            Console.WriteLine("\n81 ilin İzmire Düz Hat kus ucusu  uzaklıklari:\n");
            for (int i = 1; i <= 81; i++)
            {
                if (i == 35) continue; 
                Console.WriteLine($"{i:D2} - {ilAdlari[i],-20}: {izmirDuzHatMesafe[i],4} km");
            }
        }

        public void IzmireYolBul()
        {
            int baslangic = random.Next(1, 82);
            while (baslangic == 35) 
                baslangic = random.Next(1, 82);

            Console.WriteLine($"\nbaşlangıç sehri: {ilAdlari[baslangic]} (plakasi: {baslangic})");
            Console.WriteLine($"hedef: İzmir (Plaka: 35)\n");
            Console.WriteLine("(her adımda İzmir'e en çok yaklaşan komşu şehirrr)\n");

            var yol = new List<int>();
            int mevcutSehir = baslangic;
            yol.Add(mevcutSehir);
            int toplamMesafe = 0;
            int adim = 0;

            while (mevcutSehir != 35)
            {
                if (!komsuSehirler.ContainsKey(mevcutSehir) || komsuSehirler[mevcutSehir].Count == 0)
                {
                    Console.WriteLine($"\n{ilAdlari[mevcutSehir]} şehrinin komşuları yokkk!");
                    break;
                }

                int enIyiKomsu = -1;
                int enKisaDuzHat = int.MaxValue;

                foreach (int komsu in komsuSehirler[mevcutSehir])
                {
                    if (!yol.Contains(komsu))
                    {
                        int komsuDuzHat = izmirDuzHatMesafe[komsu];
                        if (komsuDuzHat < enKisaDuzHat)
                        {
                            enKisaDuzHat = komsuDuzHat;
                            enIyiKomsu = komsu;
                        }
                    }
                }

                if (enIyiKomsu == -1)
                {
                    Console.WriteLine($"\ntüm komşularai gidildi, ve de yol bulunamadii!");
                    break;
                }

                int mesafe = mesafeMatrisi[mevcutSehir, enIyiKomsu];
                toplamMesafe += mesafe;
                adim++;

                Console.WriteLine($"adimi {adim}: {ilAdlari[mevcutSehir],-20} - {ilAdlari[enIyiKomsu],-20}   " +
                                $"mesafesi: {mesafe,4} km İzmir'e düz hat: {enKisaDuzHat,4} km");

                mevcutSehir = enIyiKomsu;
                yol.Add(mevcutSehir);

                if (yol.Count > 81)
                {
                    Console.WriteLine("\nsonsuz döngü ");
                    break;
                }
            }

            if (mevcutSehir == 35)
            {
                Console.WriteLine($"\nİzmire ulaşıltik!");
                Console.WriteLine($"toplam katedilen mesafesii: {toplamMesafe} km");
                Console.WriteLine($"geçilen sehir Sayısi: {yol.Count}");
                Console.WriteLine($"yolu: {string.Join(" - ", yol.Select(s => ilAdlari[s]))}"); //  Select  şehir numaralarını isme çeviriyo ve selecti lamda operotoru ile kullandım, Join de hepsini tireyle birleştiriyo bunu internetten ogrendim.
            }
        }

        
    }
}