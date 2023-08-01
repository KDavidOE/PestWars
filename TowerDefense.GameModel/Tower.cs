// <copyright file="Tower.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System.Collections.Generic;

    /// <summary>
    /// A class which contains basic variables for towers.
    /// </summary>
    public class Tower : ITower
    {
        private int damage;
        private double range;
        private int price;

        /// <inheritdoc/>
        public int Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        /// <inheritdoc/>
        public double Range
        {
            get { return this.range; }
            set { this.range = value; }
        }

        /// <inheritdoc/>
        public int Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        /// <inheritdoc/>
        public int TileX { get; set; }

        /// <inheritdoc/>
        public int TileY { get; set; }

        /// <inheritdoc/>
        public IList<IProjectile> Projectiles { get; private set; } = new List<IProjectile>();

        /// <inheritdoc/>
        public double AttackInterval { get; set; } = 10;

        /// <inheritdoc/>
        public double AttackTimer { get; set; } = 10;

        /// <inheritdoc/>
        public TowerType DefenseType { get; set; }

        /// <inheritdoc/>
        public void CopyFrom(ITower other)
        {
            if (other != null)
            {
                this.damage = other.Damage;
                this.price = other.Price;
                this.AttackInterval = other.AttackInterval;
                this.AttackTimer = other.AttackTimer;
                this.range = other.Range;
                this.DefenseType = other.DefenseType;
            }
        }
    }
}
