using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GameOfLife.UserControls
{
    /// <summary>
    /// Interaction logic for Params.xaml
    /// </summary>
    public partial class Params : UserControl
    {
        public Board Board { get; set; }

        public Params()
        {
            InitializeComponent();
        }

        private void SizeParameters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var life = int.Parse(LifeTextBox.Text);
                var minNeighbors = int.Parse(MinTextBox.Text);
                var maxNeighbors = int.Parse(MaxTextBox.Text);
                var toBeBorn = int.Parse(BornTextBox.Text);
                Board.SetLife(life);
                Board.SetMinNeighbors(minNeighbors);
                Board.SetMaxNeighbors(maxNeighbors);
                Board.SetNeighborsToBeBorn(toBeBorn);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
