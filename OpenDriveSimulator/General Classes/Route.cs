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
using System.IO;

using GTA.Math;

namespace OpenDriveSimulator
{
   class Route
   {
      public Vector3 PreviewCameraPosition { get; set; } = Application.DefaultCameraPosition;
      public Vector3 PreviewCameraDirection { get; set; } = Application.DefaultCameraDirection;

      public Vector3 SpawnPosition { get; set; } = Application.DefaultSpawnPosition;
      public Vector3 SpawnRotation { get; set; } = Application.DefaultSpawnDirection;

      public string Name { get; set; }
      public List<Barrier> Barriers { get; set; } = new List<Barrier>();

      public static Vector3[] GetCameraPreview(string name)
      {
         var path = Path.Combine(Application.RootFolder, "Routes", name + ".route");
         if (!File.Exists(path))
            return new Vector3[] { Application.DefaultCameraPosition, Application.DefaultCameraDirection };

         var lines = File.ReadAllLines(path);

         return new Vector3[2] { Tools.ConvertToVector3(lines[0]), Tools.ConvertToVector3(lines[1]) };
      }
      public static Route Load(string name)
      {
         Route result = new Route()
         {
            Name = name,
         };

         var path = Path.Combine(Application.RootFolder, "Routes", name + ".route");
         if (!File.Exists(path))
         {
            Application.Console.WriteLine("[Route.Load]: Path " + path + " does not exist"); 
            return result;
         }

         var lines = File.ReadAllLines(path);

         result.PreviewCameraPosition = Tools.ConvertToVector3(lines[0]);
         result.PreviewCameraDirection = Tools.ConvertToVector3(lines[1]);

         Application.Console.WriteLine("[Route.Load]: Preview set to " + result.PreviewCameraPosition.X + " | " + result.PreviewCameraPosition.Y + " | " + result.PreviewCameraPosition.Z);

         result.SpawnPosition = Tools.ConvertToVector3(lines[3]);
         result.SpawnRotation = Tools.ConvertToVector3(lines[4]);

         Application.Console.WriteLine("[Route.Load]: Spawnpoint set to " + result.SpawnPosition.X + " | " + result.SpawnPosition.Y + " | " + result.SpawnPosition.Z);

         List<Vector2> rawCoordinates = new List<Vector2>();
         for (int i = 6; i < lines.Length; i++)
         {
            if (String.IsNullOrWhiteSpace(lines[i]))
            {
               Barrier newBarrier = new Barrier();
               newBarrier.Coordinates = rawCoordinates;
               result.Barriers.Add(newBarrier);
               rawCoordinates = new List<Vector2>();
               Application.Console.WriteLine("[Route.Load]: Added barrier with midpoint " + newBarrier.Midpoint.X + " | " + newBarrier.Midpoint.Y);
               continue;
            }
            rawCoordinates.Add(Tools.ConvertToVector2(lines[i]));
         }

         Application.Console.WriteLine("[Route.Load]: Parsing complete");
         return result;
      }

      public void Save()
      {
         var path = Path.Combine(Application.RootFolder, "Routes", Name + ".route");
         if (!File.Exists(path))
            File.Delete(path);

         List<string> lines = new List<string>();

         lines.Add(PreviewCameraPosition.ConvertToString());
         lines.Add(PreviewCameraDirection.ConvertToString());
         lines.Add("");
         lines.Add(SpawnPosition.ConvertToString());
         lines.Add(SpawnRotation.ConvertToString());
         lines.Add("");

         Application.Console.WriteLine("[Route.Save]: Saving route " + Name + " with " + Barriers.Count + " barriers");
         foreach (var b in Barriers)
         {
            Application.Console.WriteLine("[Route.Save]: Writing barrier with midpoint " + b.Midpoint.X + " | " + b.Midpoint.Y);
            for (int j = 0; j < b.Coordinates.Count; j++)
            {
               lines.Add(b.Coordinates[j].ConvertToString());
               Application.Console.WriteLine(b.Coordinates[j].ConvertToString());
            }
            lines.Add("");
         }

         File.WriteAllLines(path, lines);
      }
      internal void Delete()
      {
         var path = "";
         try
         {
            path = Path.Combine(Application.RootFolder, "Routes", Name + ".route");
         }
         catch (Exception)
         {
            return;
         }
         if (File.Exists(path))
            File.Delete(path);
      }
   }
}