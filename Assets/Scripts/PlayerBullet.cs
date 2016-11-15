using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {
	private float speed;
	private float baseSpeed = 350.0f;
	private float lifetime = 1.5f;
	private float timeLived = 0.0f;
	public Vector3 velocity;
	public Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		speed = baseSpeed;
	}

	void Awake(){
		Messenger <float>.AddListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	void OnDestroy(){
		Messenger<float>.RemoveListener (GameEvent.SPEED_CHANGED, OnSpeedChanged);
	}

	private void OnSpeedChanged(float scale){
		speed = baseSpeed * scale;
	}
	// Update is called once per frame
	void Update () {
		//player bullets move quickly, and have a built in life time to limit their range.
		rb.velocity = (Vector2)transform.TransformDirection(Vector3.up) * speed * Time.deltaTime;
	}

	//using late update to clear bullets to rule out collisions that must be resolved this frame.
	void LateUpdate(){
		timeLived += Time.deltaTime;
		if (timeLived >= lifetime) {
			gameObject.SetActive (false);
		}
	}

	public void Fire(Vector3 pos, Quaternion rot){
		timeLived = 0.0f;
		gameObject.SetActive (true);
		transform.position = (pos );
		transform.rotation = rot;
	}



}
