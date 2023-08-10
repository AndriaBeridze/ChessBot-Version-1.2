using System;
using System.Drawing;
using System.Windows.Forms;
using Chess.Scripts.Data;
using Chess.Scripts.Core.Visual;
using Chess.Scripts.Core.Engine;
using Chess.Scripts.Core.Bot;
using Chess.Scripts.Core.Bot.Evaluation;
using System.Media;

using static Chess.Scripts.Core.Engine.MoveGenerator;
using static Chess.Scripts.Data.Pieces;
using static Chess.Scripts.Data.Variables;
using System.Threading.Tasks;

namespace Chess
{
      public partial class GameWindow : Form
      {
            MoveGenerator moveGenerator = new MoveGenerator();
            Buttons buttons = new Buttons();
            Board board;
            Pieces pieces = new Pieces();
            ChessBot chessBot = new ChessBot();
            Evaluator evaluator = new Evaluator();
            MoveSound moveSound = new MoveSound();
            MoveMaker moveMaker = new MoveMaker();

            Timer timer = new Timer();

            int copiedSquare = -1;
            public GameWindow()
            {
                  InitializeComponent();

                  board = new Board();

                  for(int i = 0; i < 64; i++)
                  {
                        this.Controls.Add(board.graphicBoard[i]);
                        board.graphicBoard[i].MouseDown += new MouseEventHandler(drag);
                        board.graphicBoard[i].MouseEnter += new EventHandler(drop);
                        board.graphicBoard[i].MouseUp += new MouseEventHandler(release);
                  }

                  this.Controls.Add(board.backgroundPanel);
                  this.Controls.Add(buttons.exit);

                  buttons.exit.MouseClick += new MouseEventHandler(exit);

                  timer.Stop();
                  timer.Tick += makeMove;

                  timer.Start();
                  Console.Clear();
            }
            private void makeMove(object sender, EventArgs e)
            {
                  timer.Stop();
                  makeBotMove();
            }
            public void makeBotMove()
            {
                  if(player == botPlayer) {
                        Move move = chessBot.makeMove(board.square, player);

                        var sound = playSound(move, player ^ 24);
                        switchPlayer();

                        moveMaker.makeMove(move, board.square);
                        prevMove = move;
                        board.setPieces();
                        board.clear();

                        sound.Play();

                        if (evaluator.isDraw(board.square))
                        {
                              setPlayer(-1);
                              Console.WriteLine("Draw!");
                        }
                        else if (moveGenerator.generateAllMoves(board.square, player).Count == 0)
                        {
                              if (moveGenerator.isInCheck(player, board.square))
                              {
                                    setPlayer(-1);
                                    Console.WriteLine("Chackmate, Bot Wins!");
                              }
                              else
                              {
                                    setPlayer(-1);
                                    Console.WriteLine("StaleMate!");
                              }
                        }
                  }
            }
            new void exit(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;
                  this.Close();
            }
            new void drag(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;
                  Panel currentPanel = sender as Panel;
                  board.clear();

                  if (currentPanel.BackgroundImage != null)
                  {
                        if (pieces.getColor(board.square[getIndex(currentPanel)]) == player)
                        {
                              board.VisualizeMove(moveGenerator.generateMoves(getIndex(currentPanel), board.square));
                        }

                        Bitmap bmp = (Bitmap) currentPanel.BackgroundImage;
                        bmp = new Bitmap(bmp, new Size(bmp.Width * 127 / 100, bmp.Height * 127 / 100));

                        currentPanel.BackgroundImage = null;
                        this.Cursor = new Cursor(bmp.GetHicon());
                        copiedSquare = getIndex(currentPanel);
                  }
            }

            new void drop(object sender, EventArgs e)
            {
                  Panel currentPanel = sender as Panel;

                  if (board.canMove(currentPanel))
                  {
                        Move move = new Move(copiedSquare, getIndex(currentPanel));
                        var sound = playSound(move, botPlayer);

                        chessBot.madeMoves.Add(new Move(copiedSquare, getIndex(currentPanel)));
                        board.makeVisualMove(new Move(copiedSquare, getIndex(currentPanel)));
                        board.clear();

                        sound.Play();

                        this.Cursor = Cursors.Default;

                        if (evaluator.isDraw(board.square))
                        {
                              setPlayer(-1);
                              Console.WriteLine("Draw!");
                        }
                        else if (moveGenerator.generateAllMoves(board.square, player).Count == 0)
                        {
                              if(moveGenerator.isInCheck(player, board.square))
                              {
                                    setPlayer(-1);
                                    Console.WriteLine("Chackmate, Player Wins!");
                              }
                              else
                              {
                                    setPlayer(-1);
                                    Console.WriteLine("StaleMate!");
                              }
                        }
                        else
                        {
                              board.clear();
                              timer.Start();
                        }
                  }
                  else
                  {
                        if(this.Cursor != Cursors.Default) moveSound.IllegalMove.Play();
                        board.clear();
                        board.setPieces();
                  }
                  this.Cursor = Cursors.Default;
                  copiedSquare = -1;
            }

            new void release(object sender, MouseEventArgs e)
            {
                  if (e.Button == MouseButtons.Right) return;
                  Panel currentPanel = sender as Panel;

                  if (currentPanel.ClientRectangle.Contains(currentPanel.PointToClient(MousePosition)) && copiedSquare != -1)
                  {
                        board.clear();
                        board.setPieces();
                        this.Cursor = Cursors.Default;
                        copiedSquare = -1;
                  }
            }

            SoundPlayer playSound(Move move, int color)
            {
                  moveMaker.makeMove(move, board.square);
                  if (evaluator.isDraw(board.square))
                  {
                        moveMaker.unmakeMove(board.square);
                        return moveSound.GameOver;
                  }
                  if (moveGenerator.generateAllMoves(board.square, color).Count == 0)
                  {
                        moveMaker.unmakeMove(board.square);
                        return moveSound.GameOver;
                  }
                  if (moveGenerator.isInCheck(color, board.square))
                  {
                        moveMaker.unmakeMove(board.square);
                        return moveSound.Check;
                  }
                  moveMaker.unmakeMove(board.square);

                  if (pieces.getType(board.square[move.startingSquare]) == king && Math.Abs(move.startingSquare - move.targetSquare) == 2) return moveSound.Castle;

                  if (board.square[move.targetSquare] != 0) return moveSound.Capture;
                  
                  if (pieces.getType(board.square[move.startingSquare]) == pawn && Math.Abs(move.startingSquare - move.targetSquare) % 8 != 0) return moveSound.Capture;
                 
                  return moveSound.Move;
            }
      }
}
