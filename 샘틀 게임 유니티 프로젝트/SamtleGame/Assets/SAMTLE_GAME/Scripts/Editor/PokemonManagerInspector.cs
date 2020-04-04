using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using MIT.SamtleGame.Stage2.Pokemon;

[CustomEditor(typeof(PokemonManager))]
public class PokemonManagerInspector : Editor
{
    private ReorderableList _list;
    private ReorderableList _currentSkillList;
    private readonly Dictionary<string, ReorderableList> _skillDict = new Dictionary<string, ReorderableList>();

    private void OnEnable()
    {
        _list = new ReorderableList(serializedObject,
            serializedObject.FindProperty("_pokemonList"))
        {
            displayAdd = true,
            displayRemove = true,
            draggable = true,
            // 요소 제목
            drawHeaderCallback =
            (rect) => EditorGUI.LabelField(rect, "[포켓몬 정보 리스트]", EditorStyles.boldLabel),
            // 대화창 높이 조절
            elementHeightCallback =
            (index) => GetElementHeight(_list.serializedProperty.GetArrayElementAtIndex(index)),
            // 요소 드로우
            drawElementCallback = DrawElement,
            // 추가
            onAddCallback = (ReorderableList list) =>
            {
                var index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                var e = list.serializedProperty.GetArrayElementAtIndex(index);
                e.FindPropertyRelative("_info._name").stringValue = "새 포켓몬";
            },
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        _list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = _list.serializedProperty.GetArrayElementAtIndex(index);
        var isFoldOut = element.FindPropertyRelative("_isFoldOut");

        float yElement;
        float labelWidth = 80f;
        float halfWidth = rect.width * 0.5f;
        float halfFieldWidth = halfWidth - labelWidth;
        float singleHeight = EditorGUIUtility.singleLineHeight;
        float ySpace = 3f;

        string key = element.FindPropertyRelative("_info._name").stringValue;

        rect.y += 3;

        yElement = rect.y;

        EditorGUI.indentLevel++;
        {
            isFoldOut.boolValue = EditorGUI.Foldout(new Rect(rect.x, yElement, 10, singleHeight),
                isFoldOut.boolValue, key != "" ? string.Format("No.{0, -3} {1}", index, key) : "(이름이 존재하지 않습니다)");

            yElement += singleHeight + ySpace;

            if (isFoldOut.boolValue)
            {
                // General Info
                EditorGUI.LabelField(
                    new Rect(rect.x, yElement, rect.width, singleHeight),
                    "일반 정보", EditorStyles.boldLabel);

                yElement += singleHeight + ySpace;

                EditorGUI.LabelField(
                    new Rect(rect.x, yElement, labelWidth, singleHeight),
                    "포켓몬 이름", EditorStyles.boldLabel);
                EditorGUI.PropertyField(
                     new Rect(rect.x + labelWidth, yElement, halfFieldWidth, singleHeight),
                     element.FindPropertyRelative("_info._name"), GUIContent.none);
                EditorGUI.LabelField(
                    new Rect(rect.x + halfWidth, yElement, labelWidth, singleHeight),
                    "체력", EditorStyles.boldLabel);
                EditorGUI.PropertyField(
                     new Rect(rect.x + halfWidth + labelWidth, yElement, halfFieldWidth, singleHeight),
                     element.FindPropertyRelative("_info._health"), GUIContent.none);

                yElement += singleHeight + ySpace;

                EditorGUI.LabelField(
                    new Rect(rect.x, yElement, labelWidth, singleHeight),
                    "전면 이미지", EditorStyles.boldLabel);
                EditorGUI.ObjectField(
                     new Rect(rect.x + labelWidth, yElement, halfFieldWidth, singleHeight * 3f),
                    element.FindPropertyRelative("_info._frontImage"), typeof(Sprite), GUIContent.none);
                EditorGUI.LabelField(
                    new Rect(rect.x + halfWidth, yElement, labelWidth, singleHeight),
                    "후면 이미지", EditorStyles.boldLabel);
                EditorGUI.ObjectField(
                     new Rect(rect.x + halfWidth + labelWidth, yElement, halfFieldWidth, singleHeight * 3f),
                    element.FindPropertyRelative("_info._backImage"), typeof(Sprite), GUIContent.none);

                yElement += singleHeight * 3f + ySpace;

                EditorGUI.LabelField(
                    new Rect(rect.x, yElement, labelWidth, singleHeight),
                    "울음소리", EditorStyles.boldLabel);
                EditorGUI.PropertyField(
                     new Rect(rect.x + labelWidth, yElement, rect.width - labelWidth, singleHeight),
                     element.FindPropertyRelative("_info._cryingSound"), GUIContent.none);

                yElement += singleHeight * 1.7f;

                // Skills
                ReorderableList skillList;

                if (!_skillDict.ContainsKey(key))
                {
                    skillList = new ReorderableList(element.serializedObject,
                        element.FindPropertyRelative("_info._skills"))
                    {
                        displayAdd = true,
                        displayRemove = true,
                        draggable = true,
                        drawHeaderCallback =
                        (rectangle) => EditorGUI.LabelField(rectangle, "포켓몬 정보 리스트", EditorStyles.boldLabel),
                        drawElementCallback = (innerRect, innerIndex, innerIsActive, innerIsFocused) =>
                        {
                            DrawSkills(_skillDict[key], innerRect, innerIndex, innerIsActive, innerIsFocused);
                        },
                        elementHeight = singleHeight * 9f
                    };

                    _skillDict[key] = skillList;
                }
                else
                {
                    skillList = _skillDict[key];
                }

                skillList.DoList(new Rect(rect.x, yElement, rect.width,
                    singleHeight * 4f + skillList.elementHeight * skillList.count));
            }
        }
        EditorGUI.indentLevel--;
    }

    private void DrawSkills(ReorderableList list, Rect rect, int index, bool isActive, bool isFocused)
    {
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
        float labelWidth = 70f;
        float halfWidth = rect.width * 0.5f;
        float halfFieldWidth = halfWidth - labelWidth;
        float singleHeight = EditorGUIUtility.singleLineHeight;

        rect.y += 2;
        EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, singleHeight), "스킬 이름", EditorStyles.boldLabel);
        EditorGUI.PropertyField(new Rect(rect.x + labelWidth, rect.y, rect.width - labelWidth, singleHeight),
            element.FindPropertyRelative("_name"), GUIContent.none);
        EditorGUI.LabelField(new Rect(rect.x, rect.y + singleHeight, labelWidth, singleHeight), "사용 횟수", EditorStyles.boldLabel);
        EditorGUI.PropertyField(new Rect(rect.x + labelWidth, rect.y + singleHeight + 2f, halfFieldWidth, singleHeight),
            element.FindPropertyRelative("_count"), GUIContent.none);

        // EditorGUI.LabelField(new Rect(rect.x + halfWidth, rect.y + singleHeight, labelWidth - 15, singleHeight), "이벤트", EditorStyles.boldLabel);
        EditorGUI.LabelField(new Rect(rect.x, rect.y + singleHeight * 2f + 4f, rect.width, singleHeight), "이벤트");
        EditorGUI.PropertyField(new Rect(rect.x, rect.y + singleHeight * 3f + 4f, rect.width - 45f, singleHeight),
            element.FindPropertyRelative("_event"), GUIContent.none);
    }

    private float GetElementHeight(SerializedProperty element)
    {
        var height = EditorGUIUtility.singleLineHeight + 5f;

        var isFoldOut = element.FindPropertyRelative("_isFoldOut");

        if (isFoldOut.boolValue)
        {
            height += EditorGUIUtility.singleLineHeight * 10.2f;
            var skills = element.FindPropertyRelative("_info._skills");
            height += EditorGUIUtility.singleLineHeight * Mathf.Max(1, skills.arraySize) * 9f;
        }

        return height;
    }
}
