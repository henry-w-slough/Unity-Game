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
    using System.Collections.Generic;
    using UnityEngine;

    public static class S3DTriangulator
    {
        public static bool Triangulate(List<Vector2> v, out int[] triangles, out bool flipNormals)
        {
            flipNormals = false;
            List<Vector2> vertices = new List<Vector2>(v);
            if (S3DUtils.IsClockwise(vertices))
            {
                vertices.Reverse();
                flipNormals = true;
            }

            List<int> triangleList = new List<int>();
            List<int> indexList = new List<int>();
            for (int i = 0; i < vertices.Count; i++)
            {
                indexList.Add(i);
            }
            while (indexList.Count > 3)
            {
                for (int i = 0; i < indexList.Count; i++)
                {
                    int a = indexList[i];
                    int b = S3DUtils.GetItem(indexList, i - 1);
                    int c = S3DUtils.GetItem(indexList, i + 1);

                    Vector2 va = vertices[a];
                    Vector2 vb = vertices[b];
                    Vector2 vc = vertices[c];

                    Vector2 vavb = vb - va;
                    Vector2 vavc = vc - va;
                    //Check if convex
                    if (S3DUtils.Cross(vavb, vavc) < 0f)
                    {
                        continue;
                    }
                    bool isEar = true;

                    //Check if ear contain any vertices
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        if (j == a || j == b || j == c)
                            continue;
                        if (IsPointInTriangle(vertices[j], vb, va, vc))
                        {
                            isEar = false;
                            break;
                        }
                    }
                    if (isEar)
                    {
                        triangleList.Add(b);
                        triangleList.Add(a);
                        triangleList.Add(c);

                        indexList.RemoveAt(i);
                        break;
                    }
                }
            }
            triangleList.Add(indexList[0]);
            triangleList.Add(indexList[1]);
            triangleList.Add(indexList[2]);
            if (flipNormals)
                triangleList.Reverse();
            triangles = triangleList.ToArray();
            return true;
        }

        private static bool IsPointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
        {
            Vector2 ab = b - a;
            Vector2 bc = c - b;
            Vector2 ca = a - c;

            Vector2 ap = p - a;
            Vector2 bp = p - b;
            Vector2 cp = p - c;

            float cross1 = S3DUtils.Cross(ab, ap);
            float cross2 = S3DUtils.Cross(bc, bp);
            float cross3 = S3DUtils.Cross(ca, cp);

            if (cross1 > 0f || cross2 > 0f || cross3 > 0f)
            {
                return false;
            }
            return true;
        }

    }
}
