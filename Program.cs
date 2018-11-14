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
            Engine engine = new Engine();

            while (true)
            {
                var nextMove = UpdateConsoleAndGetMove(game);

                if (game.WhoseTurn == Player.Black)
                {
                    //Random move
                    //IEnumerable<Move> validMoves = game.GetValidMoves(Player.Black);
                    //Move randomMove = validMoves.OrderBy(t => Guid.NewGuid()).FirstOrDefault();
                    //MoveType type = game.ApplyMove(randomMove, true);

                    //calculate move
                    Move bestMove = engine.CalculateBestMove(game);
                    MoveType type = game.ApplyMove(bestMove, true);
                }
                else
                {
                    String[] move = nextMove.Split(" ");
                    Move playerMove = new Move(move[0], move[1], Player.White);
                    bool isValid = game.IsValidMove(playerMove);

                    if (!isValid)
                    {
                        continue;
                    }
                    MoveType type = game.ApplyMove(playerMove, true);

                }
            }
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
