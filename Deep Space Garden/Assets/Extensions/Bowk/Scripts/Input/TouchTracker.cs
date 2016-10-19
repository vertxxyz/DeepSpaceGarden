
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;

	public class TouchTracker
	{
		private int		m_FingerID		= -1;
		private Touch	m_PrevTouch;
		private Vector2	m_PrevPosition;
		private Vector2	m_CurrPosition;
		private Vector2 m_Delta 		= Vector2.zero;
		private bool	m_IsDirty		= false;
		
		private float	m_TrackTime		= 0f;
		private Touch	m_FirstTouch;

		private int _update_count = 0;

		public TouchTracker(Touch a_Touch)
		{
			m_FingerID = a_Touch.fingerId;
			m_FirstTouch = a_Touch;
			
			Update(m_FirstTouch);

			m_PrevPosition = m_CurrPosition;

			_update_count = 0;
		}
		
		public TouchTracker(Vector2 a_MousePos)
		{
			m_FingerID = 0;
			m_FirstTouch = new Touch();
			m_Delta = Vector2.zero;

			Update(a_MousePos);

			m_PrevPosition = m_CurrPosition;
		}
		
		public void Update(Touch a_Touch)
		{
			m_PrevTouch = a_Touch;
			m_PrevPosition = m_CurrPosition;
			m_CurrPosition = m_PrevTouch.position;
			m_Delta = m_CurrPosition - m_PrevPosition;
			m_IsDirty = true;
			
			m_TrackTime += Time.deltaTime;
			_update_count++;
		}
		
		public void Update(Vector2 a_MousePos)
		{
			m_PrevTouch = new Touch();
			m_PrevPosition = m_CurrPosition;
			m_CurrPosition = a_MousePos;
			m_Delta = m_CurrPosition - m_PrevPosition;
			m_IsDirty = true;
			
			m_TrackTime += Time.deltaTime;
			_update_count++;
		}
		
		public void Clean()
		{
			m_IsDirty = false;
		}
		
		public int GetFingerID()
		{
			return m_FingerID;
		}
		
		public Vector2 GetCurrPosition()
		{
			return m_CurrPosition;
		}

		public Vector2 GetLastPosition()
		{
			return m_PrevPosition;
		}

		public Vector2 DeltaPosition()
		{
			return m_Delta;
		}
		
		public bool IsDirty()
		{
			return m_IsDirty;
		}

		public bool IsFirstFrame()
		{
			return (_update_count <= 1);
		}
		
	}

}
