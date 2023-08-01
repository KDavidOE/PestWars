// <copyright file="GameWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.View
{
    using System;
    using System.Media;
    using System.Windows;
    using TowerDefense.GameControl.VM;

    /// <summary>
    /// Interaction logic for GameWindow.xaml.
    /// </summary>
    public partial class GameWindow : Window
    {
        private SoundPlayer player;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindow"/> class.
        /// </summary>
        public GameWindow()
        {
            this.InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (this.Resources["VM"] as GameWindowViewModel).CurrentGameControl = null;
            this.player.Stop();
            this.player.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string file = "\\WindowResources\\Glorious_morning.wav";
            var path = AppDomain.CurrentDomain.BaseDirectory;
            string[] s = path.Split("\\");
            string finalPath = string.Empty;
            foreach (var item in s)
            {
                finalPath = finalPath + item + "\\";
                if (item == "TowerDefense.GameControl")
                {
                    break;
                }
            }

            var stream2 = finalPath + file;
            this.player = new SoundPlayer(stream2);
            this.player.Load();
            this.player.PlayLooping();
        }
    }
}
