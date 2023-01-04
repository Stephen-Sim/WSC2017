using Module2.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module2
{
    public partial class EditScheduleForm : Form
    {
        WSC2017_Session2Entities ent = new WSC2017_Session2Entities();

        public int scheduleId { get; set; }
        public EditScheduleForm(int scheduleId)
        {
            InitializeComponent();
            this.scheduleId = scheduleId;
        }

        PrivateFontCollection pfc = new PrivateFontCollection();

        private void EditScheduleForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(25, 106, 166);

            pfc.AddFontFile(Application.StartupPath + "\\font.TTF");
            this.panel1.Font = new Font(pfc.Families[0], 9f);

            var schedule = ent.Schedules.FirstOrDefault(x => x.ID == scheduleId);
            label2.Text = schedule.Route.Airport.IATACode;
            label4.Text = schedule.Route.Airport1.IATACode;
            label6.Text = schedule.Aircraft.Name;

            dateTimePicker1.Value = schedule.Date;
            dateTimePicker2.Value = Convert.ToDateTime(schedule.Time.ToString());
            textBox1.Text = schedule.EconomyPrice.ToString("0.00");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var forms = (Form1) Application.OpenForms["Form1"];
            forms.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var economyPrice = decimal.Parse(textBox1.Text);
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    MessageBox.Show("Economy Price could not be null!!");
                    return;
                }

                var dr = MessageBox.Show("Are you sure to update the schedule?", "Alert", MessageBoxButtons.YesNo);
                
                if (dr == DialogResult.Yes)
                {
                    var schedule = ent.Schedules.FirstOrDefault(x => x.ID == scheduleId);
                    schedule.Date = dateTimePicker1.Value.Date;
                    schedule.Time = dateTimePicker2.Value.TimeOfDay;
                    schedule.EconomyPrice = economyPrice;
                    ent.SaveChanges();

                    this.Hide();
                    new Form1().ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Invalid Input for economy price!");
                return;
            }
        }
    }
}
