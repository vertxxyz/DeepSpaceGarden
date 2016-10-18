using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomPropertyDrawer(typeof(MinMaxAttribute))]
public class MinMaxDrawer : PropertyDrawer {
	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label){
		MinMaxAttribute nP = attribute as MinMaxAttribute;

		SerializedProperty current = property.Copy();
		property.Next (false);
		SerializedProperty next = property;
		bool isFloat = current.propertyType == SerializedPropertyType.Float;
		float min = isFloat ? current.floatValue : current.intValue;
		float max = isFloat ? next.floatValue : next.intValue;
		float width = EditorGUIUtility.labelWidth;
		position.width -= width;
		Rect labelLeft = new Rect (position.x, position.y, width, position.height);
		EditorGUI.LabelField (labelLeft, nP.label);

		position = new Rect (labelLeft.xMax, position.y, position.width, position.height);
		float floatWidth = position.width * 0.125f;
		float sliderWidth = position.width-floatWidth*2;
		position.width = floatWidth;
		if (isFloat)
			min = EditorGUI.FloatField (position, min);
		else
			min = (int)EditorGUI.IntField (position, (int)min);
		position.x += floatWidth;
		position.width = sliderWidth;
		EditorGUI.MinMaxSlider (position, ref min, ref max, nP.min, nP.max);
		position.x += sliderWidth;
		position.width = floatWidth;
		max = EditorGUI.FloatField (position, max);

		if (isFloat) {
			current.floatValue = min;
			next.floatValue = max;
		} else {
			current.intValue = (int)min;
			next.intValue = (int)max;
		}
	}
}
