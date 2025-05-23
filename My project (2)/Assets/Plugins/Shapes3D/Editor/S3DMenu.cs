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

    public class S3DMenu
    {
        [MenuItem("GameObject/Shapes3D/Cylinder", false, 1)]
        static void InstantiateCylinder()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Cylinder.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Triangular Prism", false, 1)]
        static void InstantiateTriPrism()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Tri Prism.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Cone", false, 1)]
        static void InstantiateCone()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Cone.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Cube", false, 1)]
        static void InstantiateCube()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Cube.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Pyramid", false, 1)]
        static void InstantiatePyramid()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Pyramid.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Rectangular Prism", false, 1)]
        static void InstantiateRectPrism()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Rect Prism.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Sphere", false, 1)]
        static void InstantiateSphere()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Sphere.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Polygon Prism", false, 1)]
        static void InstantiatePolygonPrism()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Polygon Prism.prefab");
        }

        //HOLLOW
        [MenuItem("GameObject/Shapes3D/Hollow Cylinder", false, 1)]
        static void InstantiateHollowCylinder()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Cylinder.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Triangular Prism", false, 1)]
        static void InstantiateHollowTriPrism()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Tri Prism.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Cone", false, 1)]
        static void InstantiateHollowCone()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Cone.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Cube", false, 1)]
        static void InstantiateHollowCube()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Cube.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Pyramid", false, 1)]
        static void InstantiateHollowPyramid()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Pyramid.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Rectangular Prism", false, 1)]
        static void InstantiateHollowRectPrism()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Rect Prism.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Sphere", false, 1)]
        static void InstantiateHollowSphere()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Sphere.prefab");
        }

        [MenuItem("GameObject/Shapes3D/Hollow Polygon Prism", false, 1)]
        static void InstantiateHollowPolygonPrism()
        {
            Create("Assets/Plugins/Shapes3D/Prefabs/S3D Hollow Polygon Prism.prefab");
        }


        private static void Create(string path)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null)
            {
                GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                PrefabUtility.UnpackPrefabInstance(instantiatedPrefab, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

                if (Selection.activeGameObject != null)
                {
                    instantiatedPrefab.transform.parent = Selection.activeGameObject.transform;
                }

                instantiatedPrefab.GetComponent<S3DMesh>()?.Recreate();
            }
            else
            {
                Debug.LogWarning("Cannot find directory: " + path + ". Please put Shapes3D folder in the Plugins folder.");
            }
        }
    }
}
