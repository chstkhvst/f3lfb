using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using BLL.DTO;
using BLL.Services;

namespace Lab3Layers
{
    public partial class ContractForm : Form
    {
        ContractService cService = new ContractService();
        ClientService clientService = new ClientService();

        public ContractForm()
        {
            InitializeComponent();
            this.comboBox2.DataSource = cService.GetEmployees();
            this.comboBox2.DisplayMember = "fullname";
            this.comboBox2.ValueMember = "id";

            this.comboBox1.DataSource = clientService.GetAllClients();
            this.comboBox1.DisplayMember = "fullname";
            this.comboBox1.ValueMember = "id";

            this.comboBox3.DataSource = cService.GetTours();
            this.comboBox3.DisplayMember = "id";
            this.comboBox3.ValueMember = "id";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.comboBox1.SelectedValue == null || this.comboBox2.SelectedValue == null || this.comboBox3.SelectedValue == null)
            {
                MessageBox.Show("Ошибка при создании контракта");
            }
            ContractDto contr = new ContractDto()
            {
                id_client = (int)comboBox1.SelectedValue,
                id_employee = (int)comboBox2.SelectedValue,
                id_tour = (int)comboBox3.SelectedValue,

            };
            bool res = cService.MakeContract(contr);
            if (res)
            {
                MessageBox.Show("Success");
            }
            else MessageBox.Show("Failed");

        }
    }
}
