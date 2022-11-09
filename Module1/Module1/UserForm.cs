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

        WSC2017_Module1Entities ent = new WSC2017_Module1Entities();

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var log = ent.Logs.FirstOrDefault(x => x.Id == this.log.Id);
            log.LogoutTime = DateTime.Now;
            ent.SaveChanges();
            this.Close();
        }
    }
}
