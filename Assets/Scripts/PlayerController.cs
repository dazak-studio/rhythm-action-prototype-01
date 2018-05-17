using UnityEngine;

public class PlayerController : MonoBehaviour {

	private void Start ()
	{
		_animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody>();
		_destPoint = transform.TransformDirection(Vector3.forward);
		_runningTime = Mathf.Epsilon;
	}

	private void FixedUpdate()
	{	
		_animator.SetFloat("Speed", _runningTime);
		_animator.speed = StepSpeed * 0.25f;

		if (_runningTime > Mathf.Epsilon)
		{
			var velocity = transform.TransformDirection(Vector3.forward);
			transform.localPosition += velocity * Time.fixedDeltaTime * StepSpeed * _animator.GetFloat("Speed");
			_runningTime -= Time.fixedDeltaTime;
			var angle = Mathf.LerpAngle(transform.eulerAngles.y, _destAngle, Time.fixedDeltaTime * RotateSpeed);
			transform.eulerAngles = new Vector3(0, angle, 0);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_animator.SetBool("Attack", true);
		}

		if (!_currentBaseState.IsName("Base Layer.Attack") || _animator.IsInTransition(0)) return;
		
		_animator.SetBool("Attack", false);

	}

	public void UpdateMoveDestination(Vector3 destination)
	{
		destination.y = transform.position.y;
		_destPoint = destination;
		var dist = _destPoint - transform.position;
		_destAngle = Mathf.Atan2(dist.x, dist.z) * Mathf.Rad2Deg;
		_runningTime = 1.5f;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.transform.tag.Contains("Enemy"))
			_animator.SetBool("Attack", true);		
	}

	public float StepSpeed = 1.0f;
	public float RotateSpeed = 2.0f;

	private Animator _animator;
	private Rigidbody _rigidbody;
	private AnimatorStateInfo _currentBaseState;

	private Vector3 _destPoint;
	private float _destAngle;
	private float _runningTime;

}