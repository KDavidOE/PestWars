// <copyright file="OpeningWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.View
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using TowerDefense.GameControl.VM;

    /// <summary>
    /// Interaction logic for OpeningWindow.xaml.
    /// </summary>
    public partial class OpeningWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningWindow"/> class.
        /// </summary>
        public OpeningWindow()
        {
            this.InitializeComponent();
            ImageBrush myBrush = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "../WindowResources/fomenu.png")));
            this.Background = myBrush;

            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Width = SystemParameters.PrimaryScreenWidth;
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            Window gamewindow = new GameWindow();
            this.Hide();
            (gamewindow.Resources["VM"] as GameWindowViewModel).CurrentGameControl.IsNewGame = true;
            gamewindow.ShowDialog();
            this.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadGame_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gamewindow = new GameWindow();
            (gamewindow.Resources["VM"] as GameWindowViewModel).CurrentGameControl.GameCreationErrorEvent += this.ActivateErrorMessage;
            (gamewindow.Resources["VM"] as GameWindowViewModel).CurrentGameControl.IsNewGame = false;
            this.Hide();
            if (gamewindow.ShowDialog() == true)
            {
                this.HiddenLabel.Visibility = Visibility.Hidden;
            }

            this.Show();
        }

        private void LoadLeaderboard_Click(object sender, RoutedEventArgs e)
        {
            Window leaderboardwin = new HighScoreWindow();
            this.Hide();
            leaderboardwin.ShowDialog();
            this.Show();
        }

        private void ActivateErrorMessage(object sender, EventArgs e)
        {
            this.HiddenLabel.Visibility = Visibility.Visible;
            this.HiddenLabel.Foreground = new SolidColorBrush(Colors.Red);
            this.HiddenLabel.Width = this.myGrid.Width;
            this.HiddenLabel.FontSize = SystemParameters.PrimaryScreenHeight / 2 / 20;
            this.HiddenLabel.Content = e.ToString();
            this.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window manualwindow = new ManualWindow();
            this.Hide();
            manualwindow.ShowDialog();
            this.Show();
        }
    }
}
