// ***********************************************************************
// Assembly       : Mercury.Demo
// Author           : Matthew D. Barker
// Created          : 07-12-2026
//
// Last Modified By : Matthew D. Barker
// Last Modified On : 07-12-2026
// ***********************************************************************
//< copyright file = "Renderer.cs" >
//     Copyright (c) Matthew D. Barker. All rights reserved.
//     Submitted in partial fulfillment of CSE499 Senior Capstone Project
//     at Brigham Young University-Idaho.
// </copyright>
// ***********************************************************************

using System.Drawing.Drawing2D;
using Mercury.Demo.WinForms.Controls.Enums;

namespace Mercury.Demo.WinForms.Controls.Rendering
{
    /// <summary>
    /// Class Renderer.
    /// </summary>
    public sealed class Renderer
    {
        /// <summary>
        /// The sm renderer
        /// </summary>
        private static readonly Lazy<Renderer> sm_Renderer = new(() => new Renderer());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Renderer Instance => sm_Renderer.Value;

        /// <summary>
        /// Clears the background.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="clearColor">Color of the clear.</param>
        /// <exception cref="ArgumentNullException">graphics</exception>
        public void ClearBackground(Graphics graphics, Rectangle bounds, Color clearColor)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            using var brush = new SolidBrush(clearColor);
            graphics.FillRectangle(brush, bounds);
        }

        /// <summary>
        /// Renders the background.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="backColor">Color of the back.</param>
        /// <param name="startFillAlpha">The start fill alpha.</param>
        /// <param name="endFillAlpha">The end fill alpha.</param>
        /// <param name="cornerStyle">The corner style.</param>
        /// <param name="renderedCorners">The rendered corners.</param>
        /// <param name="cornerSize">Size of the corner.</param>
        /// <exception cref="ArgumentNullException">graphics</exception>
        public void RenderBackground(Graphics graphics, Rectangle bounds, Color backColor, int startFillAlpha, int endFillAlpha,
            CornerStyle cornerStyle, RenderedCorners renderedCorners, int cornerSize)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            if (backColor == Color.Transparent || backColor.A == 0)
                return;

            using var path = CreateBorderPath(
                new RectangleF(bounds.Left, bounds.Top, bounds.Width, bounds.Height),
                cornerStyle,
                renderedCorners,
                cornerSize);

            var colorBlend = CreateGradientBlend(backColor, startFillAlpha, endFillAlpha, bounds);

            using var gradientBrush =
                new LinearGradientBrush(bounds, Color.Empty, Color.Empty, LinearGradientMode.ForwardDiagonal);

            gradientBrush.InterpolationColors = colorBlend;

            graphics.FillPath(gradientBrush, path);
        }

        /// <summary>
        /// Renders the background.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="backColor">Color of the back.</param>
        /// <param name="cornerStyle">The corner style.</param>
        /// <param name="renderedCorners">The rendered corners.</param>
        /// <param name="cornerSize">Size of the corner.</param>
        public void RenderBackground(Graphics graphics, Rectangle bounds, Color backColor, CornerStyle cornerStyle,
            RenderedCorners renderedCorners, int cornerSize)
            => RenderBackground(graphics, bounds, backColor, 255, 255, cornerStyle, renderedCorners, cornerSize);

        /// <summary>
        /// Renders the border.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="borderColor">Color of the border.</param>
        /// <param name="borderWidth">Width of the border.</param>
        /// <param name="cornerStyle">The corner style.</param>
        /// <param name="renderedCorners">The rendered corners.</param>
        /// <param name="cornerSize">Size of the corner.</param>
        /// <exception cref="ArgumentNullException">graphics</exception>
        public void RenderBorder(
            Graphics graphics,
            Rectangle bounds,
            Color borderColor,
            int borderWidth,
            CornerStyle cornerStyle,
            RenderedCorners renderedCorners,
            int cornerSize)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return;

            if (borderWidth <= 0)
                return;

            if (borderColor == Color.Transparent || borderColor.A == 0)
                return;

            var inset = borderWidth / 2f;

            var borderBounds = new RectangleF(
                bounds.Left + inset,
                bounds.Top + inset,
                bounds.Width - borderWidth,
                bounds.Height - borderWidth);

            if (borderBounds.Width <= 0 || borderBounds.Height <= 0)
                return;

            using var path = CreateBorderPath(
                borderBounds,
                cornerStyle,
                renderedCorners,
                cornerSize);

            using var pen = new Pen(borderColor, borderWidth);
            pen.Alignment = PenAlignment.Center;

            var oldSmoothing = graphics.SmoothingMode;
            var oldPixelOffset = graphics.PixelOffsetMode;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            graphics.DrawPath(pen, path);

            graphics.SmoothingMode = oldSmoothing;
            graphics.PixelOffsetMode = oldPixelOffset;
        }

        /// <summary>
        /// Creates the border path.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="cornerStyle">The corner style.</param>
        /// <param name="renderedCorners">The rendered corners.</param>
        /// <param name="cornerSize">Size of the corner.</param>
        /// <returns>GraphicsPath.</returns>
        private GraphicsPath CreateBorderPath(
            RectangleF bounds,
            CornerStyle cornerStyle,
            RenderedCorners renderedCorners,
            int cornerSize)
        {
            var path = new GraphicsPath();

            if (bounds.Width <= 0 || bounds.Height <= 0)
                return path;

            float size = Math.Max(0, cornerSize);
            size = Math.Min(size, Math.Min(bounds.Width, bounds.Height) / 2f);

            if (cornerStyle == CornerStyle.Squared ||
                renderedCorners == RenderedCorners.None ||
                size <= 0)
            {
                path.AddRectangle(bounds);
                path.CloseFigure();
                return path;
            }

            var topLeft = renderedCorners.HasFlag(RenderedCorners.TopLeft);
            var topRight = renderedCorners.HasFlag(RenderedCorners.TopRight);
            var bottomLeft = renderedCorners.HasFlag(RenderedCorners.BottomLeft);
            var bottomRight = renderedCorners.HasFlag(RenderedCorners.BottomRight);

            if (cornerStyle == CornerStyle.Rounded)
            {
                AddRoundedPath(
                    path,
                    bounds,
                    size,
                    topLeft,
                    topRight,
                    bottomRight,
                    bottomLeft);
            }
            else if (cornerStyle == CornerStyle.Clipped)
            {
                AddClippedPath(
                    path,
                    bounds,
                    size,
                    topLeft,
                    topRight,
                    bottomRight,
                    bottomLeft);
            }
            else
            {
                path.AddRectangle(bounds);
            }

            path.CloseFigure();
            return path;
        }

        /// <summary>
        /// Adds the rounded path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="size">The size.</param>
        /// <param name="topLeft">if set to <c>true</c> [top left].</param>
        /// <param name="topRight">if set to <c>true</c> [top right].</param>
        /// <param name="bottomRight">if set to <c>true</c> [bottom right].</param>
        /// <param name="bottomLeft">if set to <c>true</c> [bottom left].</param>
        private static void AddRoundedPath(
            GraphicsPath path,
            RectangleF bounds,
            float size,
            bool topLeft,
            bool topRight,
            bool bottomRight,
            bool bottomLeft)
        {
            var left = bounds.Left;
            var top = bounds.Top;
            var right = bounds.Right;
            var bottom = bounds.Bottom;
            var diameter = size * 2f;

            path.StartFigure();

            if (topLeft)
                path.AddArc(left, top, diameter, diameter, 180f, 90f);
            else
                path.AddLine(left, top, left, top);

            path.AddLine(
                topLeft ? left + size : left,
                top,
                topRight ? right - size : right,
                top);

            if (topRight)
                path.AddArc(right - diameter, top, diameter, diameter, 270f, 90f);
            else
                path.AddLine(right, top, right, top);

            path.AddLine(
                right,
                topRight ? top + size : top,
                right,
                bottomRight ? bottom - size : bottom);

            if (bottomRight)
                path.AddArc(right - diameter, bottom - diameter, diameter, diameter, 0f, 90f);
            else
                path.AddLine(right, bottom, right, bottom);

            path.AddLine(
                bottomRight ? right - size : right,
                bottom,
                bottomLeft ? left + size : left,
                bottom);

            if (bottomLeft)
                path.AddArc(left, bottom - diameter, diameter, diameter, 90f, 90f);
            else
                path.AddLine(left, bottom, left, bottom);

            path.AddLine(
                left,
                bottomLeft ? bottom - size : bottom,
                left,
                topLeft ? top + size : top);
        }

        /// <summary>
        /// Adds the clipped path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="bounds">The bounds.</param>
        /// <param name="size">The size.</param>
        /// <param name="topLeft">if set to <c>true</c> [top left].</param>
        /// <param name="topRight">if set to <c>true</c> [top right].</param>
        /// <param name="bottomRight">if set to <c>true</c> [bottom right].</param>
        /// <param name="bottomLeft">if set to <c>true</c> [bottom left].</param>
        private static void AddClippedPath(
            GraphicsPath path,
            RectangleF bounds,
            float size,
            bool topLeft,
            bool topRight,
            bool bottomRight,
            bool bottomLeft)
        {
            var left = bounds.Left;
            var top = bounds.Top;
            var right = bounds.Right;
            var bottom = bounds.Bottom;

            var p1 = topLeft ? new PointF(left, top + size) : new PointF(left, top);
            var p2 = topLeft ? new PointF(left + size, top) : new PointF(left, top);

            var p3 = topRight ? new PointF(right - size, top) : new PointF(right, top);
            var p4 = topRight ? new PointF(right, top + size) : new PointF(right, top);

            var p5 = bottomRight ? new PointF(right, bottom - size) : new PointF(right, bottom);
            var p6 = bottomRight ? new PointF(right - size, bottom) : new PointF(right, bottom);

            var p7 = bottomLeft ? new PointF(left + size, bottom) : new PointF(left, bottom);
            var p8 = bottomLeft ? new PointF(left, bottom - size) : new PointF(left, bottom);

            path.StartFigure();

            path.AddLine(p1, p2);
            path.AddLine(p2, p3);
            path.AddLine(p3, p4);
            path.AddLine(p4, p5);
            path.AddLine(p5, p6);
            path.AddLine(p6, p7);
            path.AddLine(p7, p8);
            path.AddLine(p8, p1);
        }

        /// <summary>
        /// Calculates the gradient steps.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <returns>System.Int32.</returns>
        private static int CalculateGradientSteps(Rectangle bounds)
        {
            var span = Math.Max(bounds.Width, bounds.Height);

            if (span <= 50)
                return 2;

            return 2 + (int)Math.Ceiling((span - 50) / 25f);
        }

        /// <summary>
        /// Creates the gradient blend.
        /// </summary>
        /// <param name="baseColor">Color of the base.</param>
        /// <param name="startFillAlpha">The start fill alpha.</param>
        /// <param name="endFillAlpha">The end fill alpha.</param>
        /// <param name="bounds">The bounds.</param>
        /// <returns>ColorBlend.</returns>
        private static ColorBlend CreateGradientBlend(
            Color baseColor,
            int startFillAlpha,
            int endFillAlpha,
            Rectangle bounds)
        {
            var stepCount = Math.Max(2, CalculateGradientSteps(bounds) + 1);

            startFillAlpha = Math.Clamp(startFillAlpha, 0, 255);
            endFillAlpha = Math.Clamp(endFillAlpha, 0, 255);

            var colors = new Color[stepCount];
            var positions = new float[stepCount];

            for (var index = 0; index < stepCount; index++)
            {
                float position = index / (float)(stepCount - 1);

                int alpha = (int)Math.Round(
                    startFillAlpha + ((endFillAlpha - startFillAlpha) * position));

                alpha = Math.Clamp(alpha, 0, 255);

                colors[index] = Color.FromArgb(alpha, baseColor);
                positions[index] = position;
            }

            positions[0] = 0.0f;
            positions[stepCount - 1] = 1.0f;

            colors[0] = Color.FromArgb(startFillAlpha, baseColor);
            colors[stepCount - 1] = Color.FromArgb(endFillAlpha, baseColor);

            return new ColorBlend
            {
                Colors = colors,
                Positions = positions
            };
        }

        /// <summary>
        /// Renders the line.
        /// </summary>
        /// <param name="graphics">The graphics.</param>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="color">The color.</param>
        /// <param name="width">The width.</param>
        public static void RenderLine(Graphics graphics, Point startPoint, Point endPoint, Color color, float width = 1.0f)
        {
            using var linePen = new Pen(color, width);

            graphics.DrawLine(linePen, startPoint, endPoint);
        }

    }
}