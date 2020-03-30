using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.IO;
using System.Collections.Generic;

using MIT.SamtleGame.Stage1;

[CustomEditor(typeof(Spawner))]
public class SpawnerInspector : Editor
{  
    protected ReorderableList _civilList;
    protected ReorderableList _pegeonList;
    protected ReorderableList _bossList;
    protected ReorderableList _currentList;

    protected bool _isCivilFoldOut;
    protected bool _isPegeonFoldOut;
    protected bool _isBossFoldOut;

    private void OnEnable()
    {
        var civil = serializedObject.FindProperty("_civil");
        var pegeon = serializedObject.FindProperty("_pegeon");
        var boss = serializedObject.FindProperty("_boss");
        
        _civilList = new ReorderableList(serializedObject, civil)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = (Rect rect) => {
            DrawHeader(rect, "시민");
            },

            drawElementCallback = DrawCivilElement,

            onAddCallback = Add,

            onAddDropdownCallback = DropCivilDownMenu,

            elementHeightCallback = (index) =>
            {
                var height = EditorGUIUtility.singleLineHeight;

                if(_isCivilFoldOut)
                {
                    height += (EditorGUIUtility.singleLineHeight * 2 + 64);
                }

                return height;
            }
        };
        
        _pegeonList = new ReorderableList(serializedObject, pegeon)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = (Rect rect) => {
                DrawHeader(rect, "비둘기");
            },

            drawElementCallback = DrawPegeonElement,

            onAddCallback = Add,

            onAddDropdownCallback = DropPegeonDownMenu,

            elementHeightCallback = (index) =>
            {
                var height = EditorGUIUtility.singleLineHeight;

                if(_isPegeonFoldOut)
                {
                    height += (EditorGUIUtility.singleLineHeight * 2 + 64);
                }

                return height;
            }
        };

        _bossList = new ReorderableList(serializedObject, boss)
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,

            drawHeaderCallback = (Rect rect) => {
                DrawHeader(rect, "보스");
            },

            drawElementCallback = DrawBossElement,

            onAddCallback = Add,

            onAddDropdownCallback = DropBossDownMenu,

            elementHeightCallback = (index) =>
            {
                var height = EditorGUIUtility.singleLineHeight;

                if(_isBossFoldOut)
                {
                    height += (EditorGUIUtility.singleLineHeight * 2 + 64);
                }

                return height;
            }
        };
    }

    protected void DrawHeader(Rect rect, string headerText)
    {
        EditorGUI.LabelField(rect, headerText);
    }

    protected virtual void Add(ReorderableList list)
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
    }

    protected virtual void DrawCivilElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.indentLevel++;
        {
            if(index == 0)
            {
                _isCivilFoldOut = EditorGUI.Foldout(
                    new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight),
                    _isCivilFoldOut,
                    "시민"
                    );
            }

            if(_isCivilFoldOut)
            {
                var enemy = _civilList.serializedProperty.GetArrayElementAtIndex(index);
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
        }
        EditorGUI.indentLevel--;
    }
    protected virtual void DrawPegeonElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.indentLevel++;
        {
            if(index == 0)
            {
                _isPegeonFoldOut = EditorGUI.Foldout(
                    new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight),
                    _isPegeonFoldOut,
                    "비둘기"
                    );
            }

            if(_isPegeonFoldOut)
            {
                var enemy = _pegeonList.serializedProperty.GetArrayElementAtIndex(index);
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
        }
        EditorGUI.indentLevel--;
    }
    protected virtual void DrawBossElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        EditorGUI.indentLevel++;
        {
            if(index == 0)
            {
                _isBossFoldOut = EditorGUI.Foldout(
                    new Rect(rect.x, rect.y, 10, EditorGUIUtility.singleLineHeight),
                    _isBossFoldOut,
                    "보스"
                    );
            }

            if(_isBossFoldOut)
            {
                var enemy = _bossList.serializedProperty.GetArrayElementAtIndex(index);
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
        }
        EditorGUI.indentLevel--;
    }

    protected virtual void DropCivilDownMenu(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        _civilList = list;
        _currentList = _civilList;

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
    protected virtual void DropPegeonDownMenu(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        _pegeonList = list;
        _currentList = _pegeonList;

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
    protected virtual void DropBossDownMenu(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        _bossList = list;
        _currentList = _bossList;

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

        var index = _currentList.serializedProperty.arraySize;


        _currentList.serializedProperty.arraySize++;
        _currentList.index = index;
        var e = _currentList.serializedProperty.GetArrayElementAtIndex(index);

        e.objectReferenceValue = AssetDatabase.LoadAssetAtPath(data._path, typeof(GameObject)) as GameObject;
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _civilList.DoLayoutList();
        
        _pegeonList.DoLayoutList();

        _bossList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}
