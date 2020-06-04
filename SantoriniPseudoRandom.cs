using System;
using System.Collections.Generic;
using System.Linq;

namespace Santorini
{
    public class SantoriniPseudoRandom
    {
        Random rnd;
        public SantoriniPseudoRandom (int randSeeds)
        {
            rnd = new Random(randSeeds);
        }
        public SantoriniPseudoRandom()
        {
            rnd = new Random();
        }


        public PawnMove TakeTurn(List<PawnMove> moves, Board b)
        {
            PawnMove WinMove = moves.FirstOrDefault(x => b.IsWinningMove(x));
            if (WinMove != null)
            {
                return WinMove;
            }
            return moves.OrderBy(x => rnd.Next(0, 10000)).ToList()[0];
        }

    }
}
