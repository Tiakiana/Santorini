using System;
using System.Collections.Generic;
using System.Linq;

namespace Santorini
{
    public class QLearnAITICTACTOE
    {
        public Dictionary<string, List<PawnMove>> Policies = new Dictionary<string, List<PawnMove>>();
        public List<PawnMove> TakenMoves = new List<PawnMove>();
        public int ExplorationRate = 150;
        public bool Spectation = false;
        public PawnMove TakeTurn(TicTacToeBoard t)
        {
            if (Policies.ContainsKey(t.GetBoardState()))
            {

                PawnMove moveToTake = Policies[t.GetBoardState()].OrderByDescending(x => x.Utility).ToList()[0];
                if (Rnd.Range(1, 101) <= ExplorationRate)
                {
                    moveToTake = t.GetAvailableMoves()[Rnd.Range(0, t.GetAvailableMoves().Count)];
                }
                TakenMoves.Add(moveToTake);


                if (Spectation)
                {

                    string res = "My available moves: \n";
                    foreach (PawnMove item in Policies[t.GetBoardState()].OrderByDescending(x => x.Utility).ToList())
                    {
                        res += $"[{item.X},{item.Y}]({item.Utility}) \n";
                    }
                    Console.WriteLine(res);
                }
                return moveToTake;
            }
            else
            {
                Policies.Add(t.GetBoardState(), t.GetAvailableMoves());
                PawnMove moveToTake;
              
               
                    moveToTake = Policies[t.GetBoardState()].OrderByDescending(x => x.Utility).ToList()[0];
                   
                TakenMoves.Add(moveToTake);
                return moveToTake;
            }

        }
        public void Reward(float reward)
        {
            TakenMoves.Reverse();
            for (int i = 1; i < TakenMoves.Count+1; i++)
            {
                TakenMoves[i-1].Utility += reward / i *.1f;
            }
            TakenMoves.Clear();
            if (reward > 0)
            {
                if (Rnd.Range(0, 10) < 3)
                {
                    ExplorationRate--;
                    if (ExplorationRate < 0)
                    {
                        ExplorationRate = 0;
                    }
                }
            }
            if (reward == 0)
            {
                ExplorationRate++;
            }
        }
    }
}
