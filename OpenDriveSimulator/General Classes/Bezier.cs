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

using System.Collections.Generic;
using System.Linq;

using Accord.Math;
using Matrix = Accord.Math.Matrix;

using GTA.Math;

#pragma warning disable CS0618

namespace OpenDriveSimulator
{
   class Bezier
   {
      public Vector2 P1 { get; private set; }
      public Vector2 P2 { get; private set; }
      public Vector2 C1 { get; private set; }
      public Vector2 C2 { get; private set; }

      static float[][] M_Matrix
      {
         get
         {
            return new float[][]
            {
                new float [] {-1,  3, -3,  1},
                new float [] { 3, -6,  3,  0},
                new float [] {-3,  3,  0,  0},
                new float [] { 1,  0,  0,  0}
            };
         }
      }

      public static Bezier CreateBezierRegression(List<Vector2> points)
      {
         float[] X = points.Select(x => x.X).ToArray();
         float[] Y = points.Select(x => x.Y).ToArray();

         float[][] T = ComputeTMatrix(X, Y);

         float[] resultX = Matrix.Multiply(Matrix.Multiply(Matrix.Multiply(Matrix.Inverse(M_Matrix), Matrix.Inverse(Matrix.Multiply(Matrix.Transpose(T), T))), Matrix.Transpose(T)), X);
         float[] resultY = Matrix.Multiply(Matrix.Multiply(Matrix.Multiply(Matrix.Inverse(M_Matrix), Matrix.Inverse(Matrix.Multiply(Matrix.Transpose(T), T))), Matrix.Transpose(T)), Y);

         return new Bezier()
         {
            P1 = new Vector2(resultX[0], resultY[0]),
            C1 = new Vector2(resultX[1], resultY[1]),
            C2 = new Vector2(resultX[2], resultY[2]),
            P2 = new Vector2(resultX[3], resultY[3]),
         };
      }

      static float[][] resampleBezierCurve(float[] c1, float[] c2, float[] c3, float[] c4, float[][] T)
      {
         float[][] C = new float[][] { c1, c2, c3, c4 };

         return Matrix.Multiply(Matrix.Multiply(T, M_Matrix), C);
      }

      static float[][] ComputeTMatrix(float[] xVal, float[] yVal)
      {
         float[] NormLength = new float[xVal.Length];
         NormLength[0] = 0;

         for (int i = 1; i < xVal.Length; i++)
         {
            float distance = (float)System.Math.Sqrt(System.Math.Pow(xVal[i] - xVal[i - 1], 2) + System.Math.Pow(yVal[i] - yVal[i - 1], 2));

            NormLength[i] += NormLength[i - 1] + distance;
         }
         if(NormLength[NormLength.Length - 1] != 0)
            for (int i = 0; i < NormLength.Length; i++)
            {
               NormLength[i] /= NormLength[NormLength.Length - 1];
            }


         float[][] T = new float[xVal.Length][];
         for (int i = 0; i < xVal.Length; i++)
         {
            var temp = NormLength[i];
            T[i] = new float[] { (float)System.Math.Pow(temp, 3), (float)System.Math.Pow(temp, 2), (float)System.Math.Pow(temp, 1), (float)System.Math.Pow(temp, 0) };
         }
         return T;
      }
   }
}