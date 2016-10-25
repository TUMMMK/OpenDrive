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

namespace OpenDriveSimulator.UI.Pages
{
   internal class ManageScenariosMenu
   {
      static TextBox WeatherDiscription = new TextBox()
      {
         ScaledWidth = 700,
         ScaledHeight = 50,
         ScaledX = Tools.ScreenWidth - 900,
         ScaledY = 400
      };
      static TextBox TimeDiscription = new TextBox()
      {
         ScaledWidth = 700,
         ScaledHeight = 50,
         ScaledX = Tools.ScreenWidth - 900,
         ScaledY = 450
      };
      static TextBox CarDiscription = new TextBox()
      {
         ScaledWidth = 700,
         ScaledHeight = 50,
         ScaledX = Tools.ScreenWidth - 900,
         ScaledY = 500
      };
      static TextBox RouteDiscription = new TextBox()
      {
         ScaledWidth = 700,
         ScaledHeight = 50,
         ScaledX = Tools.ScreenWidth - 900,
         ScaledY = 550
      };

      static Scenario selectedScenario = Application.AvailableScenarios.Count != 0 ? Scenario.Load(Application.AvailableScenarios[0]) : null;

      internal static Page Create()
      {
         Page result = new Page();

         result.AddElement(new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "ManageScenarioTitle",
            Content = "Manage Scenarios",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 56,
         });


         result.AddElement(new Label()
         {
            ScaledX = 550,
            ScaledY = 340,
            ScaledWidth = 1500,
            Name = "AvailableScenariosLabel",
            Content = "Available scenarios:",
            TextColor = Application.ColorText,
            Shadow = false,
            Outline = true,
            FontSize = 40,
         });

         ItemList availableScenariosList = new ItemList()
         {
            TextColor = Application.ColorText,
            Background = Application.ColorBackground,
            Name = "AvailScenariosList",
            ScaledX = 200,
            ScaledY = 400,
            ScaledWidth = 700,
            ScaledHeight = 7 * ItemList.C_ItemHeight + 10,
         };
         if (Application.AvailableScenarios.Count != 0)
            Application.AvailableScenarios.ForEach(x => availableScenariosList.AddItem(x));
         availableScenariosList.AddItem("<create new>");
         availableScenariosList.Changed += LoadScenario;
         availableScenariosList.Selected += (a, b) => Application.GUI.SetPage(EditScenarioMenu.Create(selectedScenario));
         result.AddElement(availableScenariosList);

         result.AddElement(CarDiscription);
         result.AddElement(RouteDiscription);
         result.AddElement(TimeDiscription);
         result.AddElement(WeatherDiscription);

         var backButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "ManageScenariosBackBut",
            Content = "<- Back",
         };
         backButton.Selected += (a, b) => Application.GUI.SetPage(MainMenu.Create());
         result.AddElement(backButton);


         updateDiscription();

         return result;
      }

      private static void LoadScenario(object sender, Button e)
      {
         if (e.Content == "<create new>")
            selectedScenario = null;
         else
            selectedScenario = Scenario.Load(e.Content);
         updateDiscription();
      }

      private static void updateDiscription()
      {
         if (selectedScenario == null)
         {
            CarDiscription.Content = "Car: ---";
            WeatherDiscription.Content = "Weather: ---";
            TimeDiscription.Content = "Daytime: ---";
            RouteDiscription.Content = "Route: ---";
         }
         else
         {
            CarDiscription.Content = "Car: " + selectedScenario.VehicleType;
            WeatherDiscription.Content = "Weather: " + selectedScenario.WeatherCondition;
            TimeDiscription.Content = "Daytime: " + selectedScenario.Time;
            RouteDiscription.Content = "Route: " + selectedScenario.Route;
         }
      }
   }
}