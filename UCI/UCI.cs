using ChessDotNet;
using Serilog;
using System;
using System.Reflection;

namespace CHEP
{
    public class UCI
    {

        ChessGame game;
        public UCI()
        {
            SendId();
            SendReady();
        }

        public void Start()
        {

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.File("logs\\uci_logs.txt")
               .CreateLogger();

            while (true)
            {
                string uciCommand = Console.ReadLine();
                if (uciCommand != null)
                {
                    if (ParseUciCommand(uciCommand.Trim()))
                    {
                        //command runs
                    }
                    else
                    {
                        //when false was sent break
                        Console.WriteLine("Exiting.");
                        break;
                    }
                }
            }
        }

        private bool ParseUciCommand(string uciCommand)
        {
            if (uciCommand == null)
            {
                Console.WriteLine("Invalid command");
                return true; // Invalid
            }
            string[] commandParts = uciCommand.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (commandParts.Length == 0)
            {
                Console.WriteLine("Invalid command");
                return true; // Invalid
            }

            Log.Debug(uciCommand);

            switch (commandParts[0])
            {
                case "quit":
                    return false;
                case "isready":
                    IsReady();
                    break;
                case "uci":
                    IsReady();
                    break;
                case "position":
                    Position(uciCommand);
                    break;
                case "go":
                    Go(uciCommand);
                    break;
                case "debug":
                    Debug();
                    break;
                default:
                    return true;
            }
            return true;
        }

        private void Debug()
        {
            this.game = new ChessGame(@"6r1/7p/1r6/2P3bk/2p3p1/1P1Q4/P4PPP/R5K1 w - - 0 1");
            this.Go("");
        }

        private void Go(string uciCommand)
        {
            Engine engine = new Engine();
            Move move = engine.CalculateBestMove(this.game);

            CLITools.WriteAndLog(String.Format("bestmove {0}{1}", move.OriginalPosition.ToString().ToLower(), move.NewPosition.ToString().ToLower()));
        }

        //cmd: position fen N7/P3pk1p/3p2p1/r4p2/8/4b2B/4P1KP/1R6 w - - 0 34
        private void Position(string uciCommand)
        {
            //add propper command parsing here
            if (uciCommand.Contains("startpos"))
            {
                String[] moves = uciCommand.Split(" ");
                this.game = new ChessGame();

                foreach (var move in moves)
                {
                    if (move == "position" || move == "startpos" || move == "moves")
                    {
                        continue;
                    }

                    Move m = new Move(move.Substring(0, 2), move.Substring(2, 2), this.game.WhoseTurn);
                    game.ApplyMove(m, false);
                }

                return;
            }

            //Get string after fen
            int indexOfFen = uciCommand.IndexOf("fen") + 3;
            String fen = uciCommand.Substring(indexOfFen, uciCommand.Length - indexOfFen);

            this.game = new ChessGame(fen);
        }

        private void SendId()
        {
            Console.WriteLine("id name CHEP 0.1"); // TODO: get version from assembly
            Console.WriteLine("id author Phil Baum");
        }

        private static void SendReady()
        {
            Console.WriteLine("uciok");
        }

        private void IsReady()
        {
            Console.WriteLine("readyok"); //TODO: Always ready?
        }
    }
}
