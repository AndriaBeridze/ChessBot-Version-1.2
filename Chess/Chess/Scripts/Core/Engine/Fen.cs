using System.Collections.Generic;

using static Chess.Scripts.Data.Pieces;

namespace Chess.Scripts.Core.Engine
{
      internal class Fen
      {
            public const string fenPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            Dictionary<char, int> pieceValues = new Dictionary<char, int>()
            {
                  ['n'] = knight,
                  ['k'] = king,
                  ['q'] = queen,
                  ['r'] = rook,
                  ['b'] = bishop,
                  ['p'] = pawn
            };

            public void setPosition(int[] square)
            {
                  int section = 0;
                  int currentIndex = 0;
                  currentMoveCount = 0;
                  fiftyMoveRule = 0;
                  foreach (char c in fenPosition)
                  {
                        if (c == ' ')
                        {
                              section++; 
                              continue;
                        }
                        if (section == 0)
                        {
                              if (c == '/') continue;
                              if (char.IsDigit(c))
                              {
                                    currentIndex += (int)char.GetNumericValue(c);
                                    continue;
                              }
                              else
                              {
                                    int type = pieceValues[char.ToLower(c)];
                                    int color = (!char.IsLower(c) ? white : black);
                                    square[currentIndex] = color | type;
                                    currentIndex++;
                              }
                        }
                        if(section == 1)
                        {
                              if (c == 'w') setPlayer(white);
                              else setPlayer(black);
                        }
                        if(section == 2)
                        {
                              if (c == 'K') castle[0, 1] = true;
                              if (c == 'Q') castle[0, 0] = true;

                              if (c == 'k') castle[1, 1] = true;
                              if (c == 'q') castle[1, 0] = true;
                        }
                        if(section == 3)
                        {
                              if (c != '-')
                              {
                                    if (char.IsDigit(c)) 
                                    {
                                          enPassant += (int)(8 - char.GetNumericValue(c)) * 8;
                                    }
                                    else
                                    {
                                          enPassant = 0;
                                          if (c == 'a') enPassant += 0;
                                          if (c == 'b') enPassant += 1;
                                          if (c == 'c') enPassant += 2;
                                          if (c == 'd') enPassant += 3;
                                          if (c == 'e') enPassant += 4;
                                          if (c == 'f') enPassant += 5;
                                          if (c == 'g') enPassant += 6;
                                          if (c == 'h') enPassant += 7;
                                    }
                              }
                        }
                        if(section == 4)
                        {
                              fiftyMoveRule = (int)char.GetNumericValue(c) + fiftyMoveRule * 10;
                        }
                        if(section == 5)
                        {
                              currentMoveCount = (int) char.GetNumericValue(c) + currentMoveCount * 10;
                        }
                  }
            }
      }
}
