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

using GTA.Math;

namespace OpenDriveSimulator.UI.Pages
{
   class Page
   {
      public Vector3 CameraPosition { get; set; } = Application.DefaultCameraPosition;
      public Vector3 CameraDirection { get; set; } = Application.DefaultCameraDirection;
      public float CameraFOV { get; set; } = Application.DefaultCameraFOV;
      public string Daytime { get; set; } = Application.DefaultDaytime;
      public string Weather{ get; set; } = Application.DefaultWeather;

      Dictionary<string, UIElementBase> m_UIElements = new Dictionary<string, UIElementBase>();

      public void AddElement(UIElementBase element)
      {
         m_UIElements.Add(String.IsNullOrEmpty(element.Name) ? "Element" + m_UIElements.Count : element.Name, element);
      }
      public void RemoveElement(string name)
      {
         m_UIElements.Remove(name);
      }
      public List<UIElementBase> GetElements()
      {
         return m_UIElements.Values.ToList();
      }
   }
}