// <copyright file="PlayerScore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Data
{
    /// <summary>
    /// Class to represen player.
    /// </summary>
    public class PlayerScore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScore"/> class.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="score">The score of the player.</param>
        public PlayerScore(string name, string score)
        {
            this.Name = name;
            this.Score = score;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScore"/> class.
        /// </summary>
        public PlayerScore()
        {
        }

        /// <summary>
        /// Gets or Sets the name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the score of the player.
        /// </summary>
        public string Score { get; set; }
    }
}
