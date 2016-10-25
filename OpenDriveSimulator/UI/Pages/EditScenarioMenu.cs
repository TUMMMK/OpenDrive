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
   internal class EditScenarioMenu
   {

      static MultipleChoice weatherSetting;
      static MultipleChoice daytimeSetting;
      static MultipleChoice carSetting;

      static ItemList availableRoutesList;
      static InputField nameInputField;

      public static Page Create(Scenario scenario)
      {
         Page result = new Page();

         #region Init
         if (scenario == null)
            scenario = new Scenario()
            {
               Name = "<enter name>",
               WeatherCondition = Application.DefaultWeather,
               Time = Application.DefaultDaytime,
               VehicleType = Application.DefaultCar,
            };
         result.Daytime = scenario.Time;
         result.Weather = scenario.WeatherCondition;

         var previewCamera = Route.GetCameraPreview(scenario.Route);
         result.CameraPosition = previewCamera[0];
         result.CameraDirection = previewCamera[1];

         #endregion

         result.AddElement(new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "EditScenarioTitle",
            Content = "Edit Scenario",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 56,
         });

         #region Name
         result.AddElement(new Label()
         {
            Name = "ScenarioNameLabel",
            Content = "Name:",
            TextColor = Application.ColorText,
            Shadow = true,
            Outline = true,
            FontSize = 34,
            ScaledWidth = 150,
            ScaledHeight = 75,
            ScaledX = 100,
            ScaledY = 250,
         });
         nameInputField = new InputField()
         {
            Name = "NameInput",
            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = 200,
            ScaledY = 250,
            Content = scenario != null ? scenario.Name : "<enter name>",
         };
         result.AddElement(nameInputField);
         #endregion

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
         availableRoutesList = new ItemList()
         {
            Name = "AvailRoutesList",

            ScaledX = 200,
            ScaledY = 400,
            ScaledWidth = 700,
            ScaledHeight = 7 * ItemList.C_ItemHeight + 10,
         };
         Application.AvailableRoutes.ForEach(x => availableRoutesList.AddItem(x));
         availableRoutesList.SelectItem(scenario.Route);
         availableRoutesList.Changed += (a, b) =>
         {
            previewCamera = Route.GetCameraPreview(b.Content);
            Tools.SetCamera(previewCamera[0], previewCamera[1], 90f);
         };
         result.AddElement(availableRoutesList);
         #endregion

         #region Settings
         result.AddElement(new Label()
         {
            ScaledX = Tools.ScreenWidth - 550,
            ScaledY = 340,
            ScaledWidth = 1500,
            Name = "SettingsLabel",
            Content = "Settings:",
            TextColor = Application.ColorText,
            Shadow = false,
            Outline = true,
            FontSize = 40,
         });

         weatherSetting = new MultipleChoice("Weather: ", Application.AvailableWeathers, 0.35f, scenario.WeatherCondition)
         {
            Name = "WeatherSetting",

            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = Tools.ScreenWidth - 900,
            ScaledY = 400
         };
         weatherSetting.Changed += (a, w) => Tools.SetWeather(w);     
         result.AddElement(weatherSetting);

         daytimeSetting = new MultipleChoice("Daytime: ", Application.AvailableDaytimes, 0.35f, scenario.Time)
         {
            Name = "DaytimeSetting",

            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = Tools.ScreenWidth - 900,
            ScaledY = 450
         };
         daytimeSetting.Changed += (a, w) => Tools.SetDaytime(w);
         result.AddElement(daytimeSetting);

         carSetting = new MultipleChoice("Car: ", Application.AvailableCars, 0.35f, scenario.VehicleType)
         {
            Name = "CarSetting",

            ScaledWidth = 700,
            ScaledHeight = 50,
            ScaledX = Tools.ScreenWidth - 900,
            ScaledY = 500
         };
         result.AddElement(carSetting);
         #endregion

         #region Control buttons
         var abortButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "EditScenarioAbortBut",
            Content = "Abort",
         };
         abortButton.Selected += (a, b) => Application.GUI.SetPage(ManageScenariosMenu.Create());
         result.AddElement(abortButton);

         var deleteButton = new Button()
         {
            ScaledX = Tools.ScreenWidth / 2 - 175,
            ScaledY = 950,

            ScaledWidth = 350,
            ScaledHeight = 50,

            Name = "EditScenarioDeleteBut",
            Content = "Delete scenario",

            TextColor = System.Drawing.Color.Red
         };
         deleteButton.Selected += (a, b) =>
         {
            scenario.Delete();
            Application.GUI.SetPage(ManageScenariosMenu.Create());
         };
         result.AddElement(deleteButton);

         var applyButton = new Button()
         {
            ScaledX = Tools.ScreenWidth - 250,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "EditScenarioApplyBut",
            Content = "OK",
         };
         applyButton.Selected += (a, b) =>
         {
            ApplyChanges(scenario);
            Application.GUI.SetPage(ManageScenariosMenu.Create());
         };
         result.AddElement(applyButton);
         #endregion

         return result;
      }

      private static void ApplyChanges(Scenario oldScenario)
      {

         Scenario scenario = new Scenario()
         {
            Name = nameInputField.Content,
            Route = availableRoutesList.SelectedItem,
            Time = daytimeSetting.SelectedItem,
            VehicleType = carSetting.SelectedItem,
            WeatherCondition = weatherSetting.SelectedItem,
         };
         if(oldScenario.Name != "<enter name>")
         {
            Application.Console.WriteLine("[EditScenarioMenu.Apply]: Deleting old scenario " + oldScenario?.Name);
            oldScenario.Delete();
         }
         Application.Console.WriteLine("[EditScenarioMenu.Apply]: Saving new scenario " + scenario.Name);
         scenario.Save();
      }
   }
}