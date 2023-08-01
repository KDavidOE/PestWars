// <copyright file="ScoreBoardLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Logic
{
    using System.Collections.Generic;
    using TowerDefense.GameControl.Data;
    using TowerDefense.GameLogic;
    using TowerDefense.Repository;

    /// <summary>
    /// Logic for getting scoreboard.
    /// </summary>
    public class ScoreBoardLogic : IScoreBoardLogic
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreBoardLogic"/> class.
        /// </summary>
        /// <param name="gamelog">Game logic used in game.</param>
        public ScoreBoardLogic(IGameLogic gamelog)
        {
            this.GLogic = gamelog;
        }

        /// <inheritdoc/>
        public IGameLogic GLogic { get; set; }

        /// <inheritdoc/>
        public IList<PlayerScore> GetScores()
        {
            List<PlayerScore> scores = new List<PlayerScore>();
            if (this.GLogic.LoadHighscore())
            {
                foreach (var item in this.GLogic.GetHighScores())
                {
                    scores.Add(ConvertPlayerData.ConvertToPlayerData(item));
                }
            }

            return scores;
        }

        /// <inheritdoc/>
        public bool SaveGameCanBeLoaded()
        {
            return this.GLogic.LoadGame();
        }
    }
}
