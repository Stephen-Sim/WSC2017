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
    public partial class EditRoleForm : Form
    {
        public EditRoleForm(User user, User admin, Log log)
        {
            InitializeComponent();
            this.User = user;
            Admin = admin;
            Log = log;
        }

        public User User { get; set; }
        public User Admin { get; set; }
        public Log Log { get; set; }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = (AdminForms)Application.OpenForms["AdminForms"];
            form.Show();
        }

        private void EditRoleForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = User.Email;
            textBox2.Text = User.FirstName;
            textBox3.Text = User.LastName;
            textBox4.Text = User.Office.Title;

            if (this.User.RoleID == 2)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        public WSC2017_Module1Entities ent { get; set; } = new WSC2017_Module1Entities();
        private void button1_Click(object sender, EventArgs e)
        {
            var user = ent.Users.FirstOrDefault(x => x.ID == this.User.ID);

            if (radioButton1.Checked == true)
            {
                user.RoleID = 2;
            }
            else
            {
                user.RoleID = 1;
            }

            ent.SaveChanges();

            this.Hide();
            new AdminForms(this.Admin, this.Log).ShowDialog();
        }
    }
}
