using Chess.Scripts.Core.Engine;
using System.Collections.Generic;
using Chess.Scripts.Core.Bot.Evaluation;

using static Chess.Scripts.Data.Pieces;
using static Chess.Scripts.Core.Engine.MoveGenerator;
using System;
using Chess.Scripts.Data;

namespace Chess.Scripts.Core.Bot
{
      internal class ChessBot
      {
            Random rnd = new Random();
            MoveGenerator moveGenerator = new MoveGenerator();
            Evaluator evaluator = new Evaluator();
            MoveMaker moveMaker = new MoveMaker();
            Pieces pieces = new Pieces();

            Move bestMove;
            public List<Move> madeMoves = new List<Move>();
            static int negativeInfinity = -1000000000, positiveInfinity = 1000000000;

            int[] pieceValue = new int[7]
            {
                  0, positiveInfinity, 900, 500, 300, 300, 100
            };
            int depth = 4;

            public Move makeMove(int[] square, int color)
            {
                  for (int j = 0; j < 10000; j++)
                  {
                        List<List<Move>> newOpenings = new List<List<Move>>();
                        foreach (List<Move> opening in pieces.possibleOpenings)
                        {
                              if (currentMoveCount > opening.Count) continue;
                              bool ok = true;
                              for (int i = 0; i < madeMoves.Count; i++)
                              {
                                    if (madeMoves[i].startingSquare != opening[i].startingSquare) ok = false;
                                    if (madeMoves[i].targetSquare != opening[i].targetSquare) ok = false;
                              }
                              if (ok)
                              {
                                    newOpenings.Add(opening);
                              }
                        }
                        pieces.possibleOpenings = newOpenings;
                  }
                  
                  if (pieces.possibleOpenings.Count != 0)
                  {
                        bestMove = pieces.possibleOpenings[rnd.Next(pieces.possibleOpenings.Count)][currentMoveCount - 1];
                  }
                  else
                  {
                        bestMove = new Move(-1, -1);
                        think(square, color, depth, true, negativeInfinity * 2, positiveInfinity * 2);
                  }
                  madeMoves.Add(bestMove);
                  return bestMove;
            }
            public int think(int[] square, int color, int depth, bool firstMove, int alpha, int beta)
            {
                  if (depth == 0) return searchCaptures(square, color, alpha, beta);

                  List<Move> moves = moveGenerator.generateAllMoves(square, color);
                  if (moves.Count == 0)
                  {
                        if (moveGenerator.isInCheck(color, square)) return negativeInfinity - depth;
                        return 0;
                  }

                  orderMoves(moves, square);
                  foreach (Move move in moves)
                  {
                        moveMaker.makeMove(move, square);
                        int currentValue = -think(square, color ^ 24, depth - 1, false, -beta, -alpha);
                        moveMaker.unmakeMove(square);

                        if (currentValue >= beta) return beta;

                        if (alpha < currentValue && firstMove) bestMove = move;

                        alpha = Math.Max(alpha, currentValue);
                  }
                  return alpha;
            }

            public void orderMoves(List<Move> moves, int[] square)
            {
                  List<Tuple<int, int>> ind = new List<Tuple<int, int>>();
                  List<Move> currentMoves = new List<Move>();
                  int i = 0;
                  foreach (Move move in moves)
                  {
                        int moveScore = 0;
                        int movePieceType = pieces.getType(square[move.startingSquare]);
                        int movePieceColor = pieces.getColor(square[move.startingSquare]);
                        int capturePieceType = pieces.getType(square[move.targetSquare]);

                        if (capturePieceType != 0)
                        {
                              moveScore = 10 * pieceValue[capturePieceType] - pieceValue[movePieceType];
                        }

                        if ((move.targetSquare / 8 == 0 || move.targetSquare / 8 == 7) && movePieceType == pawn)
                        {
                              moveScore += 900;
                        }

                        if (move.targetSquare + (movePieceColor == white ? -1 : 1) * 7 >= 0 && move.targetSquare + (movePieceColor == white ? -1 : 1) * 7 <= 63)
                        {
                              if (square[move.targetSquare + (movePieceColor == white ? -1 : 1) * 7] == (24 - movePieceColor) + pawn)
                              {
                                    moveScore -= pieceValue[movePieceType];
                              }
                        }

                        if (move.targetSquare + (movePieceColor == white ? -1 : 1) * 9 >= 0 && move.targetSquare + (movePieceColor == white ? -1 : 1) * 9 <= 63)
                        {
                              if (square[move.targetSquare + (movePieceColor == white ? -1 : 1) * 9] == (24 - movePieceColor) + pawn)
                              {
                                    moveScore -= pieceValue[movePieceType];
                              }
                        }

                        ind.Add(new Tuple<int, int>(moveScore, i));
                        currentMoves.Add(move);
                        i++;
                  }
                  ind.Sort();
                  i = 0;
                  for (int j = currentMoves.Count - 1; j >= 0; j--)
                  {
                        moves[i] = currentMoves[ind[j].Item2];
                        i++;
                  }
            }

            public int searchCaptures(int[] square, int color, int alpha, int beta)
            {
                  int eval = evaluator.evaluate(square, color);
                  if(eval >= beta)
                  {
                        return beta;
                  }
                  alpha = Math.Max(alpha, eval);

                  List<Move> moves = moveGenerator.generateCaptureMoves(square, color);
                  if (moves.Count == 0)
                  {
                        if (moveGenerator.isInCheck(color, square)) return negativeInfinity;
                  }

                  orderMoves(moves, square);
                  foreach (Move move in moves)
                  {
                        moveMaker.makeMove(move, square);
                        int currentValue = -searchCaptures(square, color ^ 24, -beta, -alpha);
                        moveMaker.unmakeMove(square);

                        if (currentValue >= beta) return beta;

                        alpha = Math.Max(alpha, currentValue);
                  }
                  return alpha;
            }
      }
}
