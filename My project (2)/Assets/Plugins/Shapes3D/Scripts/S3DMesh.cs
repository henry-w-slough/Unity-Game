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
    using UnityEditor;
    using UnityEngine;
    using System.Linq;
    using System.Collections.Generic;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class S3DMesh : MonoBehaviour
    {
        private Mesh mesh;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;
        public S3DComplete completeMesh;
        [HideInInspector] public List<S3DData> meshData;
        [HideInInspector] public S3DData currentMesh;
        [HideInInspector] public int selectedMeshIndex = 0; //is updated through SubclassListPropertyDrawer, used by S3DMeshEditor & VertexHandlesEditor
        private Vector3 pivot = Vector3.zero;
        private Vector3 scale = Vector3.one;

        //Saving mesh
        string prevName = "";
        string savePath = "";
        bool isNotPrefab = true;
        public void Recreate()
        {
            if (meshFilter == null)
                meshFilter = GetComponent<MeshFilter>();
            if (meshCollider == null)
                meshCollider = GetComponent<MeshCollider>();
            mesh = new Mesh();

            if (completeMesh != null)
            {
                meshData.Clear();
                meshData = completeMesh.GetMeshData();
                pivot = completeMesh.GetPivotPoint();
                scale = completeMesh.GetScale();
                if (meshData == null) return;
                S3DData fullMesh = meshData[0];
                for (int i = 1; i < meshData.Count; i++)
                {
                    fullMesh = S3DBooleanOperator.CombineMesh(fullMesh, meshData[i]);
                }
                currentMesh = fullMesh;

            }
            else
            {

            }
            ChangePivotPoint();

            if (currentMesh == null)
                return;
            mesh.vertices = currentMesh.meshVertices;
            mesh.uv = currentMesh.meshUV;
            mesh.triangles = currentMesh.flipNormals ? currentMesh.meshTriangles.Reverse().ToArray() : currentMesh.meshTriangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

#if UNITY_EDITOR
            if (EditorApplication.isPlaying) {
                meshFilter.mesh = mesh;
                meshCollider.sharedMesh = mesh;
            }
            else
            {
                //saveMesh
                savePath = "Assets/Plugins/Shapes3D/GeneratedMeshes/" + prevName + gameObject.GetInstanceID() + ".asset";

                if (!prevName.Equals(gameObject.name))
                {
                    if (prevName.Equals(""))
                    {
                        prevName = gameObject.name;
                    }
                    else
                    {
                        AssetDatabase.DeleteAsset(savePath);
                        prevName = gameObject.name;
                    }
                    savePath = "Assets/Plugins/Shapes3D/GeneratedMeshes/" + prevName + gameObject.GetInstanceID() + ".asset";
                }
                AssetDatabase.CreateAsset(mesh, savePath);

                meshFilter.mesh = AssetDatabase.LoadAssetAtPath(savePath, typeof(Mesh)) as Mesh;
                meshCollider.sharedMesh = AssetDatabase.LoadAssetAtPath(savePath, typeof(Mesh)) as Mesh;
            }
#else
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
#endif

        }
#if UNITY_EDITOR
        public void DeleteMeshWhenDeletedFromScene()
        {
            if (EditorApplication.isPlaying)
                return;
            if (isNotPrefab && !string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(savePath))) //check if not prefab and path is valid
                AssetDatabase.DeleteAsset(savePath);
        }
#endif

        public void ReverseNormals()
        {
            if (currentMesh == null)
                return;

            currentMesh.flipNormals = !currentMesh.flipNormals;
            Recreate();
        }

        public void SetMeshIndex(int index)
        {
            selectedMeshIndex = index;
        }

        private void ChangePivotPoint()
        {
            for (int i = 0; i < currentMesh.meshVertices.Length; i++)
            {
                currentMesh.meshVertices[i] = new Vector3(
                    (currentMesh.meshVertices[i].x - pivot.x) * scale.x,
                    (currentMesh.meshVertices[i].y - pivot.y) * scale.y,
                    (currentMesh.meshVertices[i].z - pivot.z) * scale.z
                    );
            }
        }

        public void SetIsNotPrefab(bool setbool)
        {
            isNotPrefab = setbool;
        }

    }
}

