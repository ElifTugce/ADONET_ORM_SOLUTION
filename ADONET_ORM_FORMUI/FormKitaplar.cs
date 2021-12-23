using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADONET_ORM_BLL;
using ADONET_ORM_Entities;
using ADONET_ORM_Entities.Entities;

namespace ADONET_ORM_FORMUI
{
    public partial class FormKitaplar : Form
    {
        public FormKitaplar()
        {
            InitializeComponent();
        }

        //Global alan
        YazarlarORM myYazarORM = new YazarlarORM();
        TurlerORM myTurlerORM = new TurlerORM();
        KitaplarORM myKitapORM = new KitaplarORM();

        private void FormKitaplar_Load(object sender, EventArgs e)
        {
            TumYazarlariComboyaGetir();
            TumTurleriComboyaGetir();
            //buraya sonra dön
            TumKitaplariGrideViewModelleGetir();
            TumKitaplariSilComboyaGetir();
            TumKitaplariGuncelleComboyaGetir();


        }

        private void TumKitaplariGuncelleComboyaGetir()
        {
            comboBoxKitapGuncelle.DisplayMember = "KitapAdi";
            comboBoxKitapGuncelle.ValueMember = "KitapId";
            comboBoxKitapGuncelle.DataSource = myKitapORM.Select();
        }

        private void TumKitaplariSilComboyaGetir()
        {
            cmbBox_Sil_Kitap.DisplayMember = "KitapAdi";
            cmbBox_Sil_Kitap.ValueMember = "KitapId";

            // cmbBox_Sil_Kitap.DataSource = myKitapORM.Select();
            //Yukarıdaki gibi yapmak istemezsek yani
            // KitaplarORM class'ından instance almak istemezsek 
            // class içine tanımladığımız static property aracılığıyla o instance'a ulaşmış oluruz
            // aslında burada kendimize arka planda instance oluşturuyoruz ve static nesne aracılığıyla o nesneyi kullanıyoruz.

            cmbBox_Sil_Kitap.DataSource = KitaplarORM.Current.Select();
            

        }

        private void TumKitaplariGrideViewModelleGetir()
        {
            dataGridViewKitaplar.DataSource = myKitapORM.KitaplariViewModelleGetir();

            dataGridViewKitaplar.Columns["SilindiMi"].Visible = false;
            dataGridViewKitaplar.Columns["TurId"].Visible = false;
            dataGridViewKitaplar.Columns["YazarId"].Visible = false;
            for (int i = 0; i < dataGridViewKitaplar.Columns.Count; i++)
            {
                dataGridViewKitaplar.Columns[i].Width = 120;
            }
        }

        private void TumTurleriComboyaGetir()
        {
            cmbBox_Ekle_Tur.DisplayMember = "TurAdi";
            cmbBox_Ekle_Tur.ValueMember = "TurId";
            cmbBox_Ekle_Tur.DataSource = myTurlerORM.TurleriGetir();
            cmbBox_Ekle_Tur.SelectedIndex = 0;

            cmbBox_Guncelle_Tur.DisplayMember = "TurAdi";
            cmbBox_Guncelle_Tur.ValueMember = "TurId";
            cmbBox_Guncelle_Tur.DataSource = myTurlerORM.TurleriGetir();
        }

        private void TumYazarlariComboyaGetir()
        {
            cmbBox_Ekle_Yazar.DisplayMember = "YazarAdSoyad";
            cmbBox_Ekle_Yazar.ValueMember = "YazarId";
            cmbBox_Ekle_Yazar.DataSource = myYazarORM.Select();

            cmbBox_Guncelle_Yazar.DisplayMember = "YazarAdSoyad";
            cmbBox_Guncelle_Yazar.ValueMember = "YazarId";
            cmbBox_Guncelle_Yazar.DataSource = myYazarORM.Select();
        }

        private void btnKitapEkle_Click(object sender, EventArgs e)
        {
            try
            {
                if (numericUpDown_Ekle_SayfaSayisi.Value <= 0)
                {
                    MessageBox.Show("HATA: Sayfa sayısı sıfırdan büyük olmalıdır!");
                    return;

                }
                if (numericUpDown_Ekle_Stok.Value <= 0)
                {
                    MessageBox.Show("HATA: Kitap stoğu sıfırdan büyük olmalıdır!");
                    return;
                }

                if ((int)cmbBox_Ekle_Yazar.SelectedValue <= 0)
                {
                    MessageBox.Show("HATA: Kitabın bir yazarı olmalıdır! Yazar seçiniz! ");
                    return;

                }

                Kitap yeniKitap = new Kitap()
                {
                    KayitTarihi = DateTime.Now,
                    KitapAdi = txtKitapEkle.Text.Trim(),
                    SayfaSayisi = (int)numericUpDown_Ekle_SayfaSayisi.Value,
                    Stok = (int)numericUpDown_Ekle_Stok.Value,
                    SilindiMi = false,
                    YazarId = (int)cmbBox_Ekle_Yazar.SelectedValue
                };

                //TurId null mı değil mi?

                if ((int)cmbBox_Ekle_Tur.SelectedValue == -1) // Tür yok'u seçmiş
                {
                    yeniKitap.TurId = null;
                }
                else
                {
                    yeniKitap.TurId = (int)cmbBox_Ekle_Tur.SelectedValue;
                }

                if (myKitapORM.Insert(yeniKitap))
                {
                    MessageBox.Show($"{yeniKitap.KitapAdi} kitabı sisteme eklendi...");
                    TumKitaplariGrideViewModelleGetir();
                    EkleSayfasiKontrolleriTemizle();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA:" + ex.Message);
            }
        }

        private void EkleSayfasiKontrolleriTemizle()
        {
            txtKitapEkle.Clear();
            cmbBox_Ekle_Yazar.SelectedIndex = -1;
            cmbBox_Ekle_Tur.SelectedIndex = -1;
            numericUpDown_Ekle_SayfaSayisi.Value = 0;
            numericUpDown_Ekle_Stok.Value = 0;
        }

        private void btnKitapSil_Click(object sender, EventArgs e)
        {
            try
            {
                if ((int)cmbBox_Sil_Kitap.SelectedValue <= 0)
                {
                    MessageBox.Show("Lütfen kitap seçimi yapınız!","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Hand);
                    return;

                }

                
                Kitap kitabim = myKitapORM.SelectET((int)cmbBox_Sil_Kitap.SelectedValue);


                DialogResult cevap = MessageBox.Show($"B kitabı silmek yerine pasifleştirmek ister misiniz? \n pasifleştirmek için --> Evete Tıklayınız \n Silmek için --> Hayır'a tıklayınız", "SİLME ONAY", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (cevap == DialogResult.Yes)
                {
                    //pasifleştirme işlemi update ile yapılmalıdır.
                    kitabim.SilindiMi = true;
                    switch (myKitapORM.Update(kitabim))
                    {
                        case true:
                            MessageBox.Show($"{kitabim.KitapAdi} sistemde pasifleştirildi");
                            //temizlik
                            SilmeSayfasiKontrolleriTemizle();
                            TumKitaplariSilComboyaGetir();
                            break;
                        case false:
                            throw new Exception($"HATA: {kitabim.KitapAdi} pasifleştirme işleminde beklenmedik bir hata oldu");
                            
                    }

                }
                else if (cevap == DialogResult.No)
                {
                    // Silme
                    //linq yazdık
                    var oduncListe = OduncIslemlerORM.Current.Select().Where(x => x.KitapId == kitabim.KitapId).ToList();

                    if (oduncListe.Count>0)
                    {
                        MessageBox.Show("Dikkat: Bu kitap ödünç alınmıştır. Silemezsiniz...", "BİLGİ",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    //Yukarıdaki if e girmezse return olmaz.
                    //Return olmazsa kod aşağı doğru okunmaya devam eder.

                    
                    if (myKitapORM.Delete(kitabim)) 
                    {
                        MessageBox.Show($"{kitabim.KitapAdi} adlı kitap silinmiştir.");
                        //temizlik 
                        SilmeSayfasiKontrolleriTemizle();
                        TumKitaplariSilComboyaGetir();
                    }
                    else
                    {
                        throw new Exception($"HATA:{kitabim.KitapAdi} silinmemiştir");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("HATA: Silme işleminde beklenmedik bir hata oldu." + ex.ToString());
            }
        }

        private void SilmeSayfasiKontrolleriTemizle()
        {
            cmbBox_Sil_Kitap.SelectedIndex = -1;
            richTextBoxKitap.Clear();
            
        }

        private void comboBoxKitapGuncelle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GuncelleSayfasiTemizle();
                if (comboBoxKitapGuncelle.SelectedIndex >=0)
                {
                    Kitap secilenKitap = myKitapORM.SelectET((int)comboBoxKitapGuncelle.SelectedValue);
                    txt_GuncelleKitapAdi.Text = secilenKitap.KitapAdi;
                    numericUpDown_Guncelle_SayfaSayisi.Value = secilenKitap.SayfaSayisi;
                    numericUpDown_Guncelle_Stok.Value = secilenKitap.Stok;


                    // null
                    if (secilenKitap.TurId==null)
                    {
                        //cmbBox_Guncelle_Tur.SelectedIndex = 0;
                        //cmbBox_Guncelle_Tur.SelectedValue = -1;
                        cmbBox_Guncelle_Tur.SelectedValue = ProgramBilgileri.TurYokSelectedValue;
                    }
                    else
                    {
                        cmbBox_Guncelle_Tur.SelectedValue = secilenKitap.TurId;
                    }
                    
                    cmbBox_Guncelle_Yazar.SelectedValue = secilenKitap.YazarId;

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show("HATA:" + ex.Message);
            }
        }
        private void GuncelleSayfasiTemizle()
        {
            txt_GuncelleKitapAdi.Text = string.Empty;
            numericUpDown_Guncelle_SayfaSayisi.Value = 0;
            numericUpDown_Guncelle_Stok.Value = 0;
            cmbBox_Guncelle_Tur.SelectedIndex = -1;
            cmbBox_Guncelle_Yazar.SelectedIndex = -1;
        }

        private void btnKitapGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxKitapGuncelle.SelectedIndex >= 0)
                {
                    if (numericUpDown_Guncelle_SayfaSayisi.Value <= 0)
                    {
                        throw new Exception("HATA: sayfa sayısı 0'dan büyük olmalıdır!");
                    }
                    if (numericUpDown_Guncelle_Stok.Value <= 0)
                    {
                        throw new Exception("HATA: kitap stoğu 0'dan büyük olmalıdır!");
                    }


                    Kitap secilenKitap = myKitapORM.SelectET((int)comboBoxKitapGuncelle.SelectedValue);
                    if (secilenKitap == null)
                    {
                        //1.yol
                        throw new Exception("HATA: Kitap Bulunamadı!");

                        ////2.yol
                        //MessageBox.Show("HATA: Kitap Bulunamadı!");
                        //return;


                    }

                    else

                        secilenKitap.KitapAdi = txt_GuncelleKitapAdi.Text.Trim();
                    secilenKitap.SayfaSayisi = (int)numericUpDown_Guncelle_SayfaSayisi.Value;
                    secilenKitap.Stok = (int)numericUpDown_Guncelle_Stok.Value;
                    secilenKitap.SilindiMi = false;
                    secilenKitap.YazarId = (int)cmbBox_Guncelle_Yazar.SelectedValue;

                    if ((int)cmbBox_Guncelle_Tur.SelectedValue == -1)
                    {
                        secilenKitap.TurId = null;
                    }

                    else
                    {
                        secilenKitap.TurId = (int)cmbBox_Guncelle_Tur.SelectedValue;
                    }
                    switch (myKitapORM.Update(secilenKitap))
                    {
                        case true:
                            MessageBox.Show($"{secilenKitap.KitapAdi} başarıyla güncellendi!");
                            //temizlik:
                            TumKitaplariGuncelleComboyaGetir();
                            break;
                        case false:
                            throw new Exception($"HATA: {secilenKitap.KitapAdi} güncellenirken beklenmedik bir hata oluştu! ");
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        private void cmbBox_Sil_Kitap_SelectedIndexChanged(object sender, EventArgs e)
        {
            //richtextbox dolsun
            //1.yöntem

            if (cmbBox_Sil_Kitap.SelectedIndex >= 0)
            {
                Kitap secilenKitap = myKitapORM.SelectET((int)cmbBox_Sil_Kitap.SelectedValue);

                if (secilenKitap != null)
                {
                    richTextBoxKitap.Clear();
                    //1. yol
                    richTextBoxKitap.Text = $"Kitap: {secilenKitap.KitapAdi}\n" +
                        $"Yazar: {myYazarORM.Select().FirstOrDefault(x => x.YazarId == secilenKitap.YazarId).YazarAdSoyad}\n" +
                        $"Sayfa Sayısı: {secilenKitap.SayfaSayisi}\n" +
                       $"Türü: {(secilenKitap.TurId == null ? "Türü Yok" : myTurlerORM.Select().FirstOrDefault(x => x.TurId == secilenKitap.TurId).TurAdi)} " +
                        $"Stok:{secilenKitap.Stok} adet stokta bulunmaktadır";

                    // Yukarıdaki turu değişkenini ve $ 'ı kullanmak istemezseniz aşağıdaki gibi yazarız


                }
            }

            //    // 2. yöntem
            //    if (cmbBox_Sil_Kitap.SelectedIndex >= 0)
            //    {
            //        KitapViewModel seciliKitap = myKitapORM.KitaplariViewModelleGetir().FirstOrDefault(x => x.KitapId == (int)cmbBox_Sil_Kitap.SelectedValue);
            //        if (seciliKitap != null)
            //        {
            //            richTextBoxKitap.Clear();
            //            richTextBoxKitap.Text = "Kitap: " + seciliKitap.KitapAdi
            //                + "\nTür:" + seciliKitap.TurAdi
            //                + "\nYazar: " + seciliKitap.YazarAdSoyad
            //                + "\n Sayfa Sayısı:" + seciliKitap.SayfaSayisi
            //                + "\n Stok: " + seciliKitap.Stok + " adet bulunmaktadır";
            //        }
            //    }

        }


        private void tabControl1_Click(object sender, EventArgs e)
        {
            // tablar değiştikçe temizlik yapılsın

            EkleSayfasiKontrolleriTemizle();
            comboBoxKitapGuncelle.SelectedIndex = -1;
            GuncelleSayfasiTemizle();
            SilmeSayfasiKontrolleriTemizle();

        }
    }
}
