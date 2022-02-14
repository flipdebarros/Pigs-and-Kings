using UnityEngine;

// [StateName("Move")]
public class MoveState : ActorState {

	[SerializeField]
	private float speed;
	
	public override void UpdateState (ActorBehaviour actor) {
		actor.XVelocity = actor.MoveAxis * speed;
	}
}
