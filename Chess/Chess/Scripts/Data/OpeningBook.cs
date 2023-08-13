using System.Collections.Generic;
using static Chess.Scripts.Core.Engine.MoveGenerator;

namespace Chess.Scripts.Data
{
      internal class OpeningBook
      {
            public string[] openings = System.IO.File.ReadAllLines("Openings.txt");
            public List<List<Move>> allOpenings = new List<List<Move>>();
            public OpeningBook()
            {
                  foreach(string opening in openings)
                  {
                        List<Move> curOpening = new List<Move>();
                        Move curMove = new Move(-1, -1);
                        int ind = 0;
                        foreach (char c in opening)
                        {
                              if(c == ',')
                              {
                                    curOpening.Add(curMove);
                                    curMove = new Move(-1, -1);
                              }
                              else if(c == ' ')
                              {
                                    ind = (ind + 1) % 2;
                              }
                              else
                              {
                                    if(ind == 0)
                                    {
                                          if (curMove.startingSquare == -1) curMove.startingSquare = (char)char.GetNumericValue(c);
                                          else curMove.startingSquare = curMove.startingSquare * 10 + (char)char.GetNumericValue(c);
                                    }
                                    else
                                    {
                                          if (curMove.targetSquare == -1) curMove.targetSquare = (char)char.GetNumericValue(c);
                                          else curMove.targetSquare = curMove.targetSquare * 10 + (char)char.GetNumericValue(c);
                                    }
                              }
                        }
                        allOpenings.Add(curOpening);
                  }
            }
      }
}
