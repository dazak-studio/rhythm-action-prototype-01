using UnityEngine;

public class EventManager : MonoBehaviour {
	private void Start()
	{
		_playerController = _player.GetComponent<PlayerController>();
	}

	private void Update () {
		MouseInput();		
	}

	private void MouseInput()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			RaycastHit hitFire;
			var rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(rayFire, out hitFire))
			{				
				_playerController.UpdateMoveDestination(hitFire);
			}
			else
			{
				_playerController.UpdateMoveDestination(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
		}
	}

	[SerializeField] private GameObject _player;
	private PlayerController _playerController;
}
