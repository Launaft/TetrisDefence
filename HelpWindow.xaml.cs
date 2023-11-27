using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TetrisDefence
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        int _pageNumber = 0;

        List<ImageBrush> _pages = new List<ImageBrush>()
        {
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(1).png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(2).png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(3).png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(4).png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(5).png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(6).png"))),
            new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/help_v2(7).png"))),
        };

        public HelpWindow()
        {
            InitializeComponent();

            Page_Grid.Background = _pages[_pageNumber];
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_pageNumber == 6)
            {
                 _pageNumber = 0;
            }
            else
                _pageNumber++;

            Page_Grid.Background = _pages[_pageNumber];
        }

        private void PreviousBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_pageNumber == 0)
            {
                _pageNumber = 6;
            }
            else
                _pageNumber--;

            Page_Grid.Background = _pages[_pageNumber];
        }
    }
}
