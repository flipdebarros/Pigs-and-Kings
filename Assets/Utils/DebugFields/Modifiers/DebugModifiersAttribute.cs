using UnityEditor;
using UnityEngine;

namespace Utils.DebugFields {
public abstract class DebugModifierAttribute : PropertyAttribute {
	public string ModifyField { get; }

	protected DebugModifierAttribute(string fieldName) {
		ModifyField = fieldName;
	}

	#if UNITY_EDITOR
	public abstract void OnGUI(SerializedProperty property);
	#endif
}

}