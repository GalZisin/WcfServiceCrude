using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WcfWebApp
{
    public partial class Form1 : Form
    {
        private string baseUrl = "http://localhost:49737/SuperHeroService.svc";
        //private string baseUrl = "http://localhost/WcfWebHeroDB/SuperHeroService.svc";
        private readonly DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
        private readonly DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
        public Form1()
        {
            InitializeComponent();
            ConstructDataGridView();
        }
        private void ConstructDataGridView()
        {
            //ADD COLUMNS
            dataGridView1.ColumnCount = 7;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "First Name";
            dataGridView1.Columns[2].Name = "Last Name";
            dataGridView1.Columns[3].Name = "Hero Name";
            dataGridView1.Columns[4].Name = "Place Of Birth";
            dataGridView1.Columns[5].Name = "Combat Points";
            dataGridView1.Columns[6].Name = "Date Birth";

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AddEditButtonColumn();
            AddDeleteButtonColumn();
            dataGridView1.Columns.Add(btn1);
            dataGridView1.Columns.Add(btn2);
            AddRow();
        }
        private void AddRow()
        {
            foreach (SuperHero hero in GetAllHeroes())
            {
                dataGridView1.Rows.Add(hero.Id, hero.FirstName, hero.LastName, hero.HeroName, hero.PlaceOfBirth, hero.Combat, hero.DateBirth);
            }
        }
        private void AddEditButtonColumn()
        {
            btn1.HeaderText = @"";
            btn1.Name = "EditButton";
            btn1.Text = "Edit";
            btn1.UseColumnTextForButtonValue = true;
        }
        private void AddDeleteButtonColumn()
        {
            btn2.HeaderText = @"";
            btn2.Name = "DeleteButton";
            btn2.Text = "Delete";
            btn2.UseColumnTextForButtonValue = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "DeleteButton")
            {
                if (MessageBox.Show("Are you sure want to delete this record?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteHero();
                }

            }
            if (dataGridView1.Columns[e.ColumnIndex].Name == "EditButton")
            {
                EditHero();
            }
        }
        //private void showdata()
        //{
        //    List<SuperHero> HeroList = GetAllHeroes();
        //    dataGridView1.DataSource = HeroList;
        //    dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        //}
        private void Form1_Load(object sender, EventArgs e)
        {
            //showdata();
        }

        private List<SuperHero> GetAllHeroes()
        {
            //string URL = "http://localhost/WCFWebHero/SuperHeroService.svc/GetAllHeroes";
            //string URL = "http://localhost:49737/SuperHeroService.svc/GetAllHeroes";
            //string URL = "http://localhost/WcfWebHeroDB/SuperHeroService.svc/GetAllHeroes";
            string URL = baseUrl + "/GetAllHeroes";
            HttpClient client_get = new HttpClient();
            client_get.BaseAddress = new Uri(URL);
            string json = client_get.GetStringAsync("").Result;
            List<SuperHero> result = JsonConvert.DeserializeObject<List<SuperHero>>(json);
            return result;
        }
        private async void DeleteHero()
        {
            string heroId = (string)dataGridView1.CurrentRow.Cells[0].Value;
            //string URL = "http://localhost/WCFWebHero/SuperHeroService.svc/DeleteHero/" + heroId;
            //string URL = "http://localhost:49737/SuperHeroService.svc/DeleteHero/" + heroId;
            //string URL = "http://localhost/WcfWebHeroDB/SuperHeroService.svc/DeleteHero/" + heroId;
            string URL = baseUrl + "/DeleteHero/" + heroId;
            HttpClient client_delete = new HttpClient();
            client_delete.BaseAddress = new Uri(URL);
            var response_post = await client_delete.DeleteAsync(URL);
            dataGridView1.Rows.Clear();
            ConstructDataGridView();
        }
        //private async void UpdateHero()
        //{

        //}
        private async void Update_Click(object sender, EventArgs e)
        {
            string heroId = (string)dataGridView1.CurrentRow.Cells[0].Value;
            //string URL = " http://localhost:49737/SuperHeroService.svc/UpdateHero/" + heroId;
            //string URL = "http://localhost/WCFWebHero/SuperHeroService.svc/UpdateHero/" + heroId;
            //string URL = "http://localhost/WcfWebHeroDB/SuperHeroService.svc/UpdateHero/" + heroId;
            string URL = baseUrl + "/UpdateHero/" + heroId;
            HttpClient client_update = new HttpClient();
            client_update.BaseAddress = new Uri(URL);
            SuperHero hero = new SuperHero();
            {
                hero.Id = (string)dataGridView1.CurrentRow.Cells[0].Value;
                hero.FirstName = textBoxFirstName.Text;
                hero.LastName = textBoxLastName.Text;
                hero.HeroName = textBoxHeroName.Text;
                hero.PlaceOfBirth = textBoxPlaceOfBirth.Text;
                hero.Combat = textBoxCombatPoints.Text;
                hero.DateBirth = textBoxDateBirth.Text;
            };
            var json = JsonConvert.SerializeObject(hero);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response_put = await client_update.PutAsync(URL, data);
            dataGridView1.Rows.Clear();
            ConstructDataGridView();
            textBoxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxHeroName.Text = "";
            textBoxPlaceOfBirth.Text = "";
            textBoxCombatPoints.Text = "";
            textBoxDateBirth.Text = "";
        }
        private void EditHero()
        {
            int i = dataGridView1.SelectedCells[0].RowIndex;
            textBoxFirstName.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
            textBoxLastName.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
            textBoxHeroName.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
            textBoxPlaceOfBirth.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
            textBoxCombatPoints.Text = dataGridView1.Rows[i].Cells[5].Value.ToString();
            textBoxDateBirth.Text = dataGridView1.Rows[i].Cells[6].Value.ToString();
        }
        private async void AddHero_Click(object sender, EventArgs e)
        {
            //string URL = "http://localhost:49737/SuperHeroService.svc/AddHero";
            //string URL = "http://localhost/WCFWebHero/SuperHeroService.svc/AddHero";
            //string URL = "http://localhost/WcfWebHeroDB/SuperHeroService.svc/AddHero";
            string URL = baseUrl + "/AddHero";
            HttpClient client_post = new HttpClient();

            client_post.BaseAddress = new Uri(URL);

            SuperHero hero = new SuperHero();
            {
                hero.Id = "0";
                hero.FirstName = textBoxFirstName.Text;
                hero.LastName = textBoxLastName.Text;
                hero.HeroName = textBoxHeroName.Text;
                hero.PlaceOfBirth = textBoxPlaceOfBirth.Text;
                hero.Combat = textBoxCombatPoints.Text;
                hero.DateBirth = textBoxDateBirth.Text;
            };

            var json = JsonConvert.SerializeObject(hero);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response_post = await client_post.PostAsync(URL, data);
            dataGridView1.Rows.Clear();
            ConstructDataGridView();
        }


    }
    public class SuperHero
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string HeroName { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Combat { get; set; }
        public string DateBirth { get; set; }
    }
}
