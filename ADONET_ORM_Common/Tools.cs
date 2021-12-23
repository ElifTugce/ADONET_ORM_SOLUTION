using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace ADONET_ORM_Common
{
    public static class Tools
    {

        public static string SQLConnectionStringSentence { get; set; } = "Server=DESKTOP-HNE43R2; Database=OKULKITAPLIGI; Trusted_Connection=True;";

        private static SqlConnection _mySqlDBConnection;
        public static SqlConnection MySqlDBConnection
        {

            get
            {
                if (_mySqlDBConnection == null)
                {
                    _mySqlDBConnection = new SqlConnection(SQLConnectionStringSentence);
                }
                return _mySqlDBConnection;
            }

            set
            {
                _mySqlDBConnection = value;
            }
        }

        public static void OpenTheConnection()
        {
            try
            {
                if (MySqlDBConnection.State != ConnectionState.Open)
                {
                    MySqlDBConnection.ConnectionString = SQLConnectionStringSentence;
                    MySqlDBConnection.Open();

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //static bir metot yani classtan nesne türetilmeden kullanılabilir.
        // geriye dönüş tipi List<ET> örn; List<Kİtap>
        // this DataTable dt diyerek metot binding yapmaktayız.
        // Bu metodu DataTable nesnelerinde EXTENSION metot olarak göreceğiz
        // Yani DataTable mydt= new   DataTable () ile nesne oluşturduktan sonra
        // mydt. dediğimizde List<ET> metodunu görebiliriz.

        public static List<ET> ToList<ET>(this DataTable dt)
            where ET : class, new()
        {
            Type type = typeof(ET);
            List<ET> list = new List<ET>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (DataRow rowitem in dt.Rows)
            {
                ET myET = new ET();

                foreach (PropertyInfo propertyitem in propertyInfos)
                {
                    object theobject = rowitem[propertyitem.Name];
                    if (theobject != null && theobject.ToString().Length > 0)
                    {
                        propertyitem.SetValue(myET, theobject);
                    }
                }

                list.Add(myET);

            }


            return list;
        }

        public static ET ToET<ET>(this DataTable dt) where ET : class, new()
        {
            Type type = typeof(ET);
            ET entity = new ET();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (DataRow rowitem in dt.Rows)
            {
                foreach (var propertyitem in propertyInfos)
                {
                    object theobject = rowitem[propertyitem.Name];
                    if (theobject != null && theobject.ToString().Length>0)
                    {
                        propertyitem.SetValue(entity, theobject);
                    }
                }

            }
            return entity;

        }
    }
}
