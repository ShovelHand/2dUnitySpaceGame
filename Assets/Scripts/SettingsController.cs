using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {
	[SerializeField] private Slider speedSlider;
	private float speed = 1.0f;
	// Use this for initialization
	void Start () {
		//remember player speed preference across game instances
		speedSlider.value = PlayerPrefs.GetFloat("speed", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeGameSpeed(){
		float speedScale = speedSlider.value;
		Messenger<float>.Broadcast (GameEvent.SPEED_CHANGED, speedScale);
	//	Debug.Log ("slider changed");
	}
}
