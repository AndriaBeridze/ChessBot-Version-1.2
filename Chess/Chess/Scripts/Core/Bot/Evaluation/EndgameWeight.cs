using Chess.Scripts.Data;
using static Chess.Scripts.Data.Pieces;

namespace Chess.Scripts.Core.Bot.Evaluation
{
      internal class EndgameWeight
      {
            Pieces pieces = new Pieces();
            public int calculate(int[] square)
            {
                  int weight = 0;
                  if (currentMoveCount > 30)
                  {
                        weight += 3;
                  }
                  else if (currentMoveCount > 20)
                  {
                        weight += 2;
                  }
                  else if (currentMoveCount > 10)
                  {
                        weight += 1;
                  }

                  int bishops = 0, knights = 0, rooks = 0;
                  int pawns = 0;
                  for(int i = 0; i < 64; i++)
                  {
                        if (pieces.getType(square[i]) == rook) rooks++;
                        if (pieces.getType(square[i]) == knight) knights++;
                        if (pieces.getType(square[i]) == bishop) bishops++;

                        if (pieces.getType(square[i]) == pawn) pawns++;
                  }

                  if (bishops == 0)
                  {
                        weight += 1;
                  }
                  if (knights == 0)
                  {
                        weight += 1;
                  }
                  if (rooks == 0)
                  {
                        weight += 2;
                  }

                  weight += (16 - pawns) / 4;

                  return weight;
            }
      }
}
