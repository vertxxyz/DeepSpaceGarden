
namespace Bowk
{

	using UnityEngine;
	using System.Collections.Generic;
	using System.Diagnostics;

	/// <summary>
	/// This improved version of the System.Collections.Generic.List that doesn't release the buffer on Clear(), resulting in better performance and less garbage collection.
	/// </summary>

	public class BetterList<T>
	{
		/// <summary>
		/// Direct access to the buffer. Note that you should not use its 'Length' parameter, but instead use BetterList.size.
		/// </summary>

		public T[] buffer;

		/// <summary>
		/// Direct access to the buffer's size. Note that it's only public for speed and efficiency. You shouldn't modify it.
		/// </summary>

		public int size = 0;

		public BetterList()
		{
		}

		public BetterList(int buffer_size)
		{
			Clear();
			Allocate(buffer_size);
		}

		public BetterList(ICollection<T> items)
		{
			Clear();
			AddRange(items);
		}

		public BetterList(BetterList<T> b_list)
		{
			Clear();
			AddRange(b_list.buffer);
		}

		/// <summary>
		/// For 'foreach' functionality.
		/// </summary>

		[DebuggerHidden]
		[DebuggerStepThrough]
		public IEnumerator<T> GetEnumerator ()
		{
			if (buffer != null)
			{
				for (int i = 0; i < size; ++i)
				{
					yield return buffer[i];
				}
			}
		}
		
		/// <summary>
		/// Convenience function. I recommend using .buffer instead.
		/// </summary>

		[DebuggerHidden]
		public T this[int i]
		{
			get { return buffer[i]; }
			set { buffer[i] = value; }
		}

		/// <summary>
		/// Helper function that expands the size of the array, maintaining the content.
		/// </summary>

		void AllocateMore ()
		{
			T[] newList = (buffer != null) ? new T[Mathf.Max(buffer.Length << 1, 32)] : new T[32];
			if (buffer != null && size > 0) buffer.CopyTo(newList, 0);
			buffer = newList;
		}

		/// <summary>
		/// Expand size of the buffer to size.
		/// </summary>

		public void Allocate(int alloc_size)
		{
			if (buffer == null || buffer.Length < alloc_size)
			{
				T[] newList = new T[alloc_size];
				if (buffer != null && size > 0) buffer.CopyTo(newList, 0);
				buffer = newList;
			}
		}

		/// <summary>
		/// Trim the unnecessary memory, resizing the buffer to be of 'Length' size.
		/// Call this function only if you are sure that the buffer won't need to resize anytime soon.
		/// </summary>

		void Trim ()
		{
			if (size > 0)
			{
				if (size < buffer.Length)
				{
					T[] newList = new T[size];
					for (int i = 0; i < size; ++i) newList[i] = buffer[i];
					buffer = newList;
				}
			}
			else buffer = null;
		}

		/// <summary>
		/// Clear the array by resetting its size to zero. Note that the memory is not actually released.
		/// </summary>

		public void Clear () { size = 0; }

		/// <summary>
		/// Clear the array and release the used memory.
		/// </summary>

		public void Release () { size = 0; buffer = null; }

		/// <summary>
		/// Add the specified item to the end of the list.
		/// </summary>

		public void Add (T item)
		{
			if (buffer == null || size == buffer.Length) AllocateMore();
			buffer[size++] = item;
		}
		
		public void AddRange(ICollection<T> items)
		{
			foreach(T item in items)
			{
				Add(item);
			}
		}

		public void Reverse()
		{
			System.Array.Reverse(buffer, 0, size);
		}

		/// <summary>
		/// Insert an item at the specified index, pushing the entries back.
		/// </summary>

		public void Insert (int index, T item)
		{
			if (buffer == null || size == buffer.Length) AllocateMore();

			if (index < size)
			{
				for (int i = size; i > index; --i) buffer[i] = buffer[i - 1];
				buffer[index] = item;
				++size;
			}
			else Add(item);
		}

		/// <summary>
		/// Returns 'true' if the specified item is within the list.
		/// </summary>

		public bool Contains (T item)
		{
			if (buffer == null) return false;
			for (int i = 0; i < size; ++i) if (buffer[i].Equals(item)) return true;
			return false;
		}

		/// <summary>
		/// Remove the specified item from the list. Note that RemoveAt() is faster and is advisable if you already know the index.
		/// </summary>

		public bool Remove (T item)
		{
			if (buffer != null)
			{
				EqualityComparer<T> comp = EqualityComparer<T>.Default;

				for (int i = 0; i < size; ++i)
				{
					if (comp.Equals(buffer[i], item))
					{
						--size;
						buffer[i] = default(T);
						for (int b = i; b < size; ++b) buffer[b] = buffer[b + 1];
						buffer[size] = default(T);
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Remove an item at the specified index.
		/// </summary>

		public void RemoveAt (int index)
		{
			if (buffer != null && index < size)
			{
				--size;
				buffer[index] = default(T);
				for (int b = index; b < size; ++b) buffer[b] = buffer[b + 1];
				buffer[size] = default(T);
			}
		}

		/// <summary>
		/// Remove an item from the end.
		/// </summary>

		public T Pop ()
		{
			if (buffer != null && size != 0)
			{
				T val = buffer[--size];
				buffer[size] = default(T);
				return val;
			}
			return default(T);
		}

		/// <summary>
		/// Mimic List's ToArray() functionality, except that in this case the list is resized to match the current size.
		/// </summary>

		public T[] ToArray () { Trim(); return buffer; }

		//class Comparer : System.Collections.IComparer
		//{
		//    public System.Comparison<T> func;
		//    public int Compare (object x, object y) { return func((T)x, (T)y); }
		//}

		//Comparer mComp = new Comparer();

		/// <summary>
		/// List.Sort equivalent. Doing Array.Sort causes GC allocations.
		/// </summary>

		//public void Sort (System.Comparison<T> comparer)
		//{
		//    if (size > 0)
		//    {
		//        mComp.func = comparer;
		//        System.Array.Sort(buffer, 0, size, mComp);
		//    }
		//}

		/// <summary>
		/// List.Sort equivalent. Manual sorting causes no GC allocations.
		/// </summary>

		[DebuggerHidden]
		[DebuggerStepThrough]
		public void Sort (CompareFunc comparer)
		{
			int start = 0;
			int max = size - 1;
			bool changed = true;

			while (changed)
			{
				changed = false;

				for (int i = start; i < max; ++i)
				{
					// Compare the two values
					if (comparer(buffer[i], buffer[i + 1]) > 0)
					{
						// Swap the values
						T temp = buffer[i];
						buffer[i] = buffer[i + 1];
						buffer[i + 1] = temp;
						changed = true;
					}
					else if (!changed)
					{
						// Nothing has changed -- we can start here next time
						start = (i == 0) ? 0 : i - 1;
					}
				}
			}
		}

		/// <summary>
		/// Comparison function should return -1 if left is less than right, 1 if left is greater than right, and 0 if they match.
		/// </summary>

		public delegate int CompareFunc (T left, T right);
	}

}