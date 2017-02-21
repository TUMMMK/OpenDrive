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

using System.Drawing;

using GTA;
using GTA.Math;

namespace OpenDriveSimulator.Marker
{
   class ArrowMarker : MarkerBase
   {
      const MarkerType c_marker = MarkerType.ChevronUpx2;
      static readonly Vector3 c_rotation = new Vector3(90.001f, -90.001f, 0f);
      static readonly Vector3 c_scale = new Vector3(2, 2, 2);
      static readonly Color c_color = Color.Red;

      public ArrowMarker()
      {
      }

      public override void Delete()
      {
      }

      public override void Draw()
      {
         var Direction3D = new Vector3(Direction.X, Direction.Y, 0);
         Direction3D.Normalize();
         var drawingPosition = Position;
         drawingPosition.Z += 1.5f;

         World.DrawMarker(c_marker, drawingPosition, Direction3D, c_rotation, c_scale, c_color);  // drawing the markers at the positions given in resampled
      }
   }
}
