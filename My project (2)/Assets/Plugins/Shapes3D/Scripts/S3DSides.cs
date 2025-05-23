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

    public class S3DSides : S3DData
    {
        private List<Vector3> topFace, bottomFace;
        private int oriShapeVertCount = -1;
        private bool initialized = false;
        private float r;
        private bool cutSide = false;
        public S3DSides() { }
        public S3DSides(S3DFace face, int startIndex, int verticesCount, float radius = 0.5f, float height = 1f) //inclusive
        {
            initialized = true;
            oriShapeVertCount = verticesCount;
            bottomFace = new List<Vector3>(S3DUtils.ChangeV2toV3xz(face.shapeVertices.GetRange(startIndex, verticesCount), -0.5f));
            bottomFace.Add(new Vector3(bottomFace[0].x, -height / 2f, bottomFace[0].z));
            topFace = new List<Vector3>(S3DUtils.ChangeV2toV3xz(face.shapeVertices.GetRange(startIndex, verticesCount), 0.5f));
            topFace.Add(new Vector3(bottomFace[0].x, height / 2f, bottomFace[0].z));
            r = radius; //for setting UV purposes

            GenerateMeshData();
        }

        public S3DSides(S3DFace bottomFace, S3DFace topFace, int startIndex, int verticesCount, float radius, bool dontRaiseHeight = false, bool cutSide = false) //inclusive
        {
            initialized = true;
            oriShapeVertCount = verticesCount;

            this.bottomFace = new List<Vector3>();
            for (int i = 0; i <= verticesCount; i++)
            {
                this.bottomFace.Add(bottomFace.meshVertices[(startIndex + i) % bottomFace.meshVertices.Length]);
            }

            this.bottomFace.Add(new Vector3(this.bottomFace[0].x, this.bottomFace[0].y, this.bottomFace[0].z));
            if (!dontRaiseHeight)
                topFace.RaiseHeight();

            //this.topFace = topFace.meshVertices.ToList().GetRange(0, verticesCount);
            this.topFace = new List<Vector3>();
            for (int i = 0; i <= verticesCount; i++)
            {
                this.topFace.Add(topFace.meshVertices[(startIndex + i) % topFace.meshVertices.Length]);
            }
            this.topFace.Add(new Vector3(this.topFace[0].x, this.topFace[0].y, this.topFace[0].z));
            r = radius; //for setting UV purposes
            this.cutSide = cutSide;
            GenerateMeshData();
        }

        public override void GenerateMeshData()
        {
            if (!initialized)
                return;
            List<Vector3> vertices = new List<Vector3>();

            List<Vector2> uv = new List<Vector2>();

            float factor = (topFace.Count) / (2f * Mathf.PI * r);
            for (int i = 0; i < topFace.Count; i++)
            {
                vertices.Add(bottomFace[i]);
                vertices.Add(topFace[i]);
                uv.Add(new Vector2(i / factor, 0f));
                uv.Add(new Vector2(i / factor, 1f));
            }
            List<int> tris = new List<int>();
            for (int i = 0; i < 2 * oriShapeVertCount; i += 2)
            {
                tris.Add(i);
                tris.Add(i + 1);
                tris.Add(i + 2);
                tris.Add(i + 2);
                tris.Add(i + 1);
                tris.Add(i + 3);

            }
            if (cutSide)
                tris = tris.GetRange(0, oriShapeVertCount * 6 - 6); //SINNIIIII
            meshVertices = vertices.ToArray();
            meshTriangles = tris.ToArray();
            meshUV = uv.ToArray();
        }



    }
}
