using UnityEngine;

// [StateName("Fall")]
public class FallState : ActorState {
	
	[SerializeField]
	private float airborneSpeed = 5f;
	[SerializeField] 
	private float terminalVelocity = 12f;

	[SerializeField] [Range(1f, 10f)]
	private float gravityMultiplier = 1f;

	protected override void OnEnter(ActorBehaviour actor) {
		actor.GravityScale *= gravityMultiplier;
	}
	
	public override void UpdateState (ActorBehaviour actor) {
		actor.YVelocity = Mathf.Max(actor.YVelocity, -terminalVelocity);
		actor.XVelocity = actor.MoveAxis * airborneSpeed;
	}

	protected override void OnExit(ActorBehaviour actor) {
		actor.GravityScale /= gravityMultiplier;
	}
}