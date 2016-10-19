
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public class TestEvent : MonoSingleton<TestEvent>
	{
		public bool test_enabled	= false;

		#if UNITY_EDITOR
		public Dictionary<string, int>	_HandlerCounts;
		
		private System.Delegate[] _listeners	= null;
		
		public void Test(System.Delegate a_Event, string a_EventName)
		{
			if (!test_enabled) return;
			if (a_Event == null) return;
			
			if (_HandlerCounts == null)
			{
				_HandlerCounts = new Dictionary<string, int>();
			}
			
			// Count handlers
			_listeners = a_Event.GetInvocationList();
			int listenCount = _listeners.Length;

			_HandlerCounts[a_EventName] = listenCount;
		}
		
		// Debug log handler count statistics
		public void LogReport()
		{
			if (!test_enabled) return;

			Debug.Log("TEST EVENT REPORT");
			Debug.Log("TIME: " + Time.time.ToString("F2"));
			Debug.Log("-----------------");
			foreach(KeyValuePair<string, int> pair in _HandlerCounts)
			{
				Debug.Log(pair.Key + ": " + pair.Value.ToString());
			}
			Debug.Log("-----------------");
		}
		#endif
	}

}