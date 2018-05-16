using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
	private void Start()
	{
		_playerController = _player.GetComponent<PlayerController>();
	}

	private void Update () {

		if (Input.GetMouseButtonDown(0))
		{
			var mousePos = Input.mousePosition;			
			var midPos = new Vector3(Screen.width, Screen.height) * .5f;
			var dist = mousePos - midPos;
			var angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg - 90.0f;
			
			_playerController.Move(angle);
		}
		
	}

	[SerializeField] private GameObject _player;
	private PlayerController _playerController;
}
