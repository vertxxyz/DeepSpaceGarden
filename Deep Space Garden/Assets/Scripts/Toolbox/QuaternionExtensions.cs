using UnityEngine;
using System.Collections;

public static class QuaternionExtensions
{
	public static Quaternion Pow(this Quaternion input, float power)
	{
		float inputMagnitude = input.Magnitude();
		Vector3 nHat = new Vector3(input.x, input.y, input.z).normalized;
		Quaternion vectorBit = new Quaternion(nHat.x, nHat.y, nHat.z, 0)
			.ScalarMultiply(power * Mathf.Acos(input.w / inputMagnitude))
				.Exp();
		return vectorBit.ScalarMultiply(Mathf.Pow(inputMagnitude, power));
	}

	public static Quaternion Exp(this Quaternion input)
	{
		float inputA = input.w;
		Vector3 inputV = new Vector3(input.x, input.y, input.z);
		float outputA = Mathf.Exp(inputA) * Mathf.Cos(inputV.magnitude);
		Vector3 outputV = Mathf.Exp(inputA) * (inputV.normalized * Mathf.Sin(inputV.magnitude));
		return new Quaternion(outputV.x, outputV.y, outputV.z, outputA);
	}
	
	public static float Magnitude(this Quaternion input)
	{
		return Mathf.Sqrt(input.x * input.x + input.y * input.y + input.z * input.z + input.w * input.w);
	}
	
	public static Quaternion ScalarMultiply(this Quaternion input, float scalar)
	{
		return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
	}

	public static Quaternion FromToRotation (Quaternion from, Quaternion to){
		return from * Quaternion.Inverse (to);
	}

	public static Quaternion Normalize (this Quaternion q) {
		Vector4 V4 = Vector4.Normalize(new Vector4 (q.x, q.y, q.z, q.w));
		return new Quaternion (V4.x, V4.y, V4.z, V4.w);
	}

	/*#region SQUAD
	public static Quaternion SQUAD(Quaternion q1, Quaternion t1, Quaternion t2, Quaternion q2, float t){
		Quaternion slerp1 = SlerpNoInvert(q1, q2, t);
		Quaternion slerp2 = SlerpNoInvert(t1, t2, t);
		return SlerpNoInvert(slerp1, slerp2, 2f * t * (1f - t));
	}

	public static Quaternion SplineSegment(Quaternion q0, Quaternion q1, Quaternion q2, Quaternion q3, float t){
		Quaternion qa = Intermediate(q0,q1,q2);
		Quaternion qb = Intermediate(q1,q2,q3);
		return SQUAD(q1, qa, qb, q2, t);
	}

	static Quaternion Intermediate(Quaternion q0, Quaternion q1, Quaternion q2){
		Quaternion q1inv = Quaternion.Inverse (q1);
		Quaternion c1 = q1inv * q2;
		Quaternion c2 = q1inv * q0;
		c1 = c1.Log_2 ();
		c2 = c2.Log_2 ();
		Quaternion c3 = Add(c2, c1);
		c3 = c3.ScalarMultiply (-0.25f);
		c3 = c3.Exp_2 ();
		Quaternion r = q1 * c3 ;
		r = r.Normalize ();
		return r;
	}

	static Quaternion Exp_2 (this Quaternion q){
		float angle = Mathf.Sqrt (q.x * q.x + q.y * q.y + q.z * q.z);
		float sinAngle = Mathf.Sin (angle);
		q.w = Mathf.Cos(angle);
		if (!Mathf.Approximately(angle,0)) {
			float coeff = sinAngle / angle;
			q.x*=coeff;
			q.y*=coeff;
			q.z*=coeff;
		}
		return q;
	}

	static Quaternion Log_2 (this Quaternion q){
		float a = Mathf.Acos(q.w), s = Mathf.Sin(a);

		if (Mathf.Approximately(s, 0 )) return Quaternion.identity;
		a /= s;
		return new Quaternion( q.x * a,  q.y * a,  q.z * a, 0 );
	}

	static Quaternion SlerpNoInvert(Quaternion from, Quaternion to, float factor){
		float dot = Quaternion.Dot(from, to);

		if (Mathf.Abs(dot) > 0.9999f) return from;

		float	theta		= Mathf.Acos(dot),
		sinT		= 1.0f / Mathf.Sin(theta),
		newFactor	= Mathf.Sin(factor * theta) * sinT,
		invFactor	= Mathf.Sin((1.0f - factor) * theta) * sinT;

		return new Quaternion( invFactor * from.x + newFactor * to.x,
			invFactor * from.y + newFactor * to.y,
			invFactor * from.z + newFactor * to.z,
			invFactor * from.w + newFactor * to.w );
	}

	static Quaternion Add (Quaternion a, Quaternion b){
		return new Quaternion (a.x+b.x,a.y+b.y,a.z+b.z,a.w + b.w);
	}
	#endregion*/
}