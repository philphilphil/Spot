using ChessDotNet;
using Serilog;
using System;
using System.Diagnostics;
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
                case "perft":
                    PerftScore(commandParts);
                    break;
                default:
                    return true;
            }
            return true;
        }

        private void PerftScore(string[] uciCommand)
        {
            int depth = int.Parse(uciCommand[1]);
            Engine e = new Engine();
            e.Perft(depth);
        }


        private void Debug()
        {
            //this.game = new ChessGame(@"6r1/7p/1r6/2P3bk/2p3p1/1P1Q4/P4PPP/R5K1 w - - 0 1");
            // this.Position("position startpos moves g2g3 d7d6 h2h4 e7e5 h4h5 g8e7 e2e4 b8d7 g3g4 d7c5 d2d4 e5d4 d1d4 c8d7 d4b4 a7a5 b4d2 c5e4 d2f4 d7c6 f2f3 e4f6 f4g5 h7h6 g5e3 f6g4 e3f4 d8d7 f1h3 e7f5 f4g4 d7e6 g1e2 c6b5 h1h2 f5e7 g4e6 f7e6 h3e6 e7c6 e6h3 c6b4 h3f5 a5a4 f5g6 e8d7 g6f5 d7c6 e2d4 c6b6 d4b5 a8b8 f5d7 c7c5 b5c3 b6a5 a2a3 b4c2 h2c2 b7b5 d7b5 b8b5 c3b5 f8e7 b5c3 h8b8 f3f4 e7f6 f4f5 b8b6 c3e2 b6b7 e2c3 b7c7 c3e2 c7d7 e2c3 f6e7 c3e2 d6d5 f5f6 e7d6 f6g7 d7g7 c1h6 g7g2 h6e3 d6e7 h5h6 a5b6 h6h7 e7f6 e3c5 b6a5 c5b4 a5a6 c2c6 a6b5 c6f6 g2f2 f6f2 d5d4");
            this.Position("position startpos moves g2g3 d7d6 h2h4 e7e5 h4h5 g8e7 e2e4 b8d7 g3g4 d7c5 d2d4 e5d4 d1d4 c8d7 d4b4 a7a5 b4d2 c5e4 d2f4 d7c6 f2f3 e4f6 f4g5 h7h6 g5e3 f6g4 e3f4 d8d7 f1h3 e7f5 f4g4 d7e6 g1e2 c6b5 h1h2 f5e7 g4e6 f7e6 h3e6 e7c6 e6h3 c6b4 h3f5 a5a4 f5g6 e8d7 g6f5 d7c6 e2d4 c6b6 d4b5 a8b8 f5d7 c7c5 b5c3 b6a5 a2a3 b4c2 h2c2 b7b5 d7b5 b8b5 c3b5 f8e7 b5c3 h8b8 f3f4 e7f6 f4f5 b8b6 c3e2 b6b7 e2c3 b7c7 c3e2 c7d7 e2c3 f6e7 c3e2 d6d5 f5f6 e7d6 f6g7 d7g7 c1h6 g7g2 h6e3 d6e7 h5h6 a5b6 h6h7 e7f6 e3c5 b6a5 c5b4 a5a6 c2c6 a6b5 c6f6 g2f2 f6f2 d5d4");
            this.Go("");
        }

        private void Go(string uciCommand)
        {
        //    Engine engine = new Engine();
        //    Move move = engine.CalculateBestMove(this.game);

        //    if (move == null)
        //    {
        //        throw new Exception("No move found.");
        //    }

        //    CLITools.WriteAndLog(String.Format("bestmove {0}{1}{2}", move.OriginalPosition.ToString().ToLower(), move.NewPosition.ToString().ToLower(), move.Promotion.ToString().ToLower()));
        }

        //cmd: position fen N7/P3pk1p/3p2p1/r4p2/8/4b2B/4P1KP/1R6 w - - 0 34
        private void Position(string uciCommand)
        {
            //using startpos command
            //if (uciCommand.Contains("startpos"))
            //{
            //    String[] moves = uciCommand.Split(" ");
            //    this.game = new ChessGame();

            //    foreach (var move in moves)
            //    {
            //        if (move == "position" || move == "startpos" || move == "moves")
            //        {
            //            continue;
            //        }

            //        Move m;
            //        string from = move.Substring(0, 2);
            //        string to = move.Substring(2, 2);

            //        //check for promotion
            //        if (move.Length > 4)
            //        {
            //            char promotion = Char.Parse(move.Substring(4, 1));
            //            m = new Move(from, to, this.game.WhoseTurn, promotion);
            //        }
            //        else
            //        {
            //            m = new Move(from, to, this.game.WhoseTurn);
            //        }

            //        game.MakeMove(m, false);
            //    }

            //    return;
            //}

            ////Get string after fen
            //int indexOfFen = uciCommand.IndexOf("fen") + 3;
            //String fen = uciCommand.Substring(indexOfFen, uciCommand.Length - indexOfFen);

            //this.game = new ChessGame(fen);
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
