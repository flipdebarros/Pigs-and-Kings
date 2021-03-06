using UnityEngine;

namespace Utils.DebugFields {

public abstract class DrawFieldAttribute : PropertyAttribute {
	public Color color;
	public bool IsColorDefined { get; private set; }
	public abstract string FieldType { get; }
	public abstract string PropertyName { get; }

	protected DrawFieldAttribute() { }

	protected DrawFieldAttribute(float r, float g, float b) {
		color = new Color(r, g, b, 1f);
		IsColorDefined = true;
	}
	
#if UNITY_EDITOR
	public abstract void Draw(object value, Vector2 position);
#endif
}

}