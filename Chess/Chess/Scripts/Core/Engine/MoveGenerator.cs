using System;
using Chess.Scripts.Data;
using System.Collections.Generic;

using static Chess.Scripts.Data.Pieces;

namespace Chess.Scripts.Core.Engine
{
      internal class MoveGenerator
      {
            static Pieces pieces = new Pieces();
            static MoveMaker moveMaker = new MoveMaker();

            public struct Move
            {

                  public int startingSquare;
                  public int targetSquare;

                  public Move(int start, int target)
                  {
                        startingSquare = start;
                        targetSquare = target;
                  }
            }
            public List<Move> generateCaptureMoves(int[] square, int color)
            {
                  List<Move> moves = new List<Move>();
                  List<Move> m = generateAllMoves(square, color);
                  foreach(Move move in m)
                  {
                        if(square[move.targetSquare] != 0)
                        {
                              moves.Add(move);
                        }
                  }
                  return moves;
            }
            public List<Move> generateAllMoves(int[] square, int color)
            {
                  List<Move> moves = new List<Move>();
                  for (int i = 0; i < 64; i++) {
                        if (pieces.getColor(square[i]) == color)
                        {
                              List<Move> m = generateMoves(i, square);
                              foreach(Move move in m)
                              {
                                    moves.Add(move);
                              }
                        }
                  }
                  return moves;
            }
            public bool isInCheck(int color, int[] square)
            {
                  for (int i = 0; i < 64; i++)
                  {
                        if (square[i] == 0) continue;
                        if (color != pieces.getColor(square[i]))
                        {
                              List<Move> moves = generatePseudoMoves(i, square);
                              foreach(Move move in moves)
                              {
                                    if (square[move.targetSquare] == color + king) return true;
                              }
                        }
                  }
                  return false;
            }
            public List<Move> generateMoves(int startingSquare, int[] square)
            {
                  List<Move> moves = new List<Move>();
                  List<Move> pseudoMoves = generatePseudoMoves(startingSquare, square);

                  int color = pieces.getColor(square[startingSquare]);
                  foreach(Move move in pseudoMoves)
                  {
                        bool canCastle = true;
                        if (pieces.getType(square[startingSquare]) == king && Math.Abs(move.targetSquare - move.startingSquare) == 2)
                        {
                              if(isInCheck(color, square)) canCastle = false;

                              moveMaker.makeMove(new Move(startingSquare, startingSquare + (move.startingSquare > move.targetSquare ? -1 : 1)), square);
                              if (isInCheck(color, square)) canCastle = false;
                              moveMaker.unmakeMove(square);
                        }
                        if (canCastle)
                        {
                              moveMaker.makeMove(move, square);
                              if (!isInCheck(color, square))
                              {
                                    moves.Add(move);
                              }
                              moveMaker.unmakeMove(square);
                        }
                  }
                  return moves;
            }

            static List<Move> generatePseudoMoves(int startingSquare, int[] square)
            {
                  if (pieces.isSlidingPiece(square[startingSquare]))
                  {
                        return generateSlidingPieceMoves(startingSquare, square);
                  }
                  else if (pieces.getType(square[startingSquare]) == knight)
                  {
                        return generateKnightMoves(startingSquare, square);
                  }
                  else if (pieces.getType(square[startingSquare]) == king)
                  {
                        return generateKingMoves(startingSquare, square);
                  }
                  else return generatePawnMoves(startingSquare, square);
            }

            static List<Move> generateSlidingPieceMoves(int startingSquare, int[] square)
            {
                  int startingIndex = (pieces.getType(square[startingSquare]) == bishop ? 4 : 0);
                  int endingIndex = (pieces.getType(square[startingSquare]) == rook ? 4 : 8);

                  List<Move> moves = new List<Move>();
                  int[] direction = new int[8]
                  {
                        -1, 1, -8, 8, -7, 7, -9, 9,
                  };
                  int west = startingSquare & 7, north = startingSquare >> 3;
                  int east = 7 - west, south = 7 - north;
                  int[] numberOfMoves = new int[8]
                  {
                        west, east, north, south, 
                        Math.Min(north, east), Math.Min(south, west), Math.Min(north, west), Math.Min(south, east) 
                  };

                  for(int i = startingIndex; i < endingIndex; i++)
                  {
                        for(int j = 1; j <= numberOfMoves[i]; j++)
                        {
                              int targetSquare = startingSquare + direction[i] * j;
                              if (pieces.getColor(square[targetSquare]) == pieces.getColor(square[startingSquare])) break;

                              moves.Add(new Move(startingSquare, targetSquare));

                              if (square[targetSquare] != 0) break;
                        }
                  }
                  return moves;
            }

            static List<Move> generateKingMoves(int startingSquare, int[] square)
            {
                  List<Move> moves = new List<Move>();
                  int[] direction = new int[8]
                  {
                        1, -1, 8, -8, 9, -9, 7, -7,
                  };
                  foreach(int dir in direction)
                  {
                        int targetSquare = startingSquare + dir;

                        if (targetSquare > 63 || targetSquare < 0) continue;
                        if (pieces.getColor(square[targetSquare]) == pieces.getColor(square[startingSquare])) continue;
                        if (Math.Abs(startingSquare % 8 - targetSquare % 8) > 1 || Math.Abs(startingSquare / 8 - targetSquare / 8) > 1) continue;

                        moves.Add(new Move(startingSquare, targetSquare));
                  }
                  int color = pieces.getColor(square[startingSquare]) == white ? 0 : 1;
                  if (castle[color, 0])
                  {
                        if (square[startingSquare - 1] == 0 && square[startingSquare - 2] == 0 && square[startingSquare - 3] == 0)
                        {
                              moves.Add(new Move(startingSquare, startingSquare - 2));
                        }
                  }
                  if (castle[color, 1])
                  {
                        if (square[startingSquare + 1] == 0 && square[startingSquare + 2] == 0)
                        {
                              moves.Add(new Move(startingSquare, startingSquare + 2));
                        }
                  }
                  return moves;
            }

            static List<Move> generateKnightMoves(int startingSquare, int[] square)
            {
                  List<Move> moves = new List<Move>();
                  int[] direction = new int[8]
                  {
                        -10, 10, 6, -6, 15, -15, 17, -17,
                  };
                  foreach (int dir in direction)
                  {
                        int targetSquare = startingSquare + dir;

                        if (targetSquare > 63 || targetSquare < 0) continue;
                        if (pieces.getColor(square[targetSquare]) == pieces.getColor(square[startingSquare])) continue;
                        if (Math.Abs(startingSquare % 8 - targetSquare % 8) * Math.Abs(startingSquare / 8 - targetSquare / 8) != 2) continue;

                        moves.Add(new Move(startingSquare, targetSquare));
                  }
                  return moves;
            }

            static List<Move> generatePawnMoves(int startingSquare, int[] square)
            {
                  List<Move> moves = new List<Move>();
                  int c = pieces.getColor(square[startingSquare]) == white ? -1 : 1;
                  for(int i = 1; i <= 1 + (startingSquare / 8 == 6 || startingSquare / 8 == 1 ? 1 : 0); i++)
                  {
                        int targetSquare = startingSquare + i * 8 * c;

                        if (targetSquare > 63 || targetSquare < 0) break;
                        if (square[targetSquare] != 0) break;

                        moves.Add(new Move(startingSquare, targetSquare));
                  }
                  for(int i = 7; i <= 9; i += 2)
                  {
                        int targetSquare = startingSquare + i * c;

                        if (targetSquare > 63 || targetSquare < 0) continue;
                        if (square[targetSquare] == 0 || pieces.getColor(square[targetSquare]) == pieces.getColor(square[startingSquare])) continue;
                        if (Math.Abs(startingSquare % 8 - targetSquare % 8) > 1 || Math.Abs(startingSquare / 8 - targetSquare / 8) > 1) continue;

                        moves.Add(new Move(startingSquare, targetSquare));
                  }
                  if(Math.Abs(enPassant - startingSquare) == 1 && enPassant != -1)
                  {
                        moves.Add(new Move(startingSquare, enPassant + (startingSquare / 8 == 3 ? -8 : 8)));
                  }
                  return moves;
            }
      }
}
