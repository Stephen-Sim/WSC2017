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
    public partial class UserForm : Form
    {
        public User user { get; set; }
        public Log log { get; set; }

        public UserForm(User user, Log log)
        {
            InitializeComponent();
            this.user = user;
            this.log = log;
        }

        private async void SpentTime()
        {
            var spenttime = DateTime.Parse((DateTime.Now - log.LogInTime).ToString()).ToString("HH : mm : ss");
            label3.Text = spenttime.ToString();
            await Task.Delay(1000);
            SpentTime();
        }

        private void loadData()
        {
            label1.Text = $"Hi, {user.FirstName} {user.LastName}, Welcome to AMONIC airlines";
            label5.Text = user.Logs.ToList().Where(x => x.LogoutTime == null).ToList().Count().ToString();

            dataGridView1.Rows.Clear();

            var item = user.Logs.OrderByDescending(x => x.LogInTime).ToList();

            for (int i = 0; i < item.Count; i++)
            {
                dataGridView1.Rows.Add(
                    item[i].LogInTime.Date.ToString("MM/dd/yyyy"),
                    item[i].LogInTime.ToString("HH : mm"),
                    item[i].LogoutTime == null ? "--" : DateTime.Parse(item[i].LogoutTime.ToString()).ToString("HH : mm"),
                    item[i].LogoutTime == null ? "--" : DateTime.Parse((item[i].LogoutTime - item[i].LogInTime).ToString()).ToString("HH : mm : ss"),
                    item[i].LogoutTime == null ? item[i].NotProperLogoutReason : string.Empty 
                    );


                if (item[i].LogoutTime == null)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        WSC2017_Module1Entities ent = new WSC2017_Module1Entities();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var log = ent.Logs.FirstOrDefault(x => x.Id == this.log.Id);
            log.LogoutTime = DateTime.Now;
            ent.SaveChanges();
            this.Close();

            var form = (Form1)Application.OpenForms["Form1"];
            form.Show();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            SpentTime();
            loadData();
        }
    }
}
