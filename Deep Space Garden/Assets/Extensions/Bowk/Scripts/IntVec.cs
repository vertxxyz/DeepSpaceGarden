
namespace Bowk
{
		
	using UnityEngine;
	using System.Collections;

	[System.Serializable]
	public struct IntRect
	{
		public IntVector2 position;
		public IntVector2 size;
	}

	[System.Serializable]
	public struct IntVector2
	{
		[SerializeField]
		private int _x;
		[SerializeField]
		private int _y;

		public int x
		{
			get {return this._x;}
			set {this._x = value;}
		}
		public int y
		{
			get {return this._y;}
			set {this._y = value;}
		}

		public IntVector2(IntVector2 v)
		{
			_x = v.x;
			_y = v.y;
		}
		
		public IntVector2(int a_x, int a_y)
		{
			_x = a_x;
			_y = a_y;
		}

		public static IntVector2 zero
		{
			get
			{
				return new IntVector2(0, 0);
			}
		}

		public Vector2 ToVector2()
		{
			return new Vector2(_x, _y);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(_x, _y, 0f);
		}

		public int SqrMagnitude()
		{
			return (_x * _x) + (_y * _y);
		}

		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.y.GetHashCode();
		}
		
		public override string ToString ()
		{
			return "x:" + _x.ToString() + ";y:" + _y.ToString();
		}

		public override bool Equals (object other)
		{
			if (!(other is IntVector2))
			{
				return false;
			}
			IntVector2 b = (IntVector2)other;
			return this.x.Equals(b.x) && this.y.Equals(b.y);
		}

		public static bool Equals(IntVector2 a, IntVector2 b)
		{
			return (a.x == b.x && a.y == b.y);
		}

		static public bool operator ==(IntVector2 a, IntVector2 b)
		{
			return (a.x == b.x && a.y == b.y);
		}
		
		static public bool operator !=(IntVector2 a, IntVector2 b)
		{
			return !(a==b);
		}
		
		static public IntVector2 operator +(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x + b.x, a.y + b.y);
		}
		
		static public IntVector2 operator -(IntVector2 a, IntVector2 b)
		{
			return new IntVector2(a.x - b.x, a.y - b.y);
		}

	}

	[System.Serializable]
	public struct IntVector3
	{
		[SerializeField]
		private int _x;
		[SerializeField]
		private int _y;
		[SerializeField]
		private int _z;

		public int x
		{
			get {return this._x;}
			set {this._x = value;}
		}
		public int y
		{
			get {return this._y;}
			set {this._y = value;}
		}
		public int z
		{
			get {return this._z;}
			set {this._z = value;}
		}

		public IntVector3(IntVector3 v)
		{
			_x = v.x;
			_y = v.y;
			_z = v.z;
		}
		
		public IntVector3(int a_x, int a_y, int a_z)
		{
			_x = a_x;
			_y = a_y;
			_z = a_z;
		}

		public static IntVector3 zero
		{
			get
			{
				return new IntVector3(0, 0, 0);
			}
		}

		public int SqrMagnitude()
		{
			return (_x * _x) + (_y * _y) + (_z * _z);
		}

		// Not the best, has collisions? Haven't looked into it fully ...
		public override int GetHashCode()
		{
			Debug.LogWarning("IntVec3 hash code not tested fully.");
			int hash = 17;
			hash = hash * 23 + x;
			hash = hash * 23 + y;
			hash = hash * 23 + z;
			return hash;
		}
		
		public override string ToString ()
		{
			return "x:" + x.ToString() + ";y:" + y.ToString() + "z:" + z.ToString();
		}

		public override bool Equals (object other)
		{
			if (!(other is IntVector3))
			{
				return false;
			}
			IntVector3 b = (IntVector3)other;
			return this.x.Equals(b.x) && this.y.Equals(b.y) && this.z.Equals(b.z);
		}

		// --

		public static bool Equals(IntVector3 a, IntVector3 b)
		{
			return (a.x == b.x && a.y == b.y && a.z == b.z);
		}
		
		static public bool operator ==(IntVector3 a, IntVector3 b)
		{
			return (a.x == b.x && a.y == b.y && a.z == b.z);
		}
		
		static public bool operator !=(IntVector3 a, IntVector3 b)
		{
			return !(a==b);
		}
		
		static public IntVector3 operator +(IntVector3 a, IntVector3 b)
		{
			return new IntVector3(a.x + b.x, a.y + b.y, a.z + b.z);
		}
		
		static public IntVector3 operator -(IntVector3 a, IntVector3 b)
		{
			return new IntVector3(a.x - b.x, a.y - b.y, a.z - b.z);
		}
	}

}
