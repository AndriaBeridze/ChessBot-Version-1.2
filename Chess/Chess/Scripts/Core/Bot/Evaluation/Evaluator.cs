using Chess.Scripts.Core.Engine;
using Chess.Scripts.Data;
using System;
using static Chess.Scripts.Data.Pieces;

namespace Chess.Scripts.Core.Bot.Evaluation
{
      internal class Evaluator
      {
            static Pieces pieces = new Pieces();
            MoveGenerator moveGenerator = new MoveGenerator();
            BonusTable bonusTable = new BonusTable();
            EndgameWeight endgameWeight = new EndgameWeight();

            static int[] pieceValue = new int[7]
            {
                  0, 0, 900, 500, 300, 300, 100,
            };
            public int evaluate(int[] square, int color)
            { 
                  if (isDraw(square)) return 0;
                  int evaluation = 0;

                  for (int i = 0; i < 64; i++)
                  {
                        evaluation += pieceValue[pieces.getType(square[i])] * (pieces.getColor(square[i]) == white ? 1 : -1);
                  }

                  int weight = endgameWeight.calculate(square);
                  evaluation += forceKingToEdge(square, white, weight) - forceKingToEdge(square, black, weight);

                  evaluation += bonusTable.calculateBonus(square);

                  if(weight > 7) evaluation += (moveGenerator.generateAllMoves(square, white).Count - moveGenerator.generateAllMoves(square, black).Count) * weight;

                  return evaluation * (color == white ? 1 : -1);
            }
            public int forceKingToEdge(int[] square, int color, int endgameWeight)
            {
                  int eval = 0;

                  int distX = 0, distY = 0;
                  for(int i = 0; i < 64; i++)
                  {
                        if (pieces.getType(square[i]) == king)
                        {
                              if (pieces.getColor(square[i]) == color)
                              {
                                    distX += i / 8;
                                    distY += i % 8;
                              }
                              else
                              {
                                    distX -= i / 8;
                                    distY -= i % 8;
                                    eval += Math.Max(3 - i / 8, i / 8 - 4) + Math.Max(3 - i % 8, i % 8 - 4);
                              }
                        }
                  }
                  eval += 14 - Math.Abs(distX) - Math.Abs(distY);
                  return eval * 10 * endgameWeight;
            }

            public bool isDraw(int[] square)
            {
                  if (fiftyMoveRule >= 50) return true;
                  int[] bishops = new int[2] { 0, 0 };
                  int[] knights = new int[2] { 0, 0 };

                  for (int i = 0; i < 64; i++)
                  {
                        if (pieces.getType(square[i]) == rook) return false;
                        if (pieces.getType(square[i]) == queen) return false;
                        if (pieces.getType(square[i]) == pawn) return false;

                        if (pieces.getType(square[i]) == bishop) bishops[pieces.getColor(square[i]) == white ? 0 : 1]++;
                        if (pieces.getType(square[i]) == knight) knights[pieces.getColor(square[i]) == white ? 0 : 1]++;
                  }
                  if (bishops[0] != 0 && bishops[0] + knights[0] >= 2) return false;
                  if (bishops[1] != 0 && bishops[1] + knights[1] >= 2) return false;

                  return true;
            }


      }
}
