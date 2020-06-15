using System;
using System.Collections.Generic;
using System.Text;

namespace Santorini
{
    public class Layer
    {
        //public decimal[,,] Inputs
        public List<double> Neurons = new List<double>();
        public List<double> Summations = new List<double>();
        public double[,] WeightRight;
        public Layer NextLayer = null;
        public Layer(int neuronsnumber, Layer connectedLayer)
        {
            NextLayer = connectedLayer;
            for (int i = 0; i < neuronsnumber; i++)
            {
                Neurons.Add(0);
                Summations.Add(0);
            }

            WeightRight = new double[Neurons.Count, connectedLayer.Neurons.Count];

            for (int x = 0; x < Neurons.Count; x++)
            {
                for (int y = 0; y < connectedLayer.Neurons.Count; y++)
                {
                    WeightRight[x, y] = Rnd.rnd.NextDouble();
                }
            }

        }


        public Layer(int neuronsNum)
        {
            
            for (int i = 0; i < neuronsNum; i++)
            {
                Neurons.Add(0);
                Summations.Add(0);
            }
        }
    }



    public class SantoriniNN
    {
        const int _NumberOfHiddenLayers = 2;
        const int _NumberOfHiddenNeurons = 160;
        const int _OutputNeurons = 144;
        const double Bias = .01;

        public Layer InputLayer { get; set; }
        public List<Layer> HiddenLayers { get; set; }
        public Layer OutputLayer { get; set; }

        public SantoriniNN()
        {
            OutputLayer = new Layer(_OutputNeurons);
            Layer lastLayer = null;
            for (int i = 0; i < _NumberOfHiddenLayers; i++)
            {
                if (lastLayer == null)
                {
                    lastLayer = new Layer(_NumberOfHiddenNeurons, OutputLayer);
                    HiddenLayers.Add(lastLayer);
                }
                else
                {
                    Layer l = new Layer(_NumberOfHiddenNeurons, lastLayer);
                    lastLayer = l;
                    HiddenLayers.Add(lastLayer);
                }
            }
            InputLayer = new Layer(175, lastLayer);
        }


        public void TakeTurn(int[,,] board , int playernumber)
        {
            GetInputs(board, playernumber);
            FeedForward(InputLayer);
            for (int i = HiddenLayers.Count - 1; i >=0 ; i--)
            {
                FeedForward(HiddenLayers[i]);
            }



        }


        public double Sigmoid(double mif)
        {
            return 1 / 1 + Math.Pow(2.7182818284590452353602,0-mif);
        }

        public void FeedForward(Layer layer)
        {
            
            if (layer.NextLayer != null)
            {
                for (int i = 0; i < layer.NextLayer.Summations.Count; i++)
                {
                    layer.NextLayer.Summations[i] = 0;
                }

                for (int i = 0; i < layer.Neurons.Count; i++)
                {
                    for (int y = 0; y < layer.NextLayer.Neurons.Count; y++)
                    {
                        layer.NextLayer.Summations[y] += layer.WeightRight[i, y] * layer.Neurons[i];
                    }

                }

                for (int i = 0; i < layer.NextLayer.Summations.Count; i++)
                {
                    layer.NextLayer.Neurons[i] = Sigmoid( layer.NextLayer.Summations[i]+Bias);
                }

            }
        }


        public void GetInputs(int[,,] board, int playernum)
        {
            int neuronCount = 0;
            for (int x = 0; x < 25; x++)
            {
                
                for (int y = 0; y < 25; y++)
                {
                    //Hvad Niveau er vi på?
                    for (int i = 0; i < 5; i++)
                    {

                        if (board[x, y, 0] == i)
                        {
                            InputLayer.Neurons[neuronCount + i] = 1;
                        }
                        else
                        {
                            InputLayer.Neurons[neuronCount + i] = 0;
                        }
                    }
                    // er det MinBrik?
                    if (board[x, y, 1] == playernum)
                    {
                        InputLayer.Neurons[neuronCount + 5] = 1;
                    }
                    else
                    {
                        InputLayer.Neurons[neuronCount + 5] = 0;
                    }
                   //Er det fjenden?
                    if ( board[x, y, 1] != playernum && board[x,y,1] != 0)
                    {
                        InputLayer.Neurons[neuronCount + 6] = 1;
                    }
                    else
                    {
                        InputLayer.Neurons[neuronCount + 6] = 0;
                    }
                   
                    neuronCount += 7;
                }
            }
        }



    }



}
