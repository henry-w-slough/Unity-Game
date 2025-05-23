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

    [RequireComponent(typeof(S3DMesh))]
    public class S3DComplete : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected Vector3 pivotPoint = new Vector3(0f, -0.5f, 0f);
        [SerializeField, HideInInspector] protected Vector3 scale = Vector3.one;
        [SerializeField, HideInInspector] protected S3DMesh s3DMesh;
        public virtual List<S3DData> GetMeshData()
        {
            Debug.Log("comp");
            return null;
        }

        public Vector3 GetPivotPoint()
        {
            return pivotPoint;
        }

        public Vector3 GetScale()
        {
            return scale;
        }
        public void Recreate()
        {
            if (s3DMesh == null) s3DMesh = GetComponent<S3DMesh>();
            s3DMesh?.Recreate();
        }
    }
}
