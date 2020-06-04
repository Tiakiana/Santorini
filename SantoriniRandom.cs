using System.Collections.Generic;
using System.Linq;

namespace Santorini
{
    public class SantoriniRandom
    {
        public static PawnMove TakeTurn(List<PawnMove> moves)
        {
            return moves.OrderBy(x => Rnd.Range(0, 10000)).ToList()[0];
        }
    }
}
