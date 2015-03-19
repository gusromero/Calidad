using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TileControlLib;

namespace MinesweeperWPF
{
    public partial class MainWindow : Window
    {
        public static RoutedCommand RestartCommand = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
            for (int i = 0; i < Size; i++)
                field.ColumnDefinitions.Add(new ColumnDefinition());
            for (int j = 0; j < Size; j++)
                field.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                {
                    var control = new TileControl();
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
                return field;
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
            var generator = new Random();

            List<TileControl> list = field.Children.Cast<TileControl>().ToList();
            for (int i = 0; i < bombs; i++)
            {
                do
                {
                    e = generator.Next(list.Count);
                } while (list[e].IsBomb);

                list[e].IsBomb = true;
                list.RemoveAt(e);
            }


            foreach (UIElement element in field.Children)
            {
                ((TileControl)element).Text = GetSuroundingBombs(Grid.GetRow(element), Grid.GetColumn(element));
            }
        }


        public string GetSuroundingBombs(int row, int column)
        {
            var result = 0;
            var rows = field.RowDefinitions.Count();
            var columns = field.ColumnDefinitions.Count();

            for (var x = row - 1; x <= row + 1; x++)
                for (var y = column - 1; y <= column + 1; y++)
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
            return result.ToString(CultureInfo.InvariantCulture);
        }

        private void TileControl_Discover(object sender, RoutedPropertyChangedEventArgs<TileControlState> e)
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

        public void SpreadBombs(IGenerator generator)
        {
            int bombs = Convert.ToInt32(Math.Truncate((decimal)(Math.Pow(Size, 2)) / 4));
            emptyTiles = Convert.ToInt32(Math.Pow(Size, 2)) - bombs;
            int e;

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


    }
}
