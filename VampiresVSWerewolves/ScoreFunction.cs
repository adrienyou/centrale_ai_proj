using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class ScoreFunction
    {
        int f1 = nb for i in ;  // Ennemy losses
        int w1 = 5;  // weight

        int f2 = b;  // Our losses
        int w2 = -5; // weight

        int f3 = c; // number of humans we converted
        int w3 = 8;

        int f4 = d; // number of humans our ennemies have converted
        int w4 = -8;

        // Who is closer to convertible Humans  (function in in State.cs)
        int f5 = sum (Distances from each of our groups to their CLOSEST SMALLER HUMAN group) 
                                - sum (Distances from each ENNEMIES groups to their CLOSEST SMALLER HUMAN group)
        int w5 = 3;

        // Who is closer to weaker ennemies  (distance function is in Position.cs)
        int f6 = sum (Distances from each of our groups to their CLOSEST SMALLER x 1.5 ENNEMIES group) 
                 - sum (Distances from each of Ennemies groups to their CLOSEST SMALLER x 1.5 group of OUR monsters) 
        int w6 = -2;

        // We won because we have more monsters !
        bool a7 = nbOurMonsters >= 1.5 * (nbEnnemies + nbHumans);
        int f7 = a7 ? 1 : 0;  // convert boolean to int
        int w7 = 1000;

        // We lost because ennemies have more monsters
        bool a8 = nbEnnemies >= 1.5 * (nbOurMonsters + nbHumans);
        int f8 = a8 ? 1 : 0;  // convert boolean to int
        int w8 = -1000;

        // We won because all ennemies were killed !
        bool a9 = nbOurMonsters==0;
        int f9 = a9 ? 1 : 0;  // convert boolean to int
        int w9 = 2000;


        // We lost because all our monsters were killed
        bool a10 = nbEnnemies==0;
        int f10 = a10 ? 1 : 0;  // convert boolean to int
        int w10 = -2000;



        evaluationScore = f1*w1 + f2*w2 + f3*w3 + f4*w4 + f5*w5 + f6*w6 + f7*w7 + f8*w8 + f9*w9 + f10*w10;

        return evaluationScore;

    }
}
