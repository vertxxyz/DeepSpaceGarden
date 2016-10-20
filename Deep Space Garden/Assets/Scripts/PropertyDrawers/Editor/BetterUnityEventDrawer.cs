using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;

[CustomPropertyDrawer (typeof(BetterUnityEventAttribute))]
public class BetterUnityEventDrawer : PropertyDrawer {

	public override float GetPropertyHeight (SerializedProperty property,
	                                         GUIContent label) {
		if (GetPersistentCallCount (property) == 0) {
			return 18 * 2;
		}
		UnityEventDrawer eventDrawer = new UnityEventDrawer ();
		return eventDrawer.GetPropertyHeight (property, label);
	}

	public override void OnGUI (Rect position,
	                            SerializedProperty property,
	                            GUIContent label) {
		UnityEventDrawer eventDrawer = new UnityEventDrawer ();
		if (GetPersistentCallCount (property) == 0) {
			Rect r = new Rect (position.x, position.y, position.width, 18);
			GUI.BeginGroup (r);
			eventDrawer.OnGUI (new Rect (0, 0, r.width, r.height), property, label);
			GUI.EndGroup ();
			r = new Rect (position.x, position.y + 18, position.width, 15);
			GUI.BeginGroup (new Rect (r.x, r.y, r.width, r.height + 3));
			eventDrawer.OnGUI (new Rect (0, -67, r.width, r.height), property, label);
			GUI.EndGroup ();
		} else {
			eventDrawer.OnGUI (position, property, new GUIContent (label));
		}
	}

	public int GetPersistentCallCount (SerializedProperty event_) {
		return event_.FindPropertyRelative ("m_PersistentCalls").FindPropertyRelative ("m_Calls").arraySize;
	}
}
