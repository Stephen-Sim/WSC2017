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
    public partial class AddUserForm : Form
    {
        public WSC2017_Module1Entities ent { get; set; } = new WSC2017_Module1Entities();
        public AddUserForm(User user, Log log)
        {
            InitializeComponent();
            this.User = user;
            this.Log = log;
        }

        public User User { get; set; }
        public Log Log { get; set; }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2017_Module1DataSet.Offices' table. You can move, or remove it, as needed.
            this.officesTableAdapter.Fill(this.wSC2017_Module1DataSet.Offices);

            comboBox1.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var form = (AdminForms)Application.OpenForms["AdminForms"];
            form.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var email = textBox1.Text;
            var firstname = textBox2.Text;
            var lastname = textBox3.Text;
            var password = textBox4.Text;
            var date = dateTimePicker1.Value.Date;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(password) || comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("All the feilds are required!!");
                return;
            }

            if (date >= DateTime.Today)
            {
                MessageBox.Show("Invalid Date!!");
                return;
            }

            var isExists = ent.Users.Where(x => x.Email == email).Any();

            if (isExists == true)
            {
                MessageBox.Show("email is taken!!");
                return;
            }

            var id = ent.Users.OrderByDescending(x => x.ID).First().ID + 1;

            var newUser = new User
            {
                ID = id,
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                Password = password,
                Birthdate = date,
                RoleID = 2,
                Active = true,
                OfficeID = (int)comboBox1.SelectedValue
            };

            ent.Users.Add(newUser);
            ent.SaveChanges();

            this.Hide();
            new AdminForms(this.User, this.Log).ShowDialog();
        }
    }
}
