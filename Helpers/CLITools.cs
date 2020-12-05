//using Rudz.Chess;
//using Serilog;
//using System;
//using System.Text;

//namespace CHEP
//{
//    static class CLITools
//    {
//        public static void PrintGame(Game game)
//        {
//            for (int i = 0; i < 8; i++)
//            {
//                StringBuilder row = new StringBuilder();
//                for (int j = 0; j < 8; j++)
//                {
//                    var piece = game.Pos[i, j];
//                    if (piece == null)
//                    {
//                        row.Append("x ");
//                    }
//                    else
//                    {
//                        row.Append(piece.Type + " ");
//                    }
//                }
//                Console.WriteLine(row);
//            }
//        }

//        public static void WriteAndLog(string message)
//        {
//            Log.Debug(message);
//            Console.WriteLine(message);
//        }
//    }
//}