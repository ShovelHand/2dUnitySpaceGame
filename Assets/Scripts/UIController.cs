using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	[SerializeField] private Text scoreLabel;
	[SerializeField] private PopupController endPopup;
	[SerializeField] private PopupController settingsPopup;

	[SerializeField] private AudioSource explosionSoundSource;
	[SerializeField] private AudioClip explodeSound;
//	[SerializeField] Camera camera;

	private int _badGuysKilled;

	//event listening stuff
	void Awake() {
		Messenger.AddListener (GameEvent.ENEMY_KILLED, OnEnemyKilled);
		Messenger.AddListener (GameEvent.PLAYER_KILLED, OnPlayerKilled);
//		Messenger.AddListener<float> (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger.RemoveListener (GameEvent.ENEMY_KILLED, OnEnemyKilled);
		Messenger.RemoveListener (GameEvent.PLAYER_KILLED, OnPlayerKilled);
//		Messenger.RemoveListener<float> (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}
	//Use this for initialization
	void Start () {
		_badGuysKilled = 0;
		scoreLabel.text = "Kill Count: ";
		scoreLabel.text += _badGuysKilled.ToString();
		endPopup.Close ();
		settingsPopup.Close ();
	
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnEnemyKilled(){
		_badGuysKilled += 1;
		scoreLabel.text = "Kill Count: ";
		scoreLabel.text += _badGuysKilled.ToString();
		explosionSoundSource.PlayOneShot (explodeSound);
	}

	private void OnPlayerKilled(){
		endPopup.Open ();
		Debug.Log ("Player Killed!");
	}

	//scale player, bullet and enemy ship speeds according to speed slider in settigns 
	private void OnSpeedChanged(float scale){
		Debug.Log ("Game speed changed");
	}
}
