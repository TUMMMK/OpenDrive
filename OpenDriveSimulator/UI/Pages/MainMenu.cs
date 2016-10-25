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

using System.Collections.Generic;

namespace OpenDriveSimulator.UI.Pages
{
   static class MainMenu
   {
      public static Page Create()
      {
         Page result = new Page();

         result.AddElement(new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "MainMenuTitle",
            Content = "Welcome to the OpenDrive Simulator!",
            Shadow = true,
            Outline = true,
            FontSize = 66,
         });

         var ListItems = new List<string>() {
            "Start simulation",
            "Manage scenarios",
            "Manage routes",
            "Settings"
         };

         ItemList list = new ItemList()
         {
            Name = "MainMenuList",
            ScaledX = (Tools.ScreenWidth - 400) / 2,
            ScaledY = 760,
            ScaledWidth = 400,
            ScaledHeight = ListItems.Count * ItemList.C_ItemHeight,
         };
         ListItems.ForEach(x => list.AddItem(x));

         list.Selected += (s, e) =>
         {
            switch ((s as Button).Content)
            {
               case "Start simulation":
                  Application.GUI.SetPage(SelectScenariosMenu.Create());
                  break;
               case "Manage scenarios":
                  Application.GUI.SetPage(ManageScenariosMenu.Create());
                  break;
               case "Manage routes":
                  Application.Console.WriteLine("Selected Manage routes");
                  Application.GUI.SetPage(ManageRoutesMenu.Create());
                  break;
               case "Settings":
                  Application.GUI.SetPage(SettingsMenu.Create());
                  break;
            }
         };

         result.AddElement(list);
         return result;
      }
   }
}
