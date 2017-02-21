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
using System.Linq;

using GTA;
using GTA.Math;

using OpenDriveSimulator.Marker;

namespace OpenDriveSimulator.Scripting
{
   class DrawMarkerScript : SimScriptBase
   {
      public enum RenderingMode
      {
         Raw,
         Resampled,
         Bezier
      }
      public enum MarkerType
      {
         Arrow,
         Fence,
         Pylon
      }

      const float c_renderingThreshold = 200.0f;

      public List<Barrier> Markers
      {
         get
         {
            return m_markers;
         }
         set
         {
            m_markers = value;
            Application.Console.WriteLine("[DMScript.SetMarker]: Barrier list assigned with new list (" + value.Count + " entries)");
            updateMarkers();
         }
      }
      List<Barrier> m_markers = new List<Barrier>();

      public Barrier PreviewMarker
      {
         get
         {
            return m_previewMarker;
         }
         set
         {
            m_previewMarker = value;
            updateMarkers();
         }
      }
      Barrier m_previewMarker = null;

      public RenderingMode CurrentRenderMode
      {
         get
         {
            return m_currentRenderMode;
         }
         set
         {
            bool updateCoordinates = (value == m_currentRenderMode);
            m_currentRenderMode = value;
            if (updateCoordinates)
            {
               updateMarkers();
            }
         }
      }
      RenderingMode m_currentRenderMode = RenderingMode.Bezier;

      public MarkerType CurrentMarkerType
      {
         get
         {
            return m_currentMarkerType;
         }  
         set
         {
            bool updateRenderingMarkers;
            if (value == m_currentMarkerType)
               updateRenderingMarkers = false;
            else
               updateRenderingMarkers = true;

            Application.Console.WriteLine("[DMScript]: update needed is " + updateRenderingMarkers.ToString());
            m_currentMarkerType = value;
            if (updateRenderingMarkers)
            {
               Application.Console.WriteLine("[DMScript]: Rendermode changed. Updating markers");
               updateMarkers();
            }
         }
      }
      MarkerType m_currentMarkerType = MarkerType.Pylon;

      Dictionary<Vector2, List<MarkerBase>> m_renderingMarkers = new Dictionary<Vector2, List<MarkerBase>>();

      public void HideMarkers()
      {
         Vector2 playerPos = new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y);
         foreach (var midpoint in m_renderingMarkers.Keys)
         {
            foreach (var marker in m_renderingMarkers[midpoint])
            {
               marker.Delete();
            }
         }
      }

      protected override void OnAbort(object sender, EventArgs e)
      {
         HideMarkers();
      }
      protected override void OnTick(object sender, EventArgs e)
      {
         drawMarkers();
      }

      void drawMarkers()
      {
         updateMarkerPositions();

         Vector2 playerPos = new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y);
         foreach (var midpoint in m_renderingMarkers.Keys)
         {
            if (midpoint.DistanceTo(playerPos) <= c_renderingThreshold)
            {
               foreach (var marker in m_renderingMarkers[midpoint])
               {
                  marker.Draw();
               }
            }
            else
            {
               foreach (var marker in m_renderingMarkers[midpoint])
               {
                  marker.Delete();
               }
            }
         }
      }

   void updateMarkers()
      {
         Application.Console.WriteLine("[DMScript.updateMarker]: Deleting old markers");
         int oldMarkerCount = 0;
         foreach (var markerList in m_renderingMarkers.Values)
         {
            foreach (var marker in markerList)
            {
               marker.Delete();
               oldMarkerCount++;
            }
         }
         Application.Console.WriteLine("[DMScript.updateMarker]: " + oldMarkerCount + " markers deleted");

         m_renderingMarkers.Clear();
         Application.Console.WriteLine("[DMScript.updateMarker]: Done");


         Application.Console.WriteLine("[DMScript.updateMarker]: Collecting new coordinates. Render mode is: " + Enum.GetName(typeof(RenderingMode), CurrentRenderMode));
         Dictionary<Vector2, List<Vector2>> coordiantes = new Dictionary<Vector2, List<Vector2>>();
         foreach (var b in Markers)
         {
            switch (m_currentRenderMode)
            {
               case RenderingMode.Raw:
                  Application.Console.WriteLine("[DMScript.updateMarker]: Adding barrier: (" + b.Midpoint.X + " | " + b.Midpoint.Y + ") with " + b.Coordinates.Count + " elements");
                  coordiantes.Add(b.Midpoint, b.Coordinates);
                  break;
               case RenderingMode.Resampled:
                  Application.Console.WriteLine("[DMScript.updateMarker]: Adding barrier: (" + b.ResampledMidpoint.X + " | " + b.ResampledMidpoint.Y + ") with " + b.ResampledCoordinates.Count + " elements");
                  coordiantes.Add(b.ResampledMidpoint, b.ResampledCoordinates);
                  break;
               case RenderingMode.Bezier:
                  Application.Console.WriteLine("[DMScript.updateMarker]: Adding barrier: (" + b.BezierMidpoint.X + " | " + b.BezierMidpoint.Y + ") with " + b.BezierCoordiantes.Count + " elements");
                  coordiantes.Add(b.BezierMidpoint, b.BezierCoordiantes);
                  break;
            }
         }
         if (PreviewMarker != null)
         {
            coordiantes.Add(PreviewMarker.BezierMidpoint, PreviewMarker.BezierCoordiantes);
         }

         foreach (var midpoint in coordiantes.Keys)
         {
            List<MarkerBase> markerList = new List<MarkerBase>();
            var coordList = coordiantes[midpoint].Select(x => Tools.ConvertToVector3(x)).ToList();
            for (int i = 0; i < coordList.Count - 1; i++)
            {
               var pos = coordList[i];
               var diff = coordList[i + 1] - pos;
               var dir = new Vector2(diff.X, diff.Y);
               dir.Normalize();
               switch (m_currentMarkerType)
               {
                  case MarkerType.Arrow:
                     markerList.Add(new ArrowMarker() { Position = pos, Direction = dir });
                     break;
                  case MarkerType.Fence:
                     markerList.Add(new FenceMarker() { Position = pos, Direction = dir });
                     break;
                  case MarkerType.Pylon:
                     markerList.Add(new PylonMarker() { Position = pos, Direction = dir });
                     break;
               }
            }
            m_renderingMarkers.Add(midpoint, markerList);
         }
      }
      const float c_heightThreshold = 2.0f;
      private void updateMarkerPositions()
      {
         Vector2 playerPos = new Vector2(Game.PlayerPed.Position.X, Game.PlayerPed.Position.Y);
         foreach (var marker in m_renderingMarkers.Where(x => x.Key.DistanceTo(playerPos) <= c_renderingThreshold).Select(x => x.Value))
         {
            var list = marker.Select(x => x.Position).ToList();

            int interpolationStartIndex = 0;
            bool interplolating = false;
            for (int i = 1; i < list.Count; i++)
            {
               var lastHeight = list[i - 1].Z;
               var currentHeight = list[i].Z;

               if (lastHeight == 0 || currentHeight == 0)
                  continue;

               if (Math.Abs(currentHeight - lastHeight) > c_heightThreshold)
               {
                  if (!interplolating)
                  {
                     interpolationStartIndex = i - 1;
                     Application.Console.WriteLine("Height jump detected. Interpolating from index " + interpolationStartIndex);
                     Application.Console.WriteLine("Height difference is " + Math.Abs(currentHeight - lastHeight) + " (from " + lastHeight + " to " + currentHeight + ")");
                     interplolating = true;
                  }
                  else
                  {
                     if (i < list.Count - 1)
                        i++;
                     int interpolationCount = i - interpolationStartIndex;
                     float interpolationFactor = 1.0f / (float)(interpolationCount + 1);
                     float startZValue = list[interpolationStartIndex].Z;
                     float endZValue = list[i].Z;
                     Application.Console.WriteLine("to index " + i + ". Start height is " + startZValue + "; end height is " + endZValue + ". Interpolating " + interpolationCount + " values");

                     for (int j = 0; j <= interpolationCount; j++)
                     {
                        var vector = list[interpolationStartIndex + j];
                        vector.Z = (float)((interpolationCount - j) * startZValue + j * endZValue) / (float)interpolationCount;
                        marker[interpolationStartIndex + j].Position = vector;
                        marker[interpolationStartIndex + j].SetToGround = false;

                        Application.Console.WriteLine("setting index " + (interpolationStartIndex + j) + " to " + vector.Z);
                     }
                     interplolating = false;
                  }
               }
            }
         }
      }
   }
}