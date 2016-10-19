
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using Bowk.Delegates;

	// Wrapper for touch/mouse input

	// Delegates
	public delegate void TouchDelegate(TouchTracker val);
	public delegate void TouchListDelegate(BetterList<TouchTracker> val);

	public class InputTouch : MonoSingleton<InputTouch>
	{
		
		// Events
		public event TouchDelegate		EventTouchBegan;
		public event TouchDelegate		EventTouchUpdate;
		public event TouchDelegate		EventTouchEnded;
		public event TouchListDelegate 	EventTouchUpdateAll;
		
		// Varaibles
		public bool		m_UseMouse	= false;
		public bool		_DrawGizmos	= false;
		
		private const int	MAXTOUCH = 12;
		
		private BetterList<TouchTracker>		m_Trackers;
		private Dictionary<int, TouchTracker>	m_TrackerLookup;

		public override void Init()
		{
			m_Trackers = new BetterList<TouchTracker>();
			m_TrackerLookup = new Dictionary<int, TouchTracker>();

			#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
			m_UseMouse = false;
			#endif
		}
		
		void Update()
		{
			// Clean all
			for(int i = 0; i < m_Trackers.size; ++i)
			{
				m_Trackers[i].Clean();
			}
			
			// Process
			bool containsKey = false;
			// TOUCH
			if (!m_UseMouse)
			{
				for(int i = 0; i < Input.touches.Length; ++i)
				{
					Touch t = Input.touches[i];

					containsKey = m_TrackerLookup.ContainsKey(t.fingerId);
					
					if (containsKey)
					{
						m_TrackerLookup[t.fingerId].Update(t);
						if (EventTouchUpdate != null) EventTouchUpdate(m_TrackerLookup[t.fingerId]);
					}
					else
					{
						if (t.fingerId < MAXTOUCH)
						{
							BeginTracking(t);
						}
					}
				}
			}
			// MOUSE
			else
			{
				if (Input.GetMouseButton(0))
				{
					containsKey = m_TrackerLookup.ContainsKey(0);
					if (containsKey)
					{
						m_TrackerLookup[0].Update(Input.mousePosition);
						if (EventTouchUpdate != null) EventTouchUpdate(m_TrackerLookup[0]);
					}
					else
					{
						BeginTracking(Input.mousePosition);
					}
				}
			}


			for(int i = m_Trackers.size-1; i >= 0; i--)
			{
				if (!m_Trackers[i].IsDirty())
				{
					EndTracking(m_Trackers[i], i);
				}
			}
			
			if (EventTouchUpdateAll != null)
			{
				EventTouchUpdateAll(m_Trackers);
			}
			
			#if UNITY_EDITOR
			// Test event handlers
			TestEvent.Instance.Test(EventTouchUpdateAll, "InputTouch.EventTouchUpdateAll");
			TestEvent.Instance.Test(EventTouchBegan, "InputTouch.EventTouchBegan");
			TestEvent.Instance.Test(EventTouchEnded, "InputTouch.EventTouchEnded");
			TestEvent.Instance.Test(EventTouchUpdate, "InputTouch.EventTouchUpdate");
			#endif
		}
		
		// Public Functions

		public TouchTracker GetTouchFirst()
		{
			if (m_Trackers == null) return null;
			if (m_Trackers.size <= 0) return null;
			return m_Trackers[0];
		}

		public TouchTracker GetTouchSecond()
		{
			if (m_Trackers == null) return null;
			if (m_Trackers.size <= 1) return null;
			return m_Trackers[1];
		}

		public int GetTouchCount()
		{
			return m_Trackers.size;
		}
		
		public TouchTracker GetTouch(int a_Touch)
		{
			//Debug.Log(m_Trackers.size);
			if (m_Trackers.size < a_Touch)
			{
				return m_Trackers[a_Touch];
			}
			return null;
		}

		public TouchTracker GetTouchByTouchID(int touch_id)
		{
			if (m_TrackerLookup.ContainsKey(touch_id))
			{
				return m_TrackerLookup[touch_id];
			}
			return null;
		}
		
		private void BeginTracking(Touch a_Touch)
		{
			TouchTracker tracker = new TouchTracker(a_Touch);
			m_Trackers.Add(tracker);
			m_TrackerLookup[a_Touch.fingerId] = tracker;
			if (EventTouchBegan != null) EventTouchBegan(tracker);
		}
		
		private void EndTracking(TouchTracker a_Tracker, int remove_at)
		{
			if (EventTouchEnded != null) EventTouchEnded(a_Tracker);
			m_Trackers.RemoveAt(remove_at);
			m_TrackerLookup.Remove(a_Tracker.GetFingerID());
		}
		
		// Mouse Touch Input
		private void BeginTracking(Vector2 a_ScreenPos)
		{
			TouchTracker tracker = new TouchTracker(a_ScreenPos);
			m_Trackers.Add(tracker);
			m_TrackerLookup[0] = tracker;
			if (EventTouchBegan != null) EventTouchBegan(tracker);
		}
		
		// Gizmos
		
		private void OnDrawGizmos()
		{
			if (_DrawGizmos && Camera.current != null && Camera.current == Camera.main && m_Trackers != null)
			{
				foreach(TouchTracker tracker in m_Trackers)
				{
					Gizmos.color = Color.red;
					Vector3 worldPos = Camera.current.ScreenToWorldPoint(tracker.GetCurrPosition());
					UtilGizmos.DrawCircleGizmo(worldPos + Camera.current.transform.forward, 1f, 
						Camera.current.transform.up, Camera.current.transform.forward);
				}
			}
		}
	}

}
