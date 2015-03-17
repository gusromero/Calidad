using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class TileControlUnitTest
    {
        [TestMethod]
        public void DiscoverEventTestMethod()
        {
            TileControlLib.TileControl control = new TileControlLib.TileControl();
            control.State = TileControlLib.TileControlState.Covered;
            control.Discover += delegate(object sender,
                System.Windows.RoutedPropertyChangedEventArgs<TileControlLib.TileControlState> e)
            {
                if (e.NewValue == e.OldValue)
                    Assert.Fail();
                else
                    Assert.IsTrue(true);
            };
            control.State = TileControlLib.TileControlState.Bomb;
        }

    }
}
