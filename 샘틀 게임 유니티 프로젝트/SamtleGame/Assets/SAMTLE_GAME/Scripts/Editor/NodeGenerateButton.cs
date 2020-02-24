using UnityEngine;
using UnityEditor;

using MIT.SamtleGame.Stage2.NPC;

[CustomEditor(typeof(GrandFather))]
public class NodeGenerateButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        GrandFather generator = (GrandFather)target;
        if(GUILayout.Button("웨이포인트 생성", GUILayout.Width(120), GUILayout.Height(30)))
        {
            generator.GenerateWayPoint();
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if(GUILayout.Button("플레이어를 위한 웨이포인트 생성", GUILayout.Width(240), GUILayout.Height(30)))
        {
            generator.GenerateWayPointForPlayer();
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if(GUILayout.Button("웨이포인트 id 초기화", GUILayout.Width(240), GUILayout.Height(30)))
        {
            generator.InitWayPoint();
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if(GUILayout.Button("플레이어 웨이포인트 id 초기화", GUILayout.Width(240), GUILayout.Height(30)))
        {
            generator.InitWayPointForPlayer();
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
