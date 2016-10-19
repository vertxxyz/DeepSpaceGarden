
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;
	using Bowk.Delegates;

	// Singleton that detects device orientation based on acc

	public class InputOrientation : MonoSingleton<InputOrientation>
	{
		public enum Orientation
		{
			Portrait		= 1,
			PortraitUpsideDown		= 2,
			LandscapeLeft			= 4,
			LandscapeRight	= 8
		};
		
		public event VoidDelegate	EventOrientationChanged	= null;
		
		public Orientation	currOrientation		= Orientation.Portrait;
		
		private const float		_MinMag			= 0.4f;
		private const float		_MaxAngle		= 30f;
		// Seconds that device needs to stay in before change is registered
		private const float		_ChangeTime 				= 0.75f;
		private float			_ChangeTimer				= 0f;
		private Orientation		_ChangingOrientation		= Orientation.Portrait;
		
		private Vector2	_vUp 	= new Vector2(0f, -1f);
		private Vector2	_vDown 	= new Vector2(0f, 1f);
		private Vector2	_vLeft 	= new Vector2(1f, 0f);
		private Vector2	_vRight = new Vector2(-1f, 0f);
		
		#if UNITY_EDITOR
		// testing ori in editor
		private Orientation	_EditorCachedOrientation	= Orientation.Portrait;
		#endif
		
		public override void Init()
		{
			// Set starting orientation to closest
			Vector2 v2Ori = new Vector2(Input.acceleration.x, Input.acceleration.y);
			if (Mathf.Abs(Vector2.Angle(v2Ori, _vUp)) < _MaxAngle)
			{
				currOrientation = Orientation.Portrait;
			}
			
			if (Mathf.Abs(Vector2.Angle(v2Ori, _vDown)) < _MaxAngle)
			{
				currOrientation = Orientation.PortraitUpsideDown;
			}
			
			if (Mathf.Abs(Vector2.Angle(v2Ori, _vLeft)) < _MaxAngle)
			{
				currOrientation = Orientation.LandscapeLeft;
			}
			
			if (Mathf.Abs(Vector2.Angle(v2Ori, _vRight)) < _MaxAngle)
			{
				currOrientation = Orientation.LandscapeRight;
			}
			
			#if ((!UNITY_IPHONE) && (!UNITY_ANDRIOD))
			//currOrientation = Orientation.Portrait;
			#endif
			#if UNITY_EDITOR
			currOrientation = Orientation.Portrait;
			#endif
			
			if (EventOrientationChanged != null)
			{
				EventOrientationChanged();
			}
		}
		
		void Update()
		{
			UpdateOrientation();
			
			#if UNITY_EDITOR
			if (_EditorCachedOrientation != currOrientation)
			{
				_EditorCachedOrientation = currOrientation;
				if (EventOrientationChanged != null)
				{
					EventOrientationChanged();
				}
			}
			
			TestEvent.Instance.Test(EventOrientationChanged, "InputOrientation.EventOrientationChanged");
			#endif
		}
		
		private void UpdateOrientation()
		{
			Vector2 v2Ori = new Vector2(Input.acceleration.x, Input.acceleration.y);
			Orientation ori = Orientation.Portrait;
			if (v2Ori.magnitude > _MinMag)
			{
				_ChangeTimer += Time.deltaTime;
				
				if (Mathf.Abs(Vector2.Angle(v2Ori, _vUp)) < _MaxAngle)
				{
					ori = Orientation.Portrait;
				}
				
				if (Mathf.Abs(Vector2.Angle(v2Ori, _vDown)) < _MaxAngle)
				{
					ori = Orientation.PortraitUpsideDown;
				}
				
				if (Mathf.Abs(Vector2.Angle(v2Ori, _vLeft)) < _MaxAngle)
				{
					ori = Orientation.LandscapeLeft;
				}
				
				if (Mathf.Abs(Vector2.Angle(v2Ori, _vRight)) < _MaxAngle)
				{
					ori = Orientation.LandscapeRight;
				}
			}
			else
			{
				_ChangeTimer = 0f;
				ori = Orientation.Portrait;
			}
			#if ((!UNITY_IPHONE) && (!UNITY_ANDRIOD))
			//ori = Orientation.Portrait;
			#endif
			#if UNITY_EDITOR
			ori = Orientation.Portrait;
			#endif
			if (ori != _ChangingOrientation)
			{
				_ChangeTimer = 0f;
				_ChangingOrientation = ori;
			}
			
			if (currOrientation != _ChangingOrientation &&
				_ChangeTimer > _ChangeTime)
			{
				currOrientation = _ChangingOrientation;
				_ChangeTimer = 0f;
				
				if (EventOrientationChanged != null)
				{
					EventOrientationChanged();
				}
				
				#if UNITY_EDITOR
				_EditorCachedOrientation = currOrientation;
				#endif
			}
		}
		
	}

}








