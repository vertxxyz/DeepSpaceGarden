
namespace Bowk
{

	using UnityEngine;
	using System.Collections;
	using Delegates;

	// A touch collider that uses one or many boxes
	// Drag and Release events are only fired if pressed first

	public class TouchColliderRects : MonoBehaviour
	{

		#if UNITY_EDITOR
		public bool DEBUG_gizmos = false;
		public Color DEBUG_gizmo_colour = Color.green;
		#endif

		public event TouchDelegate EventPress 	= null;
		public event TouchDelegate EventRelease = null;
		public event TouchDelegate EventDrag 	= null;
		public event IntDelegate EventTouchEnded 	= null;

		private BetterList<Rect> _rects = new BetterList<Rect>();

		private int _active_touch = -1;
		
		// current touch ID - used to keep track on correct finger for drag and release events

		private bool _active = false;

		void Awake()
		{
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

		//-------

		public void ClearRects()
		{
			_rects.Clear();
		}
		
		public void AddRect(Rect r)
		{
			_rects.Add(r);
		}

		public void UpdateRect(int index, Rect r)
		{
			_rects[index] = r;
		}

		//-------

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
			Vector2 touch_world_pos = Camera.main.ScreenToWorldPoint(screen_pos);
			Vector3 pos = transform.position;

			bool did_collide = false;
			for(int i = 0; !did_collide && i < _rects.size; ++i)
			{
				Rect r = _rects[i];
				r.position += new Vector2(pos.x, pos.y);

				if (touch_world_pos.x > r.xMin && touch_world_pos.x < r.xMax &&
				    touch_world_pos.y > r.yMin && touch_world_pos.y < r.yMax )
				{
					did_collide = true;
				}
			}

			return did_collide;
		}

		#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			if (!DEBUG_gizmos) return;

			Gizmos.color = DEBUG_gizmo_colour;
			for(int i = 0; i < _rects.size; ++i)
			{
				Rect r = _rects[i];
				r.position += new Vector2(transform.position.x, transform.position.y);

				Vector3 v1 = new Vector3(r.xMin, r.yMax, 0f);
				Vector3 v2 = new Vector3(r.xMax, r.yMax, 0f);
				Vector3 v3 = new Vector3(r.xMax, r.yMin, 0f);
				Vector3 v4 = new Vector3(r.xMin, r.yMin, 0f);

				Gizmos.DrawLine(v1, v2);
				Gizmos.DrawLine(v2, v3);
				Gizmos.DrawLine(v3, v4);
				Gizmos.DrawLine(v4, v1);
			}

			Gizmos.color = Color.white;
		}
		#endif

	}

}







































