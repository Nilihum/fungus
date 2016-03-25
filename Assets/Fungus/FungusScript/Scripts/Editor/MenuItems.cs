﻿using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

namespace Fungus
{

    public class MenuItems 
    {
        [MenuItem("Tools/Fungus/Create/Fungus Script", false, 2000)]
        static void CreateFungusScript()
        {
            SpawnPrefab("Prefabs/FungusScript", false);
        }

        [MenuItem("Tools/Fungus/Create/Fungus Bindings", false, 2001)]
        static void CreateFungusBindings()
        {
            SpawnPrefab("Prefabs/FungusBindings", false);
        }

        [MenuItem("Tools/Fungus/Create/Fungus Invoke", false, 2002)]
        static void CreateFungusInvoke()
        {
            SpawnPrefab("Prefabs/FungusInvoke", false);
        }

        [MenuItem("Tools/Fungus/Create/Lua File", false, 2100)]
        static void CreateLuaFile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Create Lua File", "script.txt", "txt", "Please select a file name for the new Lua script. Note: Lua files in Unity use the .txt extension.");
            if(path.Length == 0) 
            {
                return;
            }

            File.WriteAllText(path, "");
            AssetDatabase.Refresh();

            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (asset != null)
            {
                EditorUtility.FocusProjectWindow();
                EditorGUIUtility.PingObject(asset);
            }            
        }
            
        /// <summary>
        /// Spawns a prefab in the scene based on it's filename in a Resources folder in the project.
        /// If centerInScene is true then the object will be placed centered in the view window with z = 0.
        /// If centerInScene is false the the object will be placed at (0,0,0)
        /// </summary>
        public static GameObject SpawnPrefab(string prefabName, bool centerInScene)
        {
            GameObject prefab = Resources.Load<GameObject>(prefabName);
            if (prefab == null)
            {
                return null;
            }

            GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            PrefabUtility.DisconnectPrefabInstance(go);

            if (centerInScene)
            {
                SceneView view = SceneView.lastActiveSceneView;
                if (view != null)
                {
                    Camera sceneCam = view.camera;
                    Vector3 pos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                    pos.z = 0f;
                    go.transform.position = pos;
                }
            }
            else
            {
                go.transform.position = Vector3.zero;
            }

            Selection.activeGameObject = go;

            Undo.RegisterCreatedObjectUndo(go, "Create Object");

            return go;
        }
    }

}
