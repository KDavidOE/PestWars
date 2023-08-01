// <copyright file="SaveHighScoreWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.View
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using TowerDefense.GameLogic;

    /// <summary>
    /// Interaction logic for SaveHighScoreWindow.xaml.
    /// </summary>
    public partial class SaveHighScoreWindow : Window
    {
        private readonly IGameLogic gameLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveHighScoreWindow"/> class.
        /// </summary>
        public SaveHighScoreWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
            ImageBrush myBrush = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "../WindowResources/inGameMenuBG.png")));
            this.Background = myBrush;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveHighScoreWindow"/> class.
        /// </summary>
        /// <param name="logic">Logic used in the game.</param>
        public SaveHighScoreWindow(IGameLogic logic)
            : this()
        {
            this.gameLogic = logic;
            Score = this.gameLogic.GetCurrentScore();
        }

        /// <summary>
        /// Gets the current score of the player.
        /// </summary>
        public static int Score
        {
            get; private set;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.gameLogic.SaveHighscore(this.playerName.Text);
            this.DialogResult = true;
        }
    }
}
