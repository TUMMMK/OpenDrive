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
using System.IO;
using System.Linq;

using OpenDriveSimulator.Scripting;
using OpenDriveSimulator.UI.Pages;
using GTA;

namespace OpenDriveSimulator
{
   static class Application
   {
      #region Scripts
      public static List<SimScriptBase> Scripts = new List<SimScriptBase>();
      public static ConsoleScript Console
      {
         get { return GetScript<ConsoleScript>(); }
      }
      public static GUIScript GUI
      {
         get { return GetScript<GUIScript>(); }
      }
      public static Dispatcher Dispatcher
      {
         get { return GetScript<Dispatcher>(); }
      }
      public static ExecutionQueue ExecutionQueue
      {
         get { return GetScript<ExecutionQueue>(); }
      }

      public static T GetScript<T>() where T : SimScriptBase
      {
         return Scripts.OfType<T>()?.First();
      }
      #endregion

      #region Option Lists
      public static Dictionary<string, VehicleHash> VehicleTypes { get; } = new Dictionary<string, VehicleHash>()
      {
         { "Normal", VehicleHash.Blista2 },
         { "Fast", VehicleHash.Comet2 },
         { "Slow", VehicleHash.Minivan2 },
         { "Experimental", VehicleHash.Dump }
      };
      public static List<string> AvailableWeathers { get; } = Enum.GetNames(typeof(GTA.Weather)).ToList();
      public static List<string> AvailableDaytimes { get; } = new List<string>() { "Midday", "Sunrise", "Sunset", "Night" };
      public static List<string> AvailableCars { get; } = VehicleTypes.Keys.ToList();

      public static List<string> AvailableScenarios
      {
         get
         {
            var path = Path.Combine(RootFolder, "Scenarios");
            Directory.CreateDirectory(path);
            if (!Directory.Exists(path))
            {
               Directory.CreateDirectory(path);
               return new List<string>();
            }

            return Directory.GetFiles(path, "*.sce").Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
         }
      }
      public static List<string> AvailableRoutes
      {
         get
         {
            var path = Path.Combine(RootFolder, "Routes");
            if (!Directory.Exists(path))
            {
               Application.Console.WriteLine("[Application.Init]: Routes path: " + path + " does not exist. Creating new folder.");
               Directory.CreateDirectory(path);
               return new List<string>();
            }

            return Directory.GetFiles(path, "*.route").Select(x => Path.GetFileNameWithoutExtension(x)).ToList();
         }
      }
      #endregion

      #region Default Values
      public static readonly Color Highlight = Color.FromArgb(215, 215, 215);
      public static readonly Color ColorBackground = Color.FromArgb(32, 54, 68);
      public static readonly Color ColorText = Color.FromArgb(215, 215, 215);

      public static readonly GTA.Math.Vector3 DefaultCameraPosition = new GTA.Math.Vector3(364.6f, -1356.9f, 68.7f);
      public static readonly GTA.Math.Vector3 DefaultCameraDirection = new GTA.Math.Vector3(-0.4749f, 0.8674f, -0.1487f);
      public static readonly float DefaultCameraFOV = 90.0f;

      public static readonly string DefaultWeather = AvailableWeathers[0];
      public static readonly string DefaultCar = AvailableCars[0];
      public static readonly string DefaultDaytime = AvailableDaytimes[0];

      public static readonly GTA.Math.Vector3 DefaultSpawnPosition = new GTA.Math.Vector3(364.6f, -1356.9f, 68.7f);
      public static readonly GTA.Math.Vector3 DefaultSpawnDirection = new GTA.Math.Vector3(-0.4749f, 0.8674f, -0.1487f);

      #endregion

      #region Settings
      public static string RootFolder { get; private set; }
      public static string LoggingFolder { get; private set; }
      public static bool OtherCarsSetting { get; set; }
      public static bool PedestriansSetting { get; set; }
      public static bool TrafficLightSetting { get; set; }

      public static void LoadSettings()
      {
         string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
         UriBuilder uri = new UriBuilder(codeBase);
         string path = Uri.UnescapeDataString(uri.Path);
         Application.Console.WriteLine("[Application.Init]: Assembly path is: " + path);

         var lines = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(path), "OpenDriveSim", "settings.cfg"));
         var values = lines.Select(x => x.Split('=')).ToArray();

         RootFolder = values[0][1];
         Application.Console.WriteLine("[Application.Settings]: RootFolder set to: " + RootFolder);

         LoggingFolder = values[1][1];
         Application.Console.WriteLine("[Application.Settings]: LoggingFolder set to: " + LoggingFolder);

         Application.OtherCarsSetting = Convert.ToBoolean(values[2][1]);
         Application.PedestriansSetting = Convert.ToBoolean(values[3][1]);
         Application.TrafficLightSetting = Convert.ToBoolean(values[4][1]);
         DrawMarkerScript.MarkerType type;
         Enum.TryParse(values[5][1], out type); 
         GetScript<DrawMarkerScript>().CurrentMarkerType = type;

         GetScript<DrawMarkerScript>().PreviewMarker = Route.Load("SettingsDemo").Barriers[0];
      }
      internal static void SaveSettings()
      {
         string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
         UriBuilder uri = new UriBuilder(codeBase);
         string path = Uri.UnescapeDataString(uri.Path);

         List<string> lines = new List<string>();
         lines.Add("rootFolder=" + RootFolder);
         lines.Add("loggingFolder=" + LoggingFolder);
         lines.Add("otherCars=" + Application.OtherCarsSetting);
         lines.Add("pedestrians=" + Application.PedestriansSetting);
         lines.Add("trafficLights=" + Application.TrafficLightSetting);
         lines.Add("markerType=" + Enum.GetName(typeof(DrawMarkerScript.MarkerType), GetScript<DrawMarkerScript>().CurrentMarkerType));

         File.WriteAllLines(Path.Combine(Path.GetDirectoryName(path), "OpenDriveSim", "settings.cfg"), lines);
      }
      #endregion

      public static void Start()
      {
         Console.Start();
         try
         {
            LoadSettings();

            GetScript<DebugLogger>().Start();
            GetScript<ClearUIScript>().Start();
            GetScript<WorldControlScript>().Start();
            GetScript<GameMechanicRemoval>().Start();
            GetScript<DrawMarkerScript>().Start();
            GetScript<Dispatcher>().Start();
            ExecutionQueue.Start();

            Application.Console.WriteLine("[App.Start]: Initializing Camera");
            GetScript<CameraScript>().InitCamera();

            ExecutionQueue.AddAction(20, () =>
            {
               Application.Console.WriteLine("[App.Start delayed]: Starting Camera");
               Application.Console.WriteLine("[App.Start delayed]: Camera ID is " + World.RenderingCamera.NativeValue);
               GetScript<CameraScript>().Start();
               Application.Console.WriteLine("[App.Start delayed]: Starting GUI");
               StartGUI();
               Application.Console.WriteLine("[App.Start delayed]: Starting Main Menu");
               GUI.SetPage(MainMenu.Create());
               GUI.HideHelp();
            });

         }
         catch (System.Exception ex)
         {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            foreach (var key in ex.Data.Keys)
            {
               Console.WriteLine(key + ": " + ex.Data[key]);
            }
         }
      }
      static void StartGUI()
      {
         GUI.Start();
         GUI.Hide();
      }
   }
}
