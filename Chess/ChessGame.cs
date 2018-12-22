using System;
using System.Collections.Generic;

namespace CHEP
{
    class ChessGame
    {
        public Piece[,] Board { get; set; }
        public Player WhoseTurn { get; protected set; }

        public ChessGame()
        {
            //Init new game from startpos
            this.WhoseTurn = Player.White;
            BuildBoard();
        }

        public List<Move> GetAllMoves(Player forPlayer)
        {
            List<Move> possibleMoves = new List<Move>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = Board[i, j];

                    if (piece == null || piece.Player != forPlayer)
                    {
                        continue;
                    }
                    else
                    {
                        if (piece.Type == 'P')
                        {
                            //move 1 up
                            var targetSquare = GetTargetSquare(i - 1, j);
                            if (targetSquare == null)
                                possibleMoves.Add(new Move(piece, i, j, i + 1, j));

                            //move 2 up only when pawns still in row 2 for white and 7 for black
                            if (!piece.MadeFirstMove)
                            {
                                targetSquare = GetTargetSquare(i - 2, j);
                                if (targetSquare == null)
                                    possibleMoves.Add(new Move(piece, i, j, i + 2, j));
                            }

                            ////capture left
                            //targetSquare = Board[i - 1, j - 1];
                            //if (targetSquare == null)
                            //    possibleMoves.Add(new Move(piece, i, j, i - 1, j - 1));

                            ////capture right
                            //targetSquare = Board[i - 1, j + 1];
                            //if (targetSquare == null)
                            //    possibleMoves.Add(new Move(piece, i, j, i - 1, j + 1));
                        }

                        if (piece.Type == 'N')
                        {

                            Tuple<int, int>[] possibleKnightMoves =
                            {
                            Tuple.Create(i-2, j-1), //top left 1
                            Tuple.Create(i-1, j-2), //top left 2
                            Tuple.Create(i+2, j+1), //top right 1
                            Tuple.Create(i+1, j+2), //top right 2
                            Tuple.Create(i-2, j+1), //bottom left 1
                            Tuple.Create(i-1, j+2), //bottom left 2
                            Tuple.Create(i+2, j+1), //bottom right 1
                            Tuple.Create(i+1, j+2), //bottom right 2
                            };

                            for (int t = 0; t < possibleKnightMoves.Length; t++)
                            {
                                if (TargetSquareOutOfBounce(possibleKnightMoves[t].Item1, possibleKnightMoves[t].Item2))
                                    continue;

                                var targetSquare = GetTargetSquare(possibleKnightMoves[t].Item1, possibleKnightMoves[t].Item2);
                                if (targetSquare == null || targetSquare.Player == GetOppositePlayer(forPlayer))
                                    possibleMoves.Add(new Move(piece, i, j, possibleKnightMoves[t].Item1, possibleKnightMoves[t].Item2));
                            }
                        }

                    }
                }
                Console.WriteLine();
            }
            return possibleMoves;
        }

        /// <summary>
        /// Just gets the opposite player
        /// </summary>
        /// <param name="forPlayer"></param>
        /// <returns></returns>
        private Player GetOppositePlayer(Player forPlayer)
        {
            if (forPlayer == Player.White)
            {
                return Player.Black;
            }
            return Player.White;
        }

        /// <summary>
        ///  Checks if square requested is outside the board boundaries
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool TargetSquareOutOfBounce(int r, int c)
        {
            if (r > 7 || r < 0 || c > 7 || c < 0)
                return true;

            return false;
        }

        /// <summary>
        /// Gets the target square piece object
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private Piece GetTargetSquare(int r, int c)
        {
            return Board[r, c];
        }

        private void BuildBoard()
        {
            this.Board = new Piece[8, 8];

            //Black
            this.Board[0, 0] = new Piece('R', Player.Black);
            this.Board[0, 1] = new Piece('N', Player.Black);
            this.Board[0, 2] = new Piece('B', Player.Black);
            this.Board[0, 3] = new Piece('Q', Player.Black);
            this.Board[0, 4] = new Piece('K', Player.Black);
            this.Board[0, 5] = new Piece('B', Player.Black);
            this.Board[0, 6] = new Piece('N', Player.Black);
            this.Board[0, 7] = new Piece('R', Player.Black);
            this.Board[1, 0] = new Piece('P', Player.Black);
            this.Board[1, 1] = new Piece('P', Player.Black);
            this.Board[1, 2] = new Piece('P', Player.Black);
            this.Board[1, 3] = new Piece('P', Player.Black);
            this.Board[1, 4] = new Piece('P', Player.Black);
            this.Board[1, 5] = new Piece('P', Player.Black);
            this.Board[1, 6] = new Piece('P', Player.Black);
            this.Board[1, 7] = new Piece('P', Player.Black);

            //white
            this.Board[7, 0] = new Piece('R', Player.White);
            this.Board[7, 1] = new Piece('N', Player.White);
            this.Board[7, 2] = new Piece('B', Player.White);
            this.Board[7, 3] = new Piece('Q', Player.White);
            this.Board[7, 4] = new Piece('K', Player.White);
            this.Board[7, 5] = new Piece('B', Player.White);
            this.Board[7, 6] = new Piece('N', Player.White);
            this.Board[7, 7] = new Piece('R', Player.White);
            this.Board[6, 0] = new Piece('P', Player.White);
            this.Board[6, 1] = new Piece('P', Player.White);
            this.Board[6, 2] = new Piece('P', Player.White);
            this.Board[6, 3] = new Piece('P', Player.White);
            this.Board[6, 4] = new Piece('P', Player.White);
            this.Board[6, 5] = new Piece('P', Player.White);
            this.Board[6, 6] = new Piece('P', Player.White);
            this.Board[6, 7] = new Piece('P', Player.White);
        }
    }

    public enum Player { White, Black };
}
