using System;
using System.Drawing;

namespace Algorithm
{
    class Mat
    {
        public int[,] mat;
        public int width;
        public int height;

        public Mat(Bitmap bm)//1 layer only
        {
            mat = new int[bm.Height, bm.Width];
            width = bm.Width;
            height = bm.Height;
            for (int i = 0; i < bm.Height; i++)
                for (int j = 0; j < bm.Width; j++)
                {
                    int color = 0;
                    color += bm.GetPixel(j, i).R;
                    color += bm.GetPixel(j, i).G;
                    color += bm.GetPixel(j, i).B;
                    color /= 3;
                    if (color < 220)
                    {
                        mat[i, j] = 1;
                    }
                }
        }
    }
}
