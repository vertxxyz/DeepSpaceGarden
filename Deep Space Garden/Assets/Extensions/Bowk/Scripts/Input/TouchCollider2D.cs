
namespace Bowk
{

	using UnityEngine;
	using System.Collections;
	using Delegates;

	// A touch collider that used a 2D physics collider
	// Drag and Release events are only fired if pressed first

	[RequireComponent(typeof(Collider2D))]
	public class TouchCollider2D : MonoBehaviour
	{

		public event TouchDelegate EventPress 	= null;
		public event TouchDelegate EventRelease = null;
		public event TouchDelegate EventDrag 	= null;
		public event IntDelegate EventTouchEnded 	= null;

		private Collider2D _collider = null;

		private int _active_touch = -1;
		
		// current touch ID - used to keep track on correct finger for drag and release events

		private bool _active = false;

		void Awake()
		{
			_collider = GetComponent<Collider2D>();

			InputTouch.Instance.EventTouchBegan += OnTouchBegan;
			InputTouch.Instance.EventTouchEnded += OnTouchEnded;
			InputTouch.Instance.EventTouchUpdate += OnTouchUpdate;
		}

		void OnDestroy()
		{
			if (InputTouch.Exists)
			{
				InputTouch.Instance.EventTouchBegan -= OnTouchBegan;
				InputTouch.Instance.EventTouchEnded -= OnTouchEnded;
				InputTouch.Instance.EventTouchUpdate -= OnTouchUpdate;
			}
		}

		public void Activate()
		{
			if (!_active)
			{
				_active = true;
			}
		}

		public void Deactivate()
		{
			if (_active)
			{
				_active = false;
			}
		}

		public bool IsBeingTouched()
		{
			return (_active_touch >= 0);
		}

		public void CancelTouch()
		{
			if (EventTouchEnded != null)
			{
				EventTouchEnded(_active_touch);
			}
			_active_touch = -1;
		}

		private void OnTouchBegan(TouchTracker touch)
		{
			if (!_active) return;

			if (DidCollide(touch.GetCurrPosition()) && _active_touch < 0)
			{
				_active_touch = touch.GetFingerID();
				if (EventPress != null)
				{
					EventPress(touch);
				}
			}
		}

		private void OnTouchEnded(TouchTracker touch)
		{
			if (!_active) return;
			if (_active_touch >= 0 && _active_touch == touch.GetFingerID())
			{
				if (DidCollide(touch.GetCurrPosition()))
				{
					if (EventRelease != null)
					{
						EventRelease(touch);
					}
				}
				CancelTouch();
			}
		}

		private void OnTouchUpdate(TouchTracker touch)
		{
			if (!_active) return;
			if (_active_touch >= 0 && _active_touch == touch.GetFingerID())
			{
				if (touch.DeltaPosition().magnitude > 0f)
				{
					if (EventDrag != null)
					{
						EventDrag(touch);
					}
				}
			}
		}

		private bool DidCollide(Vector2 screen_pos)
		{
			Vector2 world_pos = Camera.main.ScreenToWorldPoint(screen_pos);

			return _collider.OverlapPoint(world_pos);
		}

	}

}