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
            generator.GenerateNodes();
        }
        
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
