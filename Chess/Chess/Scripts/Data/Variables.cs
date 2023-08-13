using System.Windows.Forms;
using static Chess.Scripts.Core.Engine.MoveGenerator;
using static Chess.Scripts.Data.Pieces;

namespace Chess.Scripts.Data
{
      internal class Variables
      {
            public const int squareSize = 95;

            public const int boardPaddingX = 768 - 4 * squareSize;
            public const int boardPaddingY = 432 - 4 * squareSize;
            public const int backgroundPanelSize = 8 * squareSize + 60;

            public static Move prevMove = new Move(-1, -1);

            public static int getIndex(Panel panel)
            {
                  int col = (panel.Location.X - boardPaddingX) / squareSize;
                  int row = (panel.Location.Y - boardPaddingY) / squareSize;

                  if (botPlayer == white)
                  {
                        col = 7 - col; 
                        row = 7 - row;
                  }

                  return row * 8 + col;
            }
      }
}
