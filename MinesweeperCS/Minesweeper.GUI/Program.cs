// Sergi
// CST-250

using System;
using System.Windows.Forms;

namespace Minesweeper.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmNewGame());
        }
    }
}
