using UnityEngine;

public class RhythmBar : MonoBehaviour {

	public enum STYLE
	{
		START_BAR,
		NORMAL_BAR,
		LAST_BAR
	};

	public STYLE style = STYLE.NORMAL_BAR;	


	void Awake()
	{
		start_position = GetComponent<Transform>().position;

		// in case of sprite
		max_alpha = object_core.GetComponent<SpriteRenderer>().color.a;

		// in case of effect
	}
	void Start() 
	{
		if(next_rhythm_bar != null)
		{
			goal_position = next_rhythm_bar.GetComponent<RhythmBar>().start_position;

			if(style == STYLE.LAST_BAR)
				margine_vector = start_position - goal_position;
			else
				margine_vector = goal_position - start_position;
			
		}
	}
	
	void FixedUpdate() 
	{
		//	
		float synctime = 1.0f - RhythmManager.GetInstance._syncRate;
		float want_pos_x = synctime * margine_vector.x;
		
		Vector3	cur_pos = GetComponent<Transform>().position;
		cur_pos.x = start_position.x + want_pos_x;
		GetComponent<Transform>().position = cur_pos;

		if(style == STYLE.START_BAR)
		{
			// in case of sprite
			Color color = object_core.GetComponent<SpriteRenderer>().color;
			color.a = synctime * max_alpha;
			object_core.GetComponent<SpriteRenderer>().color = color;


			// in case of effect
		}
		else if(style == STYLE.LAST_BAR)
		{
			// in case of sprite
			Color color = object_core.GetComponent<SpriteRenderer>().color;
			color.a = (1.0f - synctime) * max_alpha * 0.3f;
			object_core.GetComponent<SpriteRenderer>().color = color;


			// in case of effect
		}
	}

	public void ShowBar(bool isShow)
	{
		object_core.GetComponent<SpriteRenderer>().enabled = isShow;//.SetActive(isShow);

		/* 
		Color color = GetComponent<SpriteRenderer>().color;

		if(isShow == true)
			color.a = max_alpha;
		else
			color.a = 0.0f;

		GetComponent<SpriteRenderer>().color = color;	
		*/	
	}

	public GameObject	object_core;

	public Vector3		start_position;
	public Vector3		goal_position = Vector3.zero;
	
	public Vector3		 margine_vector;
	public GameObject	 next_rhythm_bar = null;

	private	float max_alpha;

}
