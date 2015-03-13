using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VampiresVSWerewolves
{
    class Alphabeta //with NegaMax
    {
        public int alphabeta(int depth, int alpha, int beta)
        {
            if ( gameOver || depth <= 0 )
            {
                return evaluationScore;
            }
            move(bestMove);

            foreach ( PossibleMove m in successor )
            {
                move(m);
                int score = -alphabeta(depht-1,-beta,-alpha);
                undoMove(m);

                if ( score >= alpha )
                {w
                    alpha = score;
                    bestMove = m;
                    if ( alpha >= beta )
                    {
                        break;
                    }
                }
            }
            return bestMove;
        }
    }
}
