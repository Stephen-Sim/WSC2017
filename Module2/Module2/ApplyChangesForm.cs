using Module2.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module2
{
    public partial class ApplyChangesForm : Form
    {
        public WSC2017_Session2Entities ent { get; set; } = new WSC2017_Session2Entities();
        public ApplyChangesForm()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            using (var op = new OpenFileDialog())
            {
                // op.Filter = "CSV File (*.csv)|*.csv|ALL Files|*.*";
                op.Filter = "CSV File (*.csv)|*.csv";
                op.ShowDialog();

                textBox1.Text = op.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("No file is selected!!", "Alert");
                return;
            }

            var stream = new StreamReader(textBox1.Text);

            int succ, dup, miss;
            succ = dup = miss = 0;

            // ADD,10/4/2017,6:25,75,AUH,RUH,1,983,OK
            string str = string.Empty;
            while ((str = stream.ReadLine()) != null)
            {
                string[] data = str.Split(',');

                if (data.Length != 9)
                {
                    miss++;
                    continue;
                }

                if (data[0] != "ADD" || data[0] == "EDIT")
                {
                    miss++;
                    continue;
                }

                var date = new DateTime();

                try
                {
                    date = DateTime.Parse(data[1]);
                }
                catch
                {
                    miss++;
                    continue;
                }

                var time = new DateTime();

                try
                {
                    time = Convert.ToDateTime(data[2]);
                }
                catch
                {
                    miss++;
                    continue;
                }

                string flgihtno = data[3];
                string a1 = data[4];
                string a2 = data[5];
                var aircarft = int.Parse(data[6]);
                var price = decimal.Parse(data[7]);
                bool status = data[8] == "OK" ? true : false;

                if (data[0] == "ADD")
                {
                    var route = ent.Routes.Where(x => x.Airport.IATACode == a1 && x.Airport1.IATACode == a2).FirstOrDefault();

                    if (route == null)
                    {
                        miss++;
                        continue;
                    }

                    var schedule = new Schedule
                    {
                        Date = date,
                        Time = time.TimeOfDay,
                        FlightNumber = flgihtno,
                        RouteID = route.ID,
                        AircraftID = aircarft,
                        EconomyPrice = price,
                        Confirmed = status,
                    };

                    var ifExist = ent.Schedules
                        .Where(x =>
                            x.RouteID == schedule.RouteID &&
                            x.Date == schedule.Date &&
                            x.Time == schedule.Time).FirstOrDefault();

                    if (ifExist != null)
                    {
                        dup++;
                        continue;
                    }

                    succ++;
                    ent.Schedules.Add(schedule);
                    ent.SaveChanges();
                }
                else if (data[0] == "EDIT")
                {
                    var route = ent.Routes.Where(x => x.Airport.IATACode == a1 && x.Airport1.IATACode == a2).FirstOrDefault();

                    if (route == null)
                    {
                        miss++;
                        continue;
                    }

                    var temp = new Schedule
                    {
                        Date = date,
                        Time = time.TimeOfDay,
                        FlightNumber = flgihtno,
                        RouteID = route.ID,
                        AircraftID = aircarft,
                        EconomyPrice = price,
                        Confirmed = status,
                    };

                    var schedule = ent.Schedules
                        .Where(x =>
                            x.RouteID == temp.RouteID &&
                            x.Date == temp.Date &&
                            x.Time == temp.Time).FirstOrDefault();

                    if (schedule != null)
                    {
                        miss++;
                        continue;
                    }

                    schedule = temp;

                    succ++;
                    ent.SaveChanges();
                }
            }

            // 3, 6, 7
            label3.Text = succ.ToString();
            label6.Text = dup.ToString();
            label7.Text = miss.ToString();
        }

        private void ApplyChangesForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(25, 106, 166);
        }
    }
}
