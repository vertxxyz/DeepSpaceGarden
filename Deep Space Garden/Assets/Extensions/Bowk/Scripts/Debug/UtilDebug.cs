using UnityEngine;

namespace Bowk
{

	public static class UtilDebug
	{
		private const int CIRCLE_SEGMENTS = 20;
		public static void DrawCircle(Vector3 pos, float radius, Color col, float time = 0f)
		{
			DrawCircle(pos, radius, col, time, Vector3.up, Vector3.forward);
		}

		public static void DrawCircle(Vector3 pos, float radius, Color col, float time, Vector3 up, Vector3 forward)
	    {
			forward.Normalize();
			up.Normalize();
			
	        float rotAngle = 360f / CIRCLE_SEGMENTS;
			Quaternion q = Quaternion.AngleAxis(rotAngle, forward);
			Vector3 pos1, pos2;
	        for (int i = 0; i < CIRCLE_SEGMENTS; i++)
	        {
				q = Quaternion.AngleAxis(rotAngle * i, forward);
				pos1 = pos + ((q * up) * radius);
				q = Quaternion.AngleAxis(rotAngle * (i+1), forward);
				pos2 = pos + ((q * up) * radius);
				Debug.DrawLine(pos1, pos2, col, time);
	        }
	    }

		public static void DrawRect(Rect r, Color c, float t = 0f)
		{
			Debug.DrawLine(new Vector3(r.xMin, r.yMin, 0f), new Vector3(r.xMax, r.yMin, 0f), c, t);
			Debug.DrawLine(new Vector3(r.xMax, r.yMin, 0f), new Vector3(r.xMax, r.yMax, 0f), c, t);
			Debug.DrawLine(new Vector3(r.xMax, r.yMax, 0f), new Vector3(r.xMin, r.yMax, 0f), c, t);
			Debug.DrawLine(new Vector3(r.xMin, r.yMax, 0f), new Vector3(r.xMin, r.yMin, 0f), c, t);
		}
	}

}