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
		_destination = transform.position;		
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
		var targetRot = Quaternion.LookRotation(_destination);
		
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime);
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
		_destination = destination;
//		transform.LookAt(destination);
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

	public float RotateSpeed = 0.1f; // The rate of the rotation of charcater (.0f < x <= 1.0f);
	
	private Animator _animator;
	private Rigidbody _rigidbody;
	private AnimatorStateInfo _currentBaseState;

	private Vector3 _destination;
	private readonly float _appendDistance = 1.0f; 

}