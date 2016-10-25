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
using System.Globalization;

using GTA;

namespace OpenDriveSimulator.Scripting
{
   class ScenarioScript : SimScriptBase
   {
      public Scenario CurrentScenario { get; private set; }
      public Route CurrentRoute { get; private set; }

      DateTime startTime;

      public void StartScenario(Scenario scenario)
      {
         CurrentScenario = scenario;
         CurrentRoute = Route.Load(CurrentScenario.Route);

         Application.GetScript<WorldControlScript>().CurrentWeather = CurrentScenario.WeatherCondition;
         Application.GetScript<WorldControlScript>().CurrentDaytime = CurrentScenario.Time;

         Application.GetScript<DrawMarkerScript>().Markers = CurrentRoute.Barriers;
         Application.GetScript<DrawMarkerScript>().Start();

         Ped playerPed = Game.PlayerPed;
         if(playerPed.CurrentVehicle != null)
            playerPed.CurrentVehicle.Delete();

         playerPed.Position = CurrentRoute.SpawnPosition;
         playerPed.Rotation = CurrentRoute.SpawnRotation;

         var veh = World.CreateVehicle(Application.VehicleTypes[scenario.VehicleType], playerPed.Position, playerPed.Heading);
         veh.Rotation = CurrentRoute.SpawnRotation;
         veh.PlaceOnGround();
         playerPed.SetIntoVehicle(veh, VehicleSeat.Driver);

         var Camera = Application.GetScript<CameraScript>();
         Camera.CarToFollow = veh;
         Camera.SetMode(CameraScript.CameraMode.Vehicle);
         Application.GUI.Hide();

         startTime = DateTime.Now;
         Start();
         Application.GetScript<LoggingScript>().Start();

      }

      protected override void OnAbort(object sender, EventArgs e)
      {
      }
      protected override void OnTick(object sender, EventArgs e)
      {
         if (!Application.OtherCarsSetting)
         {
            foreach (var veh in World.GetAllVehicles())
            {
               if (veh != Game.PlayerPed.CurrentVehicle)

                  veh.Delete();
            }
         }
         if (!Application.PedestriansSetting)
         {
            foreach (var ped in World.GetAllPeds())
            {
               ped.Delete();
            }
         }
         var egoVeh = Game.PlayerPed.CurrentVehicle;

         var timestamp = (long)(DateTime.Now - startTime).TotalMilliseconds;
         var pos = egoVeh.Position;
         var heading = egoVeh.Heading;
         var speed = egoVeh.Speed;
         var acc = egoVeh.Acceleration;

         string line = timestamp + "," +
            Tools.ConvertToString(pos) + "," +
            ((heading / Math.PI) * 180.0f).ToString(CultureInfo.InvariantCulture) + "," +
            speed.ToString(CultureInfo.InvariantCulture) + "," +
            acc.ToString(CultureInfo.InvariantCulture);
         Application.GetScript<LoggingScript>().AddLine(line);
      }
   }
}
