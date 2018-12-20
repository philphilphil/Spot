using ChessDotNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CHEP.Helpers
{
    /// <summary>
    /// Used to play against the engine in the console
    /// </summary>
    class PlayInConsole
    {
        public void Play()
        {
            // Let's start by creating a chess game instance.
            //min max test fen 8/8/2R2B2/2N5/3b4/8/8/8 w KQkq -

            ChessGame game = new ChessGame(@"7k/8/8/2R2B2/2N5/3b4/8/7K w - - 0 1");
            Engine engine = new Engine();

            while (true)
            {
                var nextMove = UpdateConsoleAndGetMove(game);

                if (game.WhoseTurn == Player.Black)
                {
                    //Random move
                    //IEnumerable<Move> validMoves = game.GetValidMoves(Player.Black);
                    //Move randomMove = validMoves.OrderBy(t => Guid.NewGuid()).FirstOrDefault();
                    //MoveType type = game.MakeMove(randomMove, true);

                    //calculate move
                    Move bestMove = engine.CalculateBestMove(game);
                    MoveType type = game.MakeMove(bestMove, true);
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
                    MoveType type = game.MakeMove(playerMove, true);

                }
            }
            Console.ReadKey();
        }

        private string UpdateConsoleAndGetMove(ChessGame game)
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
