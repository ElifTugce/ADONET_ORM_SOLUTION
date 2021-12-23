using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADONET_ORM_Common;


namespace ADONET_ORM_Entities.Entities
{
    [Table(TableName ="Turler", IdentityColumn ="TurId",PrimaryColumn ="TurId")]
    public class Tur
    {
        public int TurId { get; set; }
        public string TurAdi { get; set; }
        public DateTime GuncellenmeTarihi { get; set; }

    }
}
