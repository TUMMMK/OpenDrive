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

using OpenDriveSimulator.Scripting;

namespace OpenDriveSimulator.UI.Pages
{
   class SettingsMenu
   {
      public static Page Create()
      {
         Page result = new Page();

         result.AddElement(new Label()
         {
            ScaledX = 950,
            ScaledY = 100,
            ScaledWidth = 1500,
            Name = "SettingsTitle",
            Content = "Simulation settings",
            Shadow = true,
            Outline = true,
            FontSize = 56,
         });

         var otherCarsSettings = new MultipleChoice("Other cars: ", new List<string>() { "True", "False" }, 0.65f, Application.OtherCarsSetting.ToString())
         {
            Name = "otherCarsSetting",

            ScaledX = (Tools.ScreenWidth/2) - 360,
            ScaledY = 400,

            ScaledWidth = 760,
            ScaledHeight = 50,
         };
         otherCarsSettings.SelectItem(Application.OtherCarsSetting.ToString());
         result.AddElement(otherCarsSettings);

         var pedestriansSettings = new MultipleChoice("Pedestrians: ", new List<string>() { "True", "False" }, 0.65f, Application.PedestriansSetting.ToString())
         {
            Name = "pedestriansSetting",

            ScaledX = (Tools.ScreenWidth/2) - 360,
            ScaledY = 450,

            ScaledWidth = 760,
            ScaledHeight = 50,
         };
         pedestriansSettings.SelectItem(Application.PedestriansSetting.ToString());
         result.AddElement(pedestriansSettings);

         var trafficLightSettings = new MultipleChoice("Traffic lights: ", new List<string>() { "True", "False" }, 0.65f, Application.TrafficLightSetting.ToString())
         {
            Name = "trafficLightsSetting",

            ScaledX = (Tools.ScreenWidth/2) - 360,
            ScaledY = 500,

            ScaledWidth = 760,
            ScaledHeight = 50,
         };
         trafficLightSettings.SelectItem(Application.TrafficLightSetting.ToString());
         result.AddElement(trafficLightSettings);

         var barrierTypeSettings = new MultipleChoice("Barrier type: ", new List<string>(Enum.GetNames(typeof(DrawMarkerScript.MarkerType))), 0.65f,
            Enum.GetName(typeof(DrawMarkerScript.MarkerType), Application.GetScript<DrawMarkerScript>().CurrentMarkerType))
         {
            Name = "barrierTypeSettings",

            ScaledX = (Tools.ScreenWidth / 2) - 360,
            ScaledY = 550,

            ScaledWidth = 760,
            ScaledHeight = 50,
         };
         barrierTypeSettings.Changed += (a, b) =>
         {
            DrawMarkerScript.MarkerType type;
            Enum.TryParse(b, out type);
            Application.Console.WriteLine("[SettingsMenu]: Setting markertype to " + Enum.GetName(typeof(DrawMarkerScript.MarkerType), type));
            Application.GetScript<DrawMarkerScript>().CurrentMarkerType = type;
         };
         result.AddElement(barrierTypeSettings);

         #region Control buttons
         var abortButton = new Button()
         {
            ScaledX = 50,
            ScaledY = 950,

            ScaledWidth = 200,
            ScaledHeight = 50,

            Name = "SettingsAbortBut",
            Content = "Abort",
         };
         abortButton.Selected += (a, b) => Application.GUI.SetPage(MainMenu.Create());
         result.AddElement(abortButton);

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
            Application.OtherCarsSetting = Convert.ToBoolean(otherCarsSettings.SelectedItem);
            Application.PedestriansSetting = Convert.ToBoolean(pedestriansSettings.SelectedItem);
            Application.TrafficLightSetting = Convert.ToBoolean(trafficLightSettings.SelectedItem);
            Application.SaveSettings();
            Application.GUI.SetPage(MainMenu.Create());
         };
         result.AddElement(applyButton);
         #endregion

         return result;
      }
   }
}