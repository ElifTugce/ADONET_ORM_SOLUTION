using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADONET_ORM_Common;
using ADONET_ORM_Entities.Entities;

namespace ADONET_ORM_BLL
{
    public class TurlerORM : ORMBase<Tur, TurlerORM>
    {

        public List<Tur> TurleriGetir()
        {
            try
            {
                List<Tur> list = new List<Tur>();

                Tur t = new Tur()
                {
                    TurId = -1,
                    TurAdi = "Türü Yok",
                    GuncellenmeTarihi = DateTime.Now
                };
                list.Add(t);

                list.AddRange(this.Select());

                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


    }
}
