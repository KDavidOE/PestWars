// <copyright file="IEnemy.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    /// <summary>
    /// Interface for enemies to declare necessery variables.
    /// </summary>
    public interface IEnemy
    {
        /// <summary>
        /// Gets or Sets the health of an enemy object.
        /// </summary>
        int Health { get; set; }

        /// <summary>
        /// Gets or Sets the damage of an enemy object.
        /// </summary>
        int Damage { get; set; }

        /// <summary>
        /// Gets or Sets the speed of an enemy object.
        /// </summary>
        double Speed { get; set; }

        /// <summary>
        /// Gets or Sets the position of an enemy object. This referes to a specific map tile horizontally.
        /// </summary>
        double Xpos { get; set; }

        /// <summary>
        /// Gets or Sets the position of an enemy object. This referes to a specific map tile vertically.
        /// </summary>
        double Ypos { get; set; }

        /// <summary>
        /// Gets or Sets the maximum health of the enemy unit.
        /// </summary>
        int MaxHealth { get; set; }

        /// <summary>
        /// Gets or Sets the slowing time.
        /// </summary>
        double SlowTimer { get; set; }

        /// <summary>
        /// Gets or Sets the zombie's type.
        /// </summary>
        public EnemyType ZombieType { get; set; }

        /// <summary>
        /// Copies the values of another enemy into the new one.
        /// </summary>
        /// <param name="other">Other enemy.</param>
        public void CopyFrom(IEnemy other);
    }
}
