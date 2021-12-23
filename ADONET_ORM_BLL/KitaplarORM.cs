using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADONET_ORM_Common;
using ADONET_ORM_Entities.Entities;
using ADONET_ORM_Entities.ViewModels;

namespace ADONET_ORM_BLL
{
    public class KitaplarORM:ORMBase<Kitap,KitaplarORM>
    {
        TurlerORM myturORM = new TurlerORM();
        YazarlarORM myYazarORM = new YazarlarORM();


        public List<KitapViewModel> KitaplariViewModelleGetir()
        {
            try
            {
                List<KitapViewModel> returnList = new List<KitapViewModel>();

                List<Kitap> kitaplar = this.Select();
                var yazarlar = myYazarORM.Select();
                List<Tur> turler = myturORM.Select();


                foreach (Kitap item in kitaplar)
                {
                    KitapViewModel kitap = new KitapViewModel()
                    {
                        KitapId=item.KitapId,
                        KitapAdi=item.KitapAdi,
                        KayitTarihi=item.KayitTarihi,
                        SayfaSayisi=item.SayfaSayisi,
                        SilindiMi=item.SilindiMi,
                        Stok=item.Stok,
                        YazarId=item.YazarId
                    };

                    //linq 
                    kitap.YazarAdSoyad = yazarlar.Find(x => x.YazarId == item.YazarId)?.YazarAdSoyad;

                    kitap.TurId = turler.Find(x => x.TurId == item.TurId)?.TurId;

                    kitap.TurAdi = item.TurId == null ? "Türü Yok": 
                        turler.Find(x=>x.TurId==item.TurId).TurAdi;


                    returnList.Add(kitap);
                }


                return returnList;

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

    }
}
