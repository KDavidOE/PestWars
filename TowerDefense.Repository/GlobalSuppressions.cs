// <copyright file="GlobalSuppressions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "justified", Scope = "member", Target = "~M:TowerDefense.Repository.StorageRepository.LoadGame(System.String)~System.Boolean")]
[assembly: SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "justified", Scope = "member", Target = "~M:TowerDefense.Repository.StorageRepository.CreateNewGame")]
[assembly: SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "<justified>", Scope = "member", Target = "~M:TowerDefense.Repository.StorageRepository.CreateNewGame")]
