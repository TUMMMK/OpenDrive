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

using System.Drawing;

using GTA.UI;

namespace OpenDriveSimulator.UI
{
   class Label : UIElementBase
   {
      public string Content
      {
         get
         {
            return m_originalText;
         }
         set
         {
            m_originalText = value;
            m_textboxUpdateNeeded = true;
         }
      }
      public int FontSize
      {
         get
         {
            return (int)System.Math.Round(m_fontScale * 58.0f);
         }
         set
         {
            m_fontScale = value / 58.0f;
            m_textboxUpdateNeeded = true;
         }
      }
      new Color Color { get; } = Color.Transparent;

      public Color TextColor
      {
         get
         {
            return m_textColor;
         }
         set
         {
            m_textColor = value;
            m_textboxUpdateNeeded = true;
         }
      }
      public bool Shadow
      {
         get
         {
            return m_shadow;
         }
         set
         {
            m_shadow = value;
            m_textboxUpdateNeeded = true;
         }
      }
      public bool Outline
      {
         get
         {
            return m_outline;
         }
         set
         {
            m_outline = value;
            m_textboxUpdateNeeded = true;
         }
      }

      Color m_textColor;
      float m_fontScale;
      bool m_shadow = false;
      bool m_outline = false;
      bool m_textboxUpdateNeeded = true;

      string m_originalText;
      Text m_textBox;

      public Label()
      {
         FontSize = 34;
         TextColor = Application.ColorText;
      }

      public override void ScaledDraw()
      {
         ScaledDraw(new SizeF(0, 0));
      }
      public override void ScaledDraw(SizeF offset)
      {
         if (m_textboxUpdateNeeded)
         {
            m_textboxUpdateNeeded = false;
            Items.Remove(m_textBox);
            m_textBox = createTextbox(Content);
            Items.Add(m_textBox);
         }
         base.ScaledDraw(offset);
      }

      private Text createTextbox(string text)
      {

         int length = text.Length - 1;
         while (Text.GetStringWidth(text, GTA.UI.Font.ChaletLondon, m_fontScale) > Size.Width)
         {
            text = text.Substring(0, --length) + "...";
         }

         var textBox = new Text(text, new PointF(0, 0), m_fontScale, m_textColor,
               GTA.UI.Font.ChaletLondon, Alignment.Center, Shadow, Outline);

         return textBox;
      }
   }
}