// <copyright file="GameWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.VM
{
    using GalaSoft.MvvmLight;

    /// <summary>
    /// Game view model for game window.
    /// </summary>
    public class GameWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameWindowViewModel"/> class.
        /// </summary>
        public GameWindowViewModel()
        {
            this.CurrentGameControl = new TowerDefense.GameControl.GControl();
        }

        /// <summary>
        /// Gets or Sets the current game control.
        /// </summary>
        public GControl CurrentGameControl { get; set; }
    }
}
