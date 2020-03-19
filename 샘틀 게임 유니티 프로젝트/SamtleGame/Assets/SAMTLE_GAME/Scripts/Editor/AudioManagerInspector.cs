using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerInspector : Editor
{
    private ReorderableList _audioList;
    private ReorderableList _currentSound;

    private readonly Dictionary<string, ReorderableList> _soundDict = new Dictionary<string, ReorderableList>();

    #region 초기화용 함수
    private void OnEnable()
    {
        _audioList = new ReorderableList(serializedObject, serializedObject.FindProperty("_sounds"),
        true, true, true, true);

        _audioList.drawHeaderCallback = DrawAudioHeader;

        _audioList.drawElementCallback = DrawAudioElement;

        _audioList.onSelectCallback = (ReorderableList list) => 
        {
            var prefab = list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("_clip2").objectReferenceValue as GameObject;
            if(prefab)
                EditorGUIUtility.PingObject(prefab.gameObject);
        };

        _audioList.onRemoveCallback = (ReorderableList list) => 
        {
            if(EditorUtility.DisplayDialog("경고!", "정말 이 Audio를 삭제하겠습니까?", "네", "아니요"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            }       
        };

        _audioList.onAddCallback = (ReorderableList list) => 
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;

            var e = list.serializedProperty.GetArrayElementAtIndex(index);
            e.FindPropertyRelative("_name").stringValue = string.Format("Audio {0}", index);
        };

        _audioList.elementHeightCallback = (index) =>
        {
            return GetAudioHeight(_audioList.serializedProperty.GetArrayElementAtIndex(index));
        };
    }
    #endregion
    
    #region  Audio 인스펙터 라벨 제목
    private void DrawAudioHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Audio");
    }
    #endregion

    private void DrawAudioElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var audio = _audioList.serializedProperty.GetArrayElementAtIndex(index);

        var pos = new Rect(rect);

        var name = audio.FindPropertyRelative("_name");
        var isFoldOut = audio.FindPropertyRelative("_isFoldOut");
        var clips = audio.FindPropertyRelative("_clip");
        string soundDicKey = audio.propertyPath;

        EditorGUI.indentLevel++;
        {
            isFoldOut.boolValue = EditorGUI.Foldout(new Rect(pos.x, pos.y, 10, EditorGUIUtility.singleLineHeight), isFoldOut.boolValue, isFoldOut.boolValue ? "" : name.stringValue);
            
            // Audio 제목 필드
            name.stringValue = EditorGUI.TextField(new Rect(pos.x, pos.y, pos.width, EditorGUIUtility.singleLineHeight), name.stringValue);
            pos.y += EditorGUIUtility.singleLineHeight;
            
            if(isFoldOut.boolValue)
            {
                if(!_soundDict.ContainsKey(soundDicKey))
                {
                    var clipList = new ReorderableList(audio.serializedObject, clips)
                    {
                        displayAdd = true,
                        displayRemove = true,
                        draggable = true,

                        drawHeaderCallback = DrawSoundHeader,
                        drawElementCallback = (soundRect, soundIndex, soundIsActive, soundIsFocused) => 
                        {
                            DrawSoundElement(_soundDict[soundDicKey], soundRect, soundIndex, soundIsActive, soundIsFocused);
                        },
                        onAddDropdownCallback = SoundDropDownMenu
                    };
                    _soundDict[soundDicKey] = clipList;
                }

                _soundDict[soundDicKey].DoList(new Rect(pos.x, pos.y, pos.width, pos.height - EditorGUIUtility.singleLineHeight));
            }
        }
        EditorGUI.indentLevel--;
    }

    private float GetAudioHeight(SerializedProperty Audio)
    {
        var height = EditorGUIUtility.singleLineHeight;

        var isFoldOut = Audio.FindPropertyRelative("_isFoldOut");

        if(isFoldOut.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight * 2;

            var clips = Audio.FindPropertyRelative("_clip");
            height += EditorGUIUtility.singleLineHeight * Mathf.Max(1, clips.arraySize) * 2;
        }

        return height;
    }

    #region 사운드 선택 드랍다운 메뉴
    private void SoundDropDownMenu(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        _currentSound = list;

        var guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Intro/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Intro/" + Path.GetFileNameWithoutExtension(path)),
            false, clickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage1/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Stage1/" + Path.GetFileNameWithoutExtension(path)),
            false, clickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage2/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Stage2/" + Path.GetFileNameWithoutExtension(path)),
            false, clickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Stage3/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Stage3/" + Path.GetFileNameWithoutExtension(path)),
            false, clickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        guids = AssetDatabase.FindAssets("", new[]{"Assets/SAMTLE_GAME/Resources/Outro/Sounds/Audio"});
        foreach(var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            menu.AddItem(new GUIContent("Outro/" + Path.GetFileNameWithoutExtension(path)),
            false, clickHandler,
            new AudioManager.AudioCreationParams() { _audioName = Path.GetFileNameWithoutExtension(path), _path = path }
            );
        }
        menu.ShowAsContext();
    }
    #endregion

    private void DrawSoundHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "사운드 파일");
    }

    private void DrawSoundElement(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;
        EditorGUI.PropertyField
        (
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            element, GUIContent.none
        );
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        _audioList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void clickHandler(object target) 
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
