using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    public class ScoreFunction
    {
        public static int evauateScore ( ) {
        
            int f1 = nbOurMonsters - nb ;  // nb ennemies vs. nb our monsters
            int w1 = 10;  // weight


            // How many humans groups are for us * how many humans to convert (distance function in State.cs)

            int f2 =   (Distances from each ENNEMIES groups to their CLOSEST SMALLER HUMAN group) 
                                    - moy (Distances from each of our groups to their CLOSEST SMALLER HUMAN group) )
            int w2 = 5;

            // How many weaker ennemies groups are close * how many ennemies to kill  (distance function is in Position.cs)
            // Pour chacun de mes groupes, qui est le groupe ennemi le plus proche ? 
            //Est-ce que je le défonce (+) *nb ennmis ? Il me défonce (-)*nb mes monstres ? Bataille aléatoire (0) ? 

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
            int w9 = 100000;


            // We lost because all our monsters were killed
            bool a10 = nbEnnemies==0;
            int f10 = a10 ? 1 : 0;  // convert boolean to int
            int w10 = -100000;



            evaluationScore = f1*w1 + f2*w2 + f3*w3 + f4*w4 + f5*w5 + f6*w6 + f7*w7 + f8*w8 + f9*w9 + f10*w10;

            return evaluationScore;

        }

    }
}
