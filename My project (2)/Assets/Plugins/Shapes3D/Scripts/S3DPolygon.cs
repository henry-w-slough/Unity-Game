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

    [RequireComponent(typeof(S3DMesh))]
    public class S3DPolygon : S3DComplete
    {
        [SerializeField, HideInInspector] private Vector3 taperForTop = Vector3.one;
        [SerializeField, HideInInspector] private Vector3 taperForBottom = Vector3.one;

        [SerializeField, HideInInspector] private int vertex = 10;

        [SerializeField, HideInInspector] private float thickness = 0.1f;

        [SerializeField, HideInInspector] private float rotateByPiRad = 0f;
        [SerializeField, HideInInspector] private bool hollow = false;
        [SerializeField, HideInInspector] private bool convergeBottom = false;
        [SerializeField, HideInInspector] private bool convergeTop = false;
        [SerializeField, HideInInspector] private bool converging = false;
        [SerializeField, HideInInspector] private int sideRange = 1000;
        [SerializeField, HideInInspector] private float topCap = 0f;
        [SerializeField, HideInInspector] private float bottomCap = 0f;
        [SerializeField, HideInInspector] private bool fullTopCap = false;
        [SerializeField, HideInInspector] private bool fullBottomCap = false;
        public override List<S3DData> GetMeshData()
        {

            if (taperForBottom.x == 0 && taperForBottom.z == 0)
            {
                convergeBottom = true;
            }
            else if (taperForTop.x == 0 && taperForTop.z == 0)
            {
                convergeTop = true;
            }
            else
            {
                convergeBottom = convergeTop = false;
            }
            converging = convergeBottom || convergeTop;

            if (hollow)
            {
                if (convergeBottom || convergeTop)
                    return GetHollowConeData();
                else
                    return GetMeshHollowData();
            }

            List<S3DData> meshData = new List<S3DData>();
            //CreateBottomFace
            string faceEqX = Mathf.Max(0.01f, taperForBottom.x) + "*0.5f*cos(t-" + rotateByPiRad + "*pi" + ")";
            string faceEqZ = Mathf.Max(0.01f, taperForBottom.z) + "*0.5f *sin(t-" + rotateByPiRad + "*pi" + ")";
            string faceEqXTop = Mathf.Max(0.01f, taperForTop.x) + "*0.5f*cos(t-" + rotateByPiRad + "*pi" + ")";
            string faceEqZTop = Mathf.Max(0.01f, taperForTop.z) + "*0.5f *sin(t-" + rotateByPiRad + "*pi" + ")";
            S3DFace bottomFace = S3DEqToVertices.GenerateFaceFromEquation(faceEqX, faceEqZ, vertex);
            S3DFace topFace = S3DEqToVertices.GenerateFaceFromEquation(faceEqXTop, faceEqZTop, vertex);


            if (convergeBottom)
            {
                for (int i = 0; i < bottomFace.meshVertices.Length; i++)
                {
                    bottomFace.meshVertices[i] = new Vector3(0f, -0.5f, 0f);
                }
            }
            else if (convergeTop)
            {
                for (int i = 0; i < topFace.meshVertices.Length; i++)
                {
                    topFace.meshVertices[i] = new Vector3(0f, 0.5f, 0f);
                }
            }

            S3DSides side = new(bottomFace, topFace, 0, bottomFace.shapeVertices.Count, 0.5f);

            if (!converging || convergeTop)
                meshData.Add(bottomFace.Reverse());
            meshData.Add(side);
            if (!converging || convergeBottom)
                meshData.Add(topFace.RaiseHeight() as S3DFace);
            return meshData;
        }

        private List<S3DData> GetMeshHollowData()
        {
            int sideRange = this.sideRange + 1;
            List<S3DData> meshData = new List<S3DData>();
            //bottom
            string faceEqX = Mathf.Max(0.01f, taperForBottom.x) + "*0.5f*cos(t-" + rotateByPiRad + "*pi" + ")";
            string faceEqZ = Mathf.Max(0.01f, taperForBottom.z) + "*0.5f*sin(t-" + rotateByPiRad + "*pi" + ")";
            string holeEqX = Mathf.Max(0.01f, taperForBottom.x) + "*" + (0.5f - thickness) + "*cos(t-" + rotateByPiRad + "*pi" + ")";
            string holeEqZ = Mathf.Max(0.01f, taperForBottom.z) + "*" + (0.5f - thickness) + "*sin(t-" + rotateByPiRad + "*pi" + ")";

            //top
            string faceEqXTop = Mathf.Max(0.01f, taperForTop.x) + "*0.5f*cos(t-" + rotateByPiRad + "*pi" + ")";
            string faceEqZTop = Mathf.Max(0.01f, taperForTop.z) + "*0.5f*sin(t-" + rotateByPiRad + "*pi" + ")";
            string holeEqXTop = Mathf.Max(0.01f, taperForTop.x) + "*" + (0.5f - thickness) + "*cos(t-" + rotateByPiRad + "*pi" + ")";
            string holeEqZTop = Mathf.Max(0.01f, taperForTop.z) + "*" + (0.5f - thickness) + "*sin(t-" + rotateByPiRad + "*pi" + ")";

            //CreateBottomFace
            S3DFace bottomFace = S3DEqToVertices.GenerateFaceFromEquation(faceEqX, faceEqZ, vertex, sideRange);
            S3DFace bottomHole = S3DEqToVertices.GenerateFaceFromEquation(holeEqX, holeEqZ, vertex, sideRange);
            S3DFace bottom = thickness > 0 ? S3DBooleanOperator.Subtract(bottomFace, bottomHole, true, sideRange <= vertex, sideRange) : S3DBooleanOperator.Subtract(bottomHole, bottomFace, true, sideRange <= vertex, sideRange);

            //CreateTopFace
            S3DFace topFace = S3DEqToVertices.GenerateFaceFromEquation(faceEqXTop, faceEqZTop, vertex, sideRange);
            S3DFace topHole = S3DEqToVertices.GenerateFaceFromEquation(holeEqXTop, holeEqZTop, vertex, sideRange);
            S3DFace top = thickness > 0 ? S3DBooleanOperator.Subtract(topFace, topHole, true, sideRange <= vertex, sideRange) : S3DBooleanOperator.Subtract(topHole, topFace, true, sideRange <= vertex, sideRange);
            top = top.RaiseHeight();

            //Create inner side
            S3DSides innerSide = new(bottomHole, topHole, 0, bottomHole.meshVertices.Length, (0.5f - thickness), false, sideRange <= vertex);
            S3DSides outerSide = new(bottomFace, topFace, 0, bottomFace.meshVertices.Length, 0.5f, false, sideRange <= vertex);

            int l = outerSide.meshVertices.Length;
            //Create side faces
            S3DSideFace sideFaceInit = new(new List<Vector3> { outerSide.meshVertices[l-1], outerSide.meshVertices[l - 2],
           innerSide.meshVertices[l-2], innerSide.meshVertices[l - 1] });
            S3DSideFace sideFaceEnd = new(new List<Vector3> { innerSide.meshVertices[l-5], innerSide.meshVertices[l - 6], outerSide.meshVertices[l - 6],
           outerSide.meshVertices[l - 5]});

            meshData.Add(bottom.Reverse());
            meshData.Add(thickness > 0 ? innerSide.Reverse() : innerSide);
            meshData.Add(thickness > 0 ? outerSide : outerSide.Reverse());
            meshData.Add(top);
            meshData.Add(thickness > 0 ? sideFaceInit : sideFaceInit.Reverse());
            meshData.Add(thickness > 0 ? sideFaceEnd : sideFaceEnd.Reverse());
            if (topCap > 0f)
            {
                if (sideRange > vertex) fullTopCap = true;
                S3DFace face = fullTopCap ? S3DEqToVertices.GenerateFaceFromEquation(faceEqXTop, faceEqZTop, vertex).RaiseHeight() : topHole;
                S3DFace topCapInner = new S3DFace(face);
                S3DFace taperTo = S3DEqToVertices.GenerateFaceFromEquation(fullTopCap? faceEqX:holeEqX, fullTopCap ? faceEqZ : holeEqZ, vertex);
                topCapInner.ChangeHeight(0.5f - topCap);

                //if taper, then adjust to taper
                
                for (int i = 0; i < topCapInner.meshVertices.Length; i++)
                {
                    topCapInner.meshVertices[i] = Vector3.Lerp(face.meshVertices[i], taperTo.meshVertices[i], topCap / (face.meshVertices[i].y - taperTo.meshVertices[i].y));
                }

                if (sideRange <= vertex)
                {
                    S3DSides topCapSide = new(topCapInner, face, vertex - (vertex - sideRange + 1), vertex - sideRange + 1, 0.5f);
                    meshData.Add(topCapSide);

                }
                //S3DSides topCapSide = new(topCapInner, face, sideRange - 1, vertex - sideRange + 2, 0.5f);

                meshData.Add(face);
                meshData.Add(topCapInner.Reverse());
            }
            if (bottomCap > 0f)
            {
                if (sideRange > vertex) fullBottomCap = true;
                S3DFace face = fullBottomCap ? S3DEqToVertices.GenerateFaceFromEquation(faceEqX, faceEqZ, vertex) : bottomHole.ChangeHeight(-0.5f);
                S3DFace bottomCapInnerFace = new(face);
                S3DFace taperToTop = S3DEqToVertices.GenerateFaceFromEquation(fullBottomCap? faceEqXTop:holeEqXTop, fullBottomCap ? faceEqZTop : holeEqZTop, vertex).RaiseHeight();
                bottomCapInnerFace.ChangeHeight(-0.5f + bottomCap);

                //if taper, then adjust to taper
                for (int i = 0; i < bottomCapInnerFace.meshVertices.Length; i++)
                {
                    bottomCapInnerFace.meshVertices[i] = Vector3.Lerp(face.meshVertices[i], taperToTop.meshVertices[i], bottomCap / (taperToTop.meshVertices[i].y - face.meshVertices[i].y));
                }

                if (sideRange <= vertex)
                {
                    S3DSides bottomCapSide = new(face, bottomCapInnerFace, vertex - (vertex - sideRange + 1), vertex - sideRange + 1, 0.5f, true);
                    meshData.Add(bottomCapSide);
                }
                meshData.Add(face.Reverse());
                meshData.Add(bottomCapInnerFace);
            }
            return meshData;
        }

        private List<S3DData> GetHollowConeData()
        {
            //Create outer cone
            List<S3DData> meshData = new List<S3DData>();
            string outerFaceEqX = Mathf.Max(0.01f, taperForBottom.x) + "*0.5f*cos(t-" + rotateByPiRad + "*pi" + ")";
            string outerFaceEqZ = Mathf.Max(0.01f, taperForBottom.z) + "*0.5f *sin(t-" + rotateByPiRad + "*pi" + ")";
            string outerFaceEqXTop = Mathf.Max(0.01f, taperForTop.x) + "*0.5f*cos(t-" + rotateByPiRad + "*pi" + ")";
            string outerFaceEqZTop = Mathf.Max(0.01f, taperForTop.z) + "*0.5f *sin(t-" + rotateByPiRad + "*pi" + ")";
            S3DFace outerBottomFace = S3DEqToVertices.GenerateFaceFromEquation(outerFaceEqX, outerFaceEqZ, vertex, sideRange);
            S3DFace outerTopFace = S3DEqToVertices.GenerateFaceFromEquation(outerFaceEqXTop, outerFaceEqZTop, vertex, sideRange);
            //Inner cone data
            string innerFaceEqX = Mathf.Max(0.01f, taperForBottom.x) + "*" + (0.5f - thickness) + "*cos(t-" + rotateByPiRad + "*pi" + ")";
            string innerFaceEqZ = Mathf.Max(0.01f, taperForBottom.z) + "*" + (0.5f - thickness) + "*sin(t-" + rotateByPiRad + "*pi" + ")";
            string innerFaceEqXTop = Mathf.Max(0.01f, taperForTop.x) + "*" + (0.5f - thickness) + "*cos(t-" + rotateByPiRad + "*pi" + ")";
            string innerFaceEqZTop = Mathf.Max(0.01f, taperForTop.z) + "*" + (0.5f - thickness) + "*sin(t-" + rotateByPiRad + "*pi" + ")";
            S3DFace innerBottomFace = S3DEqToVertices.GenerateFaceFromEquation(innerFaceEqX, innerFaceEqZ, vertex, sideRange);
            S3DFace innerTopFace = S3DEqToVertices.GenerateFaceFromEquation(innerFaceEqXTop, innerFaceEqZTop, vertex, sideRange);

            //Cut face
            S3DFace endCap = convergeBottom ? S3DBooleanOperator.Subtract(outerTopFace, innerTopFace, true, sideRange != vertex, sideRange)
                : S3DBooleanOperator.Subtract(outerBottomFace, innerBottomFace, true, sideRange != vertex, sideRange);

            //Create outer cone
            if (convergeBottom)
            {
                for (int i = 0; i < outerBottomFace.meshVertices.Length; i++)
                {
                    outerBottomFace.meshVertices[i] = new Vector3(0f, -0.5f, 0f);
                }
                endCap.RaiseHeight();
            }
            else if (convergeTop)
            {
                for (int i = 0; i < outerTopFace.meshVertices.Length; i++)
                {
                    outerTopFace.meshVertices[i] = new Vector3(0f, 0.5f, 0f);
                }
            }

            S3DSides outerSide = new(outerBottomFace, outerTopFace, 0, outerBottomFace.meshVertices.Length, 0.5f, false, sideRange < vertex && sideRange != vertex);

            //Create inner cone
            if (convergeBottom)
            {
                for (int i = 0; i < innerBottomFace.meshVertices.Length; i++)
                {
                    innerBottomFace.meshVertices[i] = new Vector3(0f, -0.5f + thickness, 0f);
                }
            }
            else if (convergeTop)
            {
                for (int i = 0; i < innerTopFace.meshVertices.Length; i++)
                {
                    innerTopFace.meshVertices[i] = new Vector3(0f, 0.5f - thickness, 0f);
                }
            }

            S3DSides innerSide = new(innerBottomFace, innerTopFace, 0, innerBottomFace.meshVertices.Length, 0.5f, convergeTop, sideRange < vertex && sideRange != vertex);

            int l = outerSide.meshVertices.Length;
            S3DSideFace sideFaceInit = new(new List<Vector3> { outerSide.meshVertices[l-1], outerSide.meshVertices[l - 2],
           innerSide.meshVertices[l-2], innerSide.meshVertices[l - 1] });
            S3DSideFace sideFaceEnd = new(new List<Vector3> { innerSide.meshVertices[l-5], innerSide.meshVertices[l - 6], outerSide.meshVertices[l - 6],
           outerSide.meshVertices[l - 5]});

            if (thickness > 0f)
            {
                meshData.Add(outerSide);
                meshData.Add(innerSide.Reverse());
                meshData.Add(convergeBottom ? endCap : endCap.Reverse());
                meshData.Add(sideFaceInit);
                meshData.Add(sideFaceEnd);
            }
            else
            {
                meshData.Add(outerSide.Reverse());
                meshData.Add(innerSide);
                meshData.Add(convergeBottom ? endCap.Reverse() : endCap);
                meshData.Add(sideFaceInit.Reverse());
                meshData.Add(sideFaceEnd.Reverse());
            }
            //CONE CAPS
            if (convergeBottom && topCap > 0f)
            {
                if (sideRange > vertex) fullTopCap = true;
                S3DFace face = fullTopCap ? S3DEqToVertices.GenerateFaceFromEquation(outerFaceEqXTop, outerFaceEqZTop, vertex).RaiseHeight() : innerTopFace;
                S3DFace topCapInnerFace = new S3DFace(face);
                Vector3 bottomInnerVertex = fullTopCap ? outerSide.meshVertices[0] : innerSide.meshVertices[0];
                for (int i = 0; i < topCapInnerFace.meshVertices.Length; i++)
                {
                    topCapInnerFace.meshVertices[i] = Vector3.Lerp(face.meshVertices[i], bottomInnerVertex, topCap / (face.meshVertices[i].y - bottomInnerVertex.y));
                }

                //topCapInner.ChangeHeight(0.5f - topCap);
                if (sideRange <= vertex)
                {
                    S3DSides topCapSide = new(topCapInnerFace, face, vertex - (vertex - sideRange + 1), vertex - sideRange + 1, 0.5f);
                    meshData.Add(topCapSide);

                }
                //S3DSides topCapSide = new(topCapInner, face, sideRange - 1, vertex - sideRange + 2, 0.5f);

                meshData.Add(face);
                meshData.Add(topCapInnerFace.Reverse());
            }
            if (convergeTop && bottomCap > 0f)
            {
                if (sideRange > vertex) fullBottomCap = true;
                S3DFace face = fullBottomCap ? S3DEqToVertices.GenerateFaceFromEquation(outerFaceEqX, outerFaceEqZ, vertex) : innerBottomFace.ChangeHeight(-0.5f);
                S3DFace bottomCapInnerFace = new(face);
                Vector3 topInnerVertex = fullBottomCap ? outerSide.meshVertices[1] : innerSide.meshVertices[1];
                for (int i = 0; i < bottomCapInnerFace.meshVertices.Length; i++)
                {
                    bottomCapInnerFace.meshVertices[i] = Vector3.Lerp(face.meshVertices[i], topInnerVertex, bottomCap / (topInnerVertex.y - face.meshVertices[i].y));
                }
                //bottomCapInnerFace.ChangeHeight(-0.5f + bottomCap);
                if (sideRange <= vertex)
                {
                    S3DSides bottomCapSide = new(face, bottomCapInnerFace, vertex - (vertex - sideRange + 1), vertex - sideRange + 1, 0.5f, true);
                    meshData.Add(bottomCapSide);
                }
                meshData.Add(face.Reverse());
                meshData.Add(bottomCapInnerFace);
            }


            return meshData;
        }

        #region METHODS TO CALL FROM SCRIPTS

        public S3DPolygon SetVertex(int v)
        {
            vertex = Mathf.Clamp(v, 3, 50);
            return this;
        }

        public S3DPolygon SetScale(Vector3 s)
        {
            scale = new Vector3(
                Mathf.Max(0f, s.x),
                Mathf.Max(0f, s.y),
                Mathf.Max(0f, s.z)
                );
            return this;

        }
        public S3DPolygon SetPivotPoint(Vector3 p)
        {
            pivotPoint = p;
            return this;

        }

        public S3DPolygon SetTaperTop(Vector3 t)
        {
            taperForTop = new Vector3(
                    Mathf.Max(t.x, 0f),
                    1f,
                    Mathf.Max(t.z, 0f));
            return this;

        }

        public S3DPolygon SetTaperBottom(Vector3 t)
        {
            taperForBottom = new Vector3(
                    Mathf.Max(t.x, 0f),
                    1f,
                    Mathf.Max(t.z, 0f));
            return this;

        }

        public S3DPolygon SetHollow(bool b)
        {
            hollow = b;
            return this;

        }

        public S3DPolygon SetThickness(float t)
        {
            float temp = Mathf.Clamp(t, -2f, 0.45f);
            temp = temp == 0f ? 0.01f : temp;
            thickness = temp;
            return this;

        }

        public S3DPolygon SetSideRange(int s)
        {
            sideRange = Mathf.Clamp(s, 3, vertex);
            return this;

        }

        public S3DPolygon SetTopCapThickness(float t)
        {
            topCap = Mathf.Clamp(t, 0f, bottomCap > 0.5f ? 1f - bottomCap : 1f);
            return this;

        }

        public S3DPolygon SetBottomCapThickness(float t)
        {
            bottomCap = Mathf.Clamp(t, 0f, topCap > 0.5f ? 1f - topCap : 1f);
            return this;

        }

        public S3DPolygon SetFullTopCap(bool b)
        {
            fullTopCap = b;
            return this;

        }

        public S3DPolygon SetFullBottomCap(bool b)
        {
            fullBottomCap = b;
            return this;

        }

        public S3DPolygon SetRotateByPIRadians(float rad)
        {
            rotateByPiRad = rad;
            return this;
        }

        #endregion

#if UNITY_EDITOR
        [CustomEditor(typeof(S3DPolygon))]
        public class S3DPolygonEditor : Editor
        {
            private SerializedProperty staperForTop;
            private SerializedProperty staperForBottom;

            private SerializedProperty svertex;

            private SerializedProperty sthickness;

            private SerializedProperty srotateByPiRad;
            private SerializedProperty shollow;
            private SerializedProperty sconvergeBottom;
            private SerializedProperty sconvergeTop;
            private SerializedProperty sconverging;
            private SerializedProperty ssideRange;
            private SerializedProperty stopCap;
            private SerializedProperty sbottomCap;
            private SerializedProperty sfullTopCap;
            private SerializedProperty sfullBottomCap;
            private SerializedProperty sscale;
            private SerializedProperty spivotPoint;
            private void OnEnable()
            {
                staperForTop = serializedObject.FindProperty("taperForTop");
                staperForBottom = serializedObject.FindProperty("taperForBottom");
                svertex = serializedObject.FindProperty("vertex");
                sthickness = serializedObject.FindProperty("thickness");
                srotateByPiRad = serializedObject.FindProperty("rotateByPiRad");
                shollow = serializedObject.FindProperty("hollow");
                sconvergeBottom = serializedObject.FindProperty("convergeBottom");
                sconvergeTop = serializedObject.FindProperty("convergeTop");
                sconverging = serializedObject.FindProperty("converging");
                ssideRange = serializedObject.FindProperty("sideRange");
                stopCap = serializedObject.FindProperty("topCap");
                sbottomCap = serializedObject.FindProperty("bottomCap");
                sfullTopCap = serializedObject.FindProperty("fullTopCap");
                sfullBottomCap = serializedObject.FindProperty("fullBottomCap");
                sscale = serializedObject.FindProperty("scale");
                spivotPoint = serializedObject.FindProperty("pivotPoint");

                Undo.undoRedoPerformed += OnUndoRedoPerformed;
                var s3dPolygon = (S3DPolygon) target;
                s3dPolygon?.Recreate();
            }

            private void OnUndoRedoPerformed()
            {
                var s3dPolygon = (S3DPolygon)target;
                s3dPolygon?.Recreate();
            }

            private void OnDisable()
            {
                Undo.undoRedoPerformed -= OnUndoRedoPerformed;
            }

            public override void OnInspectorGUI()
            {
                var s3dPolygon = (S3DPolygon)target;

                Undo.RecordObject(s3dPolygon, "Any changes");
                serializedObject.Update();
                EditorGUI.BeginChangeCheck();

                base.OnInspectorGUI();


                DrawCustom(s3dPolygon);

                if (EditorGUI.EndChangeCheck())
                {
                    s3dPolygon.Recreate();
                    EditorUtility.SetDirty(s3dPolygon);
                }
                serializedObject.ApplyModifiedProperties();


            }

            private static void DrawCustom(S3DPolygon s3dPolygon)
            {
                EditorGUILayout.Space();
                //s3dPolygon.vertex = Mathf.Max(3, EditorGUILayout.IntField("Vertexss", s3dPolygon.vertex));
                s3dPolygon.vertex = EditorGUILayout.IntSlider("Vertex", s3dPolygon.vertex, 3, 50);

                s3dPolygon.scale = EditorGUILayout.Vector3Field("Scale", new Vector3(
                    Mathf.Max(s3dPolygon.scale.x, 0f),
                    Mathf.Max(s3dPolygon.scale.y, 0f),
                    Mathf.Max(s3dPolygon.scale.z, 0f)));

                s3dPolygon.pivotPoint = EditorGUILayout.Vector3Field("Pivot Point", new Vector3(
                    Mathf.Clamp(s3dPolygon.pivotPoint.x, -0.5f, 0.5f),
                    Mathf.Clamp(s3dPolygon.pivotPoint.y, -0.5f, 0.5f),
                    Mathf.Clamp(s3dPolygon.pivotPoint.z, -0.5f, 0.5f)));

                s3dPolygon.taperForTop = EditorGUILayout.Vector3Field("Taper for Top", new Vector3(
                    Mathf.Max(s3dPolygon.taperForTop.x, 0f),
                    1f,
                    Mathf.Max(s3dPolygon.taperForTop.z, 0f)));

                s3dPolygon.taperForBottom = EditorGUILayout.Vector3Field("Taper for Bottom", new Vector3(
                    Mathf.Max(s3dPolygon.taperForBottom.x, 0f),
                    1f,
                    Mathf.Max(s3dPolygon.taperForBottom.z, 0f)));

                s3dPolygon.hollow = EditorGUILayout.Toggle("Hollow", s3dPolygon.hollow);
                if (s3dPolygon.hollow)
                {

                    float t = EditorGUILayout.Slider("Thickness", s3dPolygon.thickness, -2f, 0.45f);
                    s3dPolygon.thickness = t == 0f ? 0.01f : t;
                    s3dPolygon.sideRange = EditorGUILayout.IntSlider("Side Range", s3dPolygon.sideRange, 3, s3dPolygon.vertex);

                    if (!s3dPolygon.convergeTop)
                    {
                        s3dPolygon.topCap = EditorGUILayout.Slider("Top Cap Thickness", s3dPolygon.topCap, 0f, s3dPolygon.bottomCap > 0.5f ? 1f - s3dPolygon.bottomCap : 1f);
                        if (s3dPolygon.topCap > 0f && s3dPolygon.sideRange < s3dPolygon.vertex)
                            s3dPolygon.fullTopCap = EditorGUILayout.Toggle("Full Top Cap", s3dPolygon.fullTopCap);
                    }
                    if (!s3dPolygon.convergeBottom)
                    {
                        s3dPolygon.bottomCap = EditorGUILayout.Slider("Bottom Cap Thickness", s3dPolygon.bottomCap, 0f, s3dPolygon.topCap > 0.5f ? 1f - s3dPolygon.topCap : 1f);
                        if (s3dPolygon.bottomCap > 0f && s3dPolygon.sideRange < s3dPolygon.vertex)
                            s3dPolygon.fullBottomCap = EditorGUILayout.Toggle("Full Bottom Cap", s3dPolygon.fullBottomCap);
                    }
                }

                s3dPolygon.rotateByPiRad = EditorGUILayout.Slider("Rotate by pi radians", s3dPolygon.rotateByPiRad, 0f, 2f);

            }

        }
#endif
    }

}

