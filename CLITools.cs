using ChessDotNet;
using System;

namespace CHEPPP
{
    static class CLITools
    {
        public static void DrawBoard(ChessGame game)
        {
            Piece[][] board = game.GetBoard();

            foreach (Piece[] row in board)
            {
                Console.WriteLine(Print(row));
            }
        }

        private static string Print(Piece[] row)
        {
            var rowString = "";

            foreach (Piece piece in row)
            {
                if (piece == null)
                {
                    rowString += "  ";
                }
                else
                {
                    rowString += piece.GetFenCharacter() + " ";
                }
            }


            return rowString;
        }

        private static string GetPieceChar(Piece piece)
        {
            throw new NotImplementedException();
        }
    }
}