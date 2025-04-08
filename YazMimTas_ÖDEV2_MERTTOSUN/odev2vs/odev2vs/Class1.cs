using System;
using System.Collections.Generic;
using System.Linq;

public class hesaplamalar
{
    public double ortalamaBul(List<double> degerler)
    {
        if (degerler == null || degerler.Count == 0)
        {
            return 0;
        }
        return degerler.Average();
    }

    public double maximumBul(List<double> degerler)
    {
        if (degerler == null || degerler.Count == 0)
        {
            return double.NaN;
        }
        return degerler.Max();
    }

    public double minimumBul(List<double> degerler)
    {
        if (degerler == null || degerler.Count == 0)
        {
            return double.NaN;
        }
        return degerler.Min();
    }

    public double standartSapmaBul(List<double> degerler)
    {
        if (degerler == null || degerler.Count == 0)
        {
            return double.NaN;
        }
        double ortalama = ortalamaBul(degerler);
        double toplam = 0;
        foreach (double deger in degerler)
        {
            toplam += Math.Pow(deger - ortalama, 2);
        }
        return Math.Sqrt(toplam / degerler.Count);
    }

    public Dictionary<double, int> frekansBul(List<double> degerler)
    {
        if (degerler == null || degerler.Count == 0)
        {
            return null;
        }
        return degerler.GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());
    }

    public double medianBul(List<double> degerler)
    {
        if (degerler == null || degerler.Count == 0)
        {
            return double.NaN;
        }
        var sortedDegerler = degerler.OrderBy(d => d).ToList();
        int n = sortedDegerler.Count;
        if (n % 2 == 1)
        {
            return sortedDegerler[n / 2];
        }
        return (sortedDegerler[n / 2 - 1] + sortedDegerler[n / 2]) / 2.0;
    }
}
