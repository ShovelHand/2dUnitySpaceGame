using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	private float timeTilSpawn = 5.0f;
	private float timeSinceLastSpawn = 0.0f;
	private float speed;
	private int blitz = 0; //only have a few ships spawn really quickly.

	[SerializeField] private GameObject EnemyType2;
	[SerializeField] private GameObject EnemyType1;
	[SerializeField] private Slider speedSlider;
	[SerializeField] private AudioSource startSound;


	private GameObject Enemy;
	private List<GameObject> Enemies = new List<GameObject>();
	private List<GameObject> Type2Enemies = new List<GameObject>();

	public GameObject player;


	void Awake(){
		Messenger <float>.AddListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	private void OnSpeedChanged(float scale){
//		speed *= scale;
	}
	// Use this for initialization
	void Start () {
		if (speedSlider != null) {
			speed = speedSlider.value;
			Debug.Log (speed);
		}

		player = GameObject.Find ("Player");
		SpawnBadGuy ();
		startSound.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		timeSinceLastSpawn += Time.deltaTime;
		if (timeSinceLastSpawn >= timeTilSpawn) {
			timeSinceLastSpawn = 0.0f;
			timeTilSpawn -= 0.2f;
			if (timeTilSpawn <= 0.4f) {
				timeTilSpawn = 0.4f;
				blitz += 1;
				if (blitz >= 5) {
					blitz = 0;
					timeTilSpawn = 2.0f;
				}
			}
			SpawnBadGuy ();
		}

	}

	public void OnRestart(){
	//	Application.LoadLevel("gamescene");
		SceneManager.LoadScene ("gamescene");
	}

	private void SpawnBadGuy(){
		if (!player.activeSelf)
			return;
		timeSinceLastSpawn = 0.0f;
	//	Debug.Log ("spawn bad guy");
		Vector3 pos = Vector3.zero;
		pos.x = Random.Range(-10, 10);
		pos.y = Random.Range(-5,5);

		//check if too close to player
		while(Vector3.Distance (player.transform.position, pos) < 4.0f){
			pos.x = Random.Range(-10,10);
			pos.y = Random.Range(-5, 5);
		}

		bool mustInstantiate = true;


		//go through list to see in there are any already instantiated enemy ships
		foreach (GameObject i in Enemies) {
			if (!i.activeSelf) {
				i.SetActive (true);
				i.transform.position = pos;
				mustInstantiate = false;
				i.GetComponent<EnemyBehaviour> ().PlayAppearEffect ();
	//			Debug.Log ("Reused old enemy");
				break;
			}
		}

		if(mustInstantiate){
//			Debug.Log ("Had to make new enemy");
			float random = Random.Range(0,11);
			if(random % 2 == 0){
				Enemy = Instantiate(EnemyType1, pos, Quaternion.identity) as GameObject;
			}else{
				Enemy = Instantiate(EnemyType2, pos, Quaternion.identity) as GameObject;
			}
			Enemy.GetComponent<EnemyBehaviour> ().PlayAppearEffect ();
	//		Enemy.GetComponent<EnemyBehaviour> ().gameSpeed = speed;
			Enemies.Add (Enemy);
		}
	}

	public void OnQuit(){
		Debug.Log ("Game exited by player");
		Application.Quit ();
	}
}
