using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Module3
{
    public partial class BookingConfirmationForm : Form
    {
        public PrivateFontCollection pfc { get; set; } = new PrivateFontCollection();
        public BookingConfirmationForm()
        {
            InitializeComponent();
        }

        private void BookingConfirmationForm_Load(object sender, EventArgs e)
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
