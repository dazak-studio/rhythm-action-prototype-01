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
		RaycastHit hitFire;
		var rayFire = Camera.main.ScreenPointToRay(Input.mousePosition);
		var isFoundHitObject = Physics.Raycast(rayFire, out hitFire);
		
		if (isFoundHitObject && hitFire.collider.gameObject.tag.Contains("Enemy"))
		{
			Outliner.FocusObject = hitFire.collider.gameObject;
		}
		else
		{
			Outliner.FocusObject = null;
		}
		
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{			
			if (isFoundHitObject)
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
