using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using CHEP.Helpers;

namespace CHEP
{
    class Program
    {
        static void Main(string[] args)
        {
            // try
            // {
            //     UCI uci = new UCI();
            //     uci.Start();
            // }
            // catch (Exception e)
            // {
            //     CLITools.WriteAndLog("Error: " + e.Message + " Inner: " + e.InnerException);
            // }

            //used to play in console
            //PlayInConsole local = new PlayInConsole();
            //local.Play();


            //testing new selfbuild chess game rules

            ChessGame game = new ChessGame();
            PrintGame(game);
            Console.WriteLine("Possible Moves: " + game.GetAllMoves(Player.White).Count.ToString() + "\n\n");

            //get a move
            Move m = game.GetAllMoves(Player.White)[4];
            game.MakeMove(m);

            PrintGame(game);
            Console.Write("Possible Moves: " + game.GetAllMoves(Player.White).Count.ToString() + "\n\n");

            //get a move
            Move m2 = game.GetAllMoves(Player.White)[6];
            game.MakeMove(m2);

            PrintGame(game);
            Console.Write("Possible Moves: " + game.GetAllMoves(Player.White).Count.ToString() + "\n\n");



            Console.Read();
        }

        private static void PrintGame(ChessGame game)
        {
            for (int i = 0; i < 8; i++)
            {
                StringBuilder row = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    var piece = game.Board[i, j];
                    if (piece == null)
                    {
                        row.Append("..");
                    }
                    else
                    {
                        row.Append(piece.Type + ".");
                    }
                }
                Console.WriteLine(row);
            }
        }
    }
}
