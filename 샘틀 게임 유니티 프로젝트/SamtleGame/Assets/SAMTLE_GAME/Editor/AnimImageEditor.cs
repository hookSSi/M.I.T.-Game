using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimImage))]
public class AnimImageEditor : Editor
{
    AnimImage _animImage = null;

    private void OnEnable() 
    {
        // AnimImage 컴포넌트 가져오기
        _animImage = (AnimImage) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
