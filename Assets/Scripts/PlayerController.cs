using UnityEngine;

public class PlayerController : MonoBehaviour {

	private void Start ()
	{
		_animator = GetComponent<Animator>();
		_destPoint = transform.TransformDirection(Vector3.forward);
		_runningTime = Mathf.Epsilon;
//		_attackedObject = null;
		_isAction = false;
	}

	private void FixedUpdate()
	{	
		_animator.SetFloat("Speed", _runningTime);
		_animator.speed = StepSpeed * 0.25f;
		_currentBaseState = _animator.GetCurrentAnimatorStateInfo(0);

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
		if (!_currentBaseState.IsName("Base Layer.Attack") || _animator.IsInTransition(0)) return;
		
		_animator.SetBool("Attack", false);

	}

	public void UpdateMoveDestination(RaycastHit hitFire)
	{
		var destination = hitFire.point;
		destination.y = transform.position.y;
		
		if (hitFire.collider.transform.tag.Contains("Enemy") 
		    && Vector3.Distance(transform.position, destination) <= AttackRange)
		{						
			Attack(hitFire.collider.gameObject);
		}
		else
		{
			UpdateMoveDestination(destination);
		}
	}
	
	public void UpdateMoveDestination(Vector3 destination)
	{
		_isAction = false;
		_destPoint = destination;
		destination.y = transform.position.y;
		var dist = _destPoint - transform.position;
		_destAngle = Mathf.Atan2(dist.x, dist.z) * Mathf.Rad2Deg;
		_runningTime = 1.5f;
	}

	private void OnCollisionEnter(Collision other)
	{		
		if (other.transform.tag.Contains("Enemy")
//		    && _attackedObject != other.gameObject
		    && _isAction == false)
		{
			Attack(other.gameObject);
		}
	}

	private void Attack(GameObject target)
	{
		var destination = target.transform.position;
		destination.y = transform.position.y;
		
		transform.LookAt(destination);
		_runningTime = float.Epsilon;
		_animator.SetBool("Attack", true);
//		_attackedObject = target;
		_isAction = true;
	}

	public float StepSpeed;
	public float RotateSpeed;
	public float AttackRange;

	private Animator _animator;
	private AnimatorStateInfo _currentBaseState;
	
	private bool _isAction;
//	private GameObject _attackedObject;
	private Vector3 _destPoint;
	private float _destAngle;
	private float _runningTime;

}