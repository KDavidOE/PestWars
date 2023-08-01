// <copyright file="ErrorOccuredEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TowerDefense.GameControl
{
    using System;

    /// <summary>
    /// Class for representing error event.
    /// </summary>
    public class ErrorOccuredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorOccuredEventArgs"/> class.
        /// </summary>
        /// <param name="message">Error message.</param>
        public ErrorOccuredEventArgs(string message)
        {
            this.Message = message;
        }

        /// <summary>
        /// Gets or sets the message about the error.
        /// </summary>
        public string Message { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Message;
        }
    }
}
