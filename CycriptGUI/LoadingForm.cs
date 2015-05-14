using System.Drawing;
using System.Windows.Forms;

namespace CycriptGUI
{
    public partial class LoadingForm : Form
    {
        public LoadingForm(string status = "Loading...")
        {
            InitializeComponent();

            if (Owner != null)
            {
                Location = new Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
                    Owner.Location.Y + Owner.Height / 2 - Height / 2);
            }

            statusLabel.Text = status;
        }
    }
}
