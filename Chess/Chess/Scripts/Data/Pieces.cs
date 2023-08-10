using System.Collections.Generic;
using static Chess.Scripts.Core.Engine.MoveGenerator;

namespace Chess.Scripts.Data
{
      internal class Pieces
      {
            static OpeningBook openingBook = new OpeningBook();
            public struct affectedPiece
            {
                  public int piece;
                  public int startingSquare;
                  public int targetSquare;
                  public affectedPiece(int piece, int startingSquare, int targetSquare)
                  {
                        this.piece = piece;
                        this.startingSquare = startingSquare;
                        this.targetSquare = targetSquare;
                  }
            }
            public struct boardData
            {
                  public List<affectedPiece> pieces;
                  public bool[,] castle;
                  public int enPassant;
                  public Move prevMove;
                  public int fiftyMoveRule;
                  public boardData(List<affectedPiece> pieces, bool[,] castle, int enPassant, Move PrevMove, int fiftyMoveRule)
                  {
                        this.enPassant = enPassant;
                        this.pieces = pieces;
                        this.prevMove = PrevMove;
                        this.castle = new bool[2, 2];
                        this.fiftyMoveRule = fiftyMoveRule;
                        for(int i = 0; i < 2; i++)
                        {
                              for(int j = 0; j < 2; j++)
                              {
                                    this.castle[i, j] = castle[i, j];
                              }
                        }
                  }
            }

            public List<List<Move>> possibleOpenings = openingBook.allOpenings; 

            public static int currentMoveCount = 0;
            public static int fiftyMoveRule = 0;
            public static boardData[] allMoves = new boardData[500];

            public static bool[,] castle = { {true, true}, { true, true } };
            public static int enPassant = -1;

            public static int player = -1;
            public static int botPlayer = -2;

            public const int king = 1;
            public const int queen = 2;
            public const int rook = 3;
            public const int bishop = 4;
            public const int knight = 5;
            public const int pawn = 6;

            public const int white = 8;
            public const int black = 16;

            public static void setPlayer(int color)
            {
                  player = color;
            }
            public static void setBotPlayer(int color)
            {
                  botPlayer = color;
            }
            public static void switchPlayer()
            {
                  player ^= 24;
            }

            public int getColor(int piece)
            {
                  return piece & 24;
            }

            public int getType(int piece)
            {
                  return piece & 7;
            }

            public bool isSlidingPiece(int piece)
            {
                  if (getType(piece) == queen || getType(piece) == rook || getType(piece) == bishop) return true;
                  return false;
            }
      }
}
