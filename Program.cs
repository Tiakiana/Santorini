using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Santorini
{
    public class SantoriniUtilityAI
    {
        Random rnd;
        public SantoriniUtilityAI(int randSeeds)
        {
            rnd = new Random(randSeeds);
        }
        public SantoriniUtilityAI()
        {
            rnd = new Random();
        }
        float LevelToMe(PawnMove move, Board b)
        {
            return b.BoardState[move.X, move.Y, 0] - b.BoardState[move.PawnPosX, move.PawnPosY, 0];

        }

        public PawnMove TakeTurn(List<PawnMove> moves, Board b)
        {
            PawnMove WinMove = moves.FirstOrDefault(x => b.IsWinningMove(x));
            if (WinMove != null)
            {
                return WinMove;
            }
            moves = moves.OrderBy(x => rnd.Next(0, 10000)).ToList();

            foreach (PawnMove item in moves)
            {
                item.Utility = LevelToMe(item, b);
            }

            return moves.OrderByDescending(x => x.Utility).ThenByDescending(x => rnd.Next(0, 10000)).ToList()[0];
        }
    }


    public class Program
    {
        public void PlayTicTacToe()
        {
            TicTacToeBoard t = new TicTacToeBoard();
            RandomAI randy = new RandomAI();
            RandomAI randy2 = new RandomAI();
            QLearnAITICTACTOE Qubert = new QLearnAITICTACTOE();
            QLearnAITICTACTOE Qubert1 = new QLearnAITICTACTOE();

            int player1win = 0;
            int player2win = 0;
            int stalemate = 0;

            int player1winBM = 0;
            int player2winBM = 0;
            int stalemateBM = 0;


            List<WinRatio> winratios = new List<WinRatio>();
            for (int i = 0; i < 1000000; i++)
            {
                if (i % 10000 == 0)
                {
                    winratios.Add(new WinRatio() { Player1 = player1winBM, Player2 = player2winBM, Draw = stalemateBM });
                    stalemate++;
                    player1winBM = 0;
                    player2winBM = 0;
                    stalemateBM = 0;
                }
                t = new TicTacToeBoard();

                while (t.CheckForWinner() == 0)
                {
                    PawnMove mov = randy.TakeTurn(t);
                    t.TTTBoard[mov.X, mov.Y] = 1;
                    if (t.CheckForWinner() == 0)
                    {
                        mov = Qubert.TakeTurn(t);
                        t.TTTBoard[mov.X, mov.Y] = 2;
                    }


                }
                if (t.CheckForWinner() == 1)
                {
                    //       t.DrawBoard();
                    //      Console.WriteLine("Player 1 Win!");
                    //      Console.WriteLine();
                    player1win++;
                    player1winBM++;
                    Qubert.Reward(-1);
                    Qubert1.Reward(1);
                }
                if (t.CheckForWinner() == 2)
                {
                    //t.DrawBoard();
                    //Console.WriteLine("Player 2 Win!");
                    //Console.WriteLine();
                    player2win++;
                    player2winBM++;
                    Qubert.Reward(1);
                    Qubert1.Reward(-1);

                }
                if (t.CheckForWinner() == 3)
                {
                    // t.DrawBoard();
                    //Console.WriteLine("Noone");
                    // Console.WriteLine();
                    stalemate++;
                    stalemateBM++;
                    Qubert.Reward(.1f);
                    Qubert1.Reward(.1f);

                }
            }


            Console.WriteLine("Hello World!");
            Console.WriteLine("Player 1 victories: " + player1win);
            Console.WriteLine("Player 2 victories: " + player2win);
            Console.WriteLine("Draws " + stalemate);

            string res = "Player 1 stats: ";
            foreach (WinRatio item in winratios)
            {
                res += item.Player1 + "/";
            }
            Console.WriteLine(res);
            res = "Player 2 stats: ";
            foreach (WinRatio item in winratios)
            {
                res += item.Player2 + "/";
            }
            Console.WriteLine(res);
            res = "Draws:         ";

            Console.WriteLine(Qubert.Policies.Count + "/19663 Known boardstates");
            Console.WriteLine("" + ((float)Qubert.Policies.Count / 19663) * 100 + "% discovered");

            foreach (WinRatio item in winratios)
            {
                res += item.Draw + "/";
            }
            Console.WriteLine(res);
            Qubert.ExplorationRate = 0;
            Qubert1.ExplorationRate = 0;
            Qubert.Spectation = true;

            for (int i = 0; i < 10; i++)
            {
                t = new TicTacToeBoard();

                while (t.CheckForWinner() == 0)
                {
                    t.DrawBoard();
                    Console.WriteLine("Choose X coordinate");
                    int playerX = int.Parse(Console.ReadLine());
                    Console.WriteLine("Choose Y coordinate");
                    int playerY = int.Parse(Console.ReadLine());
                    t.TTTBoard[playerX, playerY] = 1;
                    if (t.CheckForWinner() == 0)
                    {
                        PawnMove mov = Qubert.TakeTurn(t);
                        t.TTTBoard[mov.X, mov.Y] = 2;
                    }


                }
                if (t.CheckForWinner() == 1)
                {
                    t.DrawBoard();
                    Console.WriteLine("Player 1 Win!");
                    Console.WriteLine();
                    player1win++;
                    player1winBM++;
                    Qubert.Reward(-1);
                    Qubert1.Reward(1);
                }
                if (t.CheckForWinner() == 2)
                {
                    t.DrawBoard();
                    Console.WriteLine("Player 2 Win!");
                    Console.WriteLine();
                    player2win++;
                    player2winBM++;
                    Qubert.Reward(1);
                    Qubert1.Reward(-1);

                }
                if (t.CheckForWinner() == 3)
                {
                    t.DrawBoard();
                    Console.WriteLine("Noone");
                    Console.WriteLine();
                    stalemate++;
                    stalemateBM++;
                    Qubert.Reward(-0.01f);
                    Qubert1.Reward(-0.01f);

                }
            }

        }


        static void Main(string[] args)
        {
            PlaySantorini();
        }

        public static void PlaySantorini()
        {
            Program p = new Program();
            SantoriniQLearn quintin1 = new SantoriniQLearn();
            SantoriniQLearn quintin2 = new SantoriniQLearn();
            SantoriniPseudoRandom randy = new SantoriniPseudoRandom(Rnd.Range(0, 5000));
            SantoriniUtilityAI Ursa = new SantoriniUtilityAI();

            WinRatio wins = new WinRatio();
            WinRatio winsIncremental = new WinRatio();

            string ProgressBarMine = "";
            bool printstuff = false;
            //for (int i = 1; i < 1000000; i++)
            //{
            int i = 1;
            int yx = 0;
            bool doneTraining = false;
            bool doneExploring = false;
            while (!doneTraining)
            {
                i++;
                Player player1 = new Player();
                player1.Pawns.Add(new Pawn(0, 0, 1));
                Player player2 = new Player();
                player2.Pawns.Add(new Pawn(2, 2, 2));
                Board b = new Board();
                int timesINeedToWin = 3;
                bool ActivateIntelligentOpponent = false;
                //Visuals
                if (i % 100000 == 0)
                {

                    Console.Clear();
                    //quintin1.ExplorationChance--;
                    if (doneExploring)
                    {
                        quintin1.ExplorationChance = 0;
                        yx++;
                    }
                    if (yx > 7)
                    {
                        doneTraining = true;
                    }
                    //if (i%900000 == 0)
                    //{
                    //    quintin1.ExplorationChance = 0;
                    //}

                    ProgressBarMine += "\n \nPlayer1Wins: " + winsIncremental.Player1 + "\nPlayer2Wins: " + winsIncremental.Player2 + "\nNew Board States: " + MathF.Abs(winsIncremental.Draw - quintin1.Policies.Count);
                    Console.WriteLine(ProgressBarMine);
                    Console.WriteLine("Exploration chance: " + quintin1.ExplorationChance);

                    //Hvor mange nye stadier skal vi over før vi kan gå igang med at spille?
                    if (MathF.Abs(winsIncremental.Draw - quintin1.Policies.Count) < 500000)
                    {
                        ProgressBarMine += "\nsaving";
                        //p.SaveKnowledge(quintin1);
                        ProgressBarMine += "\n Done saving";


                        doneExploring = true;
                    }

                    winsIncremental.Draw = quintin1.Policies.Count;
                    winsIncremental.Player2 = 0;
                    winsIncremental.Player1 = 0;
                    if (!ActivateIntelligentOpponent && winsIncremental.Player2 > 7600)
                    {
                        timesINeedToWin--;
                        if (timesINeedToWin == 0)
                        {
                            ActivateIntelligentOpponent = false;
                        }
                    }
                    //      quintin1.PrintEverything = true;
                    //      printstuff = true;
                }




                bool winner = false;
                while (winner == false)
                {
                    if (b.GetAvailableMoveForPlayer(player1).Count == 0)
                    {
                        winner = true;
                        //    Console.WriteLine("Player 2 killed player 1");
                        wins.Player2++;
                        winsIncremental.Player2++;
                        if (ActivateIntelligentOpponent)
                        {
                            quintin2.Reward(-1);
                        }
                        quintin1.Reward(-1);

                    }
                    else
                    {
                        b.MakeMove(quintin1.TakeTurn(b, player1), player1.Pawns[0]);

                        if (printstuff)
                        {
                            b.DrawBoard();
                        }
                        foreach (Pawn item in player1.Pawns)
                        {
                            if (b.BoardState[item.X, item.Y, 0] == 3)
                            {
                                winner = true;
                                //             Console.WriteLine("Player 1 Climbed the tower");
                                wins.Player1++;
                                winsIncremental.Player1++;
                                if (ActivateIntelligentOpponent)
                                {
                                    quintin2.Reward(1);
                                }
                                quintin1.Reward(1);

                                break;
                            }
                        }
                    }
                    if (winner == false)
                    {
                        if (b.GetAvailableMoveForPlayer(player2).Count == 0)
                        {
                            winner = true;
                            wins.Player1++;
                            winsIncremental.Player1++;
                            if (ActivateIntelligentOpponent)
                            {
                                quintin2.Reward(10);
                            }
                            quintin1.Reward(1);

                            //        Console.WriteLine("Player 1 killed player 2");
                        }
                        else
                        {
                            //                      MAKE DET MOVE MOND!
                            if (doneExploring)
                            {
                                b.MakeMove(Ursa.TakeTurn(b.GetAvailableMoveForPlayer(player2), b), player2.Pawns[0]);
                            }
                            else
                            {

                                b.MakeMove(randy.TakeTurn(b.GetAvailableMoveForPlayer(player2), b), player2.Pawns[0]);
                            }

                            if (printstuff)
                            {
                                b.DrawBoard();
                            }
                            foreach (Pawn item in player2.Pawns)
                            {
                                if (b.BoardState[item.X, item.Y, 0] == 3)
                                {
                                    winner = true;
                                    //              Console.WriteLine("Player 2 climbed the tower!");
                                    wins.Player2++;
                                    winsIncremental.Player2++;

                                    quintin1.Reward(-1);
                                    if (ActivateIntelligentOpponent)
                                    {
                                        quintin2.Reward(-1);
                                    }
                                    break;
                                }
                            }
                        }
                    }

                }
                //   b.DrawBoard();
            }

            Console.WriteLine("Player 1 Wins:" + wins.Player1);
            Console.WriteLine("Player 2 Wins:" + wins.Player2);
            Console.WriteLine("Board states known: " + quintin1.Policies.Count);
            Console.WriteLine("Press any key");
          //  p.SaveKnowledge(quintin1);
            Console.ReadKey();

            for (int hy = 0; hy < 1000; hy++)
            {


                Player player1 = new Player();
                player1.Pawns.Add(new Pawn(0, 0, 1));
                Player player2 = new Player();
                player2.Pawns.Add(new Pawn(2, 2, 2));
                Board b = new Board();
                Console.Clear();

                b.DrawBoard();
                Console.ReadKey();
                bool winner = false;
                while (winner == false)
                {
                    if (b.GetAvailableMoveForPlayer(player1).Count == 0)
                    {
                        winner = true;
                        //    Console.WriteLine("Player 2 killed player 1");
                        wins.Player2++;
                        winsIncremental.Player2++;

                        quintin1.Reward(10);

                    }
                    else
                    {



                        b.MakeMove(quintin1.TakeTurn(b, player1), player1.Pawns[0]);   //b.GetAvailableMoveForPlayer(player1), b); ; ;, player1.Pawns[0]);
                        b.DrawBoard();

                        foreach (Pawn item in player1.Pawns)
                        {
                            if (b.BoardState[item.X, item.Y, 0] == 3)
                            {
                                winner = true;
                                Console.WriteLine("Player 1 Climbed the tower");
                                wins.Player1++;
                                winsIncremental.Player1++;

                                quintin1.Reward(-1);

                                break;
                            }
                        }
                    }
                    if (winner == false)
                    {
                        if (b.GetAvailableMoveForPlayer(player2).Count == 0)
                        {
                            winner = true;
                            wins.Player1++;
                            winsIncremental.Player1++;

                            quintin1.Reward(-1);

                            Console.WriteLine("Player 1 killed player 2");
                        }
                        else
                        {
                            //                      MAKE DET MOVE MOND!
                            Console.WriteLine("Make Move Player :D");
                            PawnMove playermove = new PawnMove();


                            List<PawnMove> liste = b.GetAvailableMoveForPlayer(player2);
                        A:
                            int count = 0;
                            foreach (var item in liste)
                            {
                                Console.WriteLine(count + ": " + item.ToString());
                                count++;
                            }
                            int choose = int.Parse(Console.ReadLine());
                            try
                            {
                                b.MakeMove(liste[choose], player2.Pawns[0]);
                            }
                            catch (Exception)
                            {
                                goto A;
                            }


                            //b.MakeMove(quintin1.TakeTurn(b, player2), player2.Pawns[0]);
                            //b.DrawBoard();
                            //Console.ReadKey();

                            foreach (Pawn item in player2.Pawns)
                            {
                                if (b.BoardState[item.X, item.Y, 0] == 3)
                                {
                                    winner = true;
                                    Console.WriteLine("Player 2 climbed the tower!");
                                    wins.Player2++;
                                    winsIncremental.Player2++;


                                    break;
                                }
                            }
                        }
                    }

                }
                b.DrawBoard();
                Console.ReadLine();

            }
        }
        public class DictObject
        {
            public string BoardState;
            public List<PawnMove> Moves = new List<PawnMove>();
            public DictObject() { }
            public DictObject(string boardState, List<PawnMove> moves)
            {

                BoardState = boardState;
                Moves = moves;
            }
        }
        public class SaveItem
        {
            public List<DictObject> SavedItem;
            public SaveItem() { }
            public SaveItem(List<DictObject> d)
            {
                SavedItem = d;
            }
        }

        public void SaveKnowledge(SantoriniQLearn ql)
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveItem));
            List<DictObject> dd = new List<DictObject>();
            foreach (var item in ql.Policies)
            {
                DictObject d = new DictObject(item.Key, item.Value);
                dd.Add(d);
            }
            SaveItem s = new SaveItem(dd);
            var path = "../../save" + ".xml";

            System.IO.FileStream file = System.IO.File.Create(path);


            writer.Serialize(file, s);

            file.Close();
        }
        public SantoriniQLearn GetPlayer()
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(QLearnAITICTACTOE));

            var path = "../../save.xml";

            System.IO.StreamReader file = new System.IO.StreamReader(path);

            SantoriniQLearn Playerplayer = (SantoriniQLearn)reader.Deserialize(file);

            file.Close();

            return Playerplayer;
        }




    }


}

