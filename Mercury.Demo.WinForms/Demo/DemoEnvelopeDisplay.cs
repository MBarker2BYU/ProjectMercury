// ***********************************************************************
// Assembly     : Mercury.Demo.WinForms
// Author         : Matthew D. Barker
// Created        : 07-22-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-22-2026
// ***********************************************************************
// <copyright file="DemoEnvelopeDisplay.cs">
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

namespace Mercury.Demo.WinForms.Demo;

internal sealed record DemoEnvelopeDisplay(
    string Version,
    string EnvelopeId,
    string Algorithm,
    string ReplayToken,
    string ProtectedPayloadSize,
    string TotalFrameSize,
    string RawPayloadVisible,
    string HeaderMetadata,
    string FooterMetadata,
    string HexPreview);