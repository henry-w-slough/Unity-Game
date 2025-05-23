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
    public class S3DData
    {
        public S3DData()
        {
            if (!alreadyHasTrisAndVerts)
                GenerateMeshData(); //is used only when manually input data in inspector
        }

        public S3DData(S3DData copy)
        {
            meshVertices = copy.meshVertices.ToArray();
            meshTriangles = copy.meshTriangles.ToArray();
            meshUV = copy.meshUV.ToArray();
            alreadyHasTrisAndVerts = copy.alreadyHasTrisAndVerts;
            flipNormals = copy.flipNormals;
        }

        public string shapeName = "";

        public List<Vector2> shapeVertices = new List<Vector2>
    {
        new Vector2 (0, 6),
        new Vector2 (5, 0),
        new Vector2 (-2, 0),

    };
        [HideInInspector] public Vector3[] meshVertices;
        [HideInInspector] public int[] meshTriangles;
        [HideInInspector] public Vector2[] meshUV;
        [HideInInspector] public bool alreadyHasTrisAndVerts = false;
        public bool flipNormals = false;

        public S3DData(List<Vector2> vertices)
        {
            this.shapeVertices.Clear();
            this.shapeVertices = vertices;
            GenerateMeshData();
        }
        public S3DData(List<Vector2> vertices, List<int> tris)
        {
            this.shapeVertices.Clear();
            this.shapeVertices = vertices;
            meshVertices = S3DUtils.ChangeV2toV3xz(shapeVertices, -0.5f);
            meshTriangles = tris.ToArray();
            alreadyHasTrisAndVerts = true;
            GenerateUV();
        }

        public S3DData(List<Vector3> vertices, List<int> tris, List<Vector2> uvs)
        {
            this.shapeVertices.Clear();
            //this.shapeVertices = vertices;
            meshVertices = vertices.ToArray();
            meshTriangles = tris.ToArray();
            meshUV = uvs.ToArray();
            alreadyHasTrisAndVerts = true;
            //GenerateUV();
        }

        public S3DData(Vector3[] vertices, int[] tris, Vector2[] uvs)
        {
            this.shapeVertices.Clear();
            //this.shapeVertices = vertices;
            meshVertices = vertices.ToArray();
            meshTriangles = tris.ToArray();
            meshUV = uvs.ToArray();
            alreadyHasTrisAndVerts = true;
        }

        protected void GenerateUV()
        {
            List<Vector2> uvlist = new List<Vector2>();

            float minX = float.MaxValue, maxX = float.MinValue;
            for (int i = 0; i < shapeVertices.Count; i++)
            {
                if (shapeVertices[i].x < minX)
                    minX = shapeVertices[i].x;
                if (shapeVertices[i].x > maxX)
                    maxX = shapeVertices[i].x;
            }
            float lengthX = maxX - minX;

            for (int i = 0; i < shapeVertices.Count; i++)
            {
                uvlist.Add(new Vector2(shapeVertices[i].x / lengthX + 0.5f, shapeVertices[i].y / lengthX + 0.5f));
            }

            meshUV = uvlist.ToArray();
        }

        public virtual void GenerateMeshData()
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv = new List<Vector2>();
            /*if (Triangulator.Triangulate(shapeVertices, out triangles))
            {
                for (int i = 0; i < shapeVertices.Count; i++)
                {
                    vertices.Add(new Vector3(shapeVertices[i].x, 0f, shapeVertices[i].y));
                    uv.Add(new Vector2(0, 0));
                }
            }*/


        }

        public virtual S3DData Reverse()
        {
            System.Array.Reverse(meshTriangles);
            return this;
        }

    }
}
