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
        // "Klas�r Se�" butonu
        private void klasorsecbutton_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowser.ShowDialog();
                // Kullan�c� bir klas�r se�tiyse ve se�ilen klas�r bo� de�ilse se�ilen klas�r�n yolunu metin kutusuna yaz
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                {
                    textBox.Text = folderBrowser.SelectedPath;
                }
            }
        }
        // Hesapla butonu
        private void hesaplabutton_Click(object sender, EventArgs e)
        {
            // Metin kutusundan klas�r yolunu al
            string folderPath = textBox.Text;
            if (string.IsNullOrEmpty(folderPath))
            {
                mesajtextlabel.Text = "L�tfen ge�erli bir klas�r se�in.";
                return;
            }
            // S�cakl�k ve nem de�erlerini saklamak i�in listeler olu�tur
            List<double> sicaklikDegerleri = new List<double>();
            List<double> nemDegerleri = new List<double>();

            try
            {
                // Se�ilen klas�rdeki t�m .txt dosyalar�n� al
                foreach (string dosya in Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories))
                {      
                    // Dosyay� okumak i�in StreamReader 
                    using (StreamReader sr = new StreamReader(dosya))
                    {
                        string baslikSatiri = sr.ReadLine(); // �lk sat�r� atla

                        string satir;
                        // Dosya sonuna kadar her sat�r� oku
                        while ((satir = sr.ReadLine()) != null)
                        {
                            // Sat�r� virg�lle ay�rarak par�ala
                            string[] parcalar = satir.Split(',');
                            string zaman = parcalar[0];
                            double deger = double.Parse(parcalar[1]);
                            // Dosya ad� S�cakl�k i�eriyorsa s�cakl�k listesine ekle Nem i�eriyorsa nem listesine ekle
                            if (dosya.Contains("S�cakl�k"))
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

                // Hesaplama i�lemleri i�in strateji desenini kullanarak hesaplama yap�yoruz.
                // Hesaplama sonu�lar�n� bir metin dizisine ekleyerek kullan�c�ya g�sterilecek sonu�lar� olu�turduk
                string sonucMesaji = "";
                Context context = new Context();

                // Se�ili her bir hesaplama t�r� i�in ilgili stratejiyi belirle ve hesapla metodu �a��r
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
                // Hesaplama sonu�lar�n� metin etiketine yaz
                mesajtextlabel.Text = sonucMesaji;
                // Kullan�c�ya sonu�lar�n oldu�u klas�r yolunu g�ster
                mesajtextlabel.Text += "\nHesaplamalar ba�ar� ile yap�lm��t�r. Sonu�lar�n oldu�u klas�r: " + folderPath;
            }
            catch (Exception ex)
            {
                // Hata durumunda kullan�c�ya hatay� g�ster
                mesajtextlabel.Text = "Hesaplama esnas�nda hata ile kar��la��ld�: " + ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

         // Hesaplama stratejileri i�in context s�n�f�
    public class Context
    {
        private IHesaplamaStratejisi _hesaplamaStratejisi;

        // Se�ili stratejiyi belirlemek i�in metot
        public void SetStrateji(IHesaplamaStratejisi hesaplamaStratejisi)
        {
            _hesaplamaStratejisi = hesaplamaStratejisi;
        }

        // Se�ili stratejiyi kullanarak hesaplama yapmak i�in metot
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
                sonucMesaji += $"Global S�cakl�k Ortalamas�: {sicaklikOrtalama}, Global Nem Ortalamas�: {nemOrtalama}\n";
            }
            else
            {
                sonucMesaji += $"S�cakl�k Ortalamas�: {sicaklikOrtalama}\n";
                sonucMesaji += $"Nem Ortalamas�: {nemOrtalama}\n";
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
                sonucMesaji += $"Global S�cakl�k Maksimum: {sicaklikMaximum}, Global Nem Maksimum: {nemMaximum}\n";
            }
            else
            {
                sonucMesaji += $"S�cakl�k Maksimum: {sicaklikMaximum}\n";
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
                sonucMesaji += $"Global S�cakl�k Minimum: {sicaklikMinimum}, Global Nem Minimum: {nemMinimum}\n";
            }
            else
            {
                sonucMesaji += $"S�cakl�k Minimum: {sicaklikMinimum}\n";
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
                sonucMesaji += $"Global S�cakl�k Standart Sapmas�: {sicaklikStandartSapma}, Global Nem Standart Sapmas�: {nemStandartSapma}\n";
            }
            else
            {
                sonucMesaji += $"S�cakl�k Standart Sapmas�: {sicaklikStandartSapma}\n";
                sonucMesaji += $"Nem Standart Sapmas�: {nemStandartSapma}\n";
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
                sonucMesaji += $"Global S�cakl�k Frekans: {sicaklikFrekans}, Global Nem Frekans: {nemFrekans}\n";
            }
            else
            {
                sonucMesaji += $"S�cakl�k Frekans: {sicaklikFrekans}\n";
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
                sonucMesaji += $"Global S�cakl�k Median: {sicaklikMedian}, Global Nem Median: {nemMedian}\n";
            }
            else
            {
                sonucMesaji += $"S�cakl�k Median: {sicaklikMedian}\n";
                sonucMesaji += $"Nem Median: {nemMedian}\n";
            }
        }
    }

    // hesaplamalar s�n�f�
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