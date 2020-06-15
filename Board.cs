using System;
using System.Collections.Generic;

namespace Santorini
{
    public class Board
    {
        public int[,,] BoardState;
        public int Level1s, Level2s, Level3s, Domes;
      public bool TestForIllegalConstelations()
        {
            int play1 = 0;
            int play2 = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (BoardState[i,y,1] == 1)
                    {
                        play1++;
                    }
                    if (BoardState[i, y, 1] == 2)
                    {
                        play2++;

                    }
                }
            }
            if (play1 != 1 || play2 != 1)
            {
                Console.WriteLine("Errrør1");
                return false;
            }
            return true;

        }
        public void DrawBoard()
        {
            string res = "\n";
            for (int y = 2; y >= 0; y--)
            {
                for (int x = 0; x < 3; x++)
                {
                    res += BoardState[x, y, 0] + "" + BoardState[x, y, 1] + " ";
                }
                res += "\n";
            }
            Console.WriteLine(res);
        }
        public Board()
        {
            Level1s = 16;
            Level2s = 16;
            Level3s = 16;
            Domes = 16;

            BoardState = new int[3, 3, 2];
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    BoardState[x, y, 0] = 0;

                    if (x == 0 && y == 0 )
                    {
                        BoardState[x, y, 1] = 1;

                    }
                    else if (x == 2 && y == 2 )
                    {
                        BoardState[x, y, 1] = 2;
                    }
                    else
                    {
                        BoardState[x, y, 1] = 0;
                    }
                }
            }
        }

        public List<PawnMove> GetAvailableMoveForPlayer(Player player)
        {
            List<PawnMove> res = new List<PawnMove>();
            foreach (var item in player.Pawns)
            {
                res.AddRange(GetAvailablePawnMoves(item));
            }
            return res;
        }

        private List<PawnMove> GetAvailablePawnMoves(Pawn pawn)
        {
            List<PawnMove> res = new List<PawnMove>();
            foreach (PawnMove item in GetMovesInAllDirections())
            {
                if (IsMoveLegal(pawn, item.X, item.Y))
                {
                    int xeval = pawn.X + item.X;
                    int yeval = pawn.Y + item.Y;
                    foreach (var build in GetMovesInAllDirections())
                    {
                        if (IsWithinBounds(new Pawn() { X = xeval, Y = yeval }, build.X, build.Y) && CanBuild(xeval + build.X, yeval + build.Y,pawn.X,pawn.Y))
                        {
                            PawnMove pwn = new PawnMove(xeval, yeval, xeval + build.X, yeval + build.Y, pawn.X,pawn.Y);
                            if (pwn.X == pwn.PawnPosX && pwn.Y == pwn.PawnPosY)
                            {
                                Console.WriteLine("Vagt I gevær");
                            }
                            res.Add(pwn);
                        }
                    }
                }
            }


            return res;

        }
        private List<PawnMove> GetMovesInAllDirections()
        {
            List<PawnMove> moves = new List<PawnMove>() {
            /*new PawnMove(-1,1),
            new PawnMove(-1,-1),
           new PawnMove(1,1),
            new PawnMove(1,-1),*/
                new PawnMove(-1,0),
            new PawnMove(0,1),
            new PawnMove(1,0),
            new PawnMove(0,-1),
            new PawnMove(-1,0)
            };
            return moves;


        }

        private bool IsMoveLegal(Pawn pwn, int x, int y)
        {
            if (IsWithinBounds(pwn, x, y) && IsNotTooHigh(pwn, x, y) && BoardState[pwn.X + x, pwn.Y + y, 1] == 0)
            {
                return true;
            }
            return false;
        }
        public bool IsWinningMove(PawnMove move)
        {
            if (BoardState[move.X, move.Y, 0] == 3)
            {
                return true;
            }
            return false;
        }

        private bool CanBuild(int x, int y, int pawnoriginx, int pawnoriginy)
        {
            if (BoardState[x, y, 0] < 3 && BoardState[x, y, 1] == 0)
            {
                return true;
            }
            else
            {
                if (BoardState[x, y, 1] == 0 || x == pawnoriginx && y == pawnoriginy)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsNotTooHigh(Pawn pawn, int xmove, int ymove)
        {
            int xeval = pawn.X + xmove;
            int yeval = pawn.Y + ymove;

            if (BoardState[xeval, yeval, 0] > BoardState[pawn.X, pawn.Y, 0] + 1)
            {
                return false;
            }
            if (BoardState[xeval,yeval,0] == 4)
            {
                return false;
            }
            return true;
        }

        //tag hele brættet med
        //public string GetBoardStateString()
        //{
        //    string res = "";
        //    for (int x = 0; x < 3; x++)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            res += BoardState[x, i, 0];
        //            res += BoardState[x, i, 1];
        //        }
        //    }
        //    return res;

        //}
        public string GetBoardStateString()
        {
            string res = "";
            for (int x = 0; x < 3; x++)
            {
                for (int i = 0; i < 3; i++)
                {
                    res += BoardState[x, i, 0];
                    res += BoardState[x, i, 1];
                }
            }
            return res;

        }

        private bool IsWithinBounds(Pawn pwn, int xmove, int ymove)
        {
            int xeval = pwn.X + xmove;
            int yeval = pwn.Y + ymove;

            if (xeval > 2 || xeval < 0)
            {
                return false;
            }
            if (yeval > 2 || yeval < 0)
            {
                return false;
            }
            return true;

        }

        public void MakeMove(PawnMove move, Pawn myPawn)
        {
            BoardState[move.PawnPosX, move.PawnPosY, 1] = 0;
            myPawn.X = move.X;
            myPawn.Y = move.Y;
            BoardState[move.X, move.Y, 1] = myPawn.PlayerNumber;
            if (BoardState[move.XBuild, move.YBuild, 0] <= 3)
            {
                BoardState[move.XBuild, move.YBuild, 0]++;
            }
            //else
            //{
            //    BoardState[move.XBuild, move.YBuild, 1] = 3;

            //}
         //   DrawBoard();
        }
    }
}
