using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class Worker
    {
        // Pour l'instant void, mais retournera un Move (classe à créer)
        public static void DoWork()
        {
            //Create stop Stopwatch and start it.
            Stopwatch sw = new Stopwatch();
            sw.Start();


            while (sw.Elapsed < TimeSpan.FromSeconds(1.5))
            {
                //result == fonction qui calcule l'arbre et tout 
            }
            sw.Stop();
        }

    }
}
