// <copyright file="GameIOC.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl
{
    using CommonServiceLocator;
    using GalaSoft.MvvmLight.Ioc;

    /// <summary>
    /// Container helping with mapping, creating data connections.
    /// </summary>
    public class GameIOC : SimpleIoc, IServiceLocator
    {
        /// <summary>
        /// Gets an instance of IOC.
        /// </summary>
        public static GameIOC Instance { get; private set; } = new GameIOC();
    }
}
