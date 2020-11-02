using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameOfLife.UserControls;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Board board;
        public string numberOfGens { get; set; }
        private Storyboard myStoryboard;
        public bool figure = false;
        private List<Tuple<int, int>> figurePos;

        public MainWindow()
        {
            InitializeComponent();
            board = new Board();
            Params.Board = board;
            HeightTextBox.Text = board.GetHeight().ToString();
            WidthTextBox.Text = board.GetWidth().ToString();
            DrawBoard();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            board = new Board();
            DrawBoard();
        }

        private void DrawBoard()
        {
            for (var i = 0; i < board.GetHeight(); i++)
            {
                for (var j = 0; j < board.GetWidth(); j++)
                {
                    Rectangle rectangle = new Rectangle();
                    rectangle.Width = GameBoard.Width / board.GetWidth() - 2.0;
                    rectangle.Height = GameBoard.Height / board.GetHeight() - 2.0;
                    rectangle.Fill = board.GetFieldColor(i, j);
                    GameBoard.Children.Add(rectangle);
                    Canvas.SetLeft(rectangle, j * GameBoard.Width / board.GetWidth());
                    Canvas.SetTop(rectangle, i * GameBoard.Height / board.GetHeight());
                    rectangle.MouseDown += R_MouseDown;
                }
            }
        }

        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int x, y;
            if (Canvas.GetLeft(((Rectangle) sender)) == 0)
            {
                y = 0;
            }
            else
            {
                y = Convert.ToInt32(Canvas.GetLeft(((Rectangle) sender)) / (GameBoard.Width / board.GetWidth()));
            }

            if (Canvas.GetTop(((Rectangle) sender)) == 0)
            {
                x = 0;
            }
            else
            {
                x = Convert.ToInt32(Canvas.GetTop(((Rectangle) sender)) / (GameBoard.Height / board.GetHeight()));
            }

            if (!figure)
            {
                if (board.GetFieldColor(x, y) == Brushes.GreenYellow)
                {
                    ((Rectangle) sender).Fill = Brushes.DarkCyan;
                    board.setField(x, y, board.GetEmptyField());
                }
                else
                {
                    ((Rectangle) sender).Fill = Brushes.GreenYellow;
                    board.setField(x, y, board.GetAliveField());
                }
            }
            else
            {
                board.AddFigure(x,y, figurePos);
                GameBoard.Children.Clear();
                DrawBoard();
                figure = false;
            }
        }

        private void ButtonNextTurn_Click(object sender, RoutedEventArgs e)
        {
            board.NextTurn();
            GameBoard.Children.Clear();
            DrawBoard();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            board.SaveBoard();
        }

        private void BottonLoad_Click(object sender, RoutedEventArgs e)
        {
            board.LoadBoard();
            GameBoard.Children.Clear();
            DrawBoard();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void SizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int width = int.Parse(WidthTextBox.Text);
                int height = int.Parse(HeightTextBox.Text);

                if (width > 0 && width < 100 && height > 0 && height < 100)
                {
                    board.resizeBoard(height, width);
                    GameBoard.Children.Clear();
                    DrawBoard();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void ButtonStartGen_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int num = 0;
            try
            {
                num = int.Parse(numberOfGens);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }


            while (i < num)
            {
                board.NextTurn();
                i++;
            }

            GameBoard.Children.Clear();
            DrawBoard();
        
        }

        private void PasteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (HeightTextBox != null) && (WidthTextBox != null);
        }

        private void PasteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HeightTextBox.Text = "";
            WidthTextBox.Text = "";
            HeightTextBox.Paste();
            WidthTextBox.Paste();
        }

        private void TubButton_Click(object sender, RoutedEventArgs e)
        {
            figure = true;
            figurePos = Figures.Tub();
        }

        private void FrogButton_Click(object sender, RoutedEventArgs e)
        {
            figure = true;
            figurePos = Figures.Frog();
        }

        private void GliderButton_Click(object sender, RoutedEventArgs e)
        {
            figure = true;
            figurePos = Figures.Glider();
        }

    }
}
