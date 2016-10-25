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

namespace OpenDriveSimulator.Scripting
{
   class DebugLogger : SimScriptBase
   {
      string logFilePath;
      List<string> m_linesToStore = new List<string>();

      public override void Start()
      {
         Interval = 5000;
         base.Start();

         logFilePath = Path.Combine(Application.RootFolder, "DebugLogs");
         if (!Directory.Exists(logFilePath))
            Directory.CreateDirectory(logFilePath);
         string filename = DateTime.Now.Date.Year + "_" + DateTime.Now.Date.Month + "_" + DateTime.Now.Date.Day + ".log";
         logFilePath = Path.Combine(logFilePath, filename);
      }
      public void AddLine(string line)
      {
         m_linesToStore.Add(line);
      }

      void PrintLines()
      {
         File.AppendAllLines(logFilePath, m_linesToStore);
         m_linesToStore.Clear();
      }

      protected override void OnTick(object sender, EventArgs e)
      {
         PrintLines();
      }
      protected override void OnAbort(object sender, EventArgs e)
      {
         PrintLines();
      }
   }
}
