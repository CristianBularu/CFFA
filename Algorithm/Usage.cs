using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Algorithm
{
    public interface IUsage
    {
        string Generate(string sketchPath, int pageCount, float pageHeight);
    }
    public class Usage: IUsage
    {
        public string Generate(string sketchPath, int pageCount, float pageHeight)
        {
            Mat mat;
            float unitScale;
            using (FileStream stream = new FileStream(sketchPath, FileMode.Open))
            {
                var original = new Bitmap(stream);
                float height = original.Height;
                float width = original.Width;
                var finalHeight = (int)((float)pageCount * height / width);
                unitScale = pageHeight / finalHeight;
                var resized = new Bitmap(original, pageCount, finalHeight);
                mat = new Mat(resized);
                stream.FlushAsync();
            };

            Console.WriteLine(mat.width);
            Console.WriteLine(mat.height);
            var mtx = mat.matref();
            for (int i=0;i<mat.height;i++)
            {
                for(int j=0;j<mat.width;j++)
                    Console.Write($" {mtx[i,j]} ");
                Console.WriteLine();
            }

            processMatNew(mat, unitScale);

            return "";
        }

        private void processMat(Mat mat, float scale)
        {
            var mtx = mat.matref();
            int pgs = 1;
            for (int i = 0; i < mat.height; i ++)
            {
                int begin = -1;
                bool started = false;
                int end = -1;
                bool finished = false;
                Console.WriteLine("Page: " + pgs);
                pgs++;
                for (int j = 0; j < mat.width; j++)
                {
                    if (!started)
                        if (mtx[i, j] == 1)
                        {
                            begin = j;
                            started = true;
                            finished = false;
                        }
                    if (started && !finished)
                    {
                        if (mtx[i, j] == 0 && mtx[i, j - 1] == 1)
                        {
                            end = j - 1;
                            finished = true;
                            started = false;
                            Console.Write("    ");
                            float bg = (float)begin * scale;
                            float nd = ((float)end + 1) * scale;
                            Console.WriteLine(bg + " Into: " + nd);
                        }
                        else if (j + 1 >= mat.width)
                        {
                            end = j;
                            finished = true;
                            started = false;

                            Console.Write("    ");
                            float bg = (float)begin * scale;
                            float nd = ((float)end + 1) * scale;
                            Console.WriteLine(bg + " Into: " + nd);
                        }
                    }
                }
            }
        }

        private void processMatNew(Mat mat, float scale)
        {
            var mtx = mat.matref();
            for (int i = 0; i < mat.height; i++)
            {
                int begin = -1;
                bool looking = false;
                Console.WriteLine($"Page {i+1}:");
                for (int j = 0; j < mat.width; j++)
                {
                    if (!looking)
                    {
                        if (mtx[i, j] == 1)
                        {
                            looking = true;
                            begin = j;
                        }
                    }
                    else if (mtx[i, j] == 0)
                    {
                        looking = false;
                        if (j >= mat.width)
                            Console.WriteLine(begin * scale + " Into: end");
                        else
                            Console.WriteLine(begin * scale + " Into: " + j * scale);
                    }
                    else if (j >= mat.width)
                    {
                        looking = false;
                        Console.WriteLine(begin * scale + " Into: end");
                    }
                }
            }
        }
    }
}
