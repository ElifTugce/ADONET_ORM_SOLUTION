using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ADONET_ORM_Entities.Entities;
using ADONET_ORM_BLL;

namespace ADONET_ORM_FORMUI
{
    public partial class FormCategories : Form
    {
        public FormCategories()
        {
            InitializeComponent();
        }

        //Global alan
        List<Category> categoryList = new List<Category>();
        CategoriesORM myCategoriesORM = new CategoriesORM();

        private void FormCategories_Load(object sender, EventArgs e)
        {
            TumKategorileriGrideDoldur();
        }
        private void TumKategorileriGrideDoldur()
        {
            categoryList = myCategoriesORM.Select();

            dataGridViewCategories.DataSource = categoryList;


        }
    }
}
