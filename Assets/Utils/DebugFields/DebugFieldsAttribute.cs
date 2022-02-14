using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Utils.DebugFields {

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DebugFieldsAttribute : Attribute {

	public MonoBehaviour target;
	public FieldInfo[] fields;
	public Dictionary<string, List<FieldInfo>> modifiers;

	private Color _color = Color.green;

	public DebugFieldsAttribute(float r, float g, float b) {
		_color = new Color(r, g, b, 1f);
	}
	public DebugFieldsAttribute() { }
	
	#if UNITY_EDITOR
	public void Draw() {
		foreach (var field in fields) {
			var drawer = field.GetCustomAttribute<DrawFieldAttribute>();
			var value = field.GetValue(target);
			
			var position = (Vector2) target.gameObject.transform.position;
			Handles.color = drawer.IsColorDefined ? drawer.color : _color;

			if (modifiers.ContainsKey(field.Name)) {
				foreach (var modifier in modifiers[field.Name]) {
					switch (modifier.GetCustomAttribute<DebugModifierAttribute>()) {
						case SetOffsetAttribute _:
							position += (Vector2) modifier.GetValue(target);
							break;
						case SetColorAttribute _:
							Handles.color = (Color) modifier.GetValue(target);
							break;
					}
				}
			}

			drawer.Draw(value, position);
		}
		Handles.color = Color.white;
	}
	#endif
	
}

}
