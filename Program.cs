using System;
using System.Collections.Generic;
using System.Linq;
using CHEP.Helpers;

namespace CHEP
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UCI uci = new UCI();
                uci.Start();
            }
            catch (Exception e)
            {
                CLITools.WriteAndLog("Error: " + e.Message + " Inner: " + e.InnerException);
            }

            //used to play in console
            //PlayInConsole local = new PlayInConsole();
            //local.Play();
        }


    }
}
