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

namespace OpenDriveSimulator.Scripting
{
   abstract class SimScriptBase : GTA.Script
   {
      public bool IsRunning { get; protected set; }

      public new String Name { get; protected set;}

      protected SimScriptBase ()
      {
         Name = GetType().Name;
         Application.Scripts.Add(this);

         Tick += tickHandler;
         Aborted += OnAbort;
      }

      public virtual void Start()
      {
         IsRunning = true;
         Application.Console.WriteLine("[Script]: \"" + Name + "\" started!");
      }
      public virtual void Stop()
      {
         IsRunning = false;
         Application.Console.WriteLine("[Script]: \"" + Name + "\" stopped!");
      }



      protected abstract void OnAbort(object sender, EventArgs e);
      protected abstract void OnTick(object sender, EventArgs e);

      private void tickHandler(object sender, EventArgs e)
      {
         if (IsRunning)
            try
            {
               OnTick(sender, e);
            }
            catch (Exception ex)
            {
               Application.Console.WriteLine(ex.Message);
            }
      }
   }
}
