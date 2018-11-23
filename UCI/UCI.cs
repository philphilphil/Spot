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
            //this.game = new ChessGame(@"6r1/7p/1r6/2P3bk/2p3p1/1P1Q4/P4PPP/R5K1 w - - 0 1");
            this.Position("position startpos moves b2b3 d7d5 b1c3 b8c6 a1b1 e7e5 b1a1 f8c5 a1b1 c8g4 b1a1 d8h4 g2g3 h4f6 g1f3 a8d8 a1b1 h7h5 b1a1 g8e7 a1b1 e7f5 b1a1 f6h6 d2d4 h6d6 d4c5 d6c5 c1b2 f7f6 h2h3 g4f3 e2f3 f5d4 h3h4 c5b4 a1b1 e8g8 a2a3 b4e7 b1a1 f8e8 a3a4 c6b4 a1b1 d4c2 e1d2 e7c5 d1e2 c7c6 d2d1 d8d6 f1g2 a7a5 h1f1 g7g6 f1g1 d6d7 g2h3 f6f5 g1f1 b7b6 f1g1 d7d8 g1f1 d5d4 c3e4 c2e3 f2e3 d4e3 e4d6 b6b5 d1e1 b4c2 e1d1 c2b4 d1e1 b4c2 e1d1 d8d6 b2d4 d6d4 d1c1 c2b4 c1b2 d4d2 e2d2 e3d2 b1a1 b5a4 a1a4 c5c2");
            this.Go("");
        }

        private void Go(string uciCommand)
        {
            Engine engine = new Engine();
            Move move = engine.CalculateBestMove(this.game);

            if (move == null)
            {
                throw new Exception("No move found.");
            }

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
