// <copyright file="GameBaseModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Game model which represents the game's base.
    /// </summary>
    public class GameBaseModel : IGameModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameBaseModel"/> class.
        /// </summary>
        /// <param name="gamewidth">The width of the game area.</param>
        /// <param name="gameheight">The height of the game area.</param>
        public GameBaseModel(double gamewidth, double gameheight)
        {
            this.Enemies = new List<IEnemy>();
            this.Towers = new List<ITower>();
            this.GameWidth = gamewidth;
            this.GameHeight = gameheight;
            this.BaseHealth = 1;
            this.Money = 1500;
            this.Score = 0;
            this.MapShopRatio = 0.8;
            this.TowerQueue = new Queue<ITower>();
        }

        /// <inheritdoc/>
        public IList<IEnemy> Enemies { get; private set; }

        /// <inheritdoc/>
        public IList<ITower> Towers { get; private set; }

        /// <inheritdoc/>
        public ITower SelectedTower { get; set; }

        /// <inheritdoc/>
        public ITower EditedTower { get; set; }

        /// <inheritdoc/>
        public int BaseHealth { get; set; }

        /// <inheritdoc/>
        public int Wave { get; set; }

        /// <inheritdoc/>
        public int Money { get; set; }

        /// <inheritdoc/>
        public bool IsPaused { get; set; }

        /// <inheritdoc/>
        public Map[,] Map { get; set; }

        /// <inheritdoc/>
        public double GameWidth { get; set; }

        /// <inheritdoc/>
        public double GameHeight { get; set; }

        /// <inheritdoc/>
        public double TileSize
        {
            get { return this.GameHeight / this.Map.GetLength(0); }
            set { }
        }

        /// <inheritdoc/>
        public int Score { get; set; }

        /// <inheritdoc/>
        public double MapShopRatio { get; private set; }

        /// <inheritdoc/>
        public double ShopTileHeight
        {
            get { return this.GameHeight / (Math.Ceiling((double)this.AvailableTowers.Count / 2) + 2); }
            private set { }
        }

        /// <inheritdoc/>
        public double ShopTileWidth
        {
            get { return this.GameWidth / 10; }
            private set { }
        }

        /// <inheritdoc/>
        public Point StartPoint { get; set; }

        /// <inheritdoc/>
        public Point EndPoint { get; set; }

        /// <inheritdoc/>
        public int RefreshRate
        {
            get { return 60; } private set { }
        }

        /// <inheritdoc/>
        public Dictionary<TowerType, ITower> AvailableTowers { get; private set; } = new Dictionary<TowerType, ITower>();

        /// <inheritdoc/>
        public Dictionary<EnemyType, IEnemy> AvailableEnemies { get; private set; } = new Dictionary<EnemyType, IEnemy>();

        /// <inheritdoc/>
        public Queue<ITower> TowerQueue { get; private set; }
    }
}
