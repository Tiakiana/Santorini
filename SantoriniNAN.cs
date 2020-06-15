using System;
using System.Collections.Generic;
using System.Text;

namespace Santorini
{
public class SantoriniNAN
    {

        public const double Bias = -.5;

        const int _BoardSizeX = 3;
        const int _BoardSizeY = 3;
        const int _NumberOfPawns =1;

        const int _InputNeuronsCount = 63;
        const int _HiddenNeuronsCount = 61;
        const int _OutputNeurons = 40;
        //X er current og Y er Next Layer
        public double[] InputNeurons = new double[_InputNeuronsCount];
        public double[,] InputWeightMatrix = new double [_InputNeuronsCount,_HiddenNeuronsCount];

        public double[] Hidden1Neurons = new double[_HiddenNeuronsCount];
        public double[,] Hidden1WeightMatrix = new double [_HiddenNeuronsCount,_HiddenNeuronsCount];
        
        public double[] Hidden2Neurons = new double[_HiddenNeuronsCount];
        public double[,] Hidden2WeightMatrix = new double [_HiddenNeuronsCount,_OutputNeurons];

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
                            InputNeurons[neuronCount + i] = .1;
                        }
                        else
                        {
                            InputNeurons[neuronCount + i] = 0;
                        }
                    }
                    // er det MinBrik?
                    if (board[x, y, 1] == playernum)
                    {
                        InputNeurons[neuronCount + 5] = .1;
                    }
                    else
                    {
                        InputNeurons[neuronCount + 5] = 0;
                    }
                    //Er det fjenden?
                    if (board[x, y, 1] != playernum && board[x, y, 1] != 0)
                    {
                        InputNeurons[neuronCount + 6] = .1;
                    }
                    else
                    {
                        InputNeurons[neuronCount + 6] = 0;
                    }

                    neuronCount += 7;
                }
            }
        }

        public void FeedForward(int[,,] board, int playernum)
        {
            GetInputs(board,playernum);
            int count = 0;
            foreach (var item in InputNeurons)
            {
                count++;
                Console.WriteLine("" +item);
                if (count%7 == 0)
                {
                    Console.WriteLine();
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
                Console.WriteLine("tally is " +tally);
                Hidden1Neurons[y] = Sigmoid(tally);
            }
            foreach (var item in Hidden1Neurons)
            {
                Console.WriteLine(""+item);
            }
            Console.WriteLine("Næste række");
            Console.WriteLine();
            Console.WriteLine();

            for (int y = 0; y < Hidden1Neurons.Length; y++)
            {
                tally = 0;
                for (int x = 0; x < Hidden2Neurons.Length; x++)
                {
                    tally += Hidden1Neurons[x] * Hidden1WeightMatrix[x, y];
                }
                Console.WriteLine("tally is " + tally);

                Hidden2Neurons[y] = Sigmoid(tally);
            }

            foreach (var item in Hidden2Neurons)
            {
                Console.WriteLine("" + item);
            }
            Console.WriteLine("Næste række");

        }
    }

}
