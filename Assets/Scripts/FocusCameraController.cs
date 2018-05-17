using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCameraController : MonoBehaviour {

	private void Start()
	{
		_viewMargin = transform.position;
	}
	
	private void LateUpdate()
	{
		if (_focusObject == null) return;
		transform.position = _focusObject.transform.position + _viewMargin;			
		transform.LookAt(_focusObject.transform);
	}

	[SerializeField] private GameObject _focusObject;
	private Vector3 _viewMargin;
}
