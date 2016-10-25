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
using System.Windows.Forms;

namespace OpenDriveSimulator.UI
{
   class MultipleChoice : SelectableBase
   {
      public event EventHandler<string> Changed;

      public new float ScaledWidth
      {
         get { return base.ScaledWidth; }
         set
         {
            var actualWidth = value - 4;
            base.ScaledWidth = value;
            VariableName_TB.ScaledWidth = m_ratio * actualWidth;
            Choices_TB.ScaledWidth = (1.0f - m_ratio) * actualWidth;
            Choices_TB.ScaledX = VariableName_TB.ScaledWidth + 2;
         }
      }
      public new float ScaledHeight
      {
         get { return base.ScaledHeight; }
         set
         {
            var actualHeight = value - 4;

            base.ScaledHeight = value;
            VariableName_TB.ScaledHeight = actualHeight;
            Choices_TB.ScaledHeight = actualHeight;
         }
      }

      TextBox VariableName_TB = new TextBox()
      {
         ScaledX = 2,
         ScaledY = 2
      };
      TextBox Choices_TB = new TextBox()
      {
         ScaledY = 2
      };

      float m_ratio;
      
      public string SelectedItem { get { return m_Choices[m_selectedIndex]; } }

      List<string> m_Choices = new List<string>();
      int m_selectedIndex = 0;

      public void SelectItem(string item)
      {
         if (m_Choices.Contains(item))
         {
            m_selectedIndex = m_Choices.IndexOf(item);
         }
      }

      public override void SendKey(Keys KeyCode, bool ShiftPressed)
      {
         if (KeyCode == Keys.Right && m_selectedIndex < m_Choices.Count - 1)
         {
            m_selectedIndex++;
            Choices_TB.Content = m_Choices[m_selectedIndex];
            if (Changed != null)
               Changed(this, m_Choices[m_selectedIndex]);
         }
         if (KeyCode == Keys.Left && m_selectedIndex > 0)
         {
            m_selectedIndex--;
            Choices_TB.Content = m_Choices[m_selectedIndex];
            if (Changed != null)
               Changed(this, m_Choices[m_selectedIndex]);
         }

      }
      public MultipleChoice(string VariableName, List<string> Choices, float Ratio = 0.5f, string initialSelection = "")
      {
         m_selectedIndex = 0;
         int index = Choices.IndexOf(initialSelection);
         if (index != -1)
            m_selectedIndex = index;


         VariableName_TB.Content = VariableName;
         m_Choices = Choices;
         Choices_TB.Content = m_Choices[m_selectedIndex];
         m_ratio = Ratio;

         Items.Add(VariableName_TB);
         Items.Add(Choices_TB);
      }
   }
}
