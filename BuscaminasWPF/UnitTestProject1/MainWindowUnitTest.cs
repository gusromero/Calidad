using System;
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

    }
}
