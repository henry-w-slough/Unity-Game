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
    using System.Linq;
    using UnityEngine;

    [System.Serializable]
    public class S3DFace : S3DData
    {
        public bool hole = false;
        public S3DFace() : base()
        {

        }

        public S3DFace(S3DFace copy) : base(copy) { }
        public S3DFace(List<Vector2> vertices) : base(vertices) { }
        public S3DFace(List<Vector2> vertices, List<int> tris) : base(vertices, tris) { }

        public override void GenerateMeshData()
        {

            List<Vector3> verticesList = new List<Vector3>();
            List<Vector2> uvlist = new List<Vector2>();
            float minX = float.MaxValue, maxX = float.MinValue;
            for (int i = 0; i < base.shapeVertices.Count; i++)
            {
                if (base.shapeVertices[i].x < minX)
                    minX = base.shapeVertices[i].x;
                if (base.shapeVertices[i].x > maxX)
                    maxX = base.shapeVertices[i].x;
            }
            float lengthX = maxX - minX;

            if (S3DTriangulator.Triangulate(base.shapeVertices, out meshTriangles, out bool flipped))
            {
                for (int i = 0; i < base.shapeVertices.Count; i++)
                {
                    verticesList.Add(new Vector3(base.shapeVertices[i].x, -0.5f, base.shapeVertices[i].y));
                    uvlist.Add(new Vector2(base.shapeVertices[i].x / lengthX + 0.5f, base.shapeVertices[i].y / lengthX + 0.5f));
                }
            }
            //if (flipped)
            //    System.Array.Reverse(meshTriangles);

            meshVertices = verticesList.ToArray();
            meshUV = uvlist.ToArray();

        }

        public S3DFace RaiseHeight()
        {
            for (int i = 0; i < meshVertices.Count(); i++)
            {
                meshVertices[i] = new Vector3(meshVertices[i].x, 0.5f, meshVertices[i].z);
            }
            return this;
        }
        public S3DFace ChangeHeight(float height)
        {
            for (int i = 0; i < meshVertices.Count(); i++)
            {
                meshVertices[i] = new Vector3(meshVertices[i].x, height, meshVertices[i].z);
            }
            return this;
        }

        public S3DFace ChangeHeight(float height, int inclusiveStart, int inclusiveFinish)
        {
            for (int i = inclusiveStart; i <= inclusiveFinish; i++)
            {
                meshVertices[i] = new Vector3(meshVertices[i].x, height, meshVertices[i].z);
            }
            return this;
        }

    }
}

