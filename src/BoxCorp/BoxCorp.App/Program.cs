using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BoxCorp.App {

    struct Box {
        public int Row;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public decimal Rank;
        public int XEnd => X + Width;
        public int YEnd => Y + Height;
        public int Area => Width * Height;
    }
    class Program {
        static void Main(string[] args) {
            //Retreive List of boxes from CSV
            List<Box> boxes = ReadBoxCsv();
            boxes = FilterBoxes(boxes);
            Console.WriteLine($"Row\tX\tY\tWidth\tHeight\tRank");
            foreach (var box in boxes)
            {
                Console.WriteLine($"Box[{box.Row}]\t{box.X}\t{box.Y}\t{box.Width}\t{box.Height}\t{box.Rank}");
            }
            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }

        static List<Box> FilterBoxes(List<Box> unfiltered)
        {
            //Remove boxes with Rank threshhold below 0.5, and order by Rank Descending
            //Using LINQ due to experience and Readability -> Could improve performance by using Sort
            var output = unfiltered.Where(b => b.Rank >= 0.5m).OrderByDescending(b => b.Rank).ToList();
            //Loop through boxes
            for (int i = 0; i < output.Count; i++)
            {
                for (int j = i + 1; j < output.Count; j++)
                {
                    //Placeholders for output params in CheckIntersect
                    int xOverlap = 0;
                    int yOverlap = 0;
                    if (CheckIntersect(output[i], output[j], out xOverlap, out yOverlap))
                    {
                        //Use outted results from comparison to save Math processing
                        var jIndex = GetJaqardIndex(output[i], output[j], xOverlap, yOverlap);
                        //Remove lower ranking box if Jaqard Index > 0.4
                        if (jIndex > 0.4m) output.RemoveAt(j);
                    }
                }
            }

            return output;
        }

        static bool CheckIntersect(Box first, Box second, out int xOverlap, out int yOverlap)
        {
            //Get overlapping X
            xOverlap = Math.Max(0, Math.Min(first.XEnd, second.XEnd) - Math.Max(first.X, second.X));
            //Get overlapping Y
            yOverlap = Math.Max(0, Math.Min(first.YEnd, second.YEnd) - Math.Max(first.Y, second.Y));
            return xOverlap > 0 && yOverlap > 0;
        }

        static decimal GetJaqardIndex(Box first, Box second, int xOverlap, int yOverlap)
        {
            
            //Multiply for intersecting area
            var intersectArea = xOverlap * yOverlap;
            //Union = A1 + A2 - Intersecting Area
            var union = first.Area + second.Area - intersectArea;
            //Jaqard Index = Intersect/Union
            return decimal.Divide(intersectArea, union);
        }

        static List<Box> ReadBoxCsv() {
            using (var reader = new StreamReader(@"boxes.csv"))
            {
                List<Box> boxes = new List<Box>();
                //Initial ReadLine to process header
                reader.ReadLine();
                //Continue until all lines processed
                int currRow = 0;
                while (!reader.EndOfStream)
                {

                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var box = new Box
                    {
                        //Assuming data integrity, and forgoing Try/Catch or TryParse
                        Row = ++currRow,
                        X = int.Parse(values[0]),
                        Y = int.Parse(values[1]),
                        Width = int.Parse(values[2]),
                        Height = int.Parse(values[3]),
                        Rank = decimal.Parse(values[4], NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint)
                    };

                    boxes.Add(box);
                }

                return boxes;
            }
        }
    }
}
