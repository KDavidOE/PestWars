// <copyright file="GlobalSuppressions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "We don't need cryptographic security.", Scope = "member", Target = "~M:TowerDefense.GameLogic.MainGameLogic.SpawnNewWave")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "We don't need cryptographic security.", Scope = "member", Target = "~M:TowerDefense.GameLogic.MainGameLogic.SpawnEnemy(System.Boolean)")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "We don't need cryptographic security.", Scope = "member", Target = "~M:TowerDefense.GameLogic.MainGameLogic.AttackWithTowers")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "We don't need cryptographic security.", Scope = "member", Target = "~M:TowerDefense.GameLogic.MainGameLogic.SpawnProjectile(TowerDefense.GameModel.ITower)")]
