using System;
using System.Collections.Generic;
using System.Linq;
using CHEP.Helpers;
using ChessDotNet;

namespace CHEP
{
    class Program
    {
        static void Main(string[] args)
        {
            UCI uci = new UCI();
            uci.Start();

            //used to play in console
            //PlayInConsole local = new PlayInConsole();
            //local.Play();
        }


    }
}
