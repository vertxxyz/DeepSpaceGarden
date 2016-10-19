
namespace Bowk
{
		
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	[CustomEditor(typeof(TestEvent))]
	public class TestEventEditor : Editor
	{
		
		private TestEvent _Target	= null;
		
		public override void OnInspectorGUI()
		{
			if (!Application.isPlaying)
			{
				DrawDefaultInspector();
				return;
			}

			if (_Target == null)
			{
				_Target = (TestEvent)target;
			}

			_Target.test_enabled = EditorGUILayout.Toggle(_Target.test_enabled);

			if (!_Target.test_enabled) return;

			if (GUILayout.Button("Log Report"))
			{
				_Target.LogReport();
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();
			
			List<KeyValuePair<string, int>> handlerList = _Target._HandlerCounts.ToList();
			handlerList.Sort((nextPair,firstPair) => 
			{
	        	return firstPair.Value.CompareTo(nextPair.Value);
			});
			
			int iRank = 1;
			foreach(KeyValuePair<string, int> pair in handlerList)
			{
				EditorGUILayout.LabelField(
					iRank.ToString() + " " + pair.Key + ": " + pair.Value.ToString());
				++iRank;
			}
			
			
		}
		
		
	}

}


