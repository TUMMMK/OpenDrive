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
using System.Linq;
using System.Windows.Forms;

using OpenDriveSimulator.UI;
using OpenDriveSimulator.UI.Pages;

namespace OpenDriveSimulator.Scripting
{
   class GUIScript : SimScriptBase
   {
      public List<UIElementBase> Elements { get { return m_UIElements; } }
      public bool IsVisible { get; private set; } = true;

      List<UIElementBase> m_UIElements = new List<UIElementBase>();
      List<SelectableBase> m_Selectibles { get { return m_UIElements.OfType<SelectableBase>().Where(x => x.IsVisible).ToList(); } }

      SelectableBase m_activeElement
      {
         get
         {
            return m_activeElementHelper;
         }
         set
         {
            if (m_activeElementHelper != null)
               m_activeElementHelper.IsActive = false;
            m_activeElementHelper = value;

            if (m_activeElementHelper != null)
               m_activeElementHelper.IsActive = true;
         }
      }
      SelectableBase m_activeElementHelper;
      int m_activeElementIndex = 0;

      bool m_showHelp = false;
      string m_helpText = "";

      public void SetPage(Page content)
      {
         m_UIElements = content.GetElements();
         m_activeElement = null;
         if (m_Selectibles.Count > 0)
            m_activeElement = m_Selectibles[0];

         Tools.SetCamera(content.CameraPosition, content.CameraDirection, content.CameraFOV);
         Tools.SetWeather(content.Weather);
         Tools.SetDaytime(content.Daytime);
      }
      public void Show()
      {
         IsVisible = true;
      }
      public void Hide()
      {
         IsVisible = false;
      }

      public void SetActiveElement(string name)
      {
         var element = m_Selectibles.Where(x => x.Name == name);
         if (element.Count() != 0)
         {
            m_activeElement = element.First();
            m_activeElementIndex = m_Selectibles.IndexOf(m_activeElement);
         }
      }
      public void KeyPressed(Keys KeyCode, bool ShiftPressed)
      {
         if (m_UIElements.Count == 0)
            return;

         if (KeyCode == Keys.Tab)
         {
            if (ShiftPressed)
            {
               m_activeElementIndex--;
               if (m_activeElementIndex < 0)
                  m_activeElementIndex += m_Selectibles.Count;
            }
            else
            {
               m_activeElementIndex++;
               if (m_activeElementIndex >= m_Selectibles.Count)
                  m_activeElementIndex %= m_Selectibles.Count;
            }
            m_activeElement = m_Selectibles[m_activeElementIndex];
         }
         else
         {
            m_activeElement.SendKey(KeyCode, ShiftPressed);
         }
      }
      
      public void ShowHelp()
      {
         m_showHelp = true;
      }
      public void HideHelp()
      {
         m_showHelp = false;
      }
      public void ToggleHelp()
      {
         m_showHelp = !m_showHelp;
      }
      public void SetHelpText(string text)
      {
         m_helpText = text;
      }

      protected override void OnAbort(object sender, EventArgs e)
      {
      }
      protected override void OnTick(object sender, EventArgs e)
      {
         if (!IsVisible)
            return;
         foreach(var element in m_UIElements)
         {
            element.ScaledDraw();
         }
         if (m_showHelp)
            GTA.UI.Screen.DisplayHelpTextThisFrame(m_helpText);
      }
   }
}
