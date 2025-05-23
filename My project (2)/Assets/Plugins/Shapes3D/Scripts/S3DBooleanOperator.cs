/*
 * Copyright (c) [2023] [AD STUDIOS & VOLVE STUDIOS]
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are NOT permitted. You may only use this package to make
 * games.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

namespace S3DShapes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class S3DBooleanOperator
    {
        public static S3DData CombineMesh2(S3DData mesh0, S3DData mesh1)
        {
            List<Vector2> verticesCombined = new List<Vector2>(mesh0.shapeVertices);
            verticesCombined.AddRange(mesh1.shapeVertices);

            List<Vector2> outerLoop = GetOuterLoop(verticesCombined);

            S3DFace newFace = new S3DFace(outerLoop);
            return newFace;
        }

        public static S3DData CombineMesh(S3DData mesh0, S3DData mesh1)
        {
            List<Vector3> verticesCombined = new List<Vector3>(mesh1.meshVertices.ToList());
            List<int> trisCombined = new List<int>(mesh1.meshTriangles);
            List<Vector2> uvCombined = new List<Vector2>(mesh1.meshUV);
            int startingIndex = mesh1.meshVertices.Length;
            int[] mesh0Tris = mesh0.meshTriangles.ToArray();
            for (int i = 0; i < mesh0.meshTriangles.Count(); i++)
            {
                mesh0Tris[i] += startingIndex;
            }
            verticesCombined.AddRange(mesh0.meshVertices);
            trisCombined.AddRange(mesh0Tris);
            uvCombined.AddRange(mesh0.meshUV);

            S3DData combined = new S3DData(verticesCombined, trisCombined, uvCombined);
            return combined;
        }
        public static S3DFace Subtract(S3DFace face, S3DFace hole, bool areBothSamePolygons = false, bool sideCut = false, int sideRangeEndVertex = -1)
        {
            if (areBothSamePolygons)
            {
                if (sideRangeEndVertex > 1 && sideCut)
                    return CutPolygonFromPolygon(face, hole, sideRangeEndVertex);
                return CutPolygonFromPolygon(face, hole);
            }
            S3DFace[] faces = SeparateFacesForSubtraction(face, hole);

            //Combine triangles
            return (S3DFace)CombineMesh(faces[0], faces[1]);
        }

        private static S3DFace CutPolygonFromPolygon(S3DFace face, S3DFace hole, int endVertex)
        {
            if (face.shapeVertices.Count != hole.shapeVertices.Count)
            {
                Debug.Log("Shapes do not have the same # vertices");
                return null;
            }
            int oneShapeVerticesCount = face.shapeVertices.Count;
            //Shift to remove the last face
            int faceShift = -1, holeShift = -1;

            face.shapeVertices = S3DUtils.ShiftListIndex(face.shapeVertices, faceShift);
            hole.shapeVertices = S3DUtils.ShiftListIndex(hole.shapeVertices, holeShift);

            List<Vector2> combinedVertices = new List<Vector2>(face.shapeVertices);
            combinedVertices.AddRange(hole.shapeVertices);
            List<int> triangles = new List<int>();


            for (int i = 0; i < endVertex - 1; i++)
            {
                triangles.Add(i);
                triangles.Add(i + oneShapeVerticesCount + 1);
                triangles.Add(i + oneShapeVerticesCount);
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + oneShapeVerticesCount + 1);
            }
            triangles.Add(oneShapeVerticesCount + oneShapeVerticesCount - 1);
            triangles.Add(oneShapeVerticesCount - 1);
            triangles.Add(oneShapeVerticesCount);
            triangles.Add(oneShapeVerticesCount - 1);
            triangles.Add(0);
            triangles.Add(oneShapeVerticesCount);
            triangles.Reverse();

            triangles = triangles.GetRange(0, endVertex * 6 - 6);

            return new S3DFace(combinedVertices, triangles);
        }

        private static S3DFace CutPolygonFromPolygon(S3DFace face, S3DFace hole)
        {
            if (face.shapeVertices.Count != hole.shapeVertices.Count)
            {
                Debug.Log("Shapes do not have the same # vertices");
                return null;
            }
            int oneShapeVerticesCount = face.shapeVertices.Count;
            //Find leftmost vertex for both
            int faceLeftMostIndex = 0, holeLeftMostIndex = 0;
            for (int i = 1; i < oneShapeVerticesCount; i++)
            {
                if (face.shapeVertices[i].x < face.shapeVertices[faceLeftMostIndex].x)
                {
                    faceLeftMostIndex = i;
                }
                if (hole.shapeVertices[i].x < hole.shapeVertices[holeLeftMostIndex].x)
                    holeLeftMostIndex = i;
            }
            face.shapeVertices = S3DUtils.ShiftListIndex(face.shapeVertices, faceLeftMostIndex);
            hole.shapeVertices = S3DUtils.ShiftListIndex(hole.shapeVertices, holeLeftMostIndex);

            List<Vector2> combinedVertices = new List<Vector2>(face.shapeVertices);
            combinedVertices.AddRange(hole.shapeVertices);
            List<int> triangles = new List<int>();


            for (int i = 0; i < oneShapeVerticesCount - 1; i++)
            {
                triangles.Add(i);
                triangles.Add(i + oneShapeVerticesCount + 1);
                triangles.Add(i + oneShapeVerticesCount);
                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + oneShapeVerticesCount + 1);
            }
            triangles.Add(oneShapeVerticesCount + oneShapeVerticesCount - 1);
            triangles.Add(oneShapeVerticesCount - 1);
            triangles.Add(oneShapeVerticesCount);
            triangles.Add(oneShapeVerticesCount - 1);
            triangles.Add(0);
            triangles.Add(oneShapeVerticesCount);
            triangles.Reverse();

            return new S3DFace(combinedVertices, triangles);
        }



        private static S3DFace[] SeparateFacesForSubtraction(S3DFace f, S3DFace h)
        {
            S3DFace bigFace, smallFace;

            List<Vector2> verticesF = new List<Vector2>(f.shapeVertices);
            List<Vector2> verticesH = new List<Vector2>(h.shapeVertices);

            Vector2 f0 = Vector2.zero; //Vertex cut on face
            Vector2 f1 = Vector2.zero; //Vertex cut on face
            Vector2 h0 = verticesH[0];
            Vector2 h1 = verticesH[1];
            bool foundF0 = false;
            int f0Index = -1;
            for (int i = 0; i < verticesF.Count; i++)
            {
                if (foundF0)
                    break;
                f0 = verticesF[i];
                for (int j = 0; j < verticesH.Count; j++)
                {
                    if (!S3DUtils.DoesIntersect(h0, f0, S3DUtils.GetItem(verticesH, j), S3DUtils.GetItem(verticesH, j + 1)))
                    {
                        foundF0 = true;
                        f0Index = i;
                        break;
                    }
                }
            }
            bool foundF1 = false;
            int f1Index = -1;
            int h1Index = 1;
            while (!foundF1)
            {
                h1 = verticesH[h1Index];
                for (int i = 0; i < verticesF.Count; i++)
                {
                    if (foundF1)
                        break;
                    if (i == f0Index)
                        continue;
                    f1 = verticesF[i];
                    bool doesNotIntersect = true;
                    for (int j = 0; j < verticesH.Count; j++)
                    {
                        if (j == h1Index || j == h1Index - 1)
                            continue;
                        if (S3DUtils.DoesIntersect(h1, f1, S3DUtils.GetItem(verticesH, j), S3DUtils.GetItem(verticesH, j + 1)))
                        {
                            doesNotIntersect = false;
                            break;
                        }
                    }
                    if (doesNotIntersect)
                    {
                        foundF1 = true;
                        f1Index = i;
                    }
                }
                if (!foundF1)
                {
                    h1Index++;
                    if (h1Index >= verticesH.Count)
                    {
                        throw new Exception("f1 not found");
                    }

                }
            }

            List<Vector2> verticesSmall = new List<Vector2> { h0 };
            verticesSmall.AddRange(verticesF.GetRange(f0Index, (f1Index + verticesF.Count - f0Index) % verticesF.Count + 1));
            verticesSmall.Add(h1);
            verticesSmall.AddRange(verticesH.GetRange(1, h1Index - 1));

            smallFace = new S3DFace(verticesSmall);

            List<Vector2> verticesBig = new List<Vector2>();

            verticesBig.Add(f0);
            verticesBig.Add(h0);
            for (int i = verticesH.Count - 1; i >= h1Index; i--)
            {
                verticesBig.Add(verticesH[i]);
            }
            verticesBig.Add(f1);

            for (int i = 1; i < verticesF.Count - 1; i++)
            {
                verticesBig.Add(S3DUtils.GetItem(verticesF, f1Index + i));
            }
            bigFace = new S3DFace(verticesBig);

            return new S3DFace[] { bigFace, smallFace };

        }

        private static (Vector3[], Vector2[], int[]) MakeVertices(List<Vector2> shapeVertices, ref int[] triangles)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            if (S3DTriangulator.Triangulate(shapeVertices, out triangles, out bool flipNormals))
            {
                for (int i = 0; i < shapeVertices.Count; i++)
                {
                    vertices.Add(new Vector3(shapeVertices[i].x, 0f, shapeVertices[i].y));
                    uv.Add(new Vector2(0, 0));
                }
            }
            return (vertices.ToArray(), uv.ToArray(), triangles);
        }

        // Function to find the convex hull of a set of points
        public static List<Vector2> GetOuterLoop(List<Vector2> points)
        {
            if (points.Count < 3)
                return points;

            // Find the point with the lowest y-coordinate (and leftmost if tied)
            Vector2 pivot = FindPivot(points);

            // Sort the points based on polar angle from the pivot
            List<Vector2> sortedPoints = SortByPolarAngle(points, pivot);

            // Initialize the convex hull with the pivot and the first two sorted points
            List<Vector2> convexHull = new List<Vector2> { pivot, sortedPoints[0], sortedPoints[1] };

            // Iterate through the sorted points to build the convex hull
            for (int i = 2; i < sortedPoints.Count; i++)
            {
                while (convexHull.Count >= 2 && !IsLeftTurn(convexHull[convexHull.Count - 2], convexHull[convexHull.Count - 1], sortedPoints[i]))
                {
                    // Remove the last point from the convex hull if it makes a right turn
                    convexHull.RemoveAt(convexHull.Count - 1);
                }

                // Add the current point to the convex hull
                convexHull.Add(sortedPoints[i]);
            }

            return convexHull;
        }

        // Find the point with the lowest y-coordinate (and leftmost if tied)
        private static Vector2 FindPivot(List<Vector2> points)
        {
            Vector2 pivot = points[0];
            foreach (Vector2 point in points)
            {
                if (point.y < pivot.y || (point.y == pivot.y && point.x < pivot.x))
                {
                    pivot = point;
                }
            }
            return pivot;
        }

        // Sort the points based on polar angle from the pivot
        private static List<Vector2> SortByPolarAngle(List<Vector2> points, Vector2 pivot)
        {
            List<Vector2> sortedPoints = new List<Vector2>(points);
            sortedPoints.Sort((a, b) =>
            {
                float angleA = Mathf.Atan2(a.y - pivot.y, a.x - pivot.x);
                float angleB = Mathf.Atan2(b.y - pivot.y, b.x - pivot.x);

                if (angleA < angleB) return -1;
                if (angleA > angleB) return 1;

                // If angles are equal, choose the one closer to the pivot
                return Vector2.Distance(a, pivot).CompareTo(Vector2.Distance(b, pivot));
            });

            return sortedPoints;
        }

        // Check if the three points make a left turn
        private static bool IsLeftTurn(Vector2 a, Vector2 b, Vector2 c)
        {
            return ((b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y)) > 0;
        }

    }
}
