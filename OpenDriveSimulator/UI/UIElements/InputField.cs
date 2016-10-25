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
using System.Windows.Forms;

using GTA.UI;

namespace OpenDriveSimulator.UI
{
   class InputField : SelectableBase
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
      public Color Background
      {
         get { return Color; }
         set { Color = value; }
      }
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

      const float c_shadowWidth = 4.0f;

      Color m_textColor;
      float m_fontScale;
      bool m_textboxUpdateNeeded = true;
      string m_originalText;
      Text m_textBox;

      public InputField()
      {
         FontSize = 34;
         Background = Application.ColorBackground;
         TextColor = Application.ColorText;
      }

      public override void ScaledDraw()
      {
         ScaledDraw(new SizeF(0, 0));
      }

      public override void ScaledDraw(SizeF offset)
      {
         if(m_textboxUpdateNeeded)
         {
            m_textboxUpdateNeeded = false;
            Items.Remove(m_textBox);
            m_textBox = createTextbox(Content);
            Items.Add(m_textBox);
         }

         GTA.UI.Rectangle shadow = new GTA.UI.Rectangle()
         {
            Position = Tools.ScalePoint(new PointF(ScaledX - c_shadowWidth / 2, ScaledY - c_shadowWidth / 2)),
            Size = Tools.ScaleSize(new SizeF(ScaledWidth + c_shadowWidth, ScaledHeight + c_shadowWidth)),
            Color = Color.Black,
         };
         if (IsVisible)
            shadow.ScaledDraw(offset);
         base.ScaledDraw(offset);
      }

      private Text createTextbox(string text)
      {
         int length = text.Length - 1;
         while (Text.GetStringWidth(text, GTA.UI.Font.ChaletLondon, m_fontScale)  > Size.Width)
         {
            text = text.Substring(0, --length) + "...";
         }

         var textBox = new Text(text, new PointF(Size.Width / 2, 0), m_fontScale, m_textColor,
               GTA.UI.Font.ChaletLondon, Alignment.Center);

         return textBox;
      }

      public override void SendKey(Keys KeyCode, bool ShiftPressed)
      {
         if (KeyCode == Keys.Back && Content.Length > 0)
            Content = Content.Substring(0, Content.Length - 1);
         else
         {
            char letter = KeyMapping.GetChar(KeyCode, ShiftPressed);
            if (letter != '\0')
               Content += letter;
         }
      }

 
   }
}
