using System;
using System.Drawing;
using System.Windows.Forms;
using Chess.Scripts.Data;
using Chess.Scripts.Core.Visual;

using static Chess.Scripts.Data.Pieces;
using static Chess.Scripts.Data.Variables;
using static Chess.Scripts.Data.BoardThemes;
using static Chess.Scripts.Core.Engine.MoveGenerator;

namespace Chess.Forms
{
      public partial class StartingMenu : Form
      {
            Random rnd = new Random();
            Buttons buttons = new Buttons();
            static PieceImages pieceImages = new PieceImages();

            Panel whiteQueen = new Panel()
            {
                  Size = new Size(squareSize, squareSize),
                  BackColor = lightCol,
                  Location = new Point(100, 230 - squareSize),
                  BackgroundImage = pieceImages.pieceImage[white + queen]
            };
            Panel whiteKing = new Panel()
            {
                  Size = new Size(squareSize, squareSize),
                  BackColor = darkCol,
                  Location = new Point(100, 230),
                  BackgroundImage = pieceImages.pieceImage[white + king]
            };
            Panel blackQueen = new Panel()
            {
                  Size = new Size(squareSize, squareSize),
                  BackColor = darkCol,
                  Location = new Point(555, 230 - squareSize),
                  BackgroundImage = pieceImages.pieceImage[black + queen]
            };
            Panel blackKing = new Panel()
            {
                  Size = new Size(squareSize, squareSize),
                  BackColor = lightCol,
                  Location = new Point(555, 230),
                  BackgroundImage = pieceImages.pieceImage[black + king]
            };
            public StartingMenu()
            {
                  InitializeComponent();

                  this.Controls.Add(buttons.playAsWhite);
                  buttons.playAsWhite.MouseClick += new MouseEventHandler(playAsWhite);

                  this.Controls.Add(buttons.playAsBlack);
                  buttons.playAsBlack.MouseClick += new MouseEventHandler(playAsBlack);

                  this.Controls.Add(buttons.playAsRandom);
                  buttons.playAsRandom.MouseClick += new MouseEventHandler(playAsRandom);

                  this.Controls.Add(buttons.exit2);
                  buttons.exit2.MouseClick += new MouseEventHandler(exit);

                  this.Controls.Add(whiteQueen);
                  this.Controls.Add(whiteKing);

                  this.Controls.Add(blackQueen);
                  this.Controls.Add(blackKing);
            }

            new void playAsWhite(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;

                  setBotPlayer(black);
                  setPlayer(-1);
                  enPassant = -1;
                  for(int i = 0; i < 2; i++)
                  {
                        for(int j = 0; j < 2; j++)
                        {
                              castle[i, j] = false;
                        }
                  }
                  currentMoveCount = 0;
                  prevMove = new Move(-1, -1);

                  GameWindow gameWindow = new GameWindow();
                  gameWindow.Show();
            }

            new void playAsBlack(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;

                  setBotPlayer(white);
                  setPlayer(-1);
                  enPassant = -1;
                  for (int i = 0; i < 2; i++)
                  {
                        for (int j = 0; j < 2; j++)
                        {
                              castle[i, j] = false;
                        }
                  }
                  currentMoveCount = 0;
                  prevMove = new Move(-1, -1);

                  GameWindow gameWindow = new GameWindow();
                  gameWindow.Show();
            }

            new void playAsRandom(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;

                  int col = rnd.Next(2);
                  setBotPlayer((col == 1 ? white : black));
                  setPlayer(-1);
                  enPassant = -1;
                  for (int i = 0; i < 2; i++)
                  {
                        for (int j = 0; j < 2; j++)
                        {
                              castle[i, j] = false;
                        }
                  }
                  currentMoveCount = 0;
                  prevMove = new Move(-1, -1);

                  GameWindow gameWindow = new GameWindow();
                  gameWindow.Show();
            }
            new void exit(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;
                  this.Close();
            }
      }
}
