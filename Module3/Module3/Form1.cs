using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Module3
{
    public partial class Form1 : Form
    {
        public PrivateFontCollection pfc { get; set; } = new PrivateFontCollection();
        public WSC2017_Session3Entities ent { get; set; } = new WSC2017_Session3Entities();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(247, 148, 32);
            pfc.AddFontFile(Application.StartupPath + "\\font.TTF");

            try
            {
                Font = new Font(pfc.Families[0], 8f);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}
