
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;
	using Bowk.Delegates;

	// Bare state that broadcasts events

	public class EventState : FSMState
	{
		public event VoidDelegate	EventEnter;
		public event VoidDelegate	EventUpdate;
		public event VoidDelegate	EventUpdateLate;
		public event VoidDelegate	EventExit;
		
		public event VoidDelegate	EventGainedFocus;
		public event VoidDelegate	EventLostFocus;
		public event VoidDelegate	EventUnfocusedUpdate;
		
		private string	_StateName	= "NullName";
		
		public EventState(string a_StateName)
		{
			_StateName = a_StateName;
		}
		
		public override string ToString ()
		{
			return _StateName;
		}
		
		public override void Enter()
		{
			//Debug.Log(_StateName + " enter " + Time.time.ToString("F2"));
			if (EventEnter != null)
			{
				EventEnter();
			}
		}
		
		public override void Update()
		{
			if (EventUpdate != null)
			{
				EventUpdate();
			}
		}

		public override void UpdateLate()
		{
			if (EventUpdateLate != null)
			{
				EventUpdateLate();
			}

			#if UNITY_EDITOR
			TestEvent.Instance.Test(EventEnter, _StateName + ".EventEnter");
			TestEvent.Instance.Test(EventUpdate, _StateName + ".EventUpdate");
			TestEvent.Instance.Test(EventUpdateLate, _StateName + ".EventUpdateLate");
			TestEvent.Instance.Test(EventExit, _StateName + ".EventExit");
			
			TestEvent.Instance.Test(EventGainedFocus, _StateName + ".EventGainedFocus");
			TestEvent.Instance.Test(EventLostFocus, _StateName + ".EventLostFocus");
			TestEvent.Instance.Test(EventUnfocusedUpdate, _StateName + ".EventUnfocusedUpdate");
			#endif
		}
		
		public override void Exit()
		{
			//Debug.Log(_StateName + " exit " + Time.time.ToString("F2"));
			if (EventExit != null)
			{
				EventExit();
			}
		}
		
		public override void GainedFocus()
		{
			//Debug.Log(_StateName + " gained focus " + Time.time.ToString("F2"));
			if (EventGainedFocus != null)
			{
				EventGainedFocus();
			}
		}
		
		public override void LostFocus()
		{
			//Debug.Log(_StateName + " lost focus " + Time.time.ToString("F2"));
			if (EventLostFocus != null)
			{
				EventLostFocus();
			}
		}
		
		public override void UnfocusedUpdate()
		{
			if (EventUnfocusedUpdate != null)
			{
				EventUnfocusedUpdate();
			}
		}
		
		public void CleanUpEvents()
		{
			EventEnter = null;
			EventUpdate = null;
			EventUpdateLate = null;
			EventExit = null;
			
			EventGainedFocus = null;
			EventLostFocus = null;
			EventUnfocusedUpdate = null;
		}
	}

}
