using UnityEngine;

public enum ActorInput { 
	Attack, 
	Die,
	Fall,
	Grounded,
	Hit, 
	Interact,
	Jump,
	Move, 
	Timeout
}

[RequireComponent( typeof(ActorBehaviour) )]
public abstract class ActorBrain : MonoBehaviour {
	protected ActorBehaviour actor;

	protected virtual void OnEnable () {
		actor = GetComponent<ActorBehaviour>();
	}
}
