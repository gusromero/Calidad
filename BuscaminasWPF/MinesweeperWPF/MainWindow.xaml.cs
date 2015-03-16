using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TileControlLib;

namespace MinesweeperWPF
{
    public partial class MainWindow : Window
    {
        public static RoutedCommand RestartCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            CleanField();
            GenerateField();
            SpreadBombs();
            CommandBindings.Add(new CommandBinding(RestartCommand, ReStartCommandExecute, CommandBinding_CanExecute));
        }

        public void ReStartCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            CleanField();
            GenerateField();
            SpreadBombs();
        }

        public void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        public void CleanField()
        {
            field.Children.Clear();
            field.ColumnDefinitions.Clear();
            field.RowDefinitions.Clear();
        }

        public void GenerateField()
        {
            for (int i = 0; i < this.Size; i++)
                field.ColumnDefinitions.Add(new ColumnDefinition());
            for (int j = 0; j < this.Size; j++)
                field.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < this.Size; i++)
                for (int j = 0; j < this.Size; j++)
                {
                    TileControl control = new TileControl();
                    control.Discover += TileControl_Discover;
                    field.Children.Add(control);
                    Grid.SetRow(control, i);
                    Grid.SetColumn(control, j);
                }
        }

        public int Size
        {
            get { return (int)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        public Grid Field
        {
            get
            {
                return this.field;
            }
        }

        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register("Size", typeof(int), typeof(MainWindow), new PropertyMetadata(10));

        int emptyTiles = 0;

        public void SpreadBombs()
        {
            int bombs = Convert.ToInt32(Math.Truncate((decimal)(Math.Pow(this.Size, 2)) / 4));
            emptyTiles = Convert.ToInt32(Math.Pow(this.Size, 2)) - bombs;
            int e = 0;
            Random generator = new Random();

            for (int i = 0; i < bombs; i++)
            {
                do
                {
                    e = generator.Next(field.Children.Count);
                } while (((TileControl)field.Children[e]).IsBomb);

                ((TileControl)field.Children[e]).IsBomb = true;
            }

            foreach (UIElement element in field.Children)
            {
                ((TileControl)element).Text = GetSuroundingBombs(Grid.GetRow(element), Grid.GetColumn(element));
            }
        }


        public string GetSuroundingBombs(int row, int column)
        {
            int result = 0;
            int rows = field.RowDefinitions.Count();
            int columns = field.ColumnDefinitions.Count();

            for (int x = row - 1; x <= row + 1; x++)
                for (int y = column - 1; y <= column + 1; y++)
                {
                    if ((x < 0) || (x >= rows) || (y < 0) || (y >= columns))
                        continue;
                    else
                    {
                        var elements = field.Children.Cast<TileControl>().
                            Where(e => Grid.GetRow(e) == x && Grid.GetColumn(e) == y && e.IsBomb);
                        result += elements.Any() ? 1 : 0;
                    }
                }
            return result.ToString();
        }

        private void TileControl_Discover(object sender, RoutedPropertyChangedEventArgs<TileControlLib.TileControlState> e)
        {
            if (e.NewValue == TileControlState.Bomb)
                MessageBox.Show("Juego terminado. Tu pierdes");
            else
            {
                emptyTiles--;
                if (emptyTiles == 0)
                    MessageBox.Show("Juego terminado. Tu Ganas");
            }
        }

    }
}
