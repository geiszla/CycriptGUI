using System.Windows.Forms;

namespace CycriptGUI.MainProgram
{
    class Alert
    {
        public static void ShowNotImplementedMessage()
        {
            MessageBox.Show("This feature hasn't been implemented yet.", "Not implemented",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowError(string errorString, string errorTitle)
        {
            MessageBox.Show(errorString, "Error: " + errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
