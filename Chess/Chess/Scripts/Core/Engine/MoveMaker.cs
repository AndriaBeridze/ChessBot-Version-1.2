using System;
using System.Collections.Generic;
using Chess.Scripts.Data;

using static Chess.Scripts.Core.Engine.MoveGenerator;
using static Chess.Scripts.Data.Pieces;
using static Chess.Scripts.Data.Variables;

namespace Chess.Scripts.Core.Engine
{
      internal class MoveMaker
      {
            Pieces pieces = new Pieces();
            public void makeMove(Move move, int[] square)
            {
                  bool[,] prevCastle = new bool[2, 2];
                  for(int i = 0; i < 2; i++)
                  {
                        for (int j = 0; j < 2; j++)
                        {
                              prevCastle[i, j] = castle[i, j];
                        }
                  }
                  int prevFiftyMoveRule = fiftyMoveRule;
                  int prevEnPassant = enPassant;
                  List<affectedPiece> data = new List<affectedPiece>();

                  if (pieces.getType(square[move.startingSquare]) == king && Math.Abs(move.startingSquare - move.targetSquare) == 2)
                  {
                        if (move.targetSquare > move.startingSquare)
                        {
                              data.Add(new affectedPiece(square[move.startingSquare + 3], move.startingSquare + 3, move.startingSquare + 1));
                              square[move.startingSquare + 1] = square[move.startingSquare + 3];
                              square[move.startingSquare + 3] = 0;
                        }
                        else
                        {
                              data.Add(new affectedPiece(square[move.startingSquare - 4], move.startingSquare - 4, move.startingSquare - 1));
                              square[move.startingSquare - 1] = square[move.startingSquare - 4];
                              square[move.startingSquare - 4] = 0;
                        }
                  }
                  if (pieces.getType(square[move.startingSquare]) == king)
                  {
                        int color = pieces.getColor(square[move.startingSquare]) == white ? 0 : 1;
                        castle[color, 0] = false;
                        castle[color, 1] = false;
                  }
                  if (move.startingSquare == 0 || move.targetSquare == 0) castle[1, 0] = false;
                  if (move.startingSquare == 7 || move.targetSquare == 7) castle[1, 1] = false;

                  if (move.startingSquare == 56 || move.targetSquare == 56) castle[0, 0] = false;
                  if (move.startingSquare == 63 || move.targetSquare == 63) castle[0, 1] = false;

                  if (pieces.getType(square[move.startingSquare]) == pawn && Math.Abs(move.startingSquare - move.targetSquare) % 8 != 0 && square[move.targetSquare] == 0)
                  {
                        data.Add(new affectedPiece(square[enPassant], enPassant, enPassant));
                        fiftyMoveRule = 0;
                        square[enPassant] = 0;
                  }
                  else
                  {
                        if (pieces.getType(square[move.startingSquare]) == pawn || square[move.targetSquare] != 0)
                        {
                              fiftyMoveRule = 0;
                        }
                        else
                        {
                              fiftyMoveRule++;
                        }
                  }
                  enPassant = -1;
                  if (pieces.getType(square[move.startingSquare]) == pawn && Math.Abs(move.startingSquare - move.targetSquare) == 16)
                  {
                        enPassant = move.targetSquare;
                  }

                  if (pieces.getType(square[move.startingSquare]) == pawn && (move.targetSquare / 8 == 0 || move.targetSquare / 8 == 7))
                  {
                        data.Add(new affectedPiece(square[move.startingSquare], move.startingSquare, move.targetSquare));
                        if(square[move.targetSquare] != 0)
                        { 
                              data.Add(new affectedPiece(square[move.targetSquare], move.targetSquare, move.targetSquare));
                        }
                        square[move.targetSquare] = pieces.getColor(square[move.startingSquare]) | queen;
                        square[move.startingSquare] = 0;
                  }
                  else {
                        data.Add(new affectedPiece(square[move.startingSquare], move.startingSquare, move.targetSquare));
                        if (square[move.targetSquare] != 0)
                        {
                              data.Add(new affectedPiece(square[move.targetSquare], move.targetSquare, move.targetSquare));
                        }
                        square[move.targetSquare] = square[move.startingSquare];
                        square[move.startingSquare] = 0;
                  }

                  allMoves[currentMoveCount] = new boardData(data, prevCastle, prevEnPassant, prevMove, prevFiftyMoveRule);
                  currentMoveCount++;
            }

            public void unmakeMove(int[] square, bool buttonCliked = false)
            {
                  if (currentMoveCount == 0) return;
                  if (currentMoveCount <= (botPlayer == black ? 0 : 1) && buttonCliked == true) return;
                  currentMoveCount--;

                  for(int i = 0; i < 2; i++)
                  {
                        for (int j = 0; j < 2; j++)
                        {
                              castle[i, j] = allMoves[currentMoveCount].castle[i, j];
                        }
                  }
                  enPassant = allMoves[currentMoveCount].enPassant;
                  prevMove = allMoves[currentMoveCount].prevMove;
                  fiftyMoveRule = allMoves[currentMoveCount].fiftyMoveRule;
                  int color = -1;

                  foreach(affectedPiece piece in allMoves[currentMoveCount].pieces)
                  {
                        square[piece.startingSquare] = piece.piece;
                        if (piece.startingSquare != piece.targetSquare)
                        {
                              square[piece.targetSquare] = 0; 
                              color = pieces.getColor(square[piece.startingSquare]);
                        }
                  }

                  setPlayer(color);
            }
      }
}
