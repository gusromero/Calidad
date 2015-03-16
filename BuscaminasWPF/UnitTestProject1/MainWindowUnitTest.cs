using System;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class MainWindowUnitTest
    {
        [TestMethod]
        public void CleanFieldTestMethod()
        {
            MinesweeperWPF.MainWindow window = new MinesweeperWPF.MainWindow();
            window.Size = 10;

            window.CleanField();

            Assert.AreEqual(0, window.Field.ColumnDefinitions.Count);
            Assert.AreEqual(0, window.Field.RowDefinitions.Count);
            Assert.AreEqual(0, window.Field.Children.Count);
        }

        [TestMethod]
        public void GenerateFieldTestMethod()
        {
            MinesweeperWPF.MainWindow window = new MinesweeperWPF.MainWindow();
            window.Size = 10;
            window.CleanField();
            window.GenerateField();

            Assert.AreEqual(10, window.Field.ColumnDefinitions.Count);
            Assert.AreEqual(10, window.Field.RowDefinitions.Count);
            Assert.AreEqual(100, window.Field.Children.Count);

        }

        [TestMethod]
        public void SpreadBombsTestMethod()
        {
            int numero = 0;
            MinesweeperWPF.Fakes.StubIGenerator generator = new MinesweeperWPF.Fakes.StubIGenerator();
            generator.NextInt32 = (n) =>
            {
                numero++;
                return numero;
            };

            MinesweeperWPF.MainWindow window = new MinesweeperWPF.MainWindow();
            window.Size = 10;
            window.CleanField();
            window.GenerateField();
            window.SpreadBombs(generator);
            int bombs = 0;
            for (int i = 0; i < window.Field.Children.Count; i++)
                if (((TileControlLib.TileControl)window.Field.Children[i]).IsBomb)
                    bombs++;
            Assert.AreEqual(25, bombs);
            Assert.AreEqual(true, ((TileControlLib.TileControl)window.Field.Children[25]).IsBomb);
            Assert.AreEqual(false, ((TileControlLib.TileControl)window.Field.Children[26]).IsBomb);
        }
                  [TestMethod]
        public void GetSuroundingBombsTestMethod()
        {
            using (ShimsContext.Create())
            {
                MinesweeperWPF.MainWindow window = new MinesweeperWPF.MainWindow();
                new MinesweeperWPF.Fakes.ShimMainWindow(window)
                {                   
                    SpreadBombs = () =>
                    {
                        
                        ((TileControlLib.TileControl)window.Field.Children[0]).IsBomb = true;
                        ((TileControlLib.TileControl)window.Field.Children[1]).IsBomb = true;
                    }
                };
                
                window.Size = 10;
                window.CleanField();
                window.GenerateField();
                window.SpreadBombs();

                Assert.AreEqual("2", window.GetSuroundingBombs(1, 1));
            }
        }        
    }
}
