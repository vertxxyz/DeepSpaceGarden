
namespace Bowk
{
		
	using System.Collections.Generic;

	public class FiniteStateMachine
	{
		private readonly ListStack<IFSMState> _StateStack	= new ListStack<IFSMState>();
		
		public FiniteStateMachine()
		{
		}
		
		public void FSMUpdate()
		{
			if (_StateStack.Count > 0)
			{
				IFSMState currState = _StateStack.Peek();
				currState.Update();
				currState.UpdateLate();

				// Unfocus update the other states
				for (int i = 0; i < _StateStack.Count -1; ++i)
				{
					_StateStack[i].UnfocusedUpdate();
				}
				
				#if UNITY_EDITOR
				// Stack warning
				const int _StackCountWarning = 6;
				if (_StateStack.Count > _StackCountWarning)
				{
					UnityEngine.Debug.LogWarning("FSM stack hit " + 
						_StateStack.Count.ToString());
				}
				#endif
			}
		}
		
		/// <summary>
		/// Pops all stacked states, and sets the given state as the new state
		/// </summary>
		public void NewState(IFSMState newState)
		{
			while(_StateStack.Count > 0)
			{
				IFSMState state = _StateStack.Peek();
				state.Exit();
				_StateStack.Pop();
			}
			
			_StateStack.Push(newState);
			newState.Enter();
		}
		
		/// <summary>
		/// Pushes the new State onto the stack.  The Current State loses focus.
		/// </summary>
		/// <param name="newState">The new state to push onto the stack</param>
		public void PushState(IFSMState newState, params object[] objs)
		{
			IFSMState currentState = _StateStack.Peek();
			if(currentState != null)
			{
				currentState.LostFocus();
			}
			
			_StateStack.Push(newState);
			newState.Enter();
		}
		
		/// <summary>
		/// Pops the current state from the top of the stack and returns it.  If there
		/// is another state left in the stack, it regains focus.
		/// </summary>
		/// <returns>The state popped from the stack, or NULL if the stack is empty</returns>
		public IFSMState PopState()
		{
			IFSMState returnState = null;
			if(_StateStack.Count > 0)
			{
				returnState = _StateStack.Peek();
				returnState.Exit();
				_StateStack.Pop();
				
				if (_StateStack.Count > 0)
				{
					IFSMState newState = _StateStack.Peek();
					newState.GainedFocus();
				}
			}
			return returnState;
		}
		
		public IFSMState Peek()
		{
			if (_StateStack.Count > 0)
			{
				return _StateStack.Peek();
			}
			return null;
		}
		
		public bool HasState(IFSMState state)
		{
			//_StateStack
			return (_StateStack.Contains(state));
		}
		
		public string BuildStateStackString()
		{
			string str = "";
			foreach(IFSMState state in _StateStack)
			{
				str += state.ToString() + "->";
			}
			return str;
		}
		
	};

}






