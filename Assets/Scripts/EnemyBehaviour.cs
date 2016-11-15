using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour {
	private float speed;
	private float baseSpeed = 100.0f;
	private Rigidbody2D rb;
	private float angleOffset = 89.5f; //had to have this offset to have enemies face player properly. I have no idea why!
	public GameObject player;
	public float gameSpeed = 1.0f; //enemy speed scaled by game speed.

	private bool hit = false;
	private bool canFire = false;
	private bool fireTimeout = true;

	//particle effects
//	static List<ParticleSystem> explosions = new List<ParticleSystem>();
	[SerializeField] private ParticleSystem expl;
	private ParticleSystem explosion;

//	static List<ParticleSystem> WarpIns = new List<ParticleSystem>();
	[SerializeField] private ParticleSystem warpIn;
	[SerializeField] private Sprite damagedSprite;
	private Sprite undamagedSprite = null;
	private ParticleSystem warp;

	[SerializeField] private AudioSource soundSource;
	[SerializeField] private AudioClip warpInSound;

	static List<GameObject> bullets = new List<GameObject>();

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		speed = baseSpeed * gameSpeed;
	
	}

	void Awake(){
		Messenger <float>.AddListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
		explosion = Instantiate (expl) as ParticleSystem;
		warp = Instantiate (warpIn) as ParticleSystem;
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	private void OnSpeedChanged(float scale){
		speed = baseSpeed * scale;
	}

	// Update is called once per frame
	void Update () {
		//turn to look at player
	
		player = GameObject.Find ("Player");
		if (player != null) {
			Vector3 direction = player.transform.position - transform.position;
			float angle = Mathf.Atan2 (direction.y, direction.x) ;
			angle = (angle - angleOffset) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward * Time.deltaTime);
			rb.velocity = (Vector2)transform.TransformDirection(Vector3.up) * speed * Time.deltaTime;

		}
			
	}

	//kill enemy on collsion with bullet
	void OnCollisionEnter2D(Collision2D other){
	//	Debug.Log (other.gameObject.name);
		if (other.gameObject.name == "bulletPrefab(Clone)") {

			if (gameObject.name == "Enemy2Prefab(Clone)" && !hit) {
				hit = true;
				SpriteRenderer render = gameObject.GetComponent<SpriteRenderer> ();
				undamagedSprite = render.sprite;
				render.sprite = damagedSprite;
				other.gameObject.SetActive(false);
				return;
			}
		
			if (gameObject.name == "Enemy2Prefab(Clone)" && hit) {
				hit = false;
				SpriteRenderer render = gameObject.GetComponent<SpriteRenderer> ();
				render.sprite = undamagedSprite;
			}

		//	bool instantiateExplosion = true;
			//don't instantiate new explosion if can be replayed
			explosion.transform.position = transform.position;
			explosion.Play();
		
			other.gameObject.SetActive(false);
			this.gameObject.SetActive(false);
			//Broadcast event
			Messenger.Broadcast(GameEvent.ENEMY_KILLED);

		}
	}
	void OnTriggerEnter2D(Collider2D other){

		//Debug.Log (other.gameObject.name);
		if (other.gameObject.name == "Player") {
			other.gameObject.SetActive (false);
		//	Destroy (this.gameObject);
			//Broadcast event
			Messenger.Broadcast(GameEvent.PLAYER_KILLED);

		}
	}

	public void PlayAppearEffect(){
	//	Debug.Log ("warp in");
	//	transform.localScale = Vector3.zero;
		warp.transform.position = transform.position;
		warp.Play ();
		StartCoroutine (ScaleIn());
	

		/*
		//don't instantiate new explosion if can be replayed
		bool instantiateWarpEffect = true;
		foreach (ParticleSystem obj in WarpIns) {
			if (!obj.isPlaying) {
				instantiateWarpEffect = false;
				obj.transform.position = transform.position;
				obj.Play ();
				break;
			}
		}

		if (instantiateWarpEffect) {
			//	Debug.Log ("Making explosion");
			ParticleSystem warp = Instantiate (warpIn) as ParticleSystem;
			warp.transform.position = transform.position;
			warp.Play ();
			WarpIns.Add (warp);
		}
		*/
	}

	//scale in the warped in ship
	private IEnumerator ScaleIn(){
		float time = 0.0f;
		Vector3 start = Vector3.zero;
		Vector3 end = new Vector3(0.1f, 0.1f, 0.1f);

		while (time < 1.0f) {
			time += Time.deltaTime;
			transform.localScale = Vector3.Lerp (start, end, time);
			yield return null;
		}
	}
		
}
