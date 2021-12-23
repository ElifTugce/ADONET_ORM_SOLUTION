using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADONET_ORM_Entities.ViewModels
{
    public class KitapViewModel
    {
        public int KitapId { get; set; }
        public DateTime KayitTarihi { get; set; }
        public string KitapAdi { get; set; }
        public int SayfaSayisi { get; set; }
        public bool SilindiMi { get; set; }
        public int? TurId { get; set; }
        public int YazarId { get; set; }
        public int Stok { get; set; }
        public string YazarAdSoyad { get; set; }
        public string TurAdi { get; set; }

    }
}
