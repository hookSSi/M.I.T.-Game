using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

using NaughtyAttributes.Editor;

namespace MIT.SamtleGame.Attributes.Editor
{
    [CustomPropertyDrawer(typeof(GameAudioAttribute))]
    public class GameAudioPropertyDrawer : PropertyDrawerBase 
    {
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			return (property.propertyType == SerializedPropertyType.String)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				var bgmsProperty = AudioManager.Instance._sounds;
				var bgmSet = new HashSet<string>();
				bgmSet.Add("(None)");

				for (var i = 0; i < bgmsProperty.Count; i++)
				{
					var bgm = bgmsProperty[i]._name;
					bgmSet.Add(bgm);
				}

				var bgms = bgmSet.ToArray();

				string propertyString = property.stringValue;
				int index = 0;
				// check if there is an entry that matches the entry and get the index
				// we skip index 0 as that is a special custom case
				for (int i = 1; i < bgms.Length; i++)
				{
					if (bgms[i] == propertyString)
					{
						index = i;
						break;
					}
				}

				// Draw the popup box with the current selected index
				var newIndex = EditorGUI.Popup(rect, label.text, index, bgms);

				// Adjust the actual string value of the property based on the selection
				if (newIndex > 0)
				{
					property.stringValue = bgms[newIndex];
				}
				else
				{
					property.stringValue = string.Empty;
				}
			}
			else
			{
				string message = string.Format("{0}는 string 필드만 지원합니다.", typeof(GameAudioAttribute).Name);
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}
		}
    }
}
