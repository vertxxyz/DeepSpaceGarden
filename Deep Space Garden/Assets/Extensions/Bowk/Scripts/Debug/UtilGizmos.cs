using UnityEngine;

namespace Bowk
{

	public static class UtilGizmos
	{
		private const int CIRCLE_SEGMENTS = 20;

		public static void DrawCircleGizmo(Vector3 pos, float radius)
		{
			DrawCircleGizmo(pos, radius, Vector3.up, Vector3.forward);
		}

		public static void DrawCircleGizmo(Vector3 pos, float radius, Vector3 up, Vector3 forward)
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
				Gizmos.DrawLine(pos1, pos2);
				
	        }
	    }
	}

}