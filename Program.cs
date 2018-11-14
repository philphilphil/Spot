using System;
using System.Collections.Generic;
using System.Linq;
using ChessDotNet;

namespace CHEPPP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Let's start by creating a chess game instance.
            ChessGame game = new ChessGame();

            while (true)
            {
                var nextMove = UpdateConsoleAndGetMove(game);

                if (game.WhoseTurn == Player.Black)
                {
                    IEnumerable<Move> validMoves = game.GetValidMoves(Player.Black);
                    Move randomMove = validMoves.OrderBy(t => Guid.NewGuid()).FirstOrDefault();
                    MoveType type = game.ApplyMove(randomMove, true);
                }
                else
                {
                    String[] move = nextMove.Split(" ");
                    Move playerMove = new Move(move[0], move[1], Player.White);
                    bool isValid = game.IsValidMove(playerMove);
                    MoveType type = game.ApplyMove(playerMove, true);

                }
            }
  
            //Console.WriteLine("Move type: {0}", type);


            //Console.WriteLine("It's this color's turn: {0}", game.WhoseTurn);

            //// You can figure out all valid moves using GetValidMoves.
            //IEnumerable<Move> validMoves = game.GetValidMoves(Player.Black);

            //Console.WriteLine("How many valid moves does black have? {0}", validMoves.Count());

            //bool hasValidMoves = game.HasAnyValidMoves(Player.Black);
            //Console.WriteLine("Black has any valid moves: {0}", hasValidMoves);

            //CLITools.DrawBoard(game);


            Console.ReadKey();
        }
        private static string UpdateConsoleAndGetMove(ChessGame game)
        {
            Console.Clear();
            CLITools.DrawBoard(game);
            Console.WriteLine("It's this color's turn: {0}", game.WhoseTurn);

            if (game.WhoseTurn == Player.White)
            {
                Console.Write("Enter move: ");
                return Console.ReadLine();

            }

            return null;
        }
    }
}
