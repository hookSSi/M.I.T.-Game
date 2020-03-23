using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerInspector : BgmManagerInspector
{
    #region  Audio 인스펙터 라벨 제목
    private void DrawAudioHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Audio");
    }
    #endregion

    #region onRemoveCallback
    protected override void Remove(ReorderableList list)
    {
        if(EditorUtility.DisplayDialog("경고!", "정말 이 Audio를 삭제하겠습니까?", "네", "아니요"))
        {
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }         
    }
    #endregion

    #region onAddCallback
    protected override void Add(ReorderableList list)
    {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;

            var e = list.serializedProperty.GetArrayElementAtIndex(index);
            e.FindPropertyRelative("_name").stringValue = string.Format("Audio {0}", index);
    }
    #endregion

    #region 사운드 선택 드랍다운 메뉴
    protected override void DropDownMenu(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        _currentSound = list;

        var guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Intro/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Intro/" + Path.GetFileNameWithoutExtension(path)),
            false, ClickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage1/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Stage1/" + Path.GetFileNameWithoutExtension(path)),
            false, ClickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage2/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Stage2/" + Path.GetFileNameWithoutExtension(path)),
            false, ClickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage3/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Stage3/" + Path.GetFileNameWithoutExtension(path)),
            false, ClickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Outro/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Outro/" + Path.GetFileNameWithoutExtension(path)),
            false, ClickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        menu.ShowAsContext();
    }
    #endregion

    protected override void ClickHandler(object target) 
    {
        var data = (AudioManager.AudioCreationParams)target;

        var sound = _currentSound;
        var index = sound.serializedProperty.arraySize;

        sound.serializedProperty.arraySize++;
        sound.index = index;
        var e = sound.serializedProperty.GetArrayElementAtIndex(index);
        e.objectReferenceValue = AssetDatabase.LoadAssetAtPath(data._path, typeof(AudioClip)) as AudioClip;
        serializedObject.ApplyModifiedProperties();
    }
}
