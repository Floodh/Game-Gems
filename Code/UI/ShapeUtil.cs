
// using System;
// using Microsoft.Xna.Framework;
// using System.Collections.Generic;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;


// /// <summary>
// /// </summary>
// public class ShapeUtils
// {

//     private static bool SameSide(Vector2 p1, Vector2 p2, Vector2 A, Vector2 B)
//     {
//         // Convert points to Vector3 for use the Cross product, which is Vector3-only
//         Vector3 cp1 = Vector3.Cross(new Vector3(B-A, 0), new Vector3(p1-A, 0));
//         Vector3 cp2 = Vector3.Cross(new Vector3(B-A, 0), new Vector3(p2-A, 0));
//         return Vector3.Dot(cp1, cp2) >= 0;
//     }

//     /**
//     * Return true if the point p is in the triangle ABC
//     */
//     public static bool PointInTriangle(Vector2 p, Vector2 A, Vector2 B, Vector2 C)
//     {
//         return SameSide(p,A, B,C) && SameSide(p,B, A,C) && SameSide(p,C, A,B);
//     }



//     private int _vertexCount = 0;
//     private int _indexCount = 0;
//     private VertexShape[] _vertices;
//     private bool _indicesChanged = false;
//     private uint[] _indices;
//     private float _pixelSize = 1f;

//     public void DrawCircle(Vector2 center, float radius, Color c1, Color c2, float thickness = 1f)
//     {
//         EnsureSizeOrDouble(ref _vertices, _vertexCount + 4);
//         _indicesChanged = EnsureSizeOrDouble(ref _indices, _indexCount + 6) || _indicesChanged;

//         radius += _pixelSize; // Account for AA.

//         var topLeft = center + new Vector2(-radius);
//         var topRight = center + new Vector2(radius, -radius);
//         var bottomRight = center + new Vector2(radius);
//         var bottomLeft = center + new Vector2(-radius, radius);

//         float ps = _pixelSize / (radius * 2);

//         float u = 1.0f;

//         _vertices[_vertexCount + 0] = new VertexShape(new Vector3(topLeft, 0), new Vector2(-u, -u), 0f, c1, c2, thickness, ps);
//         _vertices[_vertexCount + 1] = new VertexShape(new Vector3(topRight, 0), new Vector2(u, -u), 0f, c1, c2, thickness, ps);
//         _vertices[_vertexCount + 2] = new VertexShape(new Vector3(bottomRight, 0), new Vector2(u, u), 0f, c1, c2, thickness, ps);
//         _vertices[_vertexCount + 3] = new VertexShape(new Vector3(bottomLeft, 0), new Vector2(-u, u), 0f, c1, c2, thickness, ps);

//         _triangleCount += 2;
//         _vertexCount += 4;
//         _indexCount += 6;
//     }
//     public void FillCircle(Vector2 center, float radius, Color c) {
//         DrawCircle(center, radius, c, c, 0f);
//     }

//     private bool EnsureSizeOrDouble<T>(ref T[] array, int neededCapacity)
//     {
//         if (array.Length < neededCapacity) {
//             Array.Resize(ref array, array.Length * 2);
//             return true;
//         }
//         return false;
//     }

// }