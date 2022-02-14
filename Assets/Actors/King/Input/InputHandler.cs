public class InputHandler : ActorBrain {
	private PlayerActions _inputs;

	protected override void OnEnable () {
		base.OnEnable();
		
		_inputs = new PlayerActions();
		_inputs.Enable();

		_inputs.Gameplay.Pause.performed  += _ => PauseManager.Pause();
		_inputs.Gameplay.Attack.performed += _ => { if(!PauseManager.IsPaused) actor.HandleInput(ActorInput.Attack); };
		_inputs.Gameplay.Jump.performed   += _ => { if(!PauseManager.IsPaused) actor.HandleInput(ActorInput.Jump); };
		_inputs.Gameplay.Move.performed   += context => { actor.SetAxis(ActorInput.Move, !PauseManager.IsPaused ? context.ReadValue<float>() : 0f); };
		_inputs.Gameplay.Move.canceled    += _ => actor.SetAxis(ActorInput.Move, 0f);
	}
	private void OnDisable () {
		_inputs.Disable();
		_inputs = null;
	}
}
