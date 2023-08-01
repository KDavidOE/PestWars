// <copyright file="Enemy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    using System.Linq;

    /// <summary>
    /// A class which contains basic variables for enemies.
    /// </summary>
    public class Enemy : IEnemy
    {
        private int health;
        private int damage;
        private double speed;

        /// <inheritdoc/>
        public int Health
        {
            get { return this.health; }
            set { this.health = value; }
        }

        /// <inheritdoc/>
        public int Damage
        {
            get { return this.damage; }
            set { this.damage = value; }
        }

        /// <inheritdoc/>
        public double Speed
        {
            get { return this.speed; }
            set { this.speed = value; }
        }

        /// <inheritdoc/>
        public double Xpos { get; set; }

        /// <inheritdoc/>
        public double Ypos { get; set; }

        /// <inheritdoc/>
        public int MaxHealth { get; set; }

        /// <inheritdoc/>
        public double SlowTimer { get; set; } = -1;

        /// <inheritdoc/>
        public EnemyType ZombieType { get; set; }

        /// <inheritdoc/>
        public void CopyFrom(IEnemy other)
        {
            this.GetType().GetProperties().ToList().ForEach(prop => prop.SetValue(this, prop.GetValue(other)));
        }
    }
}
