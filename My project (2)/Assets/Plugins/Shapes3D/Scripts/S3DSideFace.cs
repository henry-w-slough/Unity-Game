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

    public class S3DSideFace : S3DData
    {
        bool initialized = false;
        List<Vector3> vertices;
        public S3DSideFace(List<Vector3> vertices)
        {
            initialized = true;
            this.vertices = vertices;
            GenerateMeshData();
        }

        public override void GenerateMeshData()
        {
            if (!initialized) return;
            List<int> tris = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            tris.Add(0);
            tris.Add(1);
            tris.Add(2);
            tris.Add(2);
            tris.Add(3);
            tris.Add(0);


            uv.Add(new Vector2(1f, 1f));
            uv.Add(new Vector2(1f, 0f));
            uv.Add(new Vector2(0f, 0f));
            uv.Add(new Vector2(0f, 1f));

            meshUV = uv.ToArray();
            meshVertices = vertices.ToArray();
            meshTriangles = tris.ToArray();
        }

    }
}
