
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using Delegates;

	// Sphere collider for touch input

	// Has position and radius

	public delegate void TouchColliderDelegate(TouchCollider val);

	public class TouchCollider : MonoBehaviour
	{
		// Events
		public event BoolDelegate			EventTestTouch;
		public event TouchColliderDelegate	EventTouched;
		
		public float	m_Radius		= 1f;
		
		private Vector2	m_TouchScreenPos	= Vector2.zero;

		private bool	m_Active 	= false;
		private bool	m_Triggered = false;
		
		private TouchTracker	_ActiveTracker	= null;
		
	#if UNITY_EDITOR
		void Update()
		{
			TestEvent.Instance.Test(EventTestTouch, "TouchCollider.EventTestTouch");
			TestEvent.Instance.Test(EventTouched, "TouchCollider.EventTouched");
		}
	#endif
		
		public void EnableCollider()
		{
			if (!m_Active)
			{
				InputTouch.Instance.EventTouchUpdateAll += OnTouchUpdateAll;
				m_Active = true;
			}
		}
		
		public void DisableCollider()
		{
			if (m_Active)
			{
				InputTouch.Instance.EventTouchUpdateAll -= OnTouchUpdateAll;
				m_Active = false;
			}
		}
		
		public Vector2 GetLastTouchPosition()
		{
			return m_TouchScreenPos;
		}
		
		public TouchTracker GetTracker()
		{
			return _ActiveTracker;
		}
		
		private void OnTouchUpdateAll(BetterList<TouchTracker> a_Touches)
		{
			m_Triggered = false;
			for(int i = 0; i < a_Touches.size; ++i)
			{
				if (IsInside(a_Touches[i].GetCurrPosition()))
				{
					m_TouchScreenPos = a_Touches[i].GetCurrPosition();
					m_Triggered = true;
					_ActiveTracker = a_Touches[i];
				}
			}
			
			if (EventTestTouch != null)
			{
				EventTestTouch(m_Triggered);
			}
			
			if (m_Triggered && EventTouched != null)
			{
				EventTouched(this);
			}
		}
		
		private bool IsInside(Vector2 a_ScreenPos)
		{
			Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
			
			// Calc PixelDist
			Vector2 screenCollPos = Camera.main.WorldToScreenPoint(
				transform.position + (Camera.main.transform.right * m_Radius));
			float dist = Vector2.SqrMagnitude(screenPos-screenCollPos);
			
			if (Vector2.SqrMagnitude(screenPos-a_ScreenPos) < dist)
			{
				return true;
			}
			return false;
		}
		
	#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (m_Active && Camera.current != null)
			{
				if (m_Triggered) Gizmos.color = Color.green;
				
				UtilGizmos.DrawCircleGizmo(transform.position, m_Radius,
					Camera.current.transform.up, Camera.current.transform.forward);
			}
		}
	#endif
	}

}