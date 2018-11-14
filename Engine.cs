using ChessDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHEPPP
{
    class Engine
    {
        public Move CalculateBestMove(ChessGame game)
        {
            Move bestMove = null;
            int bestRating = -9999;
            String originalPositionFen = game.GetFen();

            //get all possible legal moves
            List<Move> validMoves = game.GetValidMoves(game.WhoseTurn).ToList();



            //itterate through all possible moves
            foreach (var item in validMoves)
            {
                //set fen back to original position
                game = new ChessGame(originalPositionFen);

                //do the move
                var turnMadeBy = game.WhoseTurn;
                MoveType type = game.ApplyMove(item, true);

                //get board value after this move
                var boardRating = GetBoardRating(game.GetBoard());

                //if its blacks turn, take negative of the rating because blacks pieces are valued in -
                if (turnMadeBy == Player.Black)
                {
                    boardRating = -boardRating; //maybe: -Math.Abs(boardRating);
                }

                //check if higher than currently best rating, if yes set to current best
                if (boardRating > bestRating)
                {
                    bestRating = boardRating;
                    bestMove = item;
                }
            }

            return bestMove;
        }

        /// <summary>
        /// Calculates the value of the current board based on pre given piece value numbers
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        private int GetBoardRating(Piece[][] board)
        {
            int rating = 0;

            foreach (var row in board)
            {
                foreach (var piece in row)
                {
                    if (piece == null)
                    {
                        continue;
                    }

                    int pieceValue = GetPieceValue(piece);
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
        private int GetPieceValue(Piece piece)
        {
            int value = 0;

            switch (piece.GetFenCharacter().ToString().ToLower())
            {
                case "p":
                    value = 10;
                    break;
                case "n":
                    value = 30;
                    break;
                case "b":
                    value = 40;
                    break;
                case "r":
                    value = 60;
                    break;
                case "q":
                    value = 90;
                    break;
                case "k":
                    value = 900;
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
