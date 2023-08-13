using System.Collections.Generic;
using Chess.Scripts.Core.Engine;
using System.Windows.Forms;
using Chess.Scripts.Data;
using System.Drawing;
using System;


using static Chess.Scripts.Core.Engine.MoveGenerator;
using static Chess.Scripts.Data.BoardThemes;
using static Chess.Scripts.Data.Variables;
using static Chess.Scripts.Data.Pieces;

namespace Chess.Scripts.Core.Visual
{
      internal class Board
      {
            MoveMaker moveMaker = new MoveMaker();
            MoveGenerator moveGenerator = new MoveGenerator();
            Fen fen = new Fen();
            PieceImages pieceImages = new PieceImages();
            Pieces pieces = new Pieces();

            public int[] square;
            public Panel[] graphicBoard;
            public Panel backgroundPanel;

            public Board()
            {
                  backgroundPanel = new Panel()
                  {
                        Size = new Size(backgroundPanelSize, backgroundPanelSize),
                        BackColor = Color.FromArgb(51, 51, 51),
                        Location = new Point(boardPaddingX - 30, boardPaddingY - 30)
                  };
                  square = new int[64];
                  graphicBoard = new Panel[64];

                  fen.setPosition(square);
                  displayBoard();
                  setPieces();
            }
            void displayBoard()
            {
                  for(int i = 0; i < 64; i++)
                  {
                        int col = i & 7, row = i >> 3;
                        graphicBoard[i] = new Panel()
                        {
                              Size = new Size(squareSize, squareSize),
                              BackColor = (row + col) % 2 == 1 ? darkCol : lightCol,
                              Location = new Point((botPlayer == black ? col : 7 - col) * squareSize + boardPaddingX, (botPlayer == black ? row : 7 - row) * squareSize + boardPaddingY),
                        };
                  }
            }

            public void setPieces()
            {
                  for(int i = 0; i < 64; i++)
                  {
                        graphicBoard[i].BackgroundImage = pieceImages.pieceImage[square[i]];
                  }
            }

            public bool canMove(Panel panel)
            {
                  return panel.BackColor == darkMoveHighlighter || panel.BackColor == lightMoveHighlighter;
            }

            public void print(int[] x)
            {
                  for(int i = 0; i < 64; i++)
                  {
                        Console.Write(x[i] + " " + (x[i] < 10 ? " " : ""));
                        if (i % 8 == 7) Console.WriteLine();
                  }
                  Console.WriteLine();
            }

            public void makeVisualMove(Move move)
            {
                  moveMaker.makeMove(move, square);
                  setPieces();
                  clear();
                  prevMove = move;
                  switchPlayer();
            }

            public void clear()
            {
                  for (int i = 0; i < 64; i++) {
                        int row = i / 8, col = i % 8;
                        if (pieces.getType(square[i]) == king && moveGenerator.isInCheck(pieces.getColor(square[i]), square))
                        {
                              graphicBoard[i].BackColor = (row + col) % 2 == 1 ? darkCheck : lightCheck;
                        }
                        else if (i == prevMove.startingSquare || i == prevMove.targetSquare)
                        {
                              if (graphicBoard[i].BackColor != lightMoved || graphicBoard[i].BackColor != darkMoved)
                              {
                                    graphicBoard[i].BackColor = (row + col) % 2 == 1 ? darkMoved : lightMoved;
                              }
                        }
                        else
                        {
                              if (graphicBoard[i].BackColor != lightCol || graphicBoard[i].BackColor != darkCol)
                              {
                                    graphicBoard[i].BackColor = (row + col) % 2 == 1 ? darkCol : lightCol;
                              }
                        }
                  }
            }

            public void VisualizeMove(List<Move> moves)
            {
                  int index = -1;

                  foreach(Move move in moves)
                  {
                        index = move.startingSquare;
                        int col = move.targetSquare >> 3, row = move.targetSquare & 7;

                        graphicBoard[move.targetSquare].BackColor = (col + row) % 2 == 1 ? darkMoveHighlighter : lightMoveHighlighter;
                  }

                  if (index == -1) return;
                  graphicBoard[index].BackColor = ((index & 7) + (index >> 3)) % 2 == 1 ? darkMoved : lightMoved;
            }
      }
}

