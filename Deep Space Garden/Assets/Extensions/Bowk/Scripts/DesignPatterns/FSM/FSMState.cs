
namespace Bowk
{

	abstract public class FSMState : IFSMState
	{
		public virtual void Enter(){}

	    public virtual void Update(){}

		public virtual void UpdateLate(){}

	    public virtual void UnfocusedUpdate(){}

	    public virtual void Exit(){}

	    public virtual void LostFocus(){}

	    public virtual void GainedFocus(){}
	}

}
