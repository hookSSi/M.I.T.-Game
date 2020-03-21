using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using MIT.SamtleGame.Intro;

[CustomEditor(typeof(IntroManager))]
public class IntroManagerInspector : Editor
{
    ReorderableList _list;
    float _textHeight = EditorGUIUtility.singleLineHeight * 2f;
    float _space = EditorGUIUtility.singleLineHeight * 0.3f;
    float _labelx = 50f;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {
        // ReorderableList
        _list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("_pages"),
                true, true, true, true);

        // 대화창 높이 조절
        _list.elementHeight = EditorGUIUtility.singleLineHeight * 4.8f;

        // 레이블
        _list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "인트로 씬 페이지", EditorStyles.boldLabel);
        };

        // 이미지/텍스트 요소
        _list.drawElementCallback = DrawElement;

        // 드롭다운 메뉴
        _list.onAddDropdownCallback = PageDropdownMenu;
    }
    
    // 리스트 요소 그리기
    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        float halfOfWidth = rect.width / 2f;

        rect.y += 3;

        // Label
        EditorGUI.LabelField(
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            "===" + (index + 1).ToString() + " 페이지===", EditorStyles.boldLabel);

        // Sprite
        EditorGUI.LabelField(
            new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 90, EditorGUIUtility.singleLineHeight),
            "그림", EditorStyles.wordWrappedLabel);
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight * 2, 90, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("_sprite"), GUIContent.none);

        // Text
        EditorGUI.PropertyField(
            new Rect(rect.x + 90, rect.y + EditorGUIUtility.singleLineHeight, rect.width - 90, _textHeight),
            element.FindPropertyRelative("_text"), GUIContent.none);

        // Delay and duration
        EditorGUI.LabelField(
            new Rect(rect.x, rect.y + _textHeight + _space + EditorGUIUtility.singleLineHeight, _labelx, EditorGUIUtility.singleLineHeight),
            "딜레이");
        EditorGUI.PropertyField(
            new Rect(rect.x + _labelx, rect.y + _textHeight + _space + EditorGUIUtility.singleLineHeight, halfOfWidth - _labelx, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("_delay"), GUIContent.none);
        EditorGUI.LabelField(
            new Rect(rect.x + halfOfWidth, rect.y + _textHeight + _space + EditorGUIUtility.singleLineHeight, _labelx, EditorGUIUtility.singleLineHeight),
            "지속시간");
        EditorGUI.PropertyField(
            new Rect(rect.x + halfOfWidth + _labelx, rect.y + _textHeight + _space + EditorGUIUtility.singleLineHeight, halfOfWidth - _labelx, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("_duration"), GUIContent.none);
    }

    // 인트로 씬 페이지 드롭다운 메뉴 그리기
    private void PageDropdownMenu(Rect buttonRect, ReorderableList l)
    {
        var menu = new GenericMenu();

        var guids = AssetDatabase.FindAssets("", new[] { "Assets/SAMTLE_GAME/Resources/Intro/Sprites" });
        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Intro/" + Path.GetFileNameWithoutExtension(path)),
            false, clickHandler,
            new IntroManager.PageCreationParams() { _spriteName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        menu.AddItem(new GUIContent("(None)"),
        false, clickHandler,
        new IntroManager.PageCreationParams() { _spriteName = "", _path = "" });

        menu.ShowAsContext();
    }

    private void clickHandler(object target)
    {
        var data = (IntroManager.PageCreationParams)target;
        var index = _list.serializedProperty.arraySize;
        _list.serializedProperty.arraySize++;
        _list.index = index;
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        if (data._spriteName == "" && data._path == "")
            element.FindPropertyRelative("_sprite").objectReferenceValue = null;
        else
        {
            element.FindPropertyRelative("_sprite").objectReferenceValue =
            AssetDatabase.LoadAssetAtPath(data._path, typeof(Sprite)) as Sprite;
        }
        element.FindPropertyRelative("_text").stringValue = "";
        element.FindPropertyRelative("_delay").floatValue = 0.1f;
        element.FindPropertyRelative("_duration").floatValue = 0.1f;
        serializedObject.ApplyModifiedProperties();
    }
}