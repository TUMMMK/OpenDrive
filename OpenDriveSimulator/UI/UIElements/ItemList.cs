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
using System.Windows.Forms;

namespace OpenDriveSimulator.UI
{
   class ItemList : SelectableBase
   {
      public List<string> ListItems { get; private set; } = new List<string>();
      List<Button> ItemVisuals = new List<Button>();

      public event EventHandler<Button> Changed;

      public const int C_ItemHeight = 54;

      int m_selectedIndex = 0;
      int m_startIndex = 0;
      int m_visibleItemsCount { get { return (int)System.Math.Floor(this.ScaledHeight / C_ItemHeight); } }

      int c_fontSize = 34;
      int c_borderSize = 2;

      public Color Background { get; set; }
      public Color TextColor
      {
         get { return this.Color; }
         set { Color = value; }
      }

      public string SelectedItem { get { return ListItems[m_selectedIndex]; } }

      public ItemList()
      {
         Background = Application.ColorBackground;
         TextColor = Application.ColorText;
      }

      public void AddItem(string Content)
      {
         ListItems.Add(Content);
         Button item = new Button()
         {
            ScaledWidth = this.ScaledWidth - 2 * c_borderSize,
            ScaledHeight = C_ItemHeight - 2 * c_borderSize,
            Background = Background,
            TextColor = TextColor,
            FontSize = c_fontSize,
            Content = Content
         };

         item.Selected += (s,e) => fireSelectEvent(s);
         ItemVisuals.Add(item);
         this.Items.Add(item);
         updateVisuals();
      }
      public void RemoveItem(string item)
      {
         int index;
         if ((index = ListItems.IndexOf(item)) != -1)
         {
            Application.Console.WriteLine("[ItemList.Remove]: item " + item + " found on index " + index + " of " + (ListItems.Count - 1));
            Application.Console.WriteLine("[ItemList.Remove]: removing index " + index + " from ItemVisuals (count: " + (ItemVisuals.Count - 1) + ")");
            ListItems.RemoveAt(index);
            Items.Remove(ItemVisuals[index]);
            ItemVisuals.RemoveAt(index);
         }
         if (m_selectedIndex == index)
         {
            m_selectedIndex = 0;
         }

         updateVisuals();
      }
      public bool Exists(string item)
      {
         return ListItems.Contains(item);
      }

      public void SelectItem(string item)
      {
         int index = ListItems.IndexOf(item);
         if (index != -1)
         {
            m_selectedIndex = index;
            updateVisuals();
         }
      }
      public void SelectItem(int index)
      {
         if (index >= 0 && index < ListItems.Count)
         {
            m_startIndex = index;
            updateVisuals();
         }
      }
      public void ClearAllItems()
      {
         ListItems.Clear();
         ItemVisuals.Clear();
         m_selectedIndex = 0;
         m_startIndex = 0;

         updateVisuals();
      }

      public override void SendKey(Keys KeyCode, bool ShiftPressed)
      {
         if (KeyCode == Keys.Return)
            ItemVisuals.Where(x => x.IsActive).First().SendKey(KeyCode, ShiftPressed);


         if (KeyCode == Keys.Down && m_selectedIndex < ListItems.Count - 1)
         {
            m_selectedIndex++;
            if(m_selectedIndex >= m_visibleItemsCount)
               m_startIndex = m_selectedIndex - (m_visibleItemsCount - 1);
            if(Changed != null)
              Changed(this, ItemVisuals[m_selectedIndex]);
            updateVisuals();
         }
         if (KeyCode == Keys.Up && m_selectedIndex > 0)
         {
            m_selectedIndex--;
            if (m_selectedIndex < m_startIndex)
               m_startIndex = m_selectedIndex;
            if (Changed != null)
               Changed(this, ItemVisuals[m_selectedIndex]);
            updateVisuals();
         }


      }

      private void updateVisuals()
      {
         if (ItemVisuals.Count == 0)
            return;
         ItemVisuals.ForEach(x => { x.Hide(); x.IsActive = false; });
         for (int i = 0; i < m_visibleItemsCount; i++ )
         {
            int index = m_startIndex + i;
            if (index >= ItemVisuals.Count)
               break;

            ItemVisuals[index].ScaledX = c_borderSize;
            ItemVisuals[index].ScaledY = c_borderSize + i * C_ItemHeight;
            ItemVisuals[index].Show();
         }
         ItemVisuals[m_selectedIndex].IsActive = true;
      }
   }
}
