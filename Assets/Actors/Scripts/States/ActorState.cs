using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GroundedState {
	Ignore,
	Grounded,
	NotGrounded,
}


[Serializable]
[RequireComponent(typeof(Animator))]
public abstract class ActorState : MonoBehaviour {

	[Serializable]
	private class StateTransition {
		public ActorState nextState;
		public ActorInput input;
		public GroundedState checkGrounded;
		public ActorAnimationInput animationInput = ActorAnimationInput.None;
		public float coolDown;
	}

	
	[SerializeField] 
	private List<StateTransition> transitions;

	private float _enterTime;

	protected virtual void OnEnter(ActorBehaviour actor) { }
	protected virtual void OnExit(ActorBehaviour actor) { }
	
	public void Enter(ActorBehaviour actor) {
		_enterTime = Time.time;
		OnEnter(actor);
	}
	public void Exit(ActorBehaviour actor) {
		OnExit(actor);
	}
	public virtual void UpdateState(ActorBehaviour actor) { }

	protected virtual void OnTransition(ActorBehaviour actor, ActorInput input) { }
	public ActorState HandleInput(ActorBehaviour actor, ActorInput input, bool buffer) {
		var transition = transitions.FirstOrDefault(t => t.input == input);

		if (buffer && !actor.Grounded)
			actor.InputBuffer(input);
		
		if (transition == null) return null;
		
		switch (transition.checkGrounded) {
			case GroundedState.Grounded when !actor.Grounded: return null;
			case GroundedState.NotGrounded when actor.Grounded: return null;
		}

		if (Time.time - _enterTime < transition.coolDown) return null;
		
		OnTransition(actor, input);
		actor.AnimationInput(transition.animationInput);
		Debug.Log(transition.nextState);
		return transition.nextState;
	}
}