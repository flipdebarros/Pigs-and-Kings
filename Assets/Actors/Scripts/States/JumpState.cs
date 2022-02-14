using UnityEngine;
using Utils.DebugFields;

// [StateName("Jump")]
[DebugFields(0f, 1f, 1f)]
public class JumpState : ActorState {

	[SerializeField] [DrawLine(dotted = true)]
	private float jumpDistance;
	[SerializeField] [DrawLine(horizontal = false, dotted = true)]
	private float jumpHeight;

	[SerializeField]
	private float airborneSpeed = 5f;

	private float _timeOfFlight;
	private float _g;
	private float _initialVerticalVelocity;

	private void OnEnable () {
		_timeOfFlight = jumpDistance / airborneSpeed;
		_g = -8f * jumpHeight / (_timeOfFlight * _timeOfFlight);
		_initialVerticalVelocity = 4f * jumpHeight / _timeOfFlight;
	}

	protected override void OnEnter(ActorBehaviour actor) {
		actor.YVelocity = _initialVerticalVelocity;
		actor.GravityScale = _g;
		actor.LeaveGround();
	}
	
	public override void UpdateState (ActorBehaviour actor) {
		actor.XVelocity = actor.MoveAxis * airborneSpeed;
	}
}