using static Chess.Scripts.Data.Pieces;
using Chess.Scripts.Data;

namespace Chess.Scripts.Core.Bot.Evaluation
{
      internal class BonusTable
      {
            Pieces pieces = new Pieces();
            EndgameWeight endgameWeight = new EndgameWeight();

            public int[,] kingBonusTable =
            {
                  {
                        -30, -40, -40, -50, -50, -40, -40, -30,
                        -30, -40, -40, -50, -50, -40, -40, -30,
                        -30, -40, -40, -50, -50, -40, -40, -30,
                        -30, -40, -40, -50, -50, -40, -40, -30,
                        -20, -30, -30, -40, -40, -30, -30, -20,
                        -10, -20, -20, -20, -20, -20, -20, -10,
                        20, 20,  -10,  -20,  -20,  -10, 20, 20,
                        20, 30, 10,  0,  0, 10, 30, 20
                  },
                  {
                        -50, -30, -30, -30, -30, -30, -30, -50,
                        -30, -20, -10, -10, -10, -10, -20, -30,
                        -30, -10,  20,  30,  30,  20, -10, -30,
                        -30, -10,  30,  40,  40,  30, -10, -30,
                        -30, -10,  30,  40,  40,  30, -10, -30,
                        -30, -10,  20,  30,  30,  20, -10, -30,
                        -30, -30,   0,   0,   0,   0, -30, -30,
                        -50, -30, -30, -30, -30, -30, -30, -50
                  }
            };
            public int[] queenBonusTable =
            {
                  -20, -10, -10,  -5,  -5, -10, -10, -20,
                  -10,   0,   0,   0,   0,   0,   0, -10,
                  -10,   0,   5,   5,   5,   5,   0, -10,
                  -5,   0,   5,   5,   5,   5,   0,  -5,
                  0,   0,   5,   5,   5,   5,   0,  -5,
                  -10,   5,   5,   5,   5,   5,   0, -10,
                  -10,   0,   5,   0,   0,   0,   0, -10,
                  -20, -10, -10,  -5,  -5, -10, -10, -20
            };
            public int[,] pawnBonusTable = {
                  {
                        0,   0,   0,   0,   0,   0,   0,   0,
                        50,  50,  50,  50,  50,  50,  50,  50,
                        10,  10,  20,  30,  30,  20,  10,  10,
                        5,   5,  10,  25,  25,  10,   5,   5,
                        0,   0,   0,  20,  20,   0,   0,   0,
                        5,  -5, -10,   0,   0, -10,  -5,   5,
                        5,  10,  10, -20, -20,  10,  10,   5,
                        0,   0,   0,   0,   0,   0,   0,   0
                  },
                  {
                        0,   0,   0,   0,   0,   0,   0,   0,
                        90,  90, 90,  90,  90,  90,  90,  90,
                        50, 50,  50,  50,  50,  50,  50,  50,
                        20, 20,  20,  20,  20,  20,  20,  20,
                        15, 15,  15,  15,  15,  15,  15,  15,
                        10, 10,  10,  10,  10,  10,  10,  10,
                        0,   0,   0,   0,   0,   0,   0,   0,
                        0,   0,   0,   0,   0,   0,   0,   0
                  }
            };
            public int[] knightBonusTable = {
                  -50, -40, -30, -30, -30, -30, -40, -50,
                  -40, -20,   0,   0,   0,   0, -20, -40,
                  -30,   0,  10,  15,  15,  10,   0, -30,
                  -30,   5,  15,  20,  20,  15,   5, -30,
                  -30,   0,  15,  20,  20,  15,   0, -30,
                  -30,   5,  10,  15,  15,  10,   5, -30,
                  -40, -20,   0,   5,   5,   0, -20, -40,
                  -50, -40, -30, -30, -30, -30, -40, -50
            };
            public int[] rookBonusTable =
            {
                  0,   0,   0,   0,   0,   0,   0,   0,
                  5,  10,  10,  10,  10,  10,  10,   5,
                  -5,   0,   0,   0,   0,   0,   0,  -5,
                  -5,   0,   0,   0,   0,   0,   0,  -5,
                  -5,   0,   0,   0,   0,   0,   0,  -5,
                  -5,   0,   0,   0,   0,   0,   0,  -5,
                  -5,   0,   0,   0,   0,   0,   0,  -5,
                  0,   0,   0,   5,   5,   0,   0,   0
            };
            public int[] bishopBonusTable =
            {
                  -20, -10, -10, -10, -10, -10, -10, -20,
                 -10,   0,   0,   0,   0,   0,   0, -10,
                 -10,   0,   5,  10,  10,   5,   0, -10,
                 -10,   5,   5,  10,  10,   5,   5, -10,
                 -10,   0,  10,  10,  10,  10,   0, -10,
                 -10,  10,  10,  10,  10,  10,  10, -10,
                 -10,   5,   0,   0,   0,   0,   5, -10,
                 -20, -10, -10, -10, -10, -10, -10, -20
            };
            public int calculateBonus(int[] square)
            {
                  int bonus = 0;
                  int weight = endgameWeight.calculate(square);

                  for(int i = 0; i < 64; i++)
                  {
                        int color = pieces.getColor(square[i]);
                        if (pieces.getType(square[i]) == king) bonus += (kingBonusTable[(weight > 8 ? 1 : 0), (color == white ? i : 63 - i)]) * (color == white ? 1 : -1);
                        if (pieces.getType(square[i]) == queen) bonus += (queenBonusTable[(color == white ? i : 63 - i)]) * (color == white ? 1 : -1);
                        if (pieces.getType(square[i]) == rook) bonus += (rookBonusTable[(color == white ? i : 63 - i)]) * (color == white ? 1 : -1);
                        if (pieces.getType(square[i]) == pawn) bonus += (pawnBonusTable[(weight > 8 ? 1 : 0), (color == white ? i : 63 - i)]) * (color == white ? 1 : -1);
                        if (pieces.getType(square[i]) == bishop) bonus += (bishopBonusTable[(color == white ? i : 63 - i)]) * (color == white ? 1 : -1);
                        if (pieces.getType(square[i]) == knight) bonus += (knightBonusTable[(color == white ? i : 63 - i)]) * (color == white ? 1 : -1);
                  }
                  return bonus;
            }
      }
}
