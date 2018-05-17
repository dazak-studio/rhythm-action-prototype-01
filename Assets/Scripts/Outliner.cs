using UnityEngine;

public class Outliner : MonoBehaviour {

	// Use this for initialization
	private void Start ()
	{
		_renderer = GetComponent<Renderer>();
		_renderer.material.shader = Shader.Find("Outline");
	}

	public void SetOutlineColor(Color newColor)
	{
		var colorArray = new float[4]{newColor.b, newColor.g, newColor.r, newColor.a};
		_renderer.material.SetFloatArray("Outline Color", colorArray);
	}

	public void SetOutlineThick(float newValue)
	{
		_renderer.material.SetFloat("Outline", newValue);
	}

	private Renderer _renderer;
}
