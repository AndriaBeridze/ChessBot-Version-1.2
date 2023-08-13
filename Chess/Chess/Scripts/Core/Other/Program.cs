using Chess.Forms;
using System;
using System.Windows.Forms;

namespace Chess
{
      static class Program
      {
            [STAThread]
            static void Main()
            {
                  Application.EnableVisualStyles();
                  Application.SetCompatibleTextRenderingDefault(false);
                  Application.Run(new StartingMenu());
            }
      }
}