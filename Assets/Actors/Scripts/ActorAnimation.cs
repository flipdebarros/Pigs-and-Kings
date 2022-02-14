using UnityEngine;

public enum ActorAnimationInput{
    None,
    Attack,
    Die,
    Hit,
    Interact,
    Jump,
    Restart
}

[RequireComponent( 
    typeof(ActorBehaviour), 
    typeof(Animator), 
    typeof(SpriteRenderer))]
public class ActorAnimation : MonoBehaviour {

    private ActorBehaviour _actor;
    private Animator _animator;
    private SpriteRenderer _sprite;

    private static readonly int XSpeed = Animator.StringToHash("xSpeed");
    private static readonly int YSpeed = Animator.StringToHash("ySpeed");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Die = Animator.StringToHash("Die");
    private static readonly int Dead = Animator.StringToHash("Dead");
    private static readonly int Leave = Animator.StringToHash("Leave");
    private static readonly int Restart = Animator.StringToHash("Restart");
    private static readonly int Jump = Animator.StringToHash("Jump");


    private void OnEnable () {
        _actor = GetComponent<ActorBehaviour>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update() {
        if (_actor.VelocityDirection.x != 0f)
            _sprite.flipX = _actor.VelocityDirection.x < 0f;

        _animator.SetFloat(XSpeed, Mathf.Abs(_actor.VelocityDirection.x));
        _animator.SetFloat(YSpeed, _actor.VelocityDirection.y);
        _animator.SetBool(Grounded, _actor.Grounded);
        _animator.SetBool(Dead, false /* TODO: _actor.IsDead */);
    }

    public void HandleInput (ActorAnimationInput inputType) {
        switch (inputType) {
            case ActorAnimationInput.Attack:
                _animator.SetTrigger(Attack);
                break;
            case ActorAnimationInput.Die:
                _animator.SetTrigger(Die);
                break;
            case ActorAnimationInput.Hit:
                _animator.SetTrigger(Hit);
                break;
            case ActorAnimationInput.Interact:
                _animator.SetTrigger(Leave);
                break;
            case ActorAnimationInput.Jump:
                _animator.SetTrigger(Jump);
                break;
            case ActorAnimationInput.Restart:
                _animator.SetTrigger(Restart);
                break;
            case ActorAnimationInput.None:
                break;
        }
        
    }
}
