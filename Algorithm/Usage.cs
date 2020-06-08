using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
//using Microsoft.Office.Interop.Excel;
//using Excel = Microsoft.Office.Interop.Excel;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;



namespace Algorithm
{

    public struct Segment
    {
        public float start;
        public float end;
        public int page;
        public int layer;
    }

    public interface IUsage
    {
        string Generate(string rootPath, long sketchId, string extension, int pageCount, float pageHeight);
    }

    public class Usage: IUsage
    {
        private string rootPath;
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string Generate(string rootPath, long sketchId, string extension, int pageCount, float pageHeight)
        {
            this.rootPath = rootPath;
            var sketchPath = $"{rootPath}\\Sketch\\{sketchId}\\o{extension}";
            float unitScale;
            if (File.Exists(sketchPath))
            {
                int[,] mtx;
                int widthM;
                int heightM;
                using (FileStream stream = new FileStream(sketchPath, FileMode.Open))
                {
                    var original = new Bitmap(stream);
                    float height = original.Height;
                    float width = original.Width;
                    var finalHeight = (int)((float)pageCount * height / width);
                    unitScale = pageHeight / finalHeight;
                    var resized = new Bitmap(original, pageCount, finalHeight);
                    var mat = new Mat(resized);
                    widthM = mat.width;
                    heightM = mat.height;
                    mtx = mat.mat;
                    stream.FlushAsync();
                };
                
                //for (int i = 0; i < heightM; i++)
                //{
                //    for (int j = 0; j < widthM; j++)
                //        Console.Write($" {mtx[i, j]} ");
                //    Console.WriteLine();
                //}
                return processMatNew(mtx, heightM, widthM, unitScale);
            }
            logger.Debug("sketch image does not exist");
            return null;
        }

        private string processMatNew(int[,] mtx, int height, int width, float scale)
        {
            var bookSegs = new List<List<Segment>>();
            int maxSegCount = 0;
            for(int j = 0; j <width; j++ )
            {//new page
                var pageSegs = new List<Segment>();
                int begin = -1;
                bool lookForEnd = false;
                for(int i = 0; i < height; i++ )
                {//new pixel
                    Segment segment = new Segment();
                    segment.page = j + 1;

                    if (!lookForEnd)
                    {
                        if (mtx[i, j] == 1)//layer 1
                        {
                            lookForEnd = true;
                            begin = i;
                            segment.layer = 1;//layer 1
                        }
                    }
                    else if (mtx[i, j] == 0)
                    {
                        lookForEnd = false;
                        segment.start = begin * scale;
                        segment.end = i * scale;
                        pageSegs.Add(segment);
                    }
                    else if (i + 1 >= height)
                    {
                        segment.start = begin * scale;
                        segment.end = -1;
                        pageSegs.Add(segment);
                    }
                    Console.WriteLine();
                }
                bookSegs.Add(pageSegs);
                maxSegCount = pageSegs.Count > maxSegCount ? pageSegs.Count : maxSegCount;
            }

            DataTable table = new DataTable();
            table.Columns.Add("Leaf/Page");
            for(int i = 0; i < maxSegCount; i++)
            {
                table.Columns.Add($"Cut {i + 1}");
            }
            var pageCount = 1;
            foreach(List<Segment> pageSegs in bookSegs)
            {
                var result = getLineForPageSegments(pageSegs, pageCount);
                table.Rows.Add(result);
                pageCount++;
            }

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfLightTable pdfLightTable = new PdfLightTable();
            pdfLightTable.Style.CellPadding = 3;
            pdfLightTable.ApplyBuiltinStyle(PdfLightTableBuiltinStyle.GridTable3Accent3);
            pdfLightTable.DataSource = table;
            pdfLightTable.Style.ShowHeader = true;
            pdfLightTable.Draw(page, new Syncfusion.Drawing.PointF(0, 0));

            var filePath = $"{rootPath}/temp{(new Random()).Next(0, 999)}.pdf";
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                doc.Save(stream);
                doc.Close(true);
                stream.Close();
            }

            if (File.Exists(filePath))
            {
                var bytes = File.ReadAllBytes(filePath);
                File.Delete(filePath);
                return Convert.ToBase64String(bytes);
            }
            return null;
        }

        private string [] getLineForPageSegments(List<Segment> pageSegs, int page)
        {
            var finalList = new List<string>
            {
                $"{page}/{page*2}"
            };
            foreach (Segment segment in pageSegs)
            {
                string end = segment.end == -1 ? "end" : $"{segment.end}";
                finalList.Add($"{segment.start}-{end}");
            }
            return finalList.ToArray();
        }

        private void processMat(Mat mat, float scale)
        {
            var mtx = mat.mat;
            int pgs = 1;
            for (int i = 0; i < mat.height; i++)
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
    }
}
