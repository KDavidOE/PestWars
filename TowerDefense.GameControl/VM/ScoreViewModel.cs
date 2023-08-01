// <copyright file="ScoreViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.VM
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonServiceLocator;
    using GalaSoft.MvvmLight;
    using TowerDefense.GameControl.Data;
    using TowerDefense.GameControl.Logic;

    /// <summary>
    /// Score view model which can be used in window.
    /// </summary>
    public class ScoreViewModel : ViewModelBase
    {
        private readonly IScoreBoardLogic slogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreViewModel"/> class.
        /// </summary>
        /// <param name="logic">Game Logic.</param>
        public ScoreViewModel(IScoreBoardLogic logic)
        {
            this.Scores = new List<PlayerScore>();

            if (logic != null)
            {
                var list = (List<PlayerScore>)logic.GetScores();
                this.Scores = list.OrderByDescending(x => x.Score).ToList();
                this.slogic = logic;
            }

            if (this.IsInDesignMode)
            {
                this.Scores.Add(new PlayerScore("NoData Dave", "100"));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreViewModel"/> class.
        /// </summary>
        public ScoreViewModel()
            : this(IsInDesignModeStatic ? null : ServiceLocator.Current.GetInstance<IScoreBoardLogic>())
        {
        }

        /// <summary>
        /// Gets the collection of PlayerScores.
        /// </summary>
        public IList<PlayerScore> Scores { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game can be loaded.
        /// </summary>
        /// <returns>Returns true if game can be loaded.</returns>
        public bool GameCanBeLoaded()
        {
            return this.slogic.SaveGameCanBeLoaded();
        }
    }
}
