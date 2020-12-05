using Rudz.Chess;
using Rudz.Chess.Factories;
using Rudz.Chess.MoveGeneration;
using Rudz.Chess.Types;
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
            //try
            //{
            //    UCI uci = new UCI();
            //    uci.Start();
            //}
            //catch (Exception e)
            //{
            //    CLITools.WriteAndLog("Error: " + e.Message + " Inner: " + e.InnerException);
            //}

            //used to play in console
            //PlayInConsole local = new PlayInConsole();
            //local.Play();

            Engine e = new Engine();
            e.Perft(6);


            //testing
            var board = new Board();
            var pieceValue = new PieceValue();
            var position = new Position(board, pieceValue);
            var game = GameFactory.Create(position);
            game.NewGame();
            var state = new State();

            var moves = position.GenerateMoves();
            Console.WriteLine("Possible Moves: " + moves.Count().ToString() + "\n\n");

            //get a move
            Move m = moves[4];
            position.MakeMove(m, state);


            moves = position.GenerateMoves();
            Console.WriteLine("Possible Moves: " + moves.Count().ToString() + "\n\n");

            m = moves[4];
            position.MakeMove(m, state);

            moves = position.GenerateMoves();
            Console.WriteLine("Possible Moves: " + moves.Count().ToString() + "\n\n");

            m = moves[4];
            position.MakeMove(m, state);

            Console.WriteLine(position.FenNotation);



            Console.Read();
        }


    }
}
