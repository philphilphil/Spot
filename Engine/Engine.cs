using ChessDotNet;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHEP
{
    class Engine
    {
        DateTime startTime;
        int[,] pawnWhite;
        int[,] pawnBlack;
        int nodeCounter = 0;

        public Move CalculateBestMove(ChessGame game)
        {
            Move bestMove = null;
            int bestRating = -9999;
            String originalPositionFen = game.GetFen();

            SetupPieceSquareEvals();

            //get all possible legal moves
            List<Move> validMoves = game.GetValidMoves(game.WhoseTurn).ToList();

            this.startTime = DateTime.Now;

            //itterate through all possible moves
            foreach (var move in validMoves)
            {
                //dummy values for now
                Console.WriteLine("info depth 1 score cp 1 time " + MillisecondsSinceStart(this.startTime) + " nodes " + nodeCounter.ToString());

                //set fen back to original position
                game = new ChessGame(originalPositionFen);

                //do the move
                var turnMadeBy = game.WhoseTurn;
                MoveType type = game.ApplyMove(move, true);

                //get best board rating after depth 4
                bool maximisingPlayer = turnMadeBy == Player.White ? true : false;
                var boardRating = MiniMaxBestMove(game, 4, -9999, 9999, maximisingPlayer);

                //if its blacks turn, take negative of the rating because blacks pieces are valued in -
                if (turnMadeBy == Player.Black)
                {
                    boardRating = -boardRating; //maybe: -Math.Abs(boardRating);
                }

                //check if higher than currently best rating, if yes set to current best
                if (boardRating > bestRating)
                {
                    bestRating = boardRating;
                    bestMove = move;
                }
            }

            return bestMove;
        }

        private void SetupPieceSquareEvals()
        {
            // Piece-Square Tables https://www.chessprogramming.org/Simplified_Evaluation_Function
            // pawn
            this.pawnWhite = new int[,]
            {
                { 0,  0,  0,  0,  0,  0,  0,  0 },
                { 50, 50, 50, 50, 50, 50, 50, 50},
                { 10, 10, 20, 30, 30, 20, 10, 10},
                { 5,  5, 10, 25, 25, 10,  5,  5},
                { 0,  0,  0, 20, 20,  0,  0,  0},
                { 5, -5,-10,  0,  0,-10, -5,  5},
                { 5, 10, 10,-20,-20, 10, 10,  5},
                { 0,  0,  0,  0,  0,  0,  0,  0 }
            };

            this.pawnBlack = new int[,]
            {
                { 0,  0,  0,  0,  0,  0,  0,  0 },
                { 5, 10, 10,-20,-20, 10, 10,  5},
                { 5, -5,-10,  0,  0,-10, -5,  5},
                { 0,  0,  0, 20, 20,  0,  0,  0},
                { 5,  5, 10, 25, 25, 10,  5,  5},
                { 10, 10, 20, 30, 30, 20, 10, 10},
                { 50, 50, 50, 50, 50, 50, 50, 50},
                { 0,  0,  0,  0,  0,  0,  0,  0 }
            };
        }

        private string MillisecondsSinceStart(DateTime startDate)
        {
            TimeSpan span = DateTime.Now - startDate;
            int ms = (int)span.TotalMilliseconds;
            return ms.ToString();
        }

        //MiniMax - https://en.wikipedia.org/wiki/Minimax#Pseudocode
        public int MiniMaxBestMove(ChessGame game, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            nodeCounter++;
            //when final depth is found, return found mov
            if (depth == 0)
            {
                return GetBoardRating(game.GetBoard());
            }

            List<Move> validMoves = game.GetValidMoves(game.WhoseTurn).ToList();


            if (!maximizingPlayer)
            {
                int bestMove = -9999;
                foreach (var move in validMoves)
                {
                    String originalPositionFen = game.GetFen();
                    var turnMadeBy = game.WhoseTurn;
                    MoveType type = game.ApplyMove(move, true);
                    bestMove = Math.Max(bestMove, MiniMaxBestMove(game, depth - 1, alpha, beta, true));
                    game = new ChessGame(originalPositionFen);

                    alpha = Math.Max(alpha, bestMove);
                    if (beta <= alpha)
                    {
                        return bestMove;
                    }
                }

                return bestMove;
            }
            else //minimizing player
            {
                int bestMove = 9999;
                foreach (var move in validMoves)
                {
                    String originalPositionFen = game.GetFen();
                    var turnMadeBy = game.WhoseTurn;
                    MoveType type = game.ApplyMove(move, true);
                    bestMove = Math.Min(bestMove, MiniMaxBestMove(game, depth - 1, alpha, beta, false));
                    game = new ChessGame(originalPositionFen);

                    beta = Math.Min(beta, bestMove);
                    if (beta <= alpha)
                    {
                        return bestMove;
                    }
                }

                return bestMove;
            }
        }


        /// <summary>
        /// Calculates the value of the current board based on pre given piece value numbers
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private int GetBoardRating(Piece[][] board)
        {
            int rating = 0;
            int counterRow = 0;
            int counterColumn = 0;

            foreach (var row in board)
            {
                counterRow++;
                counterColumn = 0;

                foreach (var piece in row)
                {
                    counterColumn++;

                    if (piece == null)
                    {
                        continue;
                    }

                    int pieceValue = GetPieceValue(piece, counterRow, counterColumn);
                    if (piece.Owner == Player.Black)
                    {
                        pieceValue = -pieceValue;
                    }

                    rating += pieceValue;
                }

            }

            return rating;
        }

        /// <summary>
        /// Get a value for each piece for each player
        /// </summary>
        /// <param name="piece"></param>
        /// <returns></returns>
        private int GetPieceValue(Piece piece, int row, int column)
        {
            int value = 0;

            switch (piece.GetFenCharacter().ToString().ToLower())
            {
                case "p":
                    value = 100 + (piece.Owner == Player.White ? this.pawnWhite[row - 1, column - 1] : this.pawnBlack[row - 1, column - 1]);
                    break;
                case "n":
                    value = 320;
                    break;
                case "b":
                    value = 330;
                    break;
                case "r":
                    value = 500;
                    break;
                case "q":
                    value = 900;
                    break;
                case "k":
                    value = 2000;
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
