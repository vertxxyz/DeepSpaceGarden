
namespace Bowk
{

	using UnityEngine;
	using System.Collections;

	public class ShuffleBag<T>
	{
		
		private BetterList<T> _data;

		public int size {get{return _data.size;}}

		public ShuffleBag(int capacity = 1)
		{
			_data = new BetterList<T>(capacity);
		}

		public void Clear()
		{
			_data.Clear();
		}

		public void Add(T item, int amount = 1)
		{
			for(int i = 0; i < amount; ++i)
			{
				_data.Add(item);
			}
		}

		public void Shuffle(ref System.Random rnd)
		{
			int n = _data.size;
			while (n > 1)
			{
				n--;
				int k = rnd.Next(0, n + 1);
				T value = _data[k];
				_data[k] = _data[n];
				_data[n] = value;
			}
		}

		public T Peek()
		{
			return _data[_data.size-1];
		}

		public T Remove()
		{
			T item = _data[_data.size-1];
			_data.RemoveAt(_data.size-1);
			return item;
		}

		public T RemoveAt(int i)
		{
			T item = _data[i];
			_data.RemoveAt(i);
			return item;
		}

		public T this[int i]
		{
			get {return _data[i];}
			set {_data[i] = value;}
		}

	}

}






