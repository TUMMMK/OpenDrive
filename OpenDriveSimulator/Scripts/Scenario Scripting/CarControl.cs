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
using System.Timers;

using GTA;

namespace OpenDriveSimulator.Scripting
{
   class CarControl : SimScriptBase
   {
      Timer m_reverseTimer;
      bool m_reverseEnabled = false;

      public CarControl()
      {
         m_reverseTimer = new Timer(1000)
         {
            AutoReset = false,
         };
         m_reverseTimer.Elapsed += enableReverse;
      }

      protected override void OnAbort(object sender, EventArgs e)
      {
      }
      protected override void OnTick(object sender, EventArgs e)
      {
         reverseGear();
      }

      void enableReverse(object sender, ElapsedEventArgs e)
      {
         Application.Dispatcher.Invoke(() => m_reverseEnabled = true);
      }
      void reverseGear()
      {
         Vehicle veh = Game.Player.Character.CurrentVehicle;
         if (!Entity.Exists(veh))
            return;

         if ( veh.Acceleration < 0 && veh.Speed < 0 ) // Accelerating backwards
         {
            if (!m_reverseEnabled)
            {
               if (!m_reverseTimer.Enabled)
                  m_reverseTimer.Start();
               veh.Speed = 0;
            }
         }
         else
         {
            m_reverseTimer.Stop();
            m_reverseEnabled = false;
         }
      }
   }
}
