// <copyright file="ConvertPlayerData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl.Logic
{
    using TowerDefense.GameControl.Data;
    using TowerDefense.Repository;

    /// <summary>
    /// Converter class for converting database player into upper layer player.
    /// </summary>
    public static class ConvertPlayerData
    {
        /// <summary>
        /// Converts PlayerStats to PlayerScore.
        /// </summary>
        /// <param name="dbPlayer">The player represented in database.</param>
        /// <returns>Returns a PlayerScore.</returns>
        public static PlayerScore ConvertToPlayerData(PlayerStats dbPlayer)
        {
            PlayerScore pldata = new PlayerScore();

            if (dbPlayer != null)
            {
                pldata = new PlayerScore(dbPlayer.Name, dbPlayer.Score);
            }

            return pldata;
        }
    }
}
