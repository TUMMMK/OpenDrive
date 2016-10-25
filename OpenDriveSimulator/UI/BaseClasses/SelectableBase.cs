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
using System.Windows.Forms;

namespace OpenDriveSimulator.UI
{
   abstract class SelectableBase : UIElementBase
   {
      public event EventHandler Selected;

      public bool IsActive { get; set; } = false;

      const float c_shadowWidth = 4.0f;

      public abstract void SendKey(Keys KeyCode, bool ShiftPressed);

      public override void ScaledDraw(SizeF offset)
      {
         if (!IsVisible)
            return;
         GTA.UI.Rectangle shadow = new GTA.UI.Rectangle()
         {
            Position = Tools.ScalePoint(new PointF(ScaledX - c_shadowWidth / 2, ScaledY - c_shadowWidth / 2)),
            Size = Tools.ScaleSize(new SizeF(ScaledWidth + c_shadowWidth, ScaledHeight + c_shadowWidth)),
            Color = IsActive ? Application.Highlight : Color.Black,
         };
         shadow.ScaledDraw(offset);
         base.ScaledDraw(offset);
      }
      public override void ScaledDraw()
      {
         if (!IsVisible)
            return;
         GTA.UI.Rectangle shadow = new GTA.UI.Rectangle()
         {
            Position = Tools.ScalePoint(new PointF(ScaledX - c_shadowWidth / 2, ScaledY - c_shadowWidth / 2)),
            Size = Tools.ScaleSize(new SizeF(ScaledWidth + c_shadowWidth, ScaledHeight + c_shadowWidth)),
            Color = IsActive ? Application.Highlight : Color.Black,
         };
         shadow.ScaledDraw();
         base.ScaledDraw();
      }

      protected void fireSelectEvent()
      {
         if (Selected != null)
            Selected(this, new EventArgs());
      }
      protected void fireSelectEvent(object differentSender)
      {
         if (Selected != null)
         Selected(differentSender, new EventArgs());
      }
   }
}
