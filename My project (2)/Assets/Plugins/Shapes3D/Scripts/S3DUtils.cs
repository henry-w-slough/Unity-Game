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

    public static class S3DUtils
    {
        public static bool IsClockwise(List<Vector2> v)
        {
            float sum = 0;
            for (int i = 0; i < v.Count; i++)
            {
                sum += (GetItem(v, i + 1).x - GetItem(v, i).x) * (GetItem(v, i + 1).y + GetItem(v, i).y);
            }

            return sum < 0;
        }

        public static T GetItem<T>(T[] array, int index)
        {
            if (index >= array.Length)
            {
                return array[index % array.Length];
            }
            else if (index < 0)
            {
                return array[index % array.Length + array.Length];
            }
            return array[index];
        }

        public static T GetItem<T>(List<T> array, int index)
        {
            if (index >= array.Count)
            {
                return array[index % array.Count];
            }
            else if (index < 0)
            {
                return array[index % array.Count + array.Count];
            }
            return array[index];
        }

        public static int GetIndex(int index, int arrayCount)
        {
            if (index >= arrayCount)
            {
                return index % arrayCount;
            }
            else if (index < 0)
            {
                return index % arrayCount + arrayCount;
            }
            return index;
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.x * b.y - a.y * b.x;
        }

        public static Vector3[] ChangeV2toV3xz(List<Vector2> v2, float y)
        {
            List<Vector3> v3 = new List<Vector3>();
            for (int i = 0; i < v2.Count; i++)
            {
                v3.Add(new Vector3(v2[i].x, y, v2[i].y));
            }
            return v3.ToArray();
        }

        public static Vector3 ChangeV2toV3xz(Vector2 v2, float y)
        {
            return new Vector3(v2.x, y, v2.y);
        }

        public static List<Vector2> ChangeV3xztoV2(List<Vector3> v3)
        {
            List<Vector2> vector2 = new List<Vector2>();
            for (int i = 0; i < v3.Count; i++)
            {
                vector2.Add(new Vector2(v3[i].x, v3[i].z));
            }
            return vector2;
        }

        public static bool DoesIntersect(Vector2 p1, Vector2 p2, Vector3 p3, Vector3 p4)
        {
            // Find the 4 orientations needed for general and special cases
            int orientation1 = GetOrientation(p1, p2, p3);
            int orientation2 = GetOrientation(p1, p2, p4);
            int orientation3 = GetOrientation(p3, p4, p1);
            int orientation4 = GetOrientation(p3, p4, p2);

            // General case
            if (orientation1 != orientation2 && orientation3 != orientation4)
                return true;

            // Special Cases

            // p1, p2, and p3 are collinear and p3 lies on the segment p1p2
            if (orientation1 == 0 && OnSegment(p1, p3, p2))
                return true;

            // p1, p2, and p4 are collinear and p4 lies on the segment p1p2
            if (orientation2 == 0 && OnSegment(p1, p4, p2))
                return true;

            // p3, p4, and p1 are collinear and p1 lies on the segment p3p4
            if (orientation3 == 0 && OnSegment(p3, p1, p4))
                return true;

            // p3, p4, and p2 are collinear and p2 lies on the segment p3p4
            if (orientation4 == 0 && OnSegment(p3, p2, p4))
                return true;

            return false; // Doesn't fall in any of the above cases
        }

        public static List<T> ShiftListIndex<T>(List<T> v, int shiftBy)
        {
            List<T> temp = new List<T>();
            for (int i = shiftBy; i < v.Count + shiftBy; i++)
            {
                temp.Add(GetItem(v, i));
            }
            return temp;

        }

        // Find the orientation of triplet (p1, p2, p3)
        private static int GetOrientation(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            float val = (p2.y - p1.y) * (p3.x - p2.x) - (p2.x - p1.x) * (p3.y - p2.y);

            if (val == 0) return 0; // Collinear
            return (val > 0) ? 1 : 2; // Clockwise or Counterclockwise
        }

        // Check if point q lies on the line segment pr
        private static bool OnSegment(Vector2 p, Vector2 q, Vector2 r)
        {
            return q.x <= Mathf.Max(p.x, r.x) && q.x >= Mathf.Min(p.x, r.x) &&
                   q.y <= Mathf.Max(p.y, r.y) && q.y >= Mathf.Min(p.y, r.y);
        }


    }
}
