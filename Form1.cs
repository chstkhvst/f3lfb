using BLL.DTO;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows.Forms;

namespace Lab3Layers
{
    public partial class Form1 : Form
    {
        List<ClientDto> allClients;
        List<ContractDto> allContracts;
        List<EmployeeDto> allEmployees;
        List<CountryDto> allCountries;
        List<TourDto> allTours;
        ClientService clientService = new ClientService();
        ContractService contractService = new ContractService();
        public Form1()
        {
            InitializeComponent();
            loadForm();
            
        }
        private void loadForm()
        {
            comboBox1.DataSource = contractService.GetCountries();
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            allEmployees = contractService.GetEmployees();
            allTours = contractService.GetTours();
            loadClients();
            loadContracts();
            //((DataGridViewComboBoxColumn)dataGridView2.Columns["genderDataGridViewTextBoxColumn"]).Items.AddRange("М", "Ж");
        }
        private void loadClients()
        {
            allClients = clientService.GetAllClients();
            bindingSource2.DataSource = allClients;
            ((DataGridViewComboBoxColumn)dataGridView2.Columns["gendercolumn"]).Items.AddRange("М", "Ж");
        }
        private void loadContracts()
        {
            allContracts = contractService.GetAllContracts();
            bindingSource1.DataSource = allContracts;
            ((DataGridViewComboBoxColumn)dataGridView1.Columns["id_employee"]).DataSource = allEmployees;
            ((DataGridViewComboBoxColumn)dataGridView1.Columns["id_employee"]).DisplayMember =
                "fullname";
            ((DataGridViewComboBoxColumn)dataGridView1.Columns["id_employee"]).ValueMember =
                "id";

            ((DataGridViewComboBoxColumn)dataGridView1.Columns["id_client"]).DataSource =
              allClients;
            ((DataGridViewComboBoxColumn)dataGridView1.Columns["id_client"]).DisplayMember =
                "fullname";
            ((DataGridViewComboBoxColumn)dataGridView1.Columns["id_client"]).ValueMember =
                "id";
        }

        //адд
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.comboBox1.DataSource = allEmployees;
            f.comboBox1.DisplayMember = "fullname";
            f.comboBox1.ValueMember = "id";

            f.comboBox2.DataSource = allClients;
            f.comboBox2.DisplayMember = "fullname";
            f.comboBox2.ValueMember = "id";

            f.comboBox3.DataSource = allTours;
            f.comboBox3.DisplayMember = "id";
            f.comboBox3.ValueMember = "id";

            DialogResult result = f.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            ContractDto contr = new ContractDto();
            contr.id_employee = (int)f.comboBox1.SelectedValue;
            contr.sign_date = f.dateTimePicker1.Value;
            contr.price = f.numericUpDown1.Value;
            contr.id_client = (int)f.comboBox2.SelectedValue;
            contr.id_tour = (int)f.comboBox3.SelectedValue;
            contractService.CreateContract(contr);
            allContracts = contractService.GetAllContracts();
            bindingSource1.DataSource = allContracts;
            //loadContracts();
            MessageBox.Show("Новый объект добавлен");
        }
        private int getSelectedRow(DataGridView dataGridView)
        {
            int index = -1;
            if (dataGridView.SelectedRows.Count > 0 || dataGridView.SelectedCells.Count == 1)
            {
                if (dataGridView.SelectedRows.Count > 0)
                    index = dataGridView.SelectedRows[0].Index;
                else index = dataGridView.SelectedCells[0].RowIndex;
            }
            return index;
        }

        //upd contr
        private void button2_Click(object sender, EventArgs e)
        {
            int index = getSelectedRow(dataGridView1);
            if (index != -1)
            {
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                ContractDto contr = allContracts.Where(i => i.id == id).FirstOrDefault();
                if (contr != null)
                {
                    Form2 f = new Form2();
                    f.comboBox1.DataSource = allEmployees;
                    f.comboBox1.DisplayMember = "fullname";
                    f.comboBox1.ValueMember = "id";

                    f.comboBox2.DataSource = allClients;
                    f.comboBox2.DisplayMember = "fullname";
                    f.comboBox2.ValueMember = "id";

                    f.comboBox3.DataSource = allTours;
                    f.comboBox3.DisplayMember = "id";
                    f.comboBox3.ValueMember = "id";

                    f.comboBox1.SelectedValue = contr.id_employee;
                    f.comboBox2.SelectedValue = contr.id_client;
                    f.comboBox3.SelectedValue = contr.id_tour;
                    f.dateTimePicker1.Value = contr.sign_date;
                    f.numericUpDown1.Value = (decimal)contr.price;

                    DialogResult result = f.ShowDialog(this);

                    if (result == DialogResult.Cancel)
                        return;

                    contr.id_employee = (int)f.comboBox1.SelectedValue;
                    contr.sign_date = f.dateTimePicker1.Value;
                    contr.price = f.numericUpDown1.Value;
                    contr.id_client = (int)f.comboBox2.SelectedValue;
                    contr.id_tour = (int)f.comboBox3.SelectedValue;

                    contractService.UpdateContract(contr);
                    allContracts = contractService.GetAllContracts();
                    bindingSource1.DataSource = allContracts;
                    MessageBox.Show("Объект обновлен");
                }
            }
            else
                MessageBox.Show("Ни один объект не выбран!");
        }

        // delete contr
        private void button3_Click(object sender, EventArgs e)
        {
            int index = getSelectedRow(dataGridView1);
            if (index != -1)
            {
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;
                contractService.DeleteContract(id);
                bindingSource1.DataSource = contractService.GetAllContracts();
            }
        }

        //клиент адд
        private void button4_Click(object sender, EventArgs e)
        {
            ClientForm f = new ClientForm();

            DialogResult result = f.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;

            ClientDto cl = new ClientDto();
            cl.fullname = f.textBox1.Text;
            cl.passport = f.textBox2.Text; 
            cl.international_passport = f.textBox3.Text;
            cl.birthday = f.dateTimePicker1.Value;
            cl.gender = f.comboBox1.SelectedItem.ToString();
            cl.discount = f.numericUpDown1.Value;

            clientService.CreateClient(cl);
            allClients = clientService.GetAllClients();
            bindingSource2.DataSource = allClients;

            MessageBox.Show("Новый объект добавлен");
        }

        // client upd
        private void button5_Click(object sender, EventArgs e)
        {
            int index = getSelectedRow(dataGridView2);
            if (index != -1)
            {
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                ClientDto cl = allClients.Where(i => i.id == id).FirstOrDefault();
                if (cl != null)
                {
                    ClientForm f = new ClientForm();

                    f.comboBox1.SelectedItem = cl.gender;
                    f.textBox1.Text = cl.fullname;
                    f.textBox2.Text = cl.passport;
                    f.textBox3.Text = cl.international_passport;
                    f.dateTimePicker1.Value = cl.birthday;
                    f.numericUpDown1.Value = (decimal)cl.discount;

                    DialogResult result = f.ShowDialog(this);

                    if (result == DialogResult.Cancel)
                        return;

                    cl.fullname = f.textBox1.Text;
                    cl.passport = f.textBox2.Text;
                    cl.international_passport = f.textBox3.Text;
                    cl.birthday = f.dateTimePicker1.Value;
                    cl.gender = f.comboBox1.SelectedItem.ToString();
                    cl.discount = f.numericUpDown1.Value;

                    clientService.UpdateClient(cl);
                    allClients = clientService.GetAllClients();
                    bindingSource2.DataSource = allClients;
                    MessageBox.Show("Объект обновлен");
                }
            }
            else
                MessageBox.Show("Ни один объект не выбран!");
        }

        //клмент делит
        private void button6_Click(object sender, EventArgs e)
        {
            int index = getSelectedRow(dataGridView2);
            if (index != -1)
            {
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;
                clientService.DeleteClient(id);
                bindingSource2.DataSource = clientService.GetAllClients();
            }
        }

        // заключить контракт
        private void button7_Click(object sender, EventArgs e)
        {
            ContractForm f = new ContractForm();
            if (f.ShowDialog() == DialogResult.OK)
                bindingSource1.DataSource = contractService.GetAllContracts();
        }

        // report1
        private void button8_Click(object sender, EventArgs e)
        {
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            var result = ReportService.GetContractsByEmployee(startDate, endDate);
            dataGridView3.DataSource = result;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string country = comboBox1.Text;
            //string countryName = dbcontext.country
            //                              .Where(c => c.id == countryId)
            //                              .Select(c => c.name)
            //                              .FirstOrDefault();
            dataGridView4.DataSource = ReportService.GetToursByCountry(country);
        }
    }
}
