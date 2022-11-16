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
    public partial class AdminForms : Form
    {
        public AdminForms(User user, Log log)
        {
            InitializeComponent();
            User = user;
            this.log = log;
            this.loadData(null);
        }

        public User User { get; set; }
        public Log log { get; set; }

        WSC2017_Module1Entities ent = new WSC2017_Module1Entities();

        private void loadData(int? id)
        {
            var users = id == null ? ent.Users.Where(x => x.ID != User.ID).ToList() : ent.Users.Where(x => x.ID != User.ID && x.OfficeID == id).ToList();

            dataGridView1.Rows.Clear();

            for (int i = 0; i < users.Count; i++)
            {
                dataGridView1.Rows.Add(
                    users[i].ID,
                    users[i].FirstName,
                    users[i].LastName,
                    (DateTime.Now.Year - users[i].Birthdate.Value.Year).ToString(),
                    users[i].Role.Title,
                    users[i].Email,
                    users[i].Office.Title
                    );

                if (users[i].Active == false)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
                else if (users[i].RoleID == 1)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }
        private void AdminForms_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2017_Module1DataSet.Offices' table. You can move, or remove it, as needed.
            this.officesTableAdapter.Fill(this.wSC2017_Module1DataSet.Offices);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var log = ent.Logs.FirstOrDefault(x => x.Id == this.log.Id);
            log.LogoutTime = DateTime.Now;
            ent.SaveChanges();
            this.Close();

            var form = (Form1)Application.OpenForms["Form1"];
            form.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var officeId = (int)comboBox1.SelectedValue;
            this.loadData(officeId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var userId = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                var user = ent.Users.FirstOrDefault(x => x.ID == userId);
                var dr = MessageBox.Show("Are yuo sure to " + (user.Active == true ? "Disable" : "Enable") + " this user login?", "alert", MessageBoxButtons.YesNo);
                
                if (dr == DialogResult.Yes)
                {

                    user.Active = !user.Active;
                    ent.SaveChanges();

                    if (comboBox1.SelectedIndex != -1)
                    {
                        this.loadData(null);
                    }
                    else
                        this.loadData((int)comboBox1.SelectedValue);
                }
            }
            catch
            {
                return;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Selected = true;
        }
    }
}
