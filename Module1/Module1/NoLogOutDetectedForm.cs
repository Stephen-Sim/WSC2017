using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module1
{
    public partial class NoLogOutDetectedForm : Form
    {
        public Log log { get; set; }
        public NoLogOutDetectedForm(Log log)
        {
            InitializeComponent();
            this.log = log;
            label1.Text = $"no logout deteceted login on {log.LogInTime.Date.ToString("dd/MM/yyyy")} at {log.LogInTime.ToString("HH : mm : ss")}";
        }

        WSC2017_Module1Entities ent = new WSC2017_Module1Entities();

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("The reason is required!!", "Alert");
                return;
            }

            if (radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("Crash reason is required!!!", "Alert");
                return;
            }

            var log = ent.Logs.FirstOrDefault(x => x.Id == this.log.Id);
            log.NotProperLogoutReason = textBox1.Text;
            log.CrashType = radioButton1.Checked ? 1 : 2;
            ent.SaveChanges();

            this.Hide();
        }
    }
}
