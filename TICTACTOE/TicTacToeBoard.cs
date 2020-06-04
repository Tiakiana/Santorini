using System;
using System.Collections.Generic;

namespace Santorini
{
    public class TicTacToeBoard
    {
        public int[,] TTTBoard = new int[3, 3];
        public TicTacToeBoard()
        {
            CreateBoard();
        }
        public void CreateBoard()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    TTTBoard[x, y] = 0;
                }
            }
        }

        public string GetBoardState()
        {
            string res = "";
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    res += TTTBoard[x, y];
                }
            }
            return res;
        }
        public static string GetBoardState(int[,] board)
        {
            string res = "";
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    res += board[x, y];
                }
            }
            return res;
        }


        public List<PawnMove> GetAvailableMoves()
        {
            List<PawnMove> moves = new List<PawnMove>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (TTTBoard[x, y] == 0)
                    {
                        moves.Add(new PawnMove() { X = x, Y = y });
                    }
                }
            }
            return moves;
        }
        public static List<PawnMove> GetAvailableMoves(int[,]board)
        {
            List<PawnMove> moves = new List<PawnMove>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[x, y] == 0)
                    {
                        moves.Add(new PawnMove() { X = x, Y = y });
                    }
                }
            }
            return moves;
        }
        public int CheckForWinner()
        {
            int res = 0;
            for (int x = 0; x < 3; x++)
            {
                string ri = "";
                for (int y = 0; y < 3; y++)
                {
                    ri += TTTBoard[x, y];
                }
                if (ri == "111")
                {
                    return 1;
                }
                if (ri == "222")
                {
                    return 2;
                }
            }

            for (int y = 0; y < 3; y++)
            {
                string ri = "";

                for (int x = 0; x < 3; x++)
                {
                    ri += TTTBoard[x, y];

                }
                if (ri == "111")
                {
                    return 1;
                }
                if (ri == "222")
                {
                    return 2;
                }


            }
            if (TTTBoard[0, 0] == 1 && TTTBoard[1, 1] == 1 && TTTBoard[2, 2] == 1)
            {
                return 1;
            }
            if (TTTBoard[0, 0] == 2 && TTTBoard[1, 1] == 2 && TTTBoard[2, 2] == 2)
            {
                return 2;
            }
            if (TTTBoard[0, 2] == 1 && TTTBoard[1, 1] == 1 && TTTBoard[2, 0] == 1)
            {
                return 1;
            }
            if (TTTBoard[0, 2] == 2 && TTTBoard[1, 1] == 2 && TTTBoard[2, 0] == 2)
            {
                return 2;
            }
            string ge = GetBoardState().ToString();
            if (!GetBoardState().Contains("0"))
            {
                return 3;
            }
            return res;
        }

        public static int CheckForWinner(int[,] board)
        {
            int res = 0;
            for (int x = 0; x < 3; x++)
            {
                string ri = "";
                for (int y = 0; y < 3; y++)
                {
                    ri += board[x, y];
                }
                if (ri == "111")
                {
                    return 1;
                }
                if (ri == "222")
                {
                    return 2;
                }
            }

            for (int y = 0; y < 3; y++)
            {
                string ri = "";

                for (int x = 0; x < 3; x++)
                {
                    ri += board[x, y];

                }
                if (ri == "111")
                {
                    return 1;
                }
                if (ri == "222")
                {
                    return 2;
                }


            }

            if (board[0, 0] == 1 && board[1, 1] == 1 && board[2, 2] == 1)
            {
                return 1;
            }
            if (board[0, 0] == 2 && board[1, 1] == 2 &&board[2, 2] == 2)
            {
                return 2;
            }
            if (board[0, 2] == 1 && board[1, 1] == 1 && board[2, 0] == 1)
            {
                return 1;
            }
            if (board[0, 2] == 2 && board[1, 1] == 2 && board[2, 0] == 2)
            {
                return 2;
            }
            //string ge = GetBoardState().ToString();
            //if (!GetBoardState().Contains("0"))
            //{
            //    return 3;
            //}
            return res;
        }
        public void DrawBoard()
        {
            string res = "";
            for (int y = 2; y >= 0; y--)
            {
                for (int x = 0; x < 3; x++)
                {
                    res += TTTBoard[x, y] +" ";
                }
                res += "\n";
            }
            Console.WriteLine(res);
        }


    }
}
