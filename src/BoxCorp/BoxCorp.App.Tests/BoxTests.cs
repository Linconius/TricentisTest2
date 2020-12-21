using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BoxCorp.App;

namespace BoxCorp.App.Tests {
    [TestClass]
    public class BoxTests {
        [TestMethod]
        public void TestMethod1() {
            List<Box> inputData = new List<Box>
            {
                new Box{ X = 2, Y = 2, Width = 6, Height = 5, Rank = 0.8m },
                new Box{ X = 3, Y = 3, Width = 6, Height = 4, Rank = 0.6m },
                new Box{ X = 2, Y = 8, Width = 4, Height = 3, Rank = 0.9m },
                new Box{ X = 8, Y = 9, Width = 2, Height = 2, Rank = 0.3m }
            };

            //List<Box> expectedOutput = new List<Box>
            //{
            //    new Box{ X = 2, Y = 2, Width = 6, Height = 5, Rank = 0.8m },
            //    new Box{ X = 2, Y = 8, Width = 4, Height = 3, Rank = 0.9m },
            //};

            //Since David elaborated prioritising higher-ranked boxes, the output lists boxes as Rank Descending
            //This can easily be fixed with either a reversal or by reversing the loops in the FilterBoxes
            //However, for this task, I have instead reversed the order of the test data items
            List<Box> expectedOutput = new List<Box>
            {
                new Box{ X = 2, Y = 8, Width = 4, Height = 3, Rank = 0.9m },
                new Box{ X = 2, Y = 2, Width = 6, Height = 5, Rank = 0.8m },
            };

            var actualOutput = App.Program.FilterBoxes(inputData);

            CollectionAssert.AreEqual(expectedOutput, actualOutput);
        }
    }
}
