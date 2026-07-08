// ***********************************************************************
// Assembly       : Mercury.Core
// Author           : Matthew D. Barker
// Created          : 07-07-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-07-2026
// ***********************************************************************
// <copyright file="Logger.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Abstractions.Logging;

namespace Mercury.Core.Logging;

/// <summary>
/// Class Logger. This class cannot be inherited.
/// </summary>
internal sealed class Logger : ILogger
{

    private Logger()
    {}

    /// <summary>
    /// The logger backing variable
    /// </summary>
    private static readonly Lazy<ILogger> sm_Logger
        = new(() => new Logger());


    /// <summary>
    /// Gets the instance.
    /// </summary>
    /// <value>The instance.</value>
    public static ILogger Instance => sm_Logger.Value;

    public void Trace(string message)
    {
        throw new NotImplementedException();
    }

    public void Info(string message)
    {
        throw new NotImplementedException();
    }

    public void Warn(string message)
    {
        throw new NotImplementedException();
    }

    public void Error(string message, Exception? ex = null)
    {
        throw new NotImplementedException();
    }
}