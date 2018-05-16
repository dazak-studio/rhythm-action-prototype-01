using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private void Start ()
	{
		_animator = GetComponent<Animator>();
	}

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
			moveRate = 0.2f;
			
		transform.localPosition += velocity * Time.fixedDeltaTime * moveRate;
		transform.Rotate(Vector3.up * horizontal * RotateSpeed);
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

	public float AnimationSpeed = 1.5f;
	public float RotateSpeed = 2.0f;
	public float ForwardSpeed = 7.0f;
	public float BackwardSpeed = 2.0f;	
	
	private Animator _animator;
	private AnimatorStateInfo _currentBaseState;

	private static int _idleState = Animator.StringToHash("Base Layer.Idle");
	private static int _walkState = Animator.StringToHash("Base Layer.Walk");
	private static int _restState = Animator.StringToHash("Base Layer.Rest");
	private static readonly int AttackState = Animator.StringToHash("Base Layer.Attack");

}
