using UnityEngine;

public class Outliner : MonoBehaviour {

	// Use this for initialization
	private void Start ()
	{
		_playerController = _playerController ? _playerController : 
			GameObject.Find("Player").GetComponent<PlayerController>();
		_renderer = GetComponent<Renderer>();	
		_renderer.material.shader = Shader.Find("Custom/Outline");
	}

	private void LateUpdate()
	{
		if (FocusObject != gameObject)
		{
			SetOutlineColor(Color.green);
			SetOutlineThick(.0f);
		}
		else
		{
			if (_playerController.IsInAttackRange(transform.position))
			{
				SetOutlineColor(Color.yellow);
			}			
			SetOutlineThick(0.1f);
		}
	}

	public void SetOutlineColor(Color newColor)
	{
		_renderer.material.SetColor("_outlineColor", newColor);
	}

	public void SetOutlineThick(float newValue)
	{
		_renderer.material.SetFloat("_outline", newValue);
	}

	private static PlayerController _playerController;
	public static GameObject FocusObject = null; 
	private Renderer _renderer;
}
