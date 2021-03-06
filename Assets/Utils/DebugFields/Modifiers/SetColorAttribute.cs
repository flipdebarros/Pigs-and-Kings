using UnityEditor;
using UnityEngine;

namespace Utils.DebugFields {
public class SetColorAttribute : DebugModifierAttribute {
	public SetColorAttribute(string fieldName) : base(fieldName) { }

	#if UNITY_EDITOR
	public override void OnGUI(SerializedProperty property) {
		if (property.propertyType != SerializedPropertyType.Color) {
			Debug.LogError("SetColor Attribute has to be on a Color to modify another variable");
			return;
		}
		property.colorValue = EditorGUILayout.ColorField(new GUIContent("Color"), property.colorValue);
	}
	#endif
}

}
