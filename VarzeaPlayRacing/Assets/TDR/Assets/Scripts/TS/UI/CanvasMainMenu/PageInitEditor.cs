﻿//Description: PageInitEditor: Custom Editor
#if (UNITY_EDITOR)
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TS.Generics
{
    [CustomEditor(typeof(PageInit))]
    public class PageInitEditor : Editor
    {
        SerializedProperty SeeInspector;                                            // use to draw default Inspector
        SerializedProperty methodsList;

        public EditorMethods_Pc editorMethods;                                         // access the component EditorMethods
        public AP_MethodModule_Pc methodModule;

        #region Init Inspector Color
        private Texture2D MakeTex(int width, int height, Color col)
        {                       // use to change the GUIStyle
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private List<Texture2D> listTex = new List<Texture2D>();
        public List<GUIStyle> listGUIStyle = new List<GUIStyle>();
        private List<Color> listColor = new List<Color>();
        #endregion

        void OnEnable()
        {
            #region Init Inspector Color
            listColor.Clear();
            listGUIStyle.Clear();
            for (var i = 0; i < inspectorColor.listColor.Length; i++)
            {
                listTex.Add(MakeTex(2, 2, inspectorColor.ReturnColor(i)));
                listGUIStyle.Add(new GUIStyle());
                listGUIStyle[i] = new GUIStyle(); listGUIStyle[i].normal.background = listTex[i];
            }
            #endregion

            #region
            // Setup the SerializedProperties.
            SeeInspector = serializedObject.FindProperty("SeeInspector");
            methodsList = serializedObject.FindProperty("methodsList");

            editorMethods = new EditorMethods_Pc();
            methodModule = new AP_MethodModule_Pc();
            #endregion
        }

        public override void OnInspectorGUI()
        {
            #region
            if (SeeInspector.boolValue)                         // If true Default Inspector is drawn on screen
                DrawDefaultInspector();

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("See Inspector :", GUILayout.Width(85));
            EditorGUILayout.PropertyField(SeeInspector, new GUIContent(""), GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();

            displayAllTheMethods(listGUIStyle[1], listGUIStyle[2]);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("");
            #endregion
        }

        //--> display a list of methods call when the scene starts
        private void displayAllTheMethods(GUIStyle style_Yellow_01, GUIStyle style_Blue)
        {
            #region
            //--> Display feedback
            PageInit myScript = (PageInit)target;

            methodModule.displayMethodList("Methods call when scene starts:",
                                           editorMethods,
                                           methodsList,
                                           myScript.methodsList,
                                           style_Blue,
                                           style_Yellow_01,
                                           "The methods are called in the same order as the list. " +
                                           "\nAll methods must be boolean methods. " +
                                           "\nOther methods will be ignored.");

            #endregion
        }

        void OnSceneGUI()
        {
        }
    }
}


#endif
