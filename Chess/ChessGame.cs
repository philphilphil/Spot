using System;
using System.Collections.Generic;

namespace CHEP
{
    class ChessGame
    {
        public Piece[,] Board { get; set; }
        private Piece PieceBackup { get; set; }
        public Player WhoseTurn { get; protected set; }
        public bool WhiteCanCastleKingSide { get; protected set; }
        public bool WhiteCanCastleQueenSide { get; protected set; }
        public bool BlackCanCastleKingSide { get; protected set; }
        public bool BlackCanCastleQueenSide { get; protected set; }
        private Tuple<int, int> WhiteKingPosition { get; set; }
        private Tuple<int, int> BlackKingPosition { get; set; }

        public ChessGame()
        {
            //Init new game from startpos
            this.WhoseTurn = Player.White;
            BuildBoard();
        }

        public bool MakeMove(Move move)
        {
            PieceBackup = Board[move.RowTo, move.ColumnTo];
            Board[move.RowTo, move.ColumnTo] = Board[move.RowFrom, move.ColumFrom];
            Board[move.RowFrom, move.ColumFrom] = null;

            this.WhoseTurn = GetOppositePlayer(this.WhoseTurn);

            //if move was a king move, save new king positions
            if (move.Piece.Type == 'K')
            {
                if (move.Piece.Player == Player.White)
                {
                    WhiteKingPosition = Tuple.Create(move.RowTo, move.RowFrom);
                }
                else
                {
                    BlackKingPosition = Tuple.Create(move.RowTo, move.RowFrom);

                }
            }

            return true;
        }


        public bool UndoMove(Move move)
        {
            Board[move.RowFrom, move.ColumFrom] = Board[move.RowTo, move.ColumnTo];
            Board[move.RowTo, move.ColumnTo] = PieceBackup;
            this.WhoseTurn = GetOppositePlayer(this.WhoseTurn);
            return true;
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
                            if (forPlayer == Player.White)
                            {
                                //1 up
                                int targetRow = i - 1, targetCol = j;
                                Piece targetSquare = null;

                                if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                {
                                    targetSquare = GetTargetSquare(targetRow, targetCol);
                                    if (targetSquare == null)
                                        ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                }

                                //move 2 up only when pawns still in row 2 for white and 7 for black
                                if (i == 6)
                                {

                                    targetRow = i - 2;
                                    if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                    {
                                        targetSquare = GetTargetSquare(targetRow, targetCol);
                                        if (targetSquare == null)
                                            ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                    }
                                }

                                //capture left
                                targetRow = i - 1;
                                targetCol = j + 1;
                                if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                {
                                    targetSquare = GetTargetSquare(targetRow, targetCol);
                                    if (targetSquare != null && targetSquare.Player == Player.Black)
                                        ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                }
                                //capture right
                                targetRow = i + 1;
                                targetCol = j + 1;

                                if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                {
                                    targetSquare = GetTargetSquare(targetRow, targetCol);
                                    if (targetSquare != null && targetSquare.Player == Player.Black)
                                        ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                }

                                //enpassant

                                //promotion
                            }
                            else if (forPlayer == Player.Black)
                            {
                                //1 up
                                int targetRow = i + 1, targetCol = j;
                                Piece targetSquare = null;

                                if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                {
                                    targetSquare = GetTargetSquare(targetRow, targetCol);
                                    if (targetSquare == null)
                                        ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                }

                                //move 2 up only when pawns still in row 2 for white and 7 for black
                                if (i == 1)
                                {
                                    targetRow = i + 2;
                                    if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                    {
                                        targetSquare = GetTargetSquare(targetRow, targetCol);
                                        if (targetSquare == null)
                                            ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                    }
                                }

                                //capture left
                                targetRow = i + 1;
                                targetCol = j - 1;

                                if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                {
                                    targetSquare = GetTargetSquare(targetRow, targetCol);
                                    if (targetSquare != null && targetSquare.Player == Player.Black)
                                        ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                }

                                //capture right
                                targetRow = i - 1;
                                targetCol = j - 1;

                                if (!TargetSquareOutOfBounce(targetRow, targetCol))
                                {
                                    targetSquare = GetTargetSquare(targetRow, targetCol);
                                    if (targetSquare != null && targetSquare.Player == Player.Black)
                                        ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                                }

                                //enpassant

                                //promotion
                            }

                        }
                        else if (piece.Type == 'N')
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
                                if (CheckSquares(i, j, possibleKnightMoves[t].Item1, possibleKnightMoves[t].Item2, piece, ref possibleMoves, forPlayer))
                                    continue;
                            }
                        }
                        else if (piece.Type == 'K')
                        {
                            Tuple<int, int>[] possibleKingMoves =
{
                            Tuple.Create(i+1, j+1),
                            Tuple.Create(i+1, j-1),
                            Tuple.Create(i+1, j),
                            Tuple.Create(i, j-1),
                            Tuple.Create(i, j+1),
                            Tuple.Create(i-1, j-1),
                            Tuple.Create(i-1, j+1),
                            Tuple.Create(i-1, j),
                            };

                            for (int t = 0; t < possibleKingMoves.Length; t++)
                            {
                                if (CheckSquares(i, j, possibleKingMoves[t].Item1, possibleKingMoves[t].Item2, piece, ref possibleMoves, forPlayer))
                                    continue;
                            }
                        }


                        if (piece.Type == 'B' || piece.Type == 'Q')
                        {
                            //top left
                            int targetRow = i, targetCol = j;

                            while (true)
                            {
                                targetRow++;
                                targetCol--;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                            //top right
                            targetRow = i;
                            targetCol = j;

                            while (true)
                            {
                                targetRow++;
                                targetCol++;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                            //bottom right
                            targetRow = i;
                            targetCol = j;

                            while (true)
                            {
                                targetRow--;
                                targetCol++;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                            //bottom left
                            targetRow = i;
                            targetCol = j;

                            while (true)
                            {
                                targetRow--;
                                targetCol--;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                        }
                        if (piece.Type == 'R' || piece.Type == 'Q')
                        {

                            //up
                            int targetRow = i, targetCol = j;
                            while (true)
                            {
                                targetRow++;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                            //down
                            targetRow = i;
                            targetCol = j;
                            while (true)
                            {
                                targetRow--;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                            //left
                            targetRow = i;
                            targetCol = j;
                            while (true)
                            {
                                targetCol--;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }

                            //right
                            targetRow = i;
                            targetCol = j;
                            while (true)
                            {
                                targetCol--;

                                if (CheckSquares(i, j, targetRow, targetCol, piece, ref possibleMoves, forPlayer))
                                    break;
                            }
                        }
                    }
                }
            }
            return possibleMoves;
        }



        /// <summary>
        /// Checks for B,N,R,Q if move is possible
        /// </summary>
        /// <param name="i">Current row in array</param>
        /// <param name="j"> Current column in array</param>
        /// <param name="targetRow">Target row</param>
        /// <param name="targetCol">Target col</param>
        /// <param name="piece">tThe Piece</param>
        /// <param name="possibleMoves">Ref list</param>
        /// <param name="forPlayer">Players turn</param>
        /// <returns></returns>
        private bool CheckSquares(int i, int j, int targetRow, int targetCol, Piece piece, ref List<Move> possibleMoves, Player forPlayer)
        {
            if (TargetSquareOutOfBounce(targetRow, targetCol))
                return true;

            Piece targetSquare = GetTargetSquare(targetRow, targetCol);

            if (targetSquare == null)
            {
                ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                return false;
            }
            else if (targetSquare.Player == GetOppositePlayer(forPlayer))
            {
                ValidateAndAddMove(piece, i, j, targetRow, targetCol, ref possibleMoves);
                //if enemypiece, no further possible
                return true;
            }
            else
            {
                //its his own piece blocking
                return true;
            }
        }

        private void ValidateAndAddMove(Piece piece, int i, int j, int targetRow, int targetCol, ref List<Move> possibleMoves)
        {
            if (KingIsInCheckNow(piece, i, j, targetRow, targetCol))
                return;

            possibleMoves.Add(new Move(piece, i, j, targetRow, targetCol));
        }

        private bool KingIsInCheckNow(Piece piece, int i, int j, int targetRow, int targetCol)
        {

            //TODO: maybe putting into one if faster?
            int kingRow = WhoseTurn == Player.White ? WhiteKingPosition.Item1 : BlackKingPosition.Item1;
            int kingCol = WhoseTurn == Player.White ? WhiteKingPosition.Item2 : BlackKingPosition.Item2;

            //Check for Pawns
            int pawn1Row, pawnCol, pawn2Row;
            if (WhoseTurn == Player.White)
            {
                //for white, check if a pawn is in top left and top right of the king
                pawnCol = kingCol + 1;
                pawn1Row = kingRow + 1;
                pawn2Row = kingRow - 1;

            }
            else
            {
                //for black, check if a pawn is in bottom left and bottom right of the king
                pawnCol = kingCol - 1;
                pawn1Row = kingRow + 1;
                pawn2Row = kingRow - 1;
            }

            if (CheckSquareForEnemiePieces(pawn1Row, pawnCol))
                return true;

            if (CheckSquareForEnemiePieces(pawn2Row, pawnCol))
                return true;

            //Check for knights
            Tuple<int, int>[] possibleKnightLocations =
                            {
                            Tuple.Create(kingRow-2, kingCol-1), //top left 1
                            Tuple.Create(kingRow-1, kingCol-2), //top left 2
                            Tuple.Create(kingRow+2, kingCol+1), //top right 1
                            Tuple.Create(kingRow+1, kingCol+2), //top right 2
                            Tuple.Create(kingRow-2, kingCol+1), //bottom left 1
                            Tuple.Create(kingRow-1, kingCol+2), //bottom left 2
                            Tuple.Create(kingRow+2, kingCol+1), //bottom right 1
                            Tuple.Create(kingRow+1, kingCol+2), //bottom right 2
                            };

            for (int t = 0; t < possibleKnightLocations.Length; t++)
            {
                if (CheckSquareForEnemiePieces(possibleKnightLocations[t].Item1, possibleKnightLocations[t].Item2))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// If piece was found at this location king is in check
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private bool CheckSquareForEnemiePieces(int r, int c)
        {
            if (TargetSquareOutOfBounce(r, c))
                return false;

            Piece targetSquare = GetTargetSquare(r, c);

            if (targetSquare == null)
                return false;

            if (targetSquare.Player == GetOppositePlayer(WhoseTurn))
            {
                //its an enemy piece
                return true;
            }


            return false;
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

            WhiteKingPosition = Tuple.Create(7, 4);
            BlackKingPosition = Tuple.Create(0, 4);
        }
    }

    public enum Player { White, Black };
    public enum Type { Pawn, Bishop, Knight, Rook, Queen, King };
}
