using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace odev2vs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // "Klasör Seç" butonu
        private void klasorsecbutton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowser.ShowDialog();
                // Kullanýcý bir klasör seçtiyse ve seçilen klasör boþ deðilse seçilen klasörün yolunu metin kutusuna yaz
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    textBox.Text = folderBrowser.SelectedPath;
                }
            }
        }
        // Hesapla butonu
        private void hesaplabutton_Click(object sender, EventArgs e)
        {
            // Metin kutusundan klasör yolunu al
            string folderPath = textBox.Text;
            if (string.IsNullOrEmpty(folderPath))
            {
                mesajtextlabel.Text = "Lütfen geçerli bir klasör seçin.";
                return;
            }
            // Sýcaklýk ve nem deðerlerini saklamak için listeler oluþtur
            List<double> sicaklikDegerleri = new List<double>();
            List<double> nemDegerleri = new List<double>();

            try
            {
                // Seçilen klasördeki tüm .txt dosyalarýný al
                foreach (string dosya in Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories))
                {      
                    // Dosyayý okumak için StreamReader 
                    using (StreamReader sr = new StreamReader(dosya))
                    {
                        string baslikSatiri = sr.ReadLine(); // Ýlk satýrý atla

                        string satir;
                        // Dosya sonuna kadar her satýrý oku
                        while ((satir = sr.ReadLine()) != null)
                        {
                            // Satýrý virgülle ayýrarak parçala
                            string[] parcalar = satir.Split(',');
                            string zaman = parcalar[0];
                            double deger = double.Parse(parcalar[1]);
                            // Dosya adý Sýcaklýk içeriyorsa sýcaklýk listesine ekle Nem içeriyorsa nem listesine ekle
                            if (dosya.Contains("Sýcaklýk"))
                            {
                                sicaklikDegerleri.Add(deger);
                            }
                            else if (dosya.Contains("Nem"))
                            {
                                nemDegerleri.Add(deger);
                            }
                        }
                    }
                }

                hesaplamalar hesap = new hesaplamalar();

                // Hesaplama iþlemleri için strateji desenini kullanarak hesaplama yapýyoruz.
                // Hesaplama sonuçlarýný bir metin dizisine ekleyerek kullanýcýya gösterilecek sonuçlarý oluþturduk
                string sonucMesaji = "";
                Context context = new Context();

                // Seçili her bir hesaplama türü için ilgili stratejiyi belirle ve hesapla metodu çaðýr
                if (checkBox1.Checked)
                {
                    context.SetStrateji(new OrtalamaHesaplama(checkBox7.Checked));
                    context.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
                }

                if (checkBox2.Checked)
                {
                    context.SetStrateji(new MaximumHesaplama(checkBox8.Checked));
                    context.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
                }

                if (checkBox3.Checked)
                {
                    context.SetStrateji(new MinimumHesaplama(checkBox9.Checked));
                    context.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
                }

                if (checkBox4.Checked)
                {
                    context.SetStrateji(new StandartSapmaHesaplama(checkBox10.Checked));
                    context.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
                }

                if (checkBox5.Checked)
                {
                    context.SetStrateji(new FrekansHesaplama(checkBox11.Checked));
                    context.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
                }

                if (checkBox6.Checked)
                {
                    context.SetStrateji(new MedianHesaplama(checkBox12.Checked));
                    context.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
                }
                // Hesaplama sonuçlarýný metin etiketine yaz
                mesajtextlabel.Text = sonucMesaji;
                // Kullanýcýya sonuçlarýn olduðu klasör yolunu göster
                mesajtextlabel.Text += "\nHesaplamalar baþarý ile yapýlmýþtýr. Sonuçlarýn olduðu klasör: " + folderPath;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanýcýya hatayý göster
                mesajtextlabel.Text = "Hesaplama esnasýnda hata ile karþýlaþýldý: " + ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

         // Hesaplama stratejileri için context sýnýfý
    public class Context
    {
        private IHesaplamaStratejisi _hesaplamaStratejisi;

        // Seçili stratejiyi belirlemek için metot
        public void SetStrateji(IHesaplamaStratejisi hesaplamaStratejisi)
        {
            _hesaplamaStratejisi = hesaplamaStratejisi;
        }

        // Seçili stratejiyi kullanarak hesaplama yapmak için metot
        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            _hesaplamaStratejisi.Hesapla(sicaklikDegerleri, nemDegerleri, ref sonucMesaji);
        }
    }

    // IHesaplamaStratejisi interface'i
    public interface IHesaplamaStratejisi
    {
        void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji);
    }

    // Ortalama hesaplama stratejisi
    public class OrtalamaHesaplama : IHesaplamaStratejisi
    {
        private readonly bool _global;

        public OrtalamaHesaplama(bool global)
        {
            _global = global;
        }

        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            hesaplamalar hesap = new hesaplamalar();
            double sicaklikOrtalama = hesap.ortalamaBul(sicaklikDegerleri);
            double nemOrtalama = hesap.ortalamaBul(nemDegerleri);

            if (_global)
            {
                sonucMesaji += $"Global Sýcaklýk Ortalamasý: {sicaklikOrtalama}, Global Nem Ortalamasý: {nemOrtalama}\n";
            }
            else
            {
                sonucMesaji += $"Sýcaklýk Ortalamasý: {sicaklikOrtalama}\n";
                sonucMesaji += $"Nem Ortalamasý: {nemOrtalama}\n";
            }
        }
    }

    // Maksimum hesaplama stratejisi
    public class MaximumHesaplama : IHesaplamaStratejisi
    {
        private readonly bool _global;

        public MaximumHesaplama(bool global)
        {
            _global = global;
        }

        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            hesaplamalar hesap = new hesaplamalar();
            double sicaklikMaximum = hesap.maximumBul(sicaklikDegerleri);
            double nemMaximum = hesap.maximumBul(nemDegerleri);

            if (_global)
            {
                sonucMesaji += $"Global Sýcaklýk Maksimum: {sicaklikMaximum}, Global Nem Maksimum: {nemMaximum}\n";
            }
            else
            {
                sonucMesaji += $"Sýcaklýk Maksimum: {sicaklikMaximum}\n";
                sonucMesaji += $"Nem Maksimum: {nemMaximum}\n";
            }
        }
    }

    // Minimum hesaplama stratejisi
    public class MinimumHesaplama : IHesaplamaStratejisi
    {
        private readonly bool _global;

        public MinimumHesaplama(bool global)
        {
            _global = global;
        }

        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            hesaplamalar hesap = new hesaplamalar();
            double sicaklikMinimum = hesap.minimumBul(sicaklikDegerleri);
            double nemMinimum = hesap.minimumBul(nemDegerleri);

            if (_global)
            {
                sonucMesaji += $"Global Sýcaklýk Minimum: {sicaklikMinimum}, Global Nem Minimum: {nemMinimum}\n";
            }
            else
            {
                sonucMesaji += $"Sýcaklýk Minimum: {sicaklikMinimum}\n";
                sonucMesaji += $"Nem Minimum: {nemMinimum}\n";
            }
        }
    }

    // Standart Sapma hesaplama stratejisi
    public class StandartSapmaHesaplama : IHesaplamaStratejisi
    {
        private readonly bool _global;

        public StandartSapmaHesaplama(bool global)
        {
            _global = global;
        }

        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            hesaplamalar hesap = new hesaplamalar();
            double sicaklikStandartSapma = hesap.standartSapmaBul(sicaklikDegerleri);
            double nemStandartSapma = hesap.standartSapmaBul(nemDegerleri);

            if (_global)
            {
                sonucMesaji += $"Global Sýcaklýk Standart Sapmasý: {sicaklikStandartSapma}, Global Nem Standart Sapmasý: {nemStandartSapma}\n";
            }
            else
            {
                sonucMesaji += $"Sýcaklýk Standart Sapmasý: {sicaklikStandartSapma}\n";
                sonucMesaji += $"Nem Standart Sapmasý: {nemStandartSapma}\n";
            }
        }
    }

    // Frekans hesaplama stratejisi
    public class FrekansHesaplama : IHesaplamaStratejisi
    {
        private readonly bool _global;

        public FrekansHesaplama(bool global)
        {
            _global = global;
        }

        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            hesaplamalar hesap = new hesaplamalar();
            var sicaklikFrekans = hesap.frekansBul(sicaklikDegerleri);
            var nemFrekans = hesap.frekansBul(nemDegerleri);

            if (_global)
            {
                sonucMesaji += $"Global Sýcaklýk Frekans: {sicaklikFrekans}, Global Nem Frekans: {nemFrekans}\n";
            }
            else
            {
                sonucMesaji += $"Sýcaklýk Frekans: {sicaklikFrekans}\n";
                sonucMesaji += $"Nem Frekans: {nemFrekans}\n";
            }
        }
    }

    // Median hesaplama stratejisi
    public class MedianHesaplama : IHesaplamaStratejisi
    {
        private readonly bool _global;

        public MedianHesaplama(bool global)
        {
            _global = global;
        }

        public void Hesapla(List<double> sicaklikDegerleri, List<double> nemDegerleri, ref string sonucMesaji)
        {
            hesaplamalar hesap = new hesaplamalar();
            double sicaklikMedian = hesap.medianBul(sicaklikDegerleri);
            double nemMedian = hesap.medianBul(nemDegerleri);

            if (_global)
            {
                sonucMesaji += $"Global Sýcaklýk Median: {sicaklikMedian}, Global Nem Median: {nemMedian}\n";
            }
            else
            {
                sonucMesaji += $"Sýcaklýk Median: {sicaklikMedian}\n";
                sonucMesaji += $"Nem Median: {nemMedian}\n";
            }
        }
    }

    // hesaplamalar sýnýfý
    public class hesaplamalar
    {
        public double ortalamaBul(List<double> degerler)
        {
            return degerler.Average();
        }

        public double maximumBul(List<double> degerler)
        {
            return degerler.Max();
        }

        public double minimumBul(List<double> degerler)
        {
            return degerler.Min();
        }

        public double standartSapmaBul(List<double> degerler)
        {
            double ortalama = ortalamaBul(degerler);
            double farklarToplami = degerler.Sum(d => Math.Pow(d - ortalama, 2));
            return Math.Sqrt(farklarToplami / degerler.Count);
        }

        public string frekansBul(List<double> degerler)
        {
            var frekanslar = degerler.GroupBy(d => d).Select(g => new { Deger = g.Key, Frekans = g.Count() });
            return string.Join(", ", frekanslar.Select(f => $"{f.Deger}: {f.Frekans}"));
        }

        public double medianBul(List<double> degerler)
        {
            var sorted = degerler.OrderBy(d => d).ToList();
            int count = sorted.Count;
            if (count % 2 == 0)
            {
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2;
            }
            else
            {
                return sorted[count / 2];
            }
        }
    }
}