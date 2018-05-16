using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCameraController : MonoBehaviour {
	
	private void Start()
	{
		if (_focusObject != null)
		{
			transform.position += _focusObject.transform.position;
			transform.parent = _focusObject.transform;
			transform.LookAt(_focusObject.transform);
		}
	}

	[SerializeField] private GameObject _focusObject;
}
