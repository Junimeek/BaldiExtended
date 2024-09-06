using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JuniTools : EditorWindow
{
    string custString = "String Here";
    bool groupEnabled;
    bool optionalSettings = true;
    float jumpMod = 1.0f;
    float impactMod = 0.5f;

    [MenuItem("Tools/JuniDev/Build Asset Bundles")]
    private static void BuildBundles()
    {
        Debug.Log("blargh");
    }

    [MenuItem("Tools/JuniDev/Baldi Painter")]
    private static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(JuniTools));
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        custString = EditorGUILayout.TextField("Text Field", custString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        optionalSettings = EditorGUILayout.Toggle("Double Jumping Enabled", optionalSettings);
        jumpMod = EditorGUILayout.Slider("Jump Modifier", jumpMod, -5, 5);
        impactMod = EditorGUILayout.Slider("Impact Modifier", impactMod, -5, 5);
        EditorGUILayout.EndToggleGroup();

        GUI.backgroundColor = Color.red;

        GUILayout.FlexibleSpace();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reset", GUILayout.Width(100), GUILayout.Height(30)))
        {
            custString = "String Here";
            optionalSettings = false;
            jumpMod = 1.0f;
            impactMod = 0.5f;
        }
        EditorGUILayout.EndHorizontal();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawCube(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
    }
}
