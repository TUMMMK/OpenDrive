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

using GTA;
using GTA.Math;

namespace OpenDriveSimulator.Marker
{
   class PylonMarker : MarkerBase
   {
      Prop m_pylon = null;
      const int c_modelHash = 546252211;

      public override void Delete()
      {
         if (m_pylon == null)
            return;

         if (m_pylon.Exists())
         {
            m_pylon.Delete();
            m_pylon = null;
         }
      }
      public override void Draw()
      {
         if (m_pylon == null)
         {
            var pos = new Vector3(Position.X, Position.Y, World.GetGroundHeight(Position));
            var rot = new Vector3(0, 0, (float)(Math.Atan(Direction.Y / Direction.X) / Math.PI * 180));

            m_pylon = World.CreateProp(new Model(c_modelHash), pos, rot, false, true);
            if (m_pylon.Exists())
            {
               m_pylon.IsPositionFrozen = true;
            }
         }
      }
   }
}
