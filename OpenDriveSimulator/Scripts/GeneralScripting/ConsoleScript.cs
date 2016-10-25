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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using GTA.UI;
using Font = GTA.UI.Font;

namespace OpenDriveSimulator.Scripting
{
   class ConsoleScript : SimScriptBase
   {
      public bool IsVisible { get; set; } = false;
      public int ScaledWidth
      {
         get { return (int) ((ConsoleBox != null) ? ConsoleBox.Size.Width : 0.0f); }
         set
         {
            ConsoleBox = new Container(ConsoleBox.Position, new SizeF(value, ConsoleBox.Size.Height), ConsoleBox.Color);
         }
      }
      public int ScaledHeight
      {
         get { return (int)((ConsoleBox != null) ? ConsoleBox.Size.Height : 0.0f); }
         set
         {
            ConsoleBox = new Container(ConsoleBox.Position, new SizeF(ConsoleBox.Size.Width, value), ConsoleBox.Color);
         }
      }
      public PointF Position
      {
         get { return (ConsoleBox != null) ? ConsoleBox.Position : new PointF(); }
         set
         {
            ConsoleBox = new Container(value, ConsoleBox.Size, ConsoleBox.Color);
         }
      }

      static readonly Color c_backgroundColor = Color.Black;
      static readonly Color c_foregroundColor = Color.White;

      const float c_fontScale = 0.18f;

      const float c_charHeight = 57.0f * c_fontScale;
      const float c_charWidth = 11.0f * c_fontScale;

      int maxLineCount { get { return (int)(ConsoleBox != null ? ConsoleBox.Size.Height / c_charHeight : 0); } }
      int maxCharCount { get { return (int)(ConsoleBox != null ? ConsoleBox.Size.Width / c_charWidth : 0); } }

      List<string> Lines = new List<string>();
      Container ConsoleBox;

      public override void Start()
      {
         base.Start();
         var width = 650;
         var Position = new PointF(Tools.ScreenWidth - width, 0);
         var Size = new SizeF(width, Tools.ScreenHeight);

         ConsoleBox = new Container(Tools.ScalePoint(Position), Tools.ScaleSize(Size), c_backgroundColor);
      }
      public void WriteLine(String line)
      {
         Application.GetScript<DebugLogger>().AddLine(line);
         Lines.AddRange(line.Replace("\t", "  ").Split('\n').Select(x => wrapString(x)));

         removeOldLines();
      }
      public void Write(String text)
      {
         var temp = text.Replace("\t", "  ").Split('\n').ToList();
         Lines[Lines.Count - 1] += temp[0];
         Lines[Lines.Count - 1] = wrapString(Lines[Lines.Count - 1]);
         temp.RemoveAt(0);

         Lines.AddRange(temp.Select(x => wrapString(x)));

         removeOldLines();
      }

      string wrapString(string text)
      {
         var textBox = new Text(text, new PointF(0, 0), c_fontScale, c_foregroundColor,
            Font.ChaletLondon, Alignment.Left, false, false, 1080);
         if (textBox.ScaledWidth <= ScaledWidth)
            return text;

         int length = text.Length - 1;
         while (textBox.ScaledWidth > ScaledWidth)
         {
            text = text.Substring(0, length) + "...";
            textBox = new Text(text, new Point(0, 0), c_fontScale, c_foregroundColor,
               Font.ChaletLondon, Alignment.Left, false, false, 1080);
            length--;
            if (length < 0)
               return "";
         }
         return text;
      }
      void removeOldLines()
      {
         while (Lines.Count > maxLineCount)
            Lines.RemoveAt(0);
      }

      protected override void OnAbort(object sender, EventArgs e)
      {
      }

      protected override void OnTick(object sender, EventArgs e)
      {
         if (!IsVisible)
            return;
         ConsoleBox.Items.Clear();

         for (int i = 0; i < Lines.Count(); i += 1)
         {
            var text = Lines[i];

            var textBox = new Text(text, new Point(0, i * (int)c_charHeight), c_fontScale, c_foregroundColor,
               Font.ChaletLondon, Alignment.Left, false, false, 1080);
            ConsoleBox.Items.Add(textBox);
         }
         ConsoleBox.ScaledDraw();
      }
   }
}
