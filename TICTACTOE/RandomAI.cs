using System.Collections.Generic;

namespace Santorini
{
    public class RandomAI
    {
        public PawnMove TakeTurn(TicTacToeBoard t)
        {
            List<PawnMove> moves = t.GetAvailableMoves();
            return moves[Rnd.Range(0, moves.Count)];
        }


    }
}
