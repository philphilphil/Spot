using Rudz.Chess;
using Rudz.Chess.Factories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CHEP
{
    class Engine
    {
        DateTime StartTime;
        int[,] PawnWhite, KnightWhite, BishopWhite, RookWhite, QueenWhite, KingWhite;
        int[,] PawnBlack, KnightBlack, BishopBlack, RookBlack, QueenBlack, KingBlack;
        int nodeCounter = 0;

        //DEBUGGING

        public Engine()
        {
            SetupPieceSquareEvals();
        }

        ///// <summary>
        ///// Calculation starting point. itterate through all legal moves and start the minimax calculation 
        ///// </summary>
        ///// <param name="game"></param>
        ///// <returns></returns>
        //public Move CalculateBestMove(ChessGame game)
        //{
        //    Move bestMove = null;
        //    int bestRating = -9999;
        //    this.StartTime = DateTime.Now;
        //    String originalPositionFen = game.GetFen();

        //    List<Move> validMoves = game.GetValidMoves(game.WhoseTurn).ToList();

        //    foreach (var move in validMoves)
        //    {
        //        //UCI Info output
        //        Console.WriteLine("info depth 1 score cp 1 time " + MillisecondsSinceStart(this.StartTime) + " nodes " + nodeCounter.ToString());

        //        //set fen back to original position
        //        game = new ChessGame(originalPositionFen);

        //        //do the move
        //        var turnMadeBy = game.WhoseTurn;
        //        MoveType type = game.MakeMove(move, true);

        //        //get best board rating after depth 4
        //        bool maximisingPlayer = turnMadeBy == Player.White ? true : false;
        //        var boardRating = MiniMaxBestMove(game, 4, -9999, 9999, maximisingPlayer);

        //        //if its blacks turn, take negative of the rating because blacks wants to minimize
        //        if (turnMadeBy == Player.Black)
        //        {
        //            boardRating = -boardRating; //maybe: -Math.Abs(boardRating);
        //        }

        //        //check if higher than currently best rating, if yes set to current best
        //        if (boardRating >= bestRating)
        //        {
        //            bestRating = boardRating;
        //            bestMove = move;
        //        }
        //    }

        //    return bestMove;
        //}

        //public void SplitPerft(int depth)
        //{
        //    ChessGame game = new ChessGame();
        //    Engine engine = new Engine();
        //    long allNodes = 0;
        //    List<Move> moves = game.GetAllMoves(game.WhoseTurn).ToList();

        //    foreach (var move in moves)
        //    {
        //        game.MakeMove(move);
        //        long nodes = engine.GetNodesForPosition(game, depth);
        //        allNodes += nodes;
        //        Console.WriteLine(String.Format("{0}:  {1}", ConvCordsToText(move.ColumFrom, move.RowFrom) + ConvCordsToText(move.ColumnTo, move.RowTo), nodes));
        //        game.UndoMove(move);
        //    }

        //    Console.WriteLine("Nodes searched: " + allNodes.ToString());

        //}

        private string ConvCordsToText(int c, int r)
        {
            string cords = "";

            if (c == 0)
            {
                cords = "a";
            }
            else if (c == 1)
            {
                cords = "b";
            }
            else if (c == 2)
            {
                cords = "c";
            }
            else if (c == 3)
            {
                cords = "d";
            }
            else if (c == 4)
            {
                cords = "e";
            }
            else if (c == 5)
            {
                cords = "f";
            }
            else if (c == 6)
            {
                cords = "g";
            }
            else if (c == 7)
            {
                cords = "h";
            }

            if (r == 0)
            {
                cords += "8";
            }
            else if (r == 1)
            {
                cords += "7";
            }
            else if (r == 2)
            {
                cords += "6";
            }
            else if (r == 3)
            {
                cords += "5";
            }
            else if (r == 4)
            {
                cords += "4";
            }
            else if (r == 5)
            {
                cords += "3";
            }
            else if (r == 6)
            {
                cords += "2";
            }
            else if (r == 7)
            {
                cords += "1";
            }


            ///cords += (r + 1).ToString();

            return cords;
        }

        public void Perft(int depth)
        {

            var board = new Board();
            var pieceValue = new PieceValue();
            var position = new Position(board, pieceValue);
            var game = GameFactory.Create(position);

            for (int i = 1; i <= depth; i++)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                game.NewGame();

                var nodes = game.Perft(i, false);
                stopWatch.Stop();

                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                Console.WriteLine(String.Format("Depth: {0} Nodes: {1} Time: {2}", i.ToString(), nodes.ToString(), elapsedTime));

                Console.WriteLine("");
            }
        }

        //private long GetNodesForPosition(ChessGame game, int depth)
        //{
        //    long nodes = 0;
        //    if (depth == 0) return 1;
        //    List<Move> moves = game.GetAllMoves(game.WhoseTurn).ToList();


        //    foreach (var move in moves)
        //    {
        //        game.MakeMove(move);
        //        nodes += GetNodesForPosition(game, depth - 1);
        //        game.UndoMove(move);
        //    }

        //    return nodes;
        //}

        /// <summary>
        /// MiniMax - https://en.wikipedia.org/wiki/Minimax#Pseudocode
        /// </summary>
        /// <param name="game"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="maximizingPlayer"></param>
        /// <returns></returns>
        //public int MiniMaxBestMove(ChessGame game, int depth, int alpha, int beta, bool maximizingPlayer)
        //{
        //    nodeCounter++;
        //    //when final depth is found, return found mov
        //    if (depth == 0)
        //    {
        //        return GetBoardRating(game.GetBoard());
        //    }

        //    List<Move> validMoves = game.GetValidMoves(game.WhoseTurn).ToList();


        //    if (!maximizingPlayer)
        //    {
        //        int bestMove = -9999;
        //        foreach (var move in validMoves)
        //        {
        //            String originalPositionFen = game.GetFen();
        //            var turnMadeBy = game.WhoseTurn;
        //            MoveType type = game.MakeMove(move, true);
        //            bestMove = Math.Max(bestMove, MiniMaxBestMove(game, depth - 1, alpha, beta, true));
        //            game = new ChessGame(originalPositionFen);

        //            alpha = Math.Max(alpha, bestMove);
        //            if (beta <= alpha)
        //            {
        //                return bestMove;
        //            }
        //        }

        //        return bestMove;
        //    }
        //    else //minimizing player
        //    {
        //        int bestMove = 9999;
        //        foreach (var move in validMoves)
        //        {
        //            String originalPositionFen = game.GetFen();
        //            var turnMadeBy = game.WhoseTurn;
        //            MoveType type = game.MakeMove(move, true);
        //            bestMove = Math.Min(bestMove, MiniMaxBestMove(game, depth - 1, alpha, beta, false));
        //            game = new ChessGame(originalPositionFen);

        //            beta = Math.Min(beta, bestMove);
        //            if (beta <= alpha)
        //            {
        //                return bestMove;
        //            }
        //        }

        //        return bestMove;
        //    }
        //}


        /// <summary>
        /// Calculates the value of the current board based on pre given piece value numbers
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        //private int GetBoardRating(Piece[][] board)
        //{
        //    int rating = 0;
        //    int counterRow = 0;
        //    int counterColumn = 0;

        //    foreach (var row in board)
        //    {
        //        counterRow++;
        //        counterColumn = 0;

        //        foreach (var piece in row)
        //        {
        //            counterColumn++;

        //            if (piece == null)
        //            {
        //                continue;
        //            }

        //            int pieceValue = GetPieceValue(piece, counterRow, counterColumn);
        //            if (piece.Owner == Player.Black)
        //            {
        //                pieceValue = -pieceValue;
        //            }

        //            rating += pieceValue;
        //        }

        //    }

        //    return rating;
        //}

        /// <summary>
        /// Calculates ms between the startDate and now
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private string MillisecondsSinceStart(DateTime startDate)
        {
            TimeSpan span = DateTime.Now - startDate;
            int ms = (int)span.TotalMilliseconds;
            return ms.ToString();
        }

        /// <summary>
        /// Get a value for each piece for each player
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        //private int GetPieceValue(Piece piece, int row, int column)
        //{
        //    int value = 0;

        //    switch (piece.GetFenCharacter().ToString().ToLower())
        //    {
        //        case "p":
        //            value = 100 + (piece.Owner == Player.White ? this.PawnWhite[row - 1, column - 1] : this.PawnBlack[row - 1, column - 1]);
        //            break;
        //        case "n":
        //            value = 320 + (piece.Owner == Player.White ? this.KnightWhite[row - 1, column - 1] : this.KnightBlack[row - 1, column - 1]);
        //            break;
        //        case "b":
        //            value = 330 + (piece.Owner == Player.White ? this.BishopWhite[row - 1, column - 1] : this.BishopBlack[row - 1, column - 1]);
        //            break;
        //        case "r":
        //            value = 500 + (piece.Owner == Player.White ? this.RookWhite[row - 1, column - 1] : this.RookBlack[row - 1, column - 1]);
        //            break;
        //        case "q":
        //            value = 900 + (piece.Owner == Player.White ? this.QueenWhite[row - 1, column - 1] : this.QueenBlack[row - 1, column - 1]);
        //            break;
        //        case "k":
        //            value = 2000 + (piece.Owner == Player.White ? this.KingWhite[row - 1, column - 1] : this.KingBlack[row - 1, column - 1]);
        //            break;
        //        default:
        //            break;
        //    }

        //    return value;
        //}

        /// <summary>
        /// Piece-Square Tables https://www.chessprogramming.org/Simplified_Evaluation_Function
        /// </summary>
        private void SetupPieceSquareEvals()
        {
            this.PawnWhite = new int[,]
            {
                 { 0,  0,  0,  0,  0,  0,  0,  0},
                 { 50, 50, 50, 50, 50, 50, 50, 50},
                 { 10, 10, 20, 30, 30, 20, 10, 10},
                 { 5,  5, 10, 25, 25, 10,  5,  5},
                 { 0,  0,  0, 20, 20,  0,  0,  0},
                 { 5, -5,-10,  0,  0,-10, -5,  5},
                 { 5, 10, 10,-20,-20, 10, 10,  5},
                 { 0,  0,  0,  0,  0,  0,  0,  0 }
            };

            this.PawnBlack = new int[,]
            {
                 { 0,  0,  0,  0,  0,  0,  0,  0},
                 { 5, 10, 10,-20,-20, 10, 10,  5},
                 { 5, -5,-10,  0,  0,-10, -5,  5},
                 { 0,  0,  0, 20, 20,  0,  0,  0},
                 { 5,  5, 10, 25, 25, 10,  5,  5},
                 { 10, 10, 20, 30, 30, 20, 10, 10},
                 { 50, 50, 50, 50, 50, 50, 50, 50},
                 { 0,  0,  0,  0,  0,  0,  0,  0 }
            };

            this.KnightWhite = new int[,]
            {
                 {-50,-40,-30,-30,-30,-30,-40,-50},
                 {-40,-20,  0,  0,  0,  0,-20,-40},
                 {-30,  0, 10, 15, 15, 10,  0,-30},
                 {-30,  5, 15, 20, 20, 15,  5,-30},
                 {-30,  0, 15, 20, 20, 15,  0,-30},
                 {-30,  5, 10, 15, 15, 10,  5,-30},
                 {-40,-20,  0,  5,  5,  0,-20,-40},
                 {-50,-40,-30,-30,-30,-30,-40,-50}
            };

            this.KnightBlack = new int[,]
            {
                 {-50,-40,-30,-30,-30,-30,-40,-50},
                 {-40,-20,  0,  5,  5,  0,-20,-40},
                 {-30,  5, 10, 15, 15, 10,  5,-30},
                 {-30,  0, 15, 20, 20, 15,  0,-30},
                 {-30,  5, 15, 20, 20, 15,  5,-30},
                 {-30,  0, 10, 15, 15, 10,  0,-30},
                 {-40,-20,  0,  0,  0,  0,-20,-40},
                 {-50,-40,-30,-30,-30,-30,-40,-50}
            };

            this.BishopWhite = new int[,]
            {
                 {-20,-10,-10,-10,-10,-10,-10,-20},
                 {-10,  0,  0,  0,  0,  0,  0,-10},
                 {-10,  0,  5, 10, 10,  5,  0,-10},
                 {-10,  5,  5, 10, 10,  5,  5,-10},
                 {-10,  0, 10, 10, 10, 10,  0,-10},
                 {-10, 10, 10, 10, 10, 10, 10,-10},
                 {-10,  5,  0,  0,  0,  0,  5,-10},
                 {-20,-10,-10,-10,-10,-10,-10,-20}
            };

            this.BishopBlack = new int[,]
            {
                 {-20,-10,-10,-10,-10,-10,-10,-20},
                 {-10,  5,  0,  0,  0,  0,  5,-10},
                 {-10, 10, 10, 10, 10, 10, 10,-10},
                 {-10,  0, 10, 10, 10, 10,  0,-10},
                 {-10,  5,  5, 10, 10,  5,  5,-10},
                 {-10,  0,  5, 10, 10,  5,  0,-10},
                 {-10,  0,  0,  0,  0,  0,  0,-10},
                 {-20,-10,-10,-10,-10,-10,-10,-20}
            };

            this.RookWhite = new int[,]
            {
                 {  0,  0,  0,  0,  0,  0,  0,  0},
                 {  5, 10, 10, 10, 10, 10, 10,  5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 {  0,  0,  0,  5,  5,  0,  0,  0}
            };

            this.RookBlack = new int[,]
            {
                 {  0,  0,  0,  5,  5,  0,  0,  0},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 { -5,  0,  0,  0,  0,  0,  0, -5},
                 {  5, 10, 10, 10, 10, 10, 10,  5},
                 {  0,  0,  0,  0,  0,  0,  0,  0}
            };

            this.QueenWhite = new int[,]
            {
                 {-20,-10,-10, -5, -5,-10,-10,-20},
                 {-10,  0,  0,  0,  0,  0,  0,-10},
                 {-10,  0,  5,  5,  5,  5,  0,-10},
                 { -5,  0,  5,  5,  5,  5,  0, -5},
                 {  0,  0,  5,  5,  5,  5,  0, -5},
                 {-10,  5,  5,  5,  5,  5,  0,-10},
                 {-10,  0,  5,  0,  0,  0,  0,-10},
                 {-20,-10,-10, -5, -5,-10,-10,-20}
            };

            this.QueenBlack = new int[,]
             {
                 {-20,-10,-10, -5, -5,-10,-10,-20},
                 {-10,  0,  5,  0,  0,  0,  0,-10},
                 {-10,  5,  5,  5,  5,  5,  0,-10},
                 {  0,  0,  5,  5,  5,  5,  0, -5},
                 { -5,  0,  5,  5,  5,  5,  0, -5},
                 {-10,  0,  5,  5,  5,  5,  0,-10},
                 {-10,  0,  0,  0,  0,  0,  0,-10},
                 {-20,-10,-10, -5, -5,-10,-10,-20}
            };

            this.KingWhite = new int[,]
            {
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-20,-30,-30,-40,-40,-30,-30,-20},
                 {-10,-20,-20,-20,-20,-20,-20,-10},
                 { 20, 20,  0,  0,  0,  0, 20, 20},
                 { 20, 30, 10,  0,  0, 10, 30, 20}
            };

            this.KingBlack = new int[,]
            {
                 { 20, 30, 10,  0,  0, 10, 30, 20},
                 { 20, 20,  0,  0,  0,  0, 20, 20},
                 {-10,-20,-20,-20,-20,-20,-20,-10},
                 {-20,-30,-30,-40,-40,-30,-30,-20},
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-30,-40,-40,-50,-50,-40,-40,-30},
                 {-30,-40,-40,-50,-50,-40,-40,-30}
            };

        }
    }
}
