using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.Animations;
using UnityEngine;
using UnityScript.Steps;

public class PlayerController : MonoBehaviour {

	private void Start ()
	{
		_animator = GetComponent<Animator>();
		_rigidbody = GetComponent<Rigidbody>();
		_destPoint = transform.TransformDirection(Vector3.forward);
		_runningTime = Mathf.Epsilon;
	}

	/*
	private void FixedUpdate()
	{		
		var horizontal = Input.GetAxis("Horizontal");
		var vertical = Input.GetAxis("Vertical");

		_animator.SetFloat("Speed", vertical);
		_animator.SetFloat("Direction", horizontal);
		_animator.speed = AnimationSpeed;
		_currentBaseState = _animator.GetCurrentAnimatorStateInfo(0);

		var velocity = Vector3.forward * vertical;
		velocity = transform.TransformDirection(velocity);

		if (vertical > 0.1f)
			velocity *= ForwardSpeed;
		else if (vertical < -0.1f)
			velocity *= BackwardSpeed;

		var moveRate = 1.0f;

		if (_currentBaseState.IsName("Base Layer.Attack"))
			moveRate = 0.1f;
			
		transform.localPosition += velocity * Time.fixedDeltaTime * moveRate;
		transform.Rotate(Vector3.up * horizontal * RotateSpeed * moveRate);
	}
	*/

	private void FixedUpdate()
	{	
		// Character position update
		_animator.SetFloat("Speed", _runningTime);
		_animator.speed = StepSpeed * 0.35f;

		if (_runningTime > Mathf.Epsilon)
		{
			var velocity = transform.TransformDirection(Vector3.forward);
			transform.localPosition += velocity * Time.fixedDeltaTime * StepSpeed * _animator.GetFloat("Speed");
			_runningTime -= Time.fixedDeltaTime;
			var angle = Mathf.LerpAngle(transform.eulerAngles.y, _destAngle, Time.fixedDeltaTime * RotateSpeed);
			transform.eulerAngles = new Vector3(0, angle, 0);
			
			
			// Character rotation
			/*
			var turnAngle = Vector3.Angle(transform.position, _destination);
			print(turnAngle);
			var rot = Mathf.LerpAngle(transform.eulerAngles.y, turnAngle, Time.deltaTime * 180.0f);
			transform.eulerAngles = Vector3.up * rot;
			*/

//			transform.LookAt(_destination);
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

	/*
	public float AnimationSpeed = 1.5f;
	public float RotateSpeed = 2.0f;
	public float ForwardSpeed = 7.0f;
	public float BackwardSpeed = 2.0f;
	*/

	public float StepSpeed = 1.0f;
	public float RotateSpeed = 2.0f;

	private Animator _animator;
	private Rigidbody _rigidbody;
	private AnimatorStateInfo _currentBaseState;

	private Vector3 _destPoint;
	private float _destAngle;
	private float _runningTime;

}