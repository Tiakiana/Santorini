using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Santorini
{
    public class SantoriniNAN
    {

        public const double Bias = -.5;

        const int _BoardSizeX = 3;
        const int _BoardSizeY = 3;
        const int _NumberOfPawns = 1;

        const int _InputNeuronsCount = 63;
        const int _HiddenNeuronsCount = 61;
        const int _OutputNeurons = 16;
        //X er current og Y er Next Layer
        public double[] InputNeurons = new double[_InputNeuronsCount];
        public double[,] InputWeightMatrix = new double[_InputNeuronsCount, _HiddenNeuronsCount];

        public double[] Hidden1Neurons = new double[_HiddenNeuronsCount];
        public double[,] Hidden1WeightMatrix = new double[_HiddenNeuronsCount, _HiddenNeuronsCount];

        public double[] Hidden2Neurons = new double[_HiddenNeuronsCount];
        public double[,] Hidden2WeightMatrix = new double[_HiddenNeuronsCount, _OutputNeurons];

        public double[] OutputNeurons = new double[_OutputNeurons];


        public SantoriniNAN()
        {
            for (int i = 0; i < OutputNeurons.Length; i++)
            {
                OutputNeurons[i] = 0;
            }

            for (int i = 0; i < Hidden1Neurons.Length; i++)
            {
                Hidden1Neurons[i] = 0;
                Hidden2Neurons[i] = 0;
            }

            for (int x = 0; x < _HiddenNeuronsCount; x++)
            {
                for (int y = 0; y < _HiddenNeuronsCount; y++)
                {
                    Hidden1WeightMatrix[x, y] = Rnd.rnd.NextDouble();
                }
            }

            for (int x = 0; x < _HiddenNeuronsCount; x++)
            {
                for (int y = 0; y < OutputNeurons.Length; y++)
                {
                    Hidden2WeightMatrix[x, y] = Rnd.rnd.NextDouble();
                }
            }
            for (int x = 0; x < _InputNeuronsCount; x++)
            {
                for (int y = 0; y < _HiddenNeuronsCount; y++)
                {
                    InputWeightMatrix[x, y] = Rnd.rnd.NextDouble();
                }
            }



        }


        public double Sigmoid(double mif)
        {
            //return 1 / 1 + Math.Pow(2.7182818284590452353602, 0 - mif);
            return 1 / (1 + Math.Exp(0 - mif));
        }

        public void GetInputs(int[,,] board, int playernum)
        {
            int neuronCount = 0;
            for (int x = 0; x < _BoardSizeX; x++)
            {

                for (int y = 0; y < _BoardSizeY; y++)
                {
                    //Hvad Niveau er vi på?
                    for (int i = 0; i < 5; i++)
                    {

                        if (board[x, y, 0] == i)
                        {
                            InputNeurons[neuronCount + i] = 1;
                        }
                        else
                        {
                            InputNeurons[neuronCount + i] = 0;
                        }
                    }
                    // er det MinBrik?
                    if (board[x, y, 1] == playernum)
                    {
                        InputNeurons[neuronCount + 5] = 1;
                    }
                    else
                    {
                        InputNeurons[neuronCount + 5] = 0;
                    }
                    //Er det fjenden?
                    if (board[x, y, 1] != playernum && board[x, y, 1] != 0)
                    {
                        InputNeurons[neuronCount + 6] = 1;
                    }
                    else
                    {
                        InputNeurons[neuronCount + 6] = 0;
                    }

                    neuronCount += 7;
                }
            }
        }

        public PawnMove TakeTurn(Board b, Player player)
        {
            int[,,] board = b.BoardState;
            GetInputs(board, player.Pawns[0].PlayerNumber);
            int count = 0;
            foreach (var item in InputNeurons)
            {
                count++;
          //      Console.WriteLine("" + item);
                if (count % 7 == 0)
                {
          //          Console.WriteLine();
                }
            }
            double tally = 0;
            for (int y = 0; y < Hidden1Neurons.Length; y++)
            {
                tally = 0;
                for (int x = 0; x < InputNeurons.Length; x++)
                {
                    tally += InputNeurons[x] * InputWeightMatrix[x, y];
                }
           //     Console.WriteLine("tally is " + tally / InputNeurons.Length);

                Hidden1Neurons[y] = Sigmoid(tally / InputNeurons.Length);
            }
            foreach (var item in Hidden1Neurons)
            {
           //     Console.WriteLine("" + item);
            }
            //Console.WriteLine("Næste række");
            //Console.WriteLine();
            //Console.WriteLine();

            for (int y = 0; y < Hidden1Neurons.Length; y++)
            {
                tally = 0;
                for (int x = 0; x < Hidden2Neurons.Length; x++)
                {
                    tally += Hidden1Neurons[x] * Hidden1WeightMatrix[x, y];
                }
            //    Console.WriteLine("tally is " + tally / Hidden1Neurons.Length);

                Hidden2Neurons[y] = Sigmoid(tally / Hidden1Neurons.Length);
            }

            foreach (var item in Hidden2Neurons)
            {
   //             Console.WriteLine("" + item);
            }
       //     Console.WriteLine("Sidste række");

            for (int y = 0; y < OutputNeurons.Length; y++)
            {
                tally = 0;
                for (int x = 0; x < Hidden2Neurons.Length; x++)
                {
                    tally += Hidden2Neurons[x] * Hidden2WeightMatrix[x, y];
                }
           //     Console.WriteLine("tally is " + tally / Hidden2Neurons.Length);

                OutputNeurons[y] = Sigmoid(tally / Hidden2Neurons.Length);
            }
            int counting = 0;
            List<NanMove> moves = new List<NanMove>();
            List<NanBuild> builds = new List<NanBuild>();
            for (int x = -1; x < 2; x++)
            {
                for (int y= -1; y < 2; y++)
                {
                    if (x== 0 && y==0)
                    {
                        continue;
                    }
                    else
                    {
                         moves.Add( new NanMove(x, y,OutputNeurons[counting]));

                        counting++;
                    }
                }
            }
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    else
                    {
                        builds.Add(new NanBuild(x, y, OutputNeurons[counting]));

                        counting++;
                    }
                }
            }

            List<PawnMove> legalMoves = b.GetAvailableMoveForPlayer(player);
            //moves = moves
            //    .Where(mymove => legalMoves
            //    .Any(yx => yx.X == mymove.moveX && yx.Y == mymove.moveY))
            //    .ToList();
            
            //builds = builds
            //    .Where(mybuild => legalMoves
            //    .Any(yx => yx.XBuild == mybuild.moveX && yx.YBuild == mybuild.moveY))
            //    .ToList();

            moves = moves.OrderByDescending(x => x.utility).ToList();
            builds = builds.OrderByDescending(x => x.utility).ToList();

            foreach (var item in legalMoves)
            {
                //if (moves.Any(x=> x.moveX+player.Pawns[0].X == item.X && x.moveY + player.Pawns[0].Y == item.Y) && builds.Any(x => x.moveY + player.Pawns[0].Y == item.YBuild && x.moveX+player.Pawns[0].X == item.XBuild))
                //{
                    item.Utility = (float)moves.First(x => x.moveX + player.Pawns[0].X == item.X && x.moveY + player.Pawns[0].Y == item.Y).utility;
                    item.Utility += (float)builds.First(x => x.moveX  == item.BuildDirectionX && x.moveY  == item.BuildDirectionY).utility;
                //}
            }

            legalMoves =legalMoves.OrderByDescending(x => x.Utility).ToList();
            foreach (var item in legalMoves)
            {
        //        Console.WriteLine(item.ToString());
            }

            return legalMoves[0];



        }
    }
}
    public class NanMove
{
    public int moveX, moveY;
    public double utility;

    public NanMove(int moveX, int moveY, double utility)
    {
        this.moveX = moveX;
        this.moveY = moveY;
        this.utility = utility;
    }
}
public class NanBuild
{
    public int moveX, moveY;
    public double utility;

    public NanBuild(int moveX, int moveY, double utility)
    {
        this.moveX = moveX;
        this.moveY = moveY;
        this.utility = utility;
    }
}
