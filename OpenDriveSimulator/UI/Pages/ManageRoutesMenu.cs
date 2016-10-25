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

using GTA.Math;

namespace OpenDriveSimulator.UI.Pages
{
   internal class ManageRoutesMenu
   {
      static Route selectedRoute = Application.AvailableRoutes.Count != 0 ? Route.Load(Application.AvailableScenarios[0]) : null;
      static ItemList availableRoutesList;
      static Vector3[] previewCamera = new Vector3[2];

      internal static Page Create()
      {
         Page result = new Page();

         result.AddElement(new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "EditScenarioTitle",
            Content = "Manage Routes",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 56,
         });

         #region Routes
         result.AddElement(new Label()
         {
            ScaledX = 550,
            ScaledY = 340,
            ScaledWidth = 1500,
            Name = "AvailableRoutesLabel",
            Content = "Route:",
            TextColor = Application.ColorText,
            Shadow = false,
            Outline = true,
            FontSize = 40,
         });

         Application.Console.WriteLine("[ManageRoutesMenu]: Creating route List");
         availableRoutesList = new ItemList()
         {
            Name = "AvailRoutesList",

            ScaledX = 200,
            ScaledY = 400,
            ScaledWidth = 700,
            ScaledHeight = 7 * ItemList.C_ItemHeight + 10,
         };
         Application.AvailableRoutes.ForEach(x => availableRoutesList.AddItem(x));
         availableRoutesList.AddItem("<create new>");
         availableRoutesList.Changed += LoadRoute;
         availableRoutesList.Selected += (a, b) => Application.GUI.SetPage(EditRouteMenu.Create(selectedRoute));

         Application.Console.WriteLine("[ManageRoutesMenu]: Filling route List");


         result.AddElement(availableRoutesList);
         #endregion

         #region Control buttons
         var backButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "EditScenarioBackBut",
            Content = "<- Back",
         };
         backButton.Selected += (a, b) => Application.GUI.SetPage(MainMenu.Create());
         result.AddElement(backButton);
         #endregion

         return result;
      }

      private static void LoadRoute(object sender, Button e)
      {
         if (e.Content != "<create new>")
         {
            selectedRoute = Route.Load(e.Content);
            previewCamera = Route.GetCameraPreview(e.Content);
         }
         else
         {
            selectedRoute = null;
            previewCamera[0] = Application.DefaultCameraPosition;
            previewCamera[1] = Application.DefaultCameraDirection;
         }
         Tools.SetCamera(previewCamera[0], previewCamera[1], 90f);
      }
   }
}