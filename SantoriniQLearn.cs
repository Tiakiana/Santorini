using System;
using System.Collections.Generic;
using System.Linq;

namespace Santorini
{
    [Serializable]
    public class SantoriniQLearn
    {
        public Dictionary<string, List<PawnMove>> Policies = new Dictionary<string, List<PawnMove>>();
        public List<PawnMove> MovesMade = new List<PawnMove>();
        public int ExplorationChance = 100;
        int counter = 0;
        public bool PrintEverything = false;
        public void Reward(float r)
        {
            counter++;
            if (counter == 7500)
            {
                Console.WriteLine("");
            }
            MovesMade.Reverse();
            for (int i = 0; i < MovesMade.Count; i++)
            {

                switch (i)
                {
                    case 0:
                        MovesMade[i].Utility += 1f * r;
                        break;
                    case 1:
                        MovesMade[i].Utility += .9f * r;
                        break;
                    case 2:
                        MovesMade[i].Utility += .8f * r;
                        break;
                    case 3:
                        MovesMade[i].Utility += .7f * r;
                        break;
                    case 4:
                        MovesMade[i].Utility += .6f * r;
                        break;
                    case 5:
                        MovesMade[i].Utility += .5f * r;
                        break;
                    case 6:
                        MovesMade[i].Utility += .4f * r;
                        break;
                    case 7:
                        MovesMade[i].Utility += .3f * r;
                        break;

                    default:
                        MovesMade[i].Utility += 0.1f * r;
                        break;
                }
            }

            MovesMade.Clear();
            if (r > 0)
            {
                if (Rnd.Range(1, 100) < ExplorationChance + 5)
                {
                //    ExplorationChance--;

                }
            }


        }

        public PawnMove TakeTurn(Board b, Player player)
        {
            string boardstat = b.GetBoardStateString();
            //       PawnMove 
            PawnMove WinMove;


            if (Policies.ContainsKey(boardstat))
            {
                WinMove = Policies[boardstat].FirstOrDefault(x => b.IsWinningMove(x));


                if (WinMove != null)
                {
                    MovesMade.Add(WinMove);
                    return WinMove;
                }


                if (Rnd.Range(1, 101) <= ExplorationChance)
                {
                    WinMove = Policies[boardstat].OrderBy(x => Rnd.Range(0, 10000)).ToList()[0];
                    MovesMade.Add(WinMove);
                    return WinMove;
                }
                else
                {
                    WinMove = Policies[boardstat].OrderByDescending(x => x.Utility).ToList()[0];
                    if (PrintEverything)
                    {
                        string res = "";
                        foreach (PawnMove item in Policies[boardstat].OrderByDescending(x => x.Utility).ToList())
                        {
                            res += item.ToString() + "\n";
                        }
                    Console.WriteLine(res);

                    }
                    MovesMade.Add(WinMove);
                    return WinMove;
                }
            }
            else
            {


                List<PawnMove> moves = b.GetAvailableMoveForPlayer(player).OrderBy(x => Rnd.Range(0, 10000)).ToList();
                Policies.Add(boardstat, moves);
                WinMove = Policies[boardstat].FirstOrDefault(x => b.IsWinningMove(x));
                if (WinMove == null)
                {
                    WinMove = moves[0];

                }

                MovesMade.Add(WinMove);


                return WinMove;
            }



        }



    }
}
