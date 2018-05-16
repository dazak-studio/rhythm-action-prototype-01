using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
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

		transform.localPosition += velocity * Time.fixedDeltaTime;
		transform.Rotate(Vector3.up * horizontal * RotateSpeed);
	}

	public float AnimationSpeed = 1.5f;
	public float RotateSpeed = 2.0f;
	public float ForwardSpeed = 7.0f;
	public float BackwardSpeed = 2.0f;
	
	private Animator _animator;
	private AnimatorStateInfo _currentBaseState;
}
