using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.IO;
using System.Collections.Generic;

using MIT.SamtleGame.Stage1;

[CustomEditor(typeof(Spawner))]
public class SpawnerInspector : Editor
{  
    protected ReorderableList _enemyList;

    private readonly Dictionary<string, ReorderableList> _enemyDict = new Dictionary<string, ReorderableList>();

    private void OnEnable()
    {
        var enemy = serializedObject.FindProperty("_civil");

        _enemyList = new ReorderableList(serializedObject, enemy)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = DrawHeader,

            drawElementCallback = DrawElement,

            onAddCallback = Add,

            onAddDropdownCallback = DropDownMenu,

            elementHeightCallback = (index) =>
            {
                var height = EditorGUIUtility.singleLineHeight;

                height += (EditorGUIUtility.singleLineHeight * 2 + 64);

                return height;
            }
        };
    }

    protected void DrawHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "적 스폰 프리팹");
    }

    protected virtual void Add(ReorderableList list)
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
    }

    protected virtual void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var enemy = _enemyList.serializedProperty.GetArrayElementAtIndex(index);
        var preview = AssetPreview.GetAssetPreview(enemy.objectReferenceValue) as Texture2D;

        rect.y += 2;
        EditorGUI.PropertyField
        (
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            enemy,
            GUIContent.none
        );

        if(preview != null)
        {
            EditorGUI.DrawPreviewTexture
            (
                new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, 64, 64),
                preview
            );
        }
    }

    protected virtual void DropDownMenu(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        _enemyList = list;

        var guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage1/Prefabs/Enemy"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Enemy/" + Path.GetFileNameWithoutExtension(path)),
            false, ClickHandler,
            new Spawner.EnemyCreationParams() { _path = path }
            );
        }

        menu.ShowAsContext();
    }

    protected virtual void ClickHandler(object target)
    {
        var data = (Spawner.EnemyCreationParams)target;

        var index = _enemyList.serializedProperty.arraySize;

        _enemyList.serializedProperty.arraySize++;
        _enemyList.index = index;
        var e = _enemyList.serializedProperty.GetArrayElementAtIndex(index);
        e.objectReferenceValue = AssetDatabase.LoadAssetAtPath(data._path, typeof(GameObject)) as GameObject;
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        _enemyList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
