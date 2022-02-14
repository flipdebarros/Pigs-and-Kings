using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Utils.DebugFields;

[RequireComponent(
	typeof(Rigidbody2D),
	typeof(ActorAnimation))] 
[DebugFields]
public class ActorBehaviour : MonoBehaviour {

	[Header("Ground Detection")]
	[SerializeField] [DrawDottedBox(1f, 0f, 0f)]
	private Rect groundContact;

	[SerializeField]
	private LayerMask groundLayer;
	
	[Header("Imprecise Input")]
	
	[SerializeField] [Range(0f, 1f)]
	private float coyoteTime = 0.2f;
	[SerializeField] [Range(0f, 1f)]
	private float bufferTime = 0.2f;
	
	private ActorState _currentState;
	private Rigidbody2D _rigidbody2D;
	private ActorAnimation _animation;
	
	public float MoveAxis { get; private set; }

	private const float VelocityDeadZone = 0.1f;
	private static float DeadZone(float x) => Mathf.Abs(x) <= VelocityDeadZone ? 0f : Mathf.Sign(x);
	private static Vector2 DeadZone(Vector2 v) => new Vector2(DeadZone(v.x), DeadZone(v.y));
	public Vector2 VelocityDirection => DeadZone(_rigidbody2D.velocity);
	public float XVelocity {
		get => _rigidbody2D.velocity.x;
		set => _rigidbody2D.velocity = new Vector2(value, _rigidbody2D.velocity.y);
	}
	public float YVelocity {
		get => _rigidbody2D.velocity.y;
		set => _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, value);
	}
	public bool IsFalling => !Grounded && VelocityDirection.y < 0f;
	public float GravityScale {
		get => _rigidbody2D.gravityScale * Physics2D.gravity.y;
		set => _rigidbody2D.gravityScale = value / Physics2D.gravity.y;
	}

	private bool _grounded = true;
	public bool Grounded {
		get => _grounded;
		private set{
			if(!_grounded && value) HandleInput(ActorInput.Grounded, false); //if state changed to true, send a grounded input
			_grounded = value;
		}
	}

	private ActorState CurrentState {
		get => _currentState;
		set {
			if(value == null) return;
			_currentState.Exit(this);
			_currentState = value;
			_currentState.Enter(this);
		}
	}
	
	private void OnEnable () {
		_currentState = GetComponent<ActorState>();
		_currentState.Enter(this);
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_animation = GetComponent<ActorAnimation>();
	}

	public void HandleInput (ActorInput input, bool buffer = true) {
		Debug.Log(CurrentState.GetType() + " " + input);
		CurrentState = CurrentState.HandleInput(this, input, buffer);
	}

	public void AnimationInput(ActorAnimationInput input) => _animation.HandleInput(input);

	public void SetAxis(ActorInput input, float axis) {
		if (input == ActorInput.Move) MoveAxis = axis;
	}

	private bool _wasFalling;
	private void Update () {
		CheckGround();
		CurrentState.UpdateState(this);

		if(!_wasFalling && IsFalling)
			HandleInput(ActorInput.Fall, false);
		_wasFalling = IsFalling;
		
	}

	private bool _inCoyoteTime;
	private void CheckGround () {
		var box = groundContact;
		box.position += (Vector2) transform.position - groundContact.size / 2f;
		var hit = Physics2D.BoxCast(box.position, box.size, 0f, Vector2.zero, Mathf.Infinity, groundLayer);


		if (hit.normal.y > 0f && VelocityDirection.y <= 0f) {
			_inCoyoteTime = false;
			Grounded = true;
		}
		else if (!hit && Grounded && !_inCoyoteTime) 
			StartCoroutine(CoyoteTime());
	}
	
	public void LeaveGround () {
		_inCoyoteTime = false;
		Grounded = false;
	}
	
	private IEnumerator CoyoteTime () {
		_inCoyoteTime = true;

		yield return new WaitForSeconds(coyoteTime);

		if(!_inCoyoteTime) yield break;
		
		Grounded = false;
		_inCoyoteTime = false;
	}

	private bool _inBufferTime;
	private IEnumerator BufferTime (ActorInput input, float t) {
		Debug.Log("Buffer: " + CurrentState.GetType() + " " + input);
		
		_inBufferTime = true;

		var startTime = Time.time;
		while (!Grounded)
			yield return null;
		
		if(Time.time - startTime <= t) HandleInput(input);
		_inBufferTime = false;
	}

	private Coroutine _currentBuffer;
	public void InputBuffer (ActorInput input) {
		if (_inBufferTime) {
			StopCoroutine(_currentBuffer);
			_inBufferTime = false;
		}

		_currentBuffer = StartCoroutine(BufferTime(input, bufferTime));
	}
}
