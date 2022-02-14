using UnityEngine;
using UnityEditor;

namespace Utils.DebugFields {

public class DrawCircleAttribute : DrawFieldAttribute {
	
	public float thickness = 0.5f;
	public override string FieldType => "Circle";
	public override string PropertyName => "Radius";
	
	public DrawCircleAttribute() {}
	public DrawCircleAttribute(float r, float g, float b) : base(r, g, b) {}
	
	private static float GetValue(object value) {
		if (value is float or int or uint)
			return value switch {
				float f => f,
				int f => f,
				uint f => f,
				_ => 0f
			};
		
		
		Debug.LogWarning("DrawCircle attribute only considers some numeric type fields. Field will be ignored");
		return 0f;
	}

#if UNITY_EDITOR
	public override void Draw (object value, Vector2 position) {
		Handles.DrawWireDisc(position, Vector3.forward, GetValue(value), thickness);
	}
#endif

}

}