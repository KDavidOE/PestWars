// <copyright file="MapEnum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameModel
{
    /// <summary>
    /// Represents the state of the game's tiles.
    /// </summary>
    public enum Map
    {
        /// <summary>
        /// Represents a buildable tile on the map.
        /// </summary>
        BUILDABLE,

        /// <summary>
        /// Represents an upwards pointing road on the map.
        /// </summary>
        ROAD_UP,

        /// <summary>
        /// Represents a downwards pointing road on the map.
        /// </summary>
        ROAD_DOWN,

        /// <summary>
        /// Represents a leftwards pointing road on the map.
        /// </summary>
        ROAD_LEFT,

        /// <summary>
        /// Represents a rightwards pointing road on the map.
        /// </summary>
        ROAD_RIGHT,

        /// <summary>
        /// Represents an occupied tile on the map.
        /// </summary>
        OCCUPIED,

        /// <summary>
        /// Represents an obstacle on the map.
        /// </summary>
        OBSTACLE,
    }
}
