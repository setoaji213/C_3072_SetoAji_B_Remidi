using System;
using System.Collections.Generic;

interface DapatDidistribusikan
{
    void KirimPaket();
}

class Agen
{
    public string idAgen;
    public string namaAgen;
    public int stokAgen;

    public Agen(string idAgen, string namaAgen, int stokAgen)
    {
        this.idAgen = idAgen;
        this.namaAgen = namaAgen;
        this.stokAgen = stokAgen;
    }

    public void TampilInfo()
    {
        Console.WriteLine("ID Agen   : " + idAgen);
        Console.WriteLine("Nama Agen : " + namaAgen);
        Console.WriteLine("Stok Agen : " + stokAgen);
    }
}

abstract class ProdukGalon
{
    private string idProduk;
    private string namaProduk;
    private int stokGudang;

    public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
    {
        this.idProduk = idProduk;
        this.namaProduk = namaProduk;
        StokGudang = stokGudang;
    }

    public string IdProduk
    {
        get { return idProduk; }
        set { idProduk = value; }
    }

    public string NamaProduk
    {
        get { return namaProduk; }
        set { namaProduk = value; }
    }

    public int StokGudang
    {
        get { return stokGudang; }
        private set { stokGudang = value < 0 ? 0 : value; }
    }

    public bool KurangiStok(int jumlah)
    {
        if (jumlah <= 0 || jumlah > stokGudang)
        {
            return false;
        }
        stokGudang -= jumlah;
        return true;
    }

    public void TambahStok(int jumlah)
    {
        if (jumlah > 0)
        {
            stokGudang += jumlah;
        }
    }

    public abstract int HitungBiaya(int jumlah);

    public virtual void TampilInfo()
    {
        Console.WriteLine("ID Produk   : " + idProduk);
        Console.WriteLine("Nama Produk : " + namaProduk);
        Console.WriteLine("Stok Gudang : " + stokGudang);
    }
}

class GalonAkua : ProdukGalon
{
    private const int harga = 20000;

    public GalonAkua(string idProduk, string namaProduk, int stokGudang)
        : base(idProduk, namaProduk, stokGudang)
    {
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * harga;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis       : Galon Akua (Rp" + harga + "/galon)");
    }
}

class GalonBerkeran : ProdukGalon
{
    private const int harga = 12000;
    public string jenisKeran;

    public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
        : base(idProduk, namaProduk, stokGudang)
    {
        this.jenisKeran = jenisKeran;
    }

    public override int HitungBiaya(int jumlah)
    {
        return jumlah * harga;
    }

    public override void TampilInfo()
    {
        base.TampilInfo();
        Console.WriteLine("Jenis       : Galon Berkeran (Rp" + harga + "/galon)");
        Console.WriteLine("Jenis Keran : " + jenisKeran);
    }
}

class DistribusiGalon : DapatDidistribusikan
{
    private Agen agenTujuan;
    private ProdukGalon produk;
    private int jumlah;

    public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
    {
        this.agenTujuan = agenTujuan;
        this.produk = produk;
        this.jumlah = jumlah;
    }

    public void KirimPaket()
    {
        if (jumlah <= 0)
        {
            Console.WriteLine("[GAGAL] Jumlah galon harus lebih dari 0. Stok tidak berubah.");
            return;
        }

        if (jumlah > produk.StokGudang)
        {
            Console.WriteLine("[GAGAL] Jumlah (" + jumlah + ") melebihi stok gudang (" + produk.StokGudang + "). Stok tidak berubah.");
            return;
        }

        produk.KurangiStok(jumlah);
        agenTujuan.stokAgen += jumlah;
        Console.WriteLine("[SUKSES] Paket berhasil dikirimkan ke agen " + agenTujuan.namaAgen + ": " + jumlah + " " + produk.NamaProduk + " (total biaya Rp" + produk.HitungBiaya(jumlah) + "). Rahasia sindikat aman.");
    }
}

class SindikatGalon
{
    
    private List<Agen> daftarAgen = new List<Agen>();
    private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

    public void TambahAgen(Agen agen)
    {
        daftarAgen.Add(agen);
    }

    public void TambahProduk(ProdukGalon produk)
    {
        daftarProduk.Add(produk);
    }

    public void TampilkanSindikat()
    {
        Console.WriteLine("===== DATA AGEN =====");
        foreach (Agen a in daftarAgen)
        {
            a.TampilInfo();
            Console.WriteLine();
        }

        Console.WriteLine("===== DATA PRODUK GALON =====");
        foreach (ProdukGalon p in daftarProduk)
        {
            p.TampilInfo();
            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Agen agn1 = new Agen("A01", "Budi", 2);
        Agen agn2 = new Agen("A02", "Andi", 3);

        Console.WriteLine("=== INFO AGEN ===");
        agn1.TampilInfo();
        Console.WriteLine();
        agn2.TampilInfo();
        Console.WriteLine();

        ProdukGalon akua = new GalonAkua("P01", "Galon Akua", 50);
        ProdukGalon berkeran = new GalonBerkeran("P02", "Galon Berkeran", 30, "Keran Tekan");

        Console.WriteLine("=== POLYMORPHISM ===");
        ProdukGalon[] semuaProduk = { akua, berkeran };
        foreach (ProdukGalon p in semuaProduk)
        {
            Console.WriteLine(p.NamaProduk + " x5 = Rp" + p.HitungBiaya(5));
        }
        Console.WriteLine();

        SindikatGalon sindikat = new SindikatGalon();
        sindikat.TambahAgen(agn1);
        sindikat.TambahAgen(agn2);
        sindikat.TambahProduk(akua);
        sindikat.TambahProduk(berkeran);

        Console.WriteLine("=== DISTRIBUSI ===");
        DapatDidistribusikan d1 = new DistribusiGalon(agn1, akua, 10);
        DapatDidistribusikan d2 = new DistribusiGalon(agn2, berkeran, 100);
        DapatDidistribusikan d3 = new DistribusiGalon(agn2, akua, 0);
        d1.KirimPaket();
        d2.KirimPaket();
        d3.KirimPaket();
        Console.WriteLine();

        sindikat.TampilkanSindikat();
    }
}