// <copyright file="IGameModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Interface for game model for declaring necessery variables for the game.
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// Gets a collection of enemies.
        /// </summary>
        IList<IEnemy> Enemies { get; }

        /// <summary>
        /// Gets a collection of towers.
        /// </summary>
        IList<ITower> Towers { get;  }

        /// <summary>
        /// Gets a collection of towers.
        /// </summary>
        Queue<ITower> TowerQueue { get; }

        /// <summary>
        /// Gets or Sets the position of the starting point where enemies are coming from.
        /// </summary>
        Point StartPoint { get; set; }

        /// <summary>
        /// Gets or Sets the position of the end point which is the base.
        /// </summary>
        Point EndPoint { get; set; }

        /// <summary>
        /// Gets or Sets the selected tower in the game.
        /// </summary>
        ITower SelectedTower { get; set; }

        /// <summary>
        /// Gets or Sets the selected tower in the game.
        /// </summary>
        ITower EditedTower { get; set; }

        /// <summary>
        /// Gets or Sets the health of the base.
        /// </summary>
        int BaseHealth { get; set; }

        /// <summary>
        /// Gets or Sets the number of waves in the game.
        /// </summary>
        int Wave { get; set; }

        /// <summary>
        /// Gets or Sets the player's money.
        /// </summary>
        int Money { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the game is paused or not.
        /// </summary>
        bool IsPaused { get; set; }

        /// <summary>
        /// Gets or sets the game map.
        /// </summary>
        Map[,] Map { get; set; }

        /// <summary>
        /// Gets or Sets the width of the game area, including the shop area.
        /// </summary>
        public double GameWidth { get; set; }

        /// <summary>
        /// Gets or Sets the height of the game area, including the shop area.
        /// </summary>
        public double GameHeight { get; set; }

        /// <summary>
        /// Gets or Sets the tile size of the game map.
        /// </summary>
        public double TileSize { get; set; }

        /// <summary>
        /// Gets available towers.
        /// </summary>
        public Dictionary<TowerType, ITower> AvailableTowers { get; }

        /// <summary>
        /// Gets or Sets the actual score of the player.
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// Gets available enemies.
        /// </summary>
        public Dictionary<EnemyType, IEnemy> AvailableEnemies { get; }

        /// <summary>
        /// Gets the width ratio between the gaming area and the shop.
        /// </summary>
        public double MapShopRatio { get; }

        /// <summary>
        /// Gets the height of one shop tile.
        /// </summary>
        public double ShopTileHeight { get; }

        /// <summary>
        /// Gets the width of one shop tile.
        /// </summary>
        public double ShopTileWidth { get; }

        /// <summary>
        /// Gets the number which represents how much fps will the game run on.
        /// </summary>
        public int RefreshRate { get; }
    }
}
