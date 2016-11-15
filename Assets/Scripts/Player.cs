using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	[SerializeField] private Transform target;
	[SerializeField] private ParticleSystem thrusters;

	[SerializeField] private GameObject bulletPrefab;
	private GameObject _bullet;
	private List<GameObject> bullets = new List<GameObject>();


	[SerializeField] private AudioSource soundSource;
	[SerializeField] private AudioSource thrusterSoundSource;
	[SerializeField] private AudioClip gunSound;
	[SerializeField] private AudioClip thrusterLoop;

	public Rigidbody2D rb;

	private float baseSpeed = 100.0f;
	public float maxMoveSpeed;
	public float moveSpeed;
	public float rotSpeed = 300.0f;
	public Vector3 velocity = Vector3.zero;
	private bool fireTimeout = false;


	Vector3 movement = Vector3.zero;

	// Use this for initialization
	void Start () {
		Debug.Log ("initializing player");
		moveSpeed = 0.0f;
		maxMoveSpeed = baseSpeed;
		rb = GetComponent<Rigidbody2D>();
		soundSource = GetComponent<AudioSource> ();
	}

	void Awake(){
		Messenger <float>.AddListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	// Update is called once per frame
	void Update () {
		//wasd input stuff
		// start with zero and add movement components progressively


		//handle rotation
		float rotInput = Input.GetAxis("Horizontal");
		movement.z = rotInput * rotSpeed * Time.deltaTime;
		if (rotInput != 0) {
			//	movement *= Time.deltaTime;
			transform.Rotate (-movement);
			velocity.x = rotInput;
		}


		//handle translation in x-y plane in direction of rotation
		float moveInput = Input.GetAxis("Vertical");
		if (moveInput != 0) {
			moveSpeed += moveInput* 3;
			velocity.y = moveInput;
			if(!thrusterSoundSource.isPlaying)thrusterSoundSource.Play ();	//default sound is the thruster loop
			if (moveSpeed > maxMoveSpeed)
				moveSpeed = maxMoveSpeed;
			
		} else {
			moveSpeed -= 0.3f;
			thrusterSoundSource.Stop ();
			if (moveSpeed < 0)
				moveSpeed = 0;	
		}

	//	Debug.Log (velocity);
	//	transform.Translate(velocity * moveSpeed * Time.deltaTime);
		rb.velocity = (Vector2)transform.TransformDirection(Vector3.up) * moveSpeed * Time.deltaTime;
		// TODO:: loop arena area
	

		//updates for thruster particles
		if (moveSpeed == 0) {
			thrusters.Stop ();
		} else if(!thrusters.isPlaying) {
			thrusters.Play ();
		}

		//Shootin' stuff!
		if (Input.GetMouseButtonDown (0) && !fireTimeout) {
		//	Debug.Log ("shooting!");
			//instantiate the bullet
			bool instantiateBullet = true;
			fireTimeout = true;
			StartCoroutine (FireTimer ());

			foreach (GameObject bullet in bullets) {
				if (bullet.activeSelf == false) {
	//				Debug.Log ("found Bullet");
					instantiateBullet = false;
					bullet.SetActive (true);
			//		bullet.transform.position = transform.TransformPoint (velocity.normalized * 3);
			//		bullet.transform.rotation = transform.rotation;
					bullet.GetComponent<PlayerBullet> ().Fire 
					(transform.TransformPoint (velocity.normalized), transform.rotation);
					break;
				}
			}
			if (instantiateBullet) {
	//			Debug.Log ("instantiated bullet");
				_bullet = Instantiate (bulletPrefab) as GameObject;
				_bullet.GetComponent<PlayerBullet> ().Fire 
				(transform.TransformPoint (velocity.normalized ), transform.rotation);
				bullets.Add (_bullet);
			}
			soundSource.PlayOneShot (gunSound);
				 
		}
		//constrain player to camera area by looping like the classic "asteroids" game
		if (transform.position.y > 5) {
			Vector3 pos = transform.position;
			pos.y = -5.0f;
			transform.position = pos;
		}
		if (transform.position.y < -5) {
			Vector3 pos = transform.position;
			pos.y = 5.0f;
			transform.position = pos;
		}
		if (transform.position.x > 10) {
			Vector3 pos = transform.position;
			pos.x = -10.0f;
			transform.position = pos;
		}
		if (transform.position.x < -10) {
			Vector3 pos = transform.position;
			pos.x = 10.0f;
			transform.position = pos;
		}
	}

	//collision with enemy bullet
	void OnCollisionEnter2D(Collision2D other){
		Debug.Log (other.gameObject.name);
		if (other.gameObject.name == "enemyBulletPrefab(Clone)") {
			other.gameObject.SetActive (false);
			this.gameObject.SetActive (false);
			Messenger.Broadcast(GameEvent.PLAYER_KILLED);
		}
	}

	private IEnumerator FireTimer(){
		float time = 0.0f;

		while (time < 0.35f) {
			time += Time.deltaTime;
			yield return null;
		}
		fireTimeout = false;
	}

	private void OnSpeedChanged(float scale){
		maxMoveSpeed = baseSpeed * scale;
	}
		
}
