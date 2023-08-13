using System.Drawing;
using static Chess.Scripts.Data.Pieces;
using static Chess.Scripts.Data.Variables;

namespace Chess.Scripts.Data
{
      internal class PieceImages
      {
            public Image[] pieceImage = new Image[24];

            public PieceImages()
            {
                  pieceImage[black + rook] = new Bitmap("Pieces/BlackRook.png");
                  pieceImage[black + queen] = new Bitmap("Pieces/BlackQueen.png");
                  pieceImage[black + king] = new Bitmap("Pieces/BlackKing.png");
                  pieceImage[black + knight] = new Bitmap("Pieces/BlackKnight.png");
                  pieceImage[black + bishop] = new Bitmap("Pieces/BlackBishop.png");
                  pieceImage[black + pawn] = new Bitmap("Pieces/BlackPawn.png");

                  pieceImage[white + rook] = new Bitmap("Pieces/WhiteRook.png");
                  pieceImage[white + queen] = new Bitmap("Pieces/WhiteQueen.png");
                  pieceImage[white + king] = new Bitmap("Pieces/WhiteKing.png");
                  pieceImage[white + knight] = new Bitmap("Pieces/WhiteKnight.png");
                  pieceImage[white + bishop] = new Bitmap("Pieces/WhiteBishop.png");
                  pieceImage[white + pawn] = new Bitmap("Pieces/WhitePawn.png");

                  for (int i = 0; i < 24; i++)
                  {
                        if (pieceImage[i] != null)
                        {
                              pieceImage[i] = new Bitmap(pieceImage[i], new Size(squareSize, squareSize));
                        }
                  }
            }
      }
}
