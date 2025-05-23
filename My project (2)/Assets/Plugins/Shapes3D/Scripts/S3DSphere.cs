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
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    using static UnityEngine.GraphicsBuffer;

    public class S3DSphere : S3DComplete
    {


        [SerializeField, HideInInspector] private int resolution = 16;

        [SerializeField, HideInInspector] private float radius = 0.5f;
        [SerializeField, HideInInspector] private bool hollow;
        [SerializeField, HideInInspector] private float thickness = 0.1f;
        [SerializeField, HideInInspector] private float height = 1f;
        [SerializeField, HideInInspector] private float heightInnerLimit;
        [SerializeField, HideInInspector] private List<Vector3> skippedVertices = new List<Vector3>();


        public override List<S3DData> GetMeshData()
        {

            S3DData outerSphere = CreateSphere(radius);
            List<S3DData> meshData = new List<S3DData> { outerSphere };

            if (height < 1f)
            {
                List<Vector2> vertices = new List<Vector2>();
                float phi = (1f - height) * Mathf.PI;
                float r = radius * Mathf.Sin(phi);
                float segment = (2f * Mathf.PI) / resolution;
                for (int i = 0; i < resolution; i++)
                {
                    vertices.Add(new Vector2(r * Mathf.Cos(i * segment), r * Mathf.Sin(i * segment)));
                }
                S3DFace outerFace = new S3DFace(vertices);
                outerFace.ChangeHeight(radius * Mathf.Cos(phi));

                if (hollow)
                {
                    List<Vector2> innerVertices = new List<Vector2>();

                    float phi2 = Mathf.Atan2(r - thickness, radius * Mathf.Cos(phi));
                    float radius2;
                    float r2;

                    if (phi2 == Mathf.PI / 2)
                    {
                        radius2 = radius - thickness;
                        r2 = radius2;
                    }
                    else
                    {
                        radius2 = radius * Mathf.Cos(phi) / Mathf.Cos(phi2);
                        r2 = radius2 * Mathf.Sin(phi2);
                    }

                    heightInnerLimit = GetHeightLimitInner(height * radius * 2f, radius2);
                    for (int i = 0; i < resolution; i++)
                    {
                        innerVertices.Add(new Vector2(r2 * Mathf.Cos(i * segment), r2 * Mathf.Sin(i * segment)));
                    }
                    S3DData innerSphere = CreateSphere(radius2, true);
                    meshData.Add(innerSphere);
                    S3DFace innerFace;
                    if (skippedVertices.Count < resolution - 1)
                    {
                        innerFace = new S3DFace(innerVertices);
                        innerFace.ChangeHeight(radius * Mathf.Cos(phi));
                    }
                    else
                    {
                        innerFace = new S3DFace(S3DUtils.ChangeV3xztoV2(skippedVertices));
                        innerFace.ChangeHeight(skippedVertices[0].y);
                    }

                    S3DFace endCap = S3DBooleanOperator.Subtract(outerFace, innerFace, true);
                    if (skippedVertices.Count < resolution - 1)
                    {
                        endCap.ChangeHeight(radius * Mathf.Cos(phi));
                    }
                    else
                    {
                        endCap.ChangeHeight(skippedVertices[0].y, resolution, resolution * 2 - 1);
                        endCap.ChangeHeight(radius * Mathf.Cos(phi), 0, resolution - 1);
                    }

                    meshData.Add(endCap);
                }
                else
                    meshData.Add(outerFace);

            }
            return meshData;

        }

        private S3DData CreateSphere(float radi, bool inner = false)
        {
            int numVertices = (resolution + 1) * (resolution + 1);
            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];
            int[] triangles = new int[resolution * resolution * 6];
            skippedVertices.Clear();

            for (int i = 0, y = 0; y <= resolution; y++)
            {
                for (int x = 0; x <= resolution; x++, i++)
                {
                    float u = x / (float)resolution;
                    float v = y / (float)resolution;
                    if (!inner)
                        v = Mathf.Max(v, 1f - height);
                    else
                    {
                        v = Mathf.Max(v, 1f - heightInnerLimit);
                        if (v == 1f - heightInnerLimit && skippedVertices.Count < resolution)
                        {
                            skippedVertices.Add(new Vector3(radi * Mathf.Cos(u * Mathf.PI * 2f) * Mathf.Sin(v * Mathf.PI),
                                radi * Mathf.Cos(v * Mathf.PI),
                                radi * Mathf.Sin(u * Mathf.PI * 2f) * Mathf.Sin(v * Mathf.PI)));
                        }
                    }

                    float theta = u * Mathf.PI * 2f;
                    float phi = v * Mathf.PI;

                    float sinTheta = Mathf.Sin(theta);
                    float cosTheta = Mathf.Cos(theta);
                    float sinPhi = Mathf.Sin(phi);
                    float cosPhi = Mathf.Cos(phi);

                    float xPos = radi * cosTheta * sinPhi;
                    float yPos = radi * cosPhi;
                    float zPos = radi * sinTheta * sinPhi;

                    vertices[i] = new Vector3(xPos, yPos, zPos);
                    uv[i] = new Vector2(u, 1f - v);
                }
            }

            int vert = 0;
            int tris = 0;

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    triangles[tris] = vert;
                    triangles[tris + 1] = vert + resolution + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + resolution + 1;
                    triangles[tris + 5] = vert + resolution + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }
            if (!inner)
                System.Array.Reverse(triangles);

            return new S3DData(vertices, triangles, uv);
        }

        private float GetHeightLimitInner(float v, float radius2)
        {
            //if (v == 0.5f)
            //    return 0.5f;
            //float d1 = 2 * radius;
            //float denom = (d1 - (radius - radius2)) / d1 - (radius - radius2);
            //float m = 1f / denom;
            //float c = -m * ((radius - radius2) / d1);
            //Debug.Log("m: " + m);
            //Debug.Log("c: " + c);
            //float x1 = radius - radius2; //0.25
            //Debug.Log(x1);
            //Debug.Log("v: " + v);
            //Debug.Log("return: " + (m * v + c));
            float d1 = 2f * radius;
            float x1 = radius - radius2;
            float y1 = 0f;
            float x2 = d1 - x1;
            float y2 = 1f;
            float m = (y2 - y1) / (x2 - x1);
            float c = -m * x1;

            return m * v + c;
        }
        #region METHODS TO CALL FROM SCRIPT

        public S3DSphere SetResolution(int r)
        {
            resolution = Mathf.Clamp(r, 4, 25);
            return this;

        }

        public S3DSphere SetScale(Vector3 s)
        {
            scale = new Vector3(
                Mathf.Max(0f, s.x),
                Mathf.Max(0f, s.y),
                Mathf.Max(0f, s.z)
                );
            return this;

        }
        public S3DSphere SetPivotPoint(Vector3 p)
        {
            pivotPoint = p;
            return this;

        }

        public S3DSphere SetRadius(float r)
        {
            radius = Mathf.Max(0.01f,r);
            return this;

        }

        public S3DSphere SetHeight(float h)
        {
            float heightLimit = thickness / radius / 2f; //2f is safety factor
            height = Mathf.Clamp(h,hollow ? Mathf.Max(heightLimit, 0.1f) : 0.1f, hollow ? 1f - heightLimit : 1.0f);
            return this;

        }

        public S3DSphere SetHollow(bool b)
        {
            hollow = b;
            return this;

        }

        public S3DSphere SetThickness(float t)
        {
            thickness = Mathf.Clamp(t, 0f, radius - 0.1f);
            return this;

        }

        #endregion


#if UNITY_EDITOR
        [CustomEditor(typeof(S3DSphere))]
        public class S3DSphereEditor : Editor
        {
            private SerializedProperty sresolution;

            private SerializedProperty sradius;
            private SerializedProperty shollow;
            private SerializedProperty sthickness;
            private SerializedProperty sheight;
            private SerializedProperty sheightInnerLimit;

            private SerializedProperty sscale;
            private SerializedProperty spivotPoint;
            private void OnEnable()
            {
                sresolution = serializedObject.FindProperty("resolution");
                sradius = serializedObject.FindProperty("radius");
                shollow = serializedObject.FindProperty("hollow");
                sthickness = serializedObject.FindProperty("thickness");
                sheight = serializedObject.FindProperty("height");
                sheightInnerLimit = serializedObject.FindProperty("heightInnerLimit");
                sscale = serializedObject.FindProperty("scale");
                spivotPoint = serializedObject.FindProperty("pivotPoint");

                Undo.undoRedoPerformed += OnUndoRedoPerformed;
                var s3dSphere = (S3DSphere)target;
                s3dSphere?.Recreate();
            }

            private void OnUndoRedoPerformed()
            {
                var s3dSphere = (S3DSphere)target;
                s3dSphere?.Recreate();
            }

            private void OnDisable()
            {
                Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            }

            public override void OnInspectorGUI()
            {
                var s3dSphere = (S3DSphere)target;

                Undo.RecordObject(s3dSphere, "Changes");
                serializedObject.Update();

                EditorGUI.BeginChangeCheck();

                base.OnInspectorGUI();
                Draw(s3dSphere);

                if (EditorGUI.EndChangeCheck())
                {
                    s3dSphere.Recreate();
                    EditorUtility.SetDirty(s3dSphere);
                }
                serializedObject.ApplyModifiedProperties();
            }

            private void Draw(S3DSphere s3dSphere)
            {
                float heightLimit = s3dSphere.thickness / s3dSphere.radius / 2f; //2f is safety factor
                s3dSphere.resolution = EditorGUILayout.IntSlider("Resolution", s3dSphere.resolution, 4, 25);
                s3dSphere.scale = EditorGUILayout.Vector3Field("Scale", new Vector3(
                    Mathf.Max(s3dSphere.scale.x, 0f),
                    Mathf.Max(s3dSphere.scale.y, 0f),
                    Mathf.Max(s3dSphere.scale.z, 0f)));

                s3dSphere.pivotPoint = EditorGUILayout.Vector3Field("Pivot Point", s3dSphere.pivotPoint);
                s3dSphere.radius = EditorGUILayout.Slider("Radius", s3dSphere.radius, 0.1f, 10f);
                s3dSphere.height = EditorGUILayout.Slider("Height", s3dSphere.height, s3dSphere.hollow ? Mathf.Max(heightLimit, 0.1f) : 0.1f, s3dSphere.hollow ? 1f - heightLimit : 1.0f);
                s3dSphere.hollow = EditorGUILayout.Toggle("Hollow", s3dSphere.hollow);
                if (s3dSphere.hollow)
                {
                    s3dSphere.thickness = EditorGUILayout.Slider("Thickness", s3dSphere.thickness, 0f, s3dSphere.radius - 0.1f);
                }
            }
        }
#endif

    }
}
