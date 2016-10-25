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

using System.IO;
using System.Linq;

using OpenDriveSimulator.Scripting;

namespace OpenDriveSimulator
{
   class Scenario
   {
      public static Scenario CurrentScenario { get; set; } = null;
      public static Route CurrentRoute { get; set; } = null;

      public string Name { get; set; }
      public string WeatherCondition { get; set; }
      public string Time { get; set; }
      public string Route { get; set; }
      public string VehicleType { get; set; }

      public static Scenario Load(string name)
      {
         var path = Path.Combine(Application.RootFolder, "Scenarios", name + ".sce");
         if (!File.Exists(path))
            return new Scenario();

         var lines = File.ReadAllLines(path);

         var values = lines.Select(x => x.Split('=')).ToArray();

         return new Scenario()
         {
            Name = values[0][1],
            Route = values[1][1],
            WeatherCondition = values[2][1],
            Time = values[3][1],
            VehicleType = values[4][1],
         };
      }
      public void Save()
      {
         var path = Path.Combine(Application.RootFolder, "Scenarios", Name + ".sce");
         if (File.Exists(path))
            File.Delete(path);
         Application.Console.WriteLine("[Scenario.Save]: Writing file: " + path);
         File.WriteAllLines(path, new string[]{
            "Name=" + Name,
            "Route=" + Route,
            "Weather=" + WeatherCondition,
            "Daytime=" + Time,
            "Car=" + VehicleType,
         });
      }
      public void Delete()
      {
         var path = Path.Combine(Application.RootFolder, "Scenarios", Name + ".sce");
         if (File.Exists(path))
         {
            Application.Console.WriteLine("[Scenario.Save]: Deleting file: " + path);
            File.Delete(path);
         }
      }
      public void Start(string UID)
      {
         CurrentScenario = this;
         CurrentRoute = OpenDriveSimulator.Route.Load(this.Route);

         var scenarioScript = Application.GetScript<Scripting.ScenarioScript>();
         var LogfilePath = Path.Combine(Application.LoggingFolder, UID + ".log");
         Application.GetScript<LoggingScript>().LogfilePath = Path.Combine(Application.LoggingFolder, UID);
         scenarioScript.StartScenario(this);
      }
   }
}