using System;

namespace Santorini
{
    public static class Rnd
    {

        public static Random rnd = new Random();
        public static int Range(int a, int b)
        {
            return rnd.Next(a, b);
        }




    }
}
