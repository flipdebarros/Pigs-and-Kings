using UnityEngine;
using UnityEditor;

namespace Utils.DebugFields {

public class DrawLineAttribute : DrawFieldAttribute {
	public bool horizontal = true;
	public bool invert;
	public bool dotted;
	public float thickness = 1f;
	public float screenSize = 3f;

	public override string FieldType => (horizontal ? "Horizontal" : "Vertical") + 
	                                    (dotted ? " Dotted" : "") + " Line";
	public override string PropertyName => "Lenght";
	
	public DrawLineAttribute() {}
	public DrawLineAttribute(float r, float g, float b) : base(r, g, b) {}
	
	private static float GetValue(object value) {
		if (value is float or int or uint)
			return value switch {
				float f => f,
				int f => f,
				uint f => f,
				_ => 0f
			};
		
		
		Debug.LogWarning("DrawLine attribute only considers some numeric type fields. Field will be ignored");
		return 0f;
	}

#if UNITY_EDITOR
	public override void Draw (object value, Vector2 position) {
		var distance = GetValue(value) * (horizontal ? Vector2.right : Vector2.up) * (invert ? -1f : 1f);
		
		if (dotted) Handles.DrawDottedLine(position, position + distance, screenSize);
		else Handles.DrawLine(position, position + distance, thickness);
	}
#endif

}

}