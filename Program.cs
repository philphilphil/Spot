using System;
using System.Collections.Generic;
using System.Linq;
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


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var piece = game.Board[i, j];

                    if (piece == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(piece.Type + " ");

                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Moves: ");
            Console.WriteLine(game.GetAllMoves(Player.White).Count.ToString());

            Console.Read();
        }
    }
}
