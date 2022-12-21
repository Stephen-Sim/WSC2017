using Module2.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ent = new WSC2017_Session2Entities();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2017_Session2DataSet.Airports' table. You can move, or remove it, as needed.
            this.airportsTableAdapter.Fill(this.wSC2017_Session2DataSet.Airports);

            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;

            dateTimePicker1.Value = new DateTime(2017, 10, 4);
        }

        public WSC2017_Session2Entities ent { get; set; }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 ||
                comboBox3.SelectedIndex == -1 || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("All the feilds are required!!", "Alert", MessageBoxButtons.OK);
                return;
            }

            if (comboBox2.SelectedIndex == comboBox1.SelectedIndex)
            {
                MessageBox.Show("Departure airport and arrival airport cant be the same!!", "Alert", MessageBoxButtons.OK);
                return;
            }

            dataGridView1.Rows.Clear();

            var allData = ent.Schedules.ToList().Where(x => 
                x.Route.DepartureAirportID == (int)comboBox1.SelectedValue &&
                x.Route.ArrivalAirportID == (int)comboBox2.SelectedValue &&
                x.Date == dateTimePicker1.Value.Date &&
                x.FlightNumber == textBox1.Text.Trim()
            ).ToList();

            if (comboBox3.SelectedIndex == 0)
            {
                allData = allData.OrderByDescending(x => x.Date).ThenByDescending(x => x.Time).ToList();
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                allData = allData.OrderByDescending(x => x.EconomyPrice).ToList();
            }
            else if (comboBox3.SelectedIndex == 3)
            {
                allData = allData.OrderByDescending(x => x.Confirmed).ToList();
            }

            for (int i = 0; i < allData.Count; i++)
            {
                dataGridView1.Rows.Add(
                    allData[i].ID,
                    allData[i].Date.ToString("dd/MM/yyyy"),
                    $"{allData[i].Time.Hours.ToString("00")} : {allData[i].Time.Minutes.ToString("00")}",
                    allData[i].Route.Airport.IATACode,
                    allData[i].Route.Airport1.IATACode,
                    allData[i].FlightNumber,
                    allData[i].Aircraft.Name,
                    $"${allData[i].EconomyPrice.ToString("0")}",
                    $"${((double)allData[i].EconomyPrice * 1.35).ToString("0")}",
                    $"${((double)allData[i].EconomyPrice * 1.35 * 1.3).ToString("0")}");

                if (!allData[i].Confirmed)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;

                var scheduleId = dataGridView1.Rows[e.RowIndex].Cells[0].Value;

                var schedule = ent.Schedules.FirstOrDefault(x => x.ID == (int)scheduleId);

                if (!schedule.Confirmed)
                {
                    button1.Text = "✅ Confirm Flight";
                }
                else
                {
                    button1.Text = "❌ Cancel Flight";
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Are you sure to confirm or cancel the flight?", "alert", MessageBoxButtons.YesNo);
            
            if (dr == DialogResult.Yes)
            {
                var scheduleId = dataGridView1.SelectedRows[0].Cells[0].Value;
                var schedule = ent.Schedules.FirstOrDefault(x => x.ID == (int)scheduleId);

                schedule.Confirmed = !schedule.Confirmed;
                ent.SaveChanges();

                button4_Click(null, null);
            }
        }
    }
}
