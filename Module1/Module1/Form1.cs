using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Module1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public int count { get; set; } = 0;

        WSC2017_Module1Entities ent = new WSC2017_Module1Entities();
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Invalid Input!! All the fields are required!!", "Alert", MessageBoxButtons.OK);
                return;
            }

            string username = textBox1.Text;
            string password = textBox2.Text;

            var user = ent.Users.FirstOrDefault(x => x.Email == username && x.Password == password);

            if (user != null)
            {
                if (user.Active == false)
                {
                    MessageBox.Show("Your acc is suspended!!");
                    return;
                }

                var l = user.Logs.Any() ? user.Logs.OrderByDescending(x => x.LogInTime).First() : null;
                if (l != null && l.LogoutTime == null && l.NotProperLogoutReason == null)
                {
                    new NoLogOutDetectedForm(l).ShowDialog();
                }

                try
                {
                    MessageBox.Show("User Found!!", "Alert", MessageBoxButtons.OK);
                    Log log = new Log();
                    log.UserId = user.ID;
                    log.LogInTime = DateTime.Now;
                    log.LogoutTime = null;

                    ent.Logs.Add(log);
                    ent.SaveChanges();

                    textBox1.Text = String.Empty;
                    textBox2.Text = String.Empty;
                    this.Hide();

                    if (user.RoleID == 1)
                    {
                        new AdminForms(user, log).ShowDialog();
                    }
                    else
                    {
                        new UserForm(user, log).ShowDialog();
                    }
                }
                catch (DbEntityValidationException err)
                {
                    Console.WriteLine(err.Message);
                    return;
                }
            }
            else
            {
                MessageBox.Show("User not Found!!", "Alert", MessageBoxButtons.OK);
                preventLogin();
            }
        }

        private async void preventLogin()
        {
            count++;

            if (count == 3)
            {
                button1.Enabled = false;
                for (int i = 10; i >= 0; i--)
                {
                    label3.Text = $"You are not able to login for {i} sec!!";
                    //label3.Text = "You are not able to login for " +  i.ToString() +  " sec!!";
                    await Task.Delay(1000);
                }
                button1.Enabled = true;
                label3.Text = String.Empty;
                count = 0;
            }
        }
    }
}
