
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;

	public static class UtilMath
	{

		// Float
		//-------------------------------------------------

		public const float MIN = 0.01f;
		public static bool CloseTo(float f, float target, float range = MIN)
		{
			if (f >= target-MIN && f <= target+MIN) return true;
			return false;
		}

		//-------------------------------------------------

		// Vec 2
		//-------------------------------------------------

		public static Vector2 ClosestPointOnLine(Vector2 a, Vector2 b, Vector2 p)
		{
			return ClosestPointOnLineSegment(a, b, p, false);
		}

		public static Vector2 ClosestPointOnLineSegment(Vector2 a, Vector2 b, Vector2 p, bool seg_clamp = true)
		{
			Vector2 ap = p - a;
			Vector2 ab = b - a;
			float ab2 = ab.x*ab.x + ab.y*ab.y;
			float ap_ab = ap.x*ab.x + ap.y*ab.y;
			float t = ap_ab / ab2;
			if (seg_clamp)
			{
				if (t < 0.0f) t = 0.0f;
				else if (t > 1.0f) t = 1.0f;
			}
			Vector2 closest = a + ab * t;
			return closest;
		}

		public static Vector3 ClosestPointOnLineSegment3D(Vector3 a, Vector3 b, Vector3 p)
		{
			Vector3 v = b - a;
			Vector3 w = p - a;

			float c1 = Vector3.Dot(w,v);
			if ( c1 <= 0 )
				return a;

			float c2 = Vector3.Dot(v,v);
			if ( c2 <= c1 )
				return b;

			float b2 = c1 / c2;
			Vector3 Pb = a + (b2 * v);
			return Pb;
		}

		public static Vector2 ClosestPointOnRect(Rect r, Vector2 p)
		{
			Vector2 p1 = new Vector2(r.xMin, r.yMax);
			Vector2 p2 = new Vector2(r.xMax, r.yMax);
			Vector2 p3 = new Vector2(r.xMax, r.yMin);
			Vector2 p4 = new Vector2(r.xMin, r.yMin);

			Vector2 c_a = ClosestPointOnLineSegment(p1, p2, p);
			Vector2 c_b = ClosestPointOnLineSegment(p2, p3, p);
			Vector2 c_c = ClosestPointOnLineSegment(p3, p4, p);
			Vector2 c_d = ClosestPointOnLineSegment(p4, p1, p);

			Vector2 closest = c_a;
			float closest_sqr = Vector2.SqrMagnitude(closest-p);

			if (Vector2.SqrMagnitude(c_b-p) < closest_sqr)
			{
				closest = c_b;
				closest_sqr = Vector2.SqrMagnitude(closest-p);
			}
			if (Vector2.SqrMagnitude(c_c-p) < closest_sqr)
			{
				closest = c_c;
				closest_sqr = Vector2.SqrMagnitude(closest-p);
			}
			if (Vector2.SqrMagnitude(c_d-p) < closest_sqr)
			{
				closest = c_d;
				closest_sqr = Vector2.SqrMagnitude(closest-p);
			}

			return closest;
		}

		//-------------------------------------------------

	}

}
