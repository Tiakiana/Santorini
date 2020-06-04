namespace Santorini
{
    public class PawnMove
    {

        public PawnMove() { }
        public PawnMove(int x,int y) {
            X = x;
            Y = y;
        }
        public PawnMove(int x, int y, int xbui, int ybui, int pawnposx, int pawnposy)
        {
            X = x;
            Y = y;
            XBuild = xbui;
            YBuild = ybui;
            PawnPosX = pawnposx;
            PawnPosY = pawnposy;
            
        }
        public int X;
        public int Y;
        public float Utility = 0;
        public int XBuild, YBuild;
        //public Pawn MyPawn;
        public int PawnPosX, PawnPosY;

        public override string ToString()
        {
            return $"Utility: {Utility} - The pawn at [{PawnPosX},{PawnPosY}] goes to [{X},{Y}] and builds at [{XBuild},{YBuild}]";


        }

    }
}
