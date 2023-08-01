// <copyright file="HighScoreWindow.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for HighScoreWindow.xaml.
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HighScoreWindow"/> class.
        /// </summary>
        public HighScoreWindow()
        {
            this.InitializeComponent();
            ImageBrush myBrush = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "../WindowResources/scoreBoardBg.png")));
            this.Background = myBrush;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if ((this.Resources["SM"] as ScoreViewModel).Scores == null)
            {
                MessageBox.Show("The are no records yet!");
                this.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
