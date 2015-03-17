using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace TileControlLib
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TileControlLib"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:TileControlLib;assembly=TileControlLib"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class TileControl : Control
    {
        static TileControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TileControl), new FrameworkPropertyMetadata(typeof(TileControl)));
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TileControl), new PropertyMetadata(String.Empty));


        public TileControlState State
        {
            get { return (TileControlState)GetValue(StateProperty); }
            set
            {
                var oldValue = (TileControlState)GetValue(StateProperty);
                SetValue(StateProperty, value);
                RaiseDiscoverEventEvent(oldValue, value);
            }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State", typeof(TileControlState), typeof(TileControl), new PropertyMetadata(TileControlState.Covered));



        public bool IsBomb
        {
            get { return (bool)GetValue(IsBombProperty); }
            set { SetValue(IsBombProperty, value); }
        }

        public static readonly DependencyProperty IsBombProperty =
            DependencyProperty.Register("IsBomb", typeof(bool), typeof(TileControl), new PropertyMetadata(false));



        public static readonly RoutedEvent DiscoverEvent = EventManager.RegisterRoutedEvent(
    "Discovered", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<TileControlState>), typeof(TileControl));

        public event RoutedPropertyChangedEventHandler<TileControlState> Discover
        {
            add { AddHandler(DiscoverEvent, value); }
            remove { RemoveHandler(DiscoverEvent, value); }
        }

        // This method raises the Tap event 
        void RaiseDiscoverEventEvent(TileControlState oldValue, TileControlState newValue)
        {
            var newEventArgs = new RoutedPropertyChangedEventArgs<TileControlState>(oldValue, newValue, DiscoverEvent);
            RaiseEvent(newEventArgs);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            if (State == TileControlState.Covered)
                State = IsBomb ? TileControlState.Bomb : TileControlState.Text;
        }


    }
}
