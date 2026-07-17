// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-13-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-13-2026
// ***********************************************************************
// <copyright file="Result.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using Mercury.Demo.WinForms.v1.Interfaces;

namespace Mercury.Demo.WinForms.v1;

/// <summary>
/// Struct Result
/// Implements the <see cref="IResult" />
/// </summary>
/// <param name="success">if set to <c>true</c> [success].</param>
/// <param name="exception">The exception.</param>
/// <seealso cref="IResult" />
internal readonly struct Result(bool success, Exception? exception = null) : IResult
{

    /// <summary>
    ///Returns as Result as successful.
    /// </summary>
    /// <value>The successful.</value>
    public static Result Successful => new Result(true);

    /// <summary>
    /// Returns Result as a failure with the specified exception.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>Result.</returns>
    public static Result Failure(Exception exception)
        => new (exception);

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> struct.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public Result(Exception exception):this(false, exception)
    {}

    /// <summary>
    /// Gets a value indicating whether this <see cref="Result"/> is success.
    /// </summary>
    /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
    public bool Success { get; } = success;

    /// <summary>
    /// Gets the exception.
    /// </summary>
    /// <value>The exception.</value>
    public Exception? Exception { get; } = exception;
}