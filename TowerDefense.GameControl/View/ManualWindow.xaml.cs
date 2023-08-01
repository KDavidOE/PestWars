// <copyright file="ManualWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.View
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;

    /// <summary>
    /// Interaction logic for ManualWindow.xaml.
    /// </summary>
    public partial class ManualWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualWindow"/> class.
        /// </summary>
        public ManualWindow()
        {
            this.InitializeComponent();
            ImageBrush myBrush = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "../WindowResources/scoreBoardBg.png")));
            this.Background = myBrush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
