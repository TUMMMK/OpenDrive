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

namespace OpenDriveSimulator
{
   class Barrier
   {
      public float Spacing
      {
         get
         {
            return m_spacing;
         }
         set
         {
            m_spacing = value;
            resampleCoordinates();
         }
      }
      private float m_spacing = 2.0f;

      public List<Vector2> Coordinates
      {
         get
         {
            return m_coordinates;
         }
         set
         {
            m_coordinates = value;
            setNewMidpoint(value);
            resampleCoordinates();
         }
      }
      private List<Vector2> m_coordinates;

      public List<Vector2> ResampledCoordinates
      {
         get
         {
            return m_resampledCoordinates;
         }
         set
         {
            m_resampledCoordinates = value;
            setNewResampledMidpoint(value);
            createBezierCoordinates();
         }
      }
      private List<Vector2> m_resampledCoordinates;

      public List<Vector2> BezierCoordiantes
      {
         get
         {
            return m_bezierCoordiantes;
         }
         private set
         {
            m_bezierCoordiantes = value;
            setBezierMidpoint(value);
         }
      }
      private List<Vector2> m_bezierCoordiantes;

      public Bezier BezierRegression
      {
         get
         {
            return m_bezierRegression;
         }
         set
         {
            m_bezierRegression = value;
            resampleBezier();
         }
      }
      private Bezier m_bezierRegression;

      public Vector2 Midpoint { get; private set; }
      public Vector2 ResampledMidpoint { get; private set; }
      public Vector2 BezierMidpoint { get; private set; }

      void resampleCoordinates()
      {
         var result = new List<Vector2>();
         int i = 0;
         int j = 1;
         while (j < Coordinates.Count)
         {
            while (Coordinates[i].DistanceTo(Coordinates[j]) < Spacing)
            {
               j++;
               if (j == Coordinates.Count)
               {
                  ResampledCoordinates = result;
                  return;
               }
            }

            result.Add(computeInterpolatedVector(Coordinates[i], Coordinates[j - 1], Coordinates[j], Spacing));
            i = j;
            j = i + 1;
         }
         ResampledCoordinates = result;
      }
      // Computes the interpolated point between p_2 and p_3 with the distance d from point p_1
      Vector2 computeInterpolatedVector(Vector2 p_1, Vector2 p_2, Vector2 p_3, float d)
      {
         Vector2 s32 = p_3 - p_2;
         Vector2 s21 = p_2 - p_1;

         float t1 = Vector2.DistanceSquared(s32, s21);
         float t2 = (float)Math.Sqrt(Vector2.DistanceSquared(s32, s21) - s32.LengthSquared() * (s21.LengthSquared() - d * d));
         float t3 = s32.LengthSquared();

         float k1 = (t1 + t2) / t3;
         float k2 = (t1 - t2) / t3;

         if (0 <= k1 && k1 <= 1)
            return new Vector2(p_2.X + k1 * s32.X, p_2.Y + k1 * s32.Y);
         else
            return new Vector2(p_2.X + k2 * s32.X, p_2.Y + k2 * s32.Y);
      }

      void createBezierCoordinates()
      {
         BezierRegression = Bezier.CreateBezierRegression(ResampledCoordinates);
      }
      void resampleBezier()
      {
         var bezierPoints = new List<Vector2>();
         for (int i = 0; i <= ResampledCoordinates.Count; i++)
         {
            float t = (float)i / (float)ResampledCoordinates.Count;

            float a = (1.0f - t) * (1.0f - t) * (1.0f - t);
            float b = 3.0f * t * (1.0f - t) * (1.0f - t);
            float c = 3.0f * t * t * (1.0f - t);
            float d = t * t * t;

            float x = a * BezierRegression.P1.X + b * BezierRegression.C1.X + c * BezierRegression.C2.X + d * BezierRegression.P2.X;
            float y = a * BezierRegression.P1.Y + b * BezierRegression.C1.Y + c * BezierRegression.C2.Y + d * BezierRegression.P2.Y;
            bezierPoints.Add(new Vector2(x, y));
         }
         BezierCoordiantes = bezierPoints;
      }

      void setNewMidpoint(List<Vector2> value)
      {
         float midX = value.Select(x => x.X).Sum() / value.Count;
         float midY = value.Select(x => x.Y).Sum() / value.Count;
         Midpoint = new Vector2(midX, midY);
      }
      void setNewResampledMidpoint(List<Vector2> value)
      {
         float midX = value.Select(x => x.X).Sum() / value.Count;
         float midY = value.Select(x => x.Y).Sum() / value.Count;
         ResampledMidpoint = new Vector2(midX, midY);
      }
      void setBezierMidpoint(List<Vector2> value)
      {
         float midX = value.Select(x => x.X).Sum() / value.Count;
         float midY = value.Select(x => x.Y).Sum() / value.Count;
         BezierMidpoint = new Vector2(midX, midY);
      }
   }
}