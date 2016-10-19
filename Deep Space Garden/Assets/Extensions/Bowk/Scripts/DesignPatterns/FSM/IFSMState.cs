
namespace Bowk
{

	public interface IFSMState
	{
	    void Enter();

	    void Update();

		void UpdateLate();

	    void UnfocusedUpdate();

	    void Exit();

	    void LostFocus();

	    void GainedFocus();
	}

}
