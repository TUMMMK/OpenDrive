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
using System.Drawing;
using System.Globalization;
using System.Linq;

using GTA;
using GTA.Math;

using OpenDriveSimulator.Scripting;

namespace OpenDriveSimulator
{
   static class Tools
   {
      public static float ScaleFactor { get { return GTA.UI.Screen.ScaledWidth / GTA.UI.Screen.Resolution.Width; } }
      public static float ScreenWidth { get { return GTA.UI.Screen.Resolution != null ? GTA.UI.Screen.Resolution.Width : 1900.0f; } }
      public static float ScreenHeight { get { return GTA.UI.Screen.Resolution != null ? GTA.UI.Screen.Resolution.Height : 1050.0f; } }

      public static PointF ScalePoint(PointF original)
      {
         return new PointF(original.X * ScaleFactor, original.Y * ScaleFactor);
      }
      public static SizeF ScaleSize(SizeF original)
      {
         return new SizeF(original.Width * ScaleFactor, original.Height * ScaleFactor);
      }

      public static Vector3 ConvertToVector3(string s)
      {
         var splittet = s.Split(',');
         var x = (float)Convert.ToDouble(splittet[0], CultureInfo.InvariantCulture);
         var y = (float)Convert.ToDouble(splittet[1], CultureInfo.InvariantCulture);
         var z = (float)Convert.ToDouble(splittet[2], CultureInfo.InvariantCulture);
         return new GTA.Math.Vector3(x, y, z);
      }
      public static Vector2 ConvertToVector2(string s)
      {
         var splittet = s.Split(',');
         var x = (float)Convert.ToDouble(splittet[0], CultureInfo.InvariantCulture);
         var y = (float)Convert.ToDouble(splittet[1], CultureInfo.InvariantCulture);
         return new GTA.Math.Vector2(x, y);
      }
      public static Vector3 ConvertToVector3(Vector2 vec)
      {
         return new Vector3(vec.X, vec.Y, World.GetGroundHeight(vec));
      }

      internal static void SetCamera(GTA.Math.Vector3 Position, GTA.Math.Vector3 Direction, float FOV)
      {
         Direction.Normalize();

         Application.Console.WriteLine("[Tools.SetCamera]: setting camera to (" + Position.X + " | " + Position.Y + " | " + Position.Z +
            ") (" + Direction.X + " | " + Direction.Y + " | " + Direction.Z + ")");

         var cameraScript = Application.Scripts.OfType<CameraScript>().First();
         cameraScript.ControlledPosition = Position;
         cameraScript.ControlledDirection = Direction;
         cameraScript.ControlledFOV = FOV;

         Application.Console.WriteLine("set camera to position (" + World.RenderingCamera.Position.X + " | " + World.RenderingCamera.Position.Y + " | " + World.RenderingCamera.Position.Z +
            ") (" + World.RenderingCamera.Direction.X + " | " + World.RenderingCamera.Direction.Y + " | " + World.RenderingCamera.Direction.Z + ")");
         Application.Console.WriteLine("set player to position (" + Game.PlayerPed.Position.X + " | " + Game.PlayerPed.Position.Y + " | " + Game.PlayerPed.Position.Z +
            ") (" + Game.PlayerPed.Rotation.X + " | " + Game.PlayerPed.Rotation.Y + " | " + Game.PlayerPed.Rotation.Z + ")");
      }
      public static void SetWeather(string weather)
      {
         Application.Scripts.OfType<WorldControlScript>().First().CurrentWeather = weather;
      }
      public static void SetDaytime(string daytime)
      {
         Application.Scripts.OfType<WorldControlScript>().First().CurrentDaytime = daytime;
         Application.Console.WriteLine("[Tools.SetTime]: Set time to " + daytime);
      }

      public static string ConvertToString(this GTA.Math.Vector2 vec)
      {
         return vec.X.ToString(CultureInfo.InvariantCulture) + "," + vec.Y.ToString(CultureInfo.InvariantCulture);
      }
      public static string ConvertToString(this GTA.Math.Vector3 vec)
      {
         return vec.X.ToString(CultureInfo.InvariantCulture) + "," + vec.Y.ToString(CultureInfo.InvariantCulture) + "," + vec.Z.ToString(CultureInfo.InvariantCulture);
      }

   }
}