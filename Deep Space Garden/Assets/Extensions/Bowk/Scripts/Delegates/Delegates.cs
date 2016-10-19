using UnityEngine;
using System.Collections.Generic;

namespace Bowk
{

	namespace Delegates
	{
		// Generic
		public delegate void VoidDelegate();
		public delegate void FloatDelegate(float val);
	    public delegate void IntDelegate(int val);
		public delegate void UIntDelegate(uint val);
		public delegate void BoolDelegate(bool val);
		
		// Unity
		public delegate void Vector2Delegate(Vector2 val);

	}

}