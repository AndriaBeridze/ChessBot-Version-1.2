using System.Drawing;
using System.Windows.Forms;
using static Chess.Scripts.Data.Variables;

namespace Chess.Scripts.Core.Visual
{
      internal class Buttons
      {
            static int sizeX = 200, sizeY = 60;
            public Button exit = new Button()
            {
                  Size = new Size(sizeX, sizeY),
                  BackColor = Color.FromArgb(60, 60, 60),
                  Location = new Point((boardPaddingX - 30 - sizeX) / 2, boardPaddingY - 30 + backgroundPanelSize - sizeY),
                  FlatStyle = FlatStyle.Flat,
                  Text = "Exit",
                  Font = new Font("Courier New", 20),
                  ForeColor = Color.FromArgb(200, 200, 200),
            };
            public Button playAsWhite = new Button()
            {
                  Size = new Size(sizeX, sizeY),
                  BackColor = Color.FromArgb(60, 60, 60),
                  Location = new Point(275, 50),
                  FlatStyle = FlatStyle.Flat,
                  Text = "Play As White",
                  Font = new Font("Courier New", 15),
                  ForeColor = Color.FromArgb(200, 200, 200),
            };
            public Button playAsBlack = new Button()
            {
                  Size = new Size(sizeX, sizeY),
                  BackColor = Color.FromArgb(60, 60, 60),
                  Location = new Point(275, 150),
                  FlatStyle = FlatStyle.Flat,
                  Text = "Play As Black",
                  Font = new Font("Courier New", 15),
                  ForeColor = Color.FromArgb(200, 200, 200),
            };
            public Button playAsRandom = new Button()
            {
                  Size = new Size(sizeX, sizeY),
                  BackColor = Color.FromArgb(60, 60, 60),
                  Location = new Point(275, 250),
                  FlatStyle = FlatStyle.Flat,
                  Text = "Play As Random",
                  Font = new Font("Courier New", 15),
                  ForeColor = Color.FromArgb(200, 200, 200),
            };
            public Button exit2 = new Button()
            {
                  Size = new Size(sizeX, sizeY),
                  BackColor = Color.FromArgb(60, 60, 60),
                  Location = new Point(275, 350),
                  FlatStyle = FlatStyle.Flat,
                  Text = "Exit",
                  Font = new Font("Courier New", 20),
                  ForeColor = Color.FromArgb(200, 200, 200),
            };
            public Buttons()
            {
                  exit.FlatAppearance.BorderSize = 0;
                  playAsWhite.FlatAppearance.BorderSize = 0;
                  playAsBlack.FlatAppearance.BorderSize = 0;
                  playAsRandom.FlatAppearance.BorderSize = 0;
                  exit2.FlatAppearance.BorderSize = 0;
            }
      }
}
