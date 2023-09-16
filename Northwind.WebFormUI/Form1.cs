using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Northwind.Business.Concrete;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;

namespace Northwind.WebFormUI
{
    public partial class Form1 : Form
    {
        private readonly ProductManager _productService;
        private readonly CategoryManager _categoryService;
        public Form1()
        {
            InitializeComponent();
            _productService = new ProductManager(new EfProductDal());
            _categoryService = new CategoryManager(new EfCategoryDal());
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProducts();

            LoadCategories();
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategoryId.DataSource = _categoryService.GetAll();
            cbxCategoryId.DisplayMember = "CategoryName";
            cbxCategoryId.ValueMember = "CategoryId";

            cbxUpdateCategoryId.DataSource = _categoryService.GetAll();
            cbxUpdateCategoryId.DisplayMember = "CategoryName";
            cbxUpdateCategoryId.ValueMember = "CategoryId";
        }

        private void LoadProducts()
        {
            dgvProduct.DataSource = _productService.GetAll();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
           if(cbxCategory.SelectedValue is Int32)
             dgvProduct.DataSource = _productService.
                 GetByCategory(Convert.ToInt32(cbxCategory.SelectedValue));

        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            dgvProduct.DataSource = _productService.GetByProductName(tbxProductName.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _productService.Add(new Product
            {
                CategoryId = Convert.ToInt32(cbxCategory.SelectedValue),
                ProductName = tbxAddProductName.Text,
                QuantityPerUnit = tbxQuantityPerUnit.Text,
                UnitPrice = Convert.ToDecimal(tbxUnitPrice.Text),
                UnitsInStock = Convert.ToInt16(tbxUnitsInStock.Text)
            });
            MessageBox.Show("Product Added!");
            LoadProducts();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            _productService.Update(new Product
            {
                ProductId = Convert.ToInt32(dgvProduct.CurrentRow.Cells[0].Value),
                CategoryId = Convert.ToInt32(cbxUpdateCategoryId.SelectedValue),
                ProductName = tbxUpdateProductName.Text,
                QuantityPerUnit = tbxUpdateQuatityPerUnit.Text,
                UnitPrice = Convert.ToDecimal(tbxUpdateUnitPrice.Text),
                UnitsInStock = Convert.ToInt16(tbxUpdateUnitsInStock.Text)
                
            });
            MessageBox.Show("Product Updated!");
            LoadProducts();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbxUpdateProductName.Text = dgvProduct.CurrentRow.Cells[1].Value.ToString();
            cbxUpdateCategoryId.SelectedValue = dgvProduct.CurrentRow.Cells[2].Value;
            tbxUpdateUnitPrice.Text = dgvProduct.CurrentRow.Cells[3].Value.ToString();  
            tbxUpdateQuatityPerUnit.Text = dgvProduct.CurrentRow.Cells[4].Value.ToString();
            tbxUpdateUnitsInStock.Text = dgvProduct.CurrentRow.Cells[5].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProduct.CurrentRow != null)
                {
                    _productService.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(dgvProduct.CurrentRow.Cells[0].Value)
                    });
                }

                MessageBox.Show("Product Deleted!");
                LoadProducts();
            }
            catch (Exception exception)
            {
               MessageBox.Show(exception.Message);
            }
           
        }
    }
}
