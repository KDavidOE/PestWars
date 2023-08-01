// <copyright file="InGameMenuWindow.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for InGameMenuWindow.xaml.
    /// </summary>
    public partial class InGameMenuWindow : Window
    {
        private readonly IGameLogic gameLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="InGameMenuWindow"/> class.
        /// </summary>
        public InGameMenuWindow()
        {
            this.InitializeComponent();
            ImageBrush myBrush = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "../WindowResources/inGameMenuBG.png")));
            this.Background = myBrush;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            this.Owner = Application.Current.MainWindow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InGameMenuWindow"/> class.
        /// </summary>
        /// <param name="logic">The game logic which can save the game state.</param>
        public InGameMenuWindow(IGameLogic logic)
            : this()
        {
            this.gameLogic = logic;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.gameLogic.SaveGame();
            this.DialogResult = true;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
