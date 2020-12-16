using System;
using System.Collections.Generic;
using System.IO;

namespace BoxCorp.App {

    struct Box {
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
            //Remove boxes with Rank threshhold below 0.5
            boxes.RemoveAll(b => b.Rank < 0.5m);
            //Loop through boxes
            foreach (Box box in boxes) {
                //TODO: Run CheckIntersecting on CSV Read, look into compiling list of intersects along with list of non-intersects
                //Can also add 0.5 rank check onto CSV read process, will save on compute.
                //Then, loop through intersects here only, and add to independent >0.5 boxes from CSV method.
            }
        }

        static bool CheckIntersect(Box first, Box second)
        {
            //Could possibly be more performant by using lesser/greater comparisions instead of Min - Max
            //Kept to matching IntersectingArea formula for readability and reliability
            return Math.Min(first.XEnd, second.XEnd) - Math.Max(first.X, second.X) > 0 &&
                    Math.Min(first.YEnd, second.YEnd) - Math.Max(first.Y, second.Y) > 0;
        }

        static decimal GetJaqardIndex(Box first, Box second)
        {
            //Get overlapping X
            var x_overlap = Math.Max(0, Math.Min(first.XEnd, second.XEnd) - Math.Max(first.X, second.X));
            //Get overlapping Y
            var y_overlap = Math.Max(0, Math.Min(first.YEnd, second.YEnd) - Math.Max(first.Y, second.Y));
            //Multiply for intersecting area
            var intersectArea = x_overlap * y_overlap;
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
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var box = new Box
                    {
                        //Assuming data integrity, and forgoing Try/Catch or TryParse
                        X = int.Parse(values[0]),
                        Y = int.Parse(values[1]),
                        Width = int.Parse(values[2]),
                        Height = int.Parse(values[3]),
                        Rank = decimal.Parse(values[4])
                    };

                    boxes.Add(box);
                }

                return boxes;
            }
        }
    }
}
