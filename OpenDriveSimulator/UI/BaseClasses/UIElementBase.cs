///////////////////////////////////////////////////////////////////////////////////
///                                                                             ///
/// This file is part of the OpenDriveSimulator project                         ///
/// (www.github.com/TUMMMK/OpenDrive)                                           ///
///                                                                             ///
/// Copyright (c) 2016 Simon Schenk (simon.schenk@tum.de)                       ///
///                                                                             ///
///////////////////////////////////////////////////////////////////////////////////
/// The MIT License                                                             ///
///                                                                             ///
/// Permission is hereby granted, free of charge, to any person obtaining a     ///
/// copy of this software and associated documentation files (the "Software"),  ///
/// to deal in the Software without restriction, including without limitation   ///
/// the rights to use, copy, modify, merge, publish, distribute, sublicense,    ///
/// and/or sell copies of the Software, and to permit persons to whom the       ///
/// Software is furnished to do so, subject to the following conditions:        ///
///                                                                             ///
/// The above copyright notice and this permission notice shall be included     ///
/// in all copies or substantial portions of the Software.                      ///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS     ///
/// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, ///
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL     ///
/// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER  ///
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING     ///
/// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER         ///
/// DEALINGS IN THE SOFTWARE.                                                   ///
///////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;

using GTA.UI;

namespace OpenDriveSimulator.UI
{
   abstract class UIElementBase : Container
   {
      public string Name { get; set; }

      public bool IsVisible { get; private set; } = true;

      public float ScaledWidth
      {
         get { return Size.Width / Tools.ScaleFactor; }
         set { Size = new SizeF(value * Tools.ScaleFactor, Size.Height); }
      }
      public float ScaledHeight
      { 
         get { return Size.Height / Tools.ScaleFactor; }
         set { Size = new SizeF(Size.Width, value * Tools.ScaleFactor); }
      }
      public float ScaledX
      {
         get { return Position.X / Tools.ScaleFactor; }
         set { Position = new PointF(value * Tools.ScaleFactor, Position.Y); }
      }
      public float ScaledY
      {
         get { return Position.Y / Tools.ScaleFactor; }
         set { Position = new PointF(Position.X, value * Tools.ScaleFactor); }
      }

      public void Show() { IsVisible = true; }
      public void Hide() { IsVisible = false; }
      public override void ScaledDraw(SizeF offset)
      {
         if (!IsVisible)
            return;
         base.ScaledDraw(offset);
      }
      public override void Draw(SizeF offset)
      {
         throw new InvalidOperationException("Draw() is not supported. Use ScaledDraw() instead!");
      }

      public override void ScaledDraw()
      {
         if (IsVisible)
            base.ScaledDraw();
      }

      public override void Draw()
      {
         throw new InvalidOperationException("Draw() is not supported. Use ScaledDraw() instead!");
      }
   }
}
