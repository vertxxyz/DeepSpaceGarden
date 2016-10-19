
namespace Bowk
{
		
	using System.Collections.Generic;

	/// <summary>
	/// Uses the List class to mimic a stack, so that all elements
	/// of the stack can still be inspected
	/// </summary>
	public class ListStack<T> : List<T>
	{
	    /// <summary>
	    /// Pushes the given item onto the stack.
	    /// </summary>
	    /// <param name="item">Item to push onto the stack</param>
	    public void Push(T item)
	    {
	        Add(item);
	    }

	    /// <summary>
	    /// Pops the item from the top of the stack and returns it
	    /// </summary>
	    /// <returns>The item popped from the top of the stack</returns>
	    public T Pop()
	    {
	        if(Count == 0)
	            return default(T);

	        T item = this[Count - 1];
	        RemoveAt(Count - 1);
	        return item;
	    }

	    /// <summary>
	    /// Returns the top item on the stack without removing
	    /// it from the stack
	    /// </summary>
	    /// <returns>Top item in the stack</returns>
	    public T Peek()
	    {
	        if(Count == 0)
	            return default(T);

	        return this[Count - 1];
	    }
	}

}




