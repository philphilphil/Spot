using System;
using System.Reflection;

namespace CHEPPP
{
    public class UCI
    {
        public UCI()
        {
            SendId();
            SendReady();
        }

        public void Start()
        {
            while (true)
            {
                string uciCommand = Console.ReadLine();
                if (uciCommand != null)
                {
                    if (ParseUciCommand(uciCommand.Trim().ToLower())) {
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

            uciCommand = commandParts[0];

            switch (uciCommand)
            {
                case "quit":
                    return false;    
                case "isready":
                    IsReady();
                    break;
                case "position":
                    Position(commandParts);
                    break;
                default:
                    return true;
            }
            return true;
        }

        private void Position(string[] parts)
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid command, Fen missing?");
                return; // Invalid
            }
        }

        private void SendId()
        {
            Console.WriteLine("id name CHEPPP 0.1"); // TODO: get version from assembly
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
