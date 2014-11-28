using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour {

	public Transform groundPoint;
	public float hurtSpeed = -2f;
	private bool touchingPlatform;
	public GameObject bullet;
	public GameObject bigBullet;
	public GameObject biggestBullet;
	SpriteRenderer spriteRenderer;
	public HealthMeter healthMeter;
	public TextMesh speedText;
	public TextMesh distanceText;

	public static float distanceTraveled;
	public static int boosts;
	public float speed;
	public float gameOverY;
	public float jumpPower;
	public float movementPower;
	public float bulletOffSet = 1f;
	public float bulletSpeed = 25f;
	public float superSpeed;
	public float nextShot = 0;
	public float chargeTime = 0;
	public float chargeFlash;
	public float hurtTime;
	public float invincibleTime;
	public int health;
	public AudioClip death;
	public AudioClip[] hurt;
	public AudioClip fall;
	public GameObject deathParticle;
	public Color color;

	BoxCollider2D boxCollider;
	Animator animator;

	bool facingRight = true;
	bool isGrounded = false;
	bool allowRoll = true;
	float mass;
	float move;
	float rollCooldown = 0f;
	float scale;


	private Vector3 startPosition;
	private ParticleSystem particle;


	void Awake(){
		particle = GetComponent<ParticleSystem>();
		animator = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D> ();
		spriteRenderer = GetComponent<SpriteRenderer>();
		GameObject healthMeterObj = GameObject.FindGameObjectWithTag("HealthMeter");
		if(healthMeterObj != null){
			healthMeter = healthMeterObj.GetComponent<HealthMeter>();
		}
	}

	void Start () {
		movementPower = 500f;
		jumpPower = 500f;
		GameEventManager.GameStart += GameStart;
		GameEventManager.GameOver += GameOver;
		startPosition = transform.localPosition;
		renderer.enabled = false;
		rigidbody2D.isKinematic = true;
		enabled = false;
		health = 3;
		mass = 1;
		superSpeed = 35f;
		particle.enableEmission = false;
	}

	void FixedUpdate()
	{
		rigidbody2D.mass = mass;
		isGrounded = Physics2D.OverlapPoint (groundPoint.position);
		animator.SetBool ("IsGrounded", isGrounded == true);
		move = Input.GetAxis ("Horizontal");

		animator.SetFloat ("Speed", Mathf.Abs (move));

		//Uncomment/Comment in the following line to toggle control in the air
		if(Input.GetAxis ("Horizontal") != 0 /*&& touchingPlatform == true*/) {
			//might need to modify this
			rigidbody2D.AddForce(new Vector2(move, 0) * movementPower * Time.deltaTime);	 

			//animation["speedy_g_run"].speed = 2.0f;
		}

		if((move > 0 && !facingRight) || (move < 0 && facingRight)){
			Flip();
		}
	}
	
	void Update()
	{

		updateRoll ();
		updateSpeed ();
		updateCharge ();
		updateDistance();
		updateInvincibility();
		hurtTime = Mathf.Max (0, hurtTime - Time.deltaTime);
		rollCooldown = Mathf.Max (0, rollCooldown - Time.deltaTime);

		if(hurtTime > 0){
			rigidbody2D.velocity = new Vector2(Orient(hurtSpeed), 0);
			return;
		}
		if(Input.GetKeyDown(KeyCode.LeftControl) && (Time.time >= nextShot)){
			shoot ();
			nextShot = Time.time + 0.1f;
			chargeTime = Time.time;
		}

		//SUPER CHARGED SHOT!!!
		if(Input.GetKeyUp (KeyCode.LeftControl)&& (Time.time >= nextShot)){
		   if ((Time.time - chargeTime) > 2.0f){
				shootBiggest();
				chargeTime = Time.time;
				nextShot = Time.time + 0.1f;
			//Charged shot
			}else if((Time.time - chargeTime) > 0.5f){
				shootBig();
				nextShot = Time.time + 0.1f;
			}else if ((Time.time >= nextShot)){
				shoot ();
				nextShot = Time.time + 0.1f;
			}
		}

		//Horizontal Movement only when grounded


		//Jump only when Grounded
		if (Input.GetKeyDown (KeyCode.Space) && (touchingPlatform == true) && isGrounded) {
			animator.SetBool("IsGrounded", false);
			rigidbody2D.AddForce(new Vector2(0,jumpPower));
			touchingPlatform = false;
		} 
		//Allow for small jumps if spacebar is released early
		else if(Input.GetKeyUp (KeyCode.Space) && rigidbody2D.velocity.y > 0){
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
		}


		distanceTraveled = transform.localPosition.x;

		if(transform.localPosition.y < gameOverY){
			audio.PlayOneShot(fall, 1.0f);
			GameEventManager.TriggerGameOver();
		}
	
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 nextScale = transform.localScale;
		nextScale.x *= -1f; 
		transform.localScale = nextScale;	
	}

	private void GameStart () {
		boosts = 0;
		distanceTraveled = 0f;
		transform.localPosition = startPosition;
		renderer.enabled = true;
		rigidbody2D.isKinematic = false;
		enabled = true;
	}
	
	private void GameOver () {
		renderer.enabled = false;
		rigidbody2D.isKinematic = true;
		enabled = false;
	}

	void OnTriggerEnter2D(Collider2D col){
		if((col.gameObject.tag == "Enemy")){
			if(speed > superSpeed){
				col.SendMessage ("applyDamage", 5);
			}
		}
	}


	void OnCollisionEnter2D (Collision2D col) {
		if(col.gameObject.tag == "Terrain"){
			touchingPlatform = true;
		}
		if(col.gameObject.tag == "Boost"){
			processBoost();
		}
	} 
	
	void OnCollisionExit2D (Collision2D col) {
		if(col.gameObject.tag == "Terrain"){
			touchingPlatform = false;
		}
	}


	public static void AddBoost () {
		boosts += 1;
	}
	void shoot(){

		if(facingRight){
			GameObject instance = Instantiate(bullet, new Vector2(transform.position.x+bulletOffSet, transform.position.y +0.3f), Quaternion.identity) as GameObject;
			instance.rigidbody2D.velocity = new Vector2( (1 * bulletSpeed)+ (transform.rigidbody2D.velocity.x), 0);
		}else{
			GameObject instance = Instantiate(bullet, new Vector2(transform.position.x-bulletOffSet, transform.position.y +0.3f), Quaternion.identity) as GameObject;
			instance.rigidbody2D.velocity = new Vector2( (-1 * bulletSpeed)+ (transform.rigidbody2D.velocity.x), 0);
			Vector3 nextScale = instance.transform.localScale;
			nextScale.x *= -1f; 
			instance.transform.localScale = nextScale;
		}
	}
	void shootBig(){
		if(facingRight){
			GameObject instance = Instantiate (bigBullet, new Vector2(transform.position.x+bulletOffSet, transform.position.y +0.3f), Quaternion.identity) as GameObject;
			instance.rigidbody2D.velocity = new Vector2( (1 * bulletSpeed)+ (transform.rigidbody2D.velocity.x), 0);
		}else{
			GameObject instance = Instantiate(bigBullet, new Vector2(transform.position.x-bulletOffSet, transform.position.y +0.3f), Quaternion.identity) as GameObject;
			instance.rigidbody2D.velocity = new Vector2( (-1 * bulletSpeed)+ (transform.rigidbody2D.velocity.x), 0);
			Vector3 nextScale = instance.transform.localScale;
			nextScale.x *= -1f; 
			instance.transform.localScale = nextScale;
		}
	}
	void shootBiggest(){
		if(facingRight){
			GameObject instance = Instantiate (biggestBullet, new Vector2(transform.position.x+(bulletOffSet*2), transform.position.y +0.3f), Quaternion.identity) as GameObject;
			instance.rigidbody2D.velocity = new Vector2( (1 * bulletSpeed)+ (transform.rigidbody2D.velocity.x), 0);
		}else{
			GameObject instance = Instantiate(biggestBullet, new Vector2(transform.position.x-(bulletOffSet*2), transform.position.y +0.3f), Quaternion.identity) as GameObject;
			instance.rigidbody2D.velocity = new Vector2( (-1 * bulletSpeed)+ (transform.rigidbody2D.velocity.x), 0);
			Vector3 nextScale = instance.transform.localScale;
			nextScale.x *= -1f; 
			instance.transform.localScale = nextScale;
		}
	}
	void applyDamage(){
		if(speed > superSpeed){
			return;
		}
		if(invincibleTime > 0){
			return;
		}
		hurtTime = 0.5f;
		invincibleTime = 2f;
		health--;
		healthMeter.removeHeart();
		if(health < 1){
			GameEventManager.TriggerGameOver();
		}
	}

	void updateInvincibility(){
		invincibleTime = Mathf.Max (0, invincibleTime - Time.deltaTime);
		Color nextColor = spriteRenderer.color;
		if(invincibleTime > 0){
			bool isFlashStrong = ((int)(invincibleTime * 15) % 2) == 0;
			nextColor.a = isFlashStrong ? 0.75f : 0.25f;
		}
		else{
			nextColor.a = 1f;
		}
		spriteRenderer.color = nextColor;
	}

	void updateSpeed(){
		//Update Odometer Text
		speed = rigidbody2D.velocity.x;
		speedText.text = "Speed: " + ((int)Mathf.Abs (speed)).ToString ();
		
		//Check if faster than superSpeed if so emit Particles
		if(rigidbody2D.velocity.x > superSpeed){
			color = Color.red;
			particle.startColor = color;
			particle.enableEmission = true;
		}
		else {
			particle.enableEmission = false;
		}
		
	}
	
	void updateDistance(){
		//Update Distance text
		distanceText.text = "Distance: " + ((int)Mathf.Floor (distanceTraveled)).ToString ();
	}
	
	float Orient(float value){
		return facingRight ? value : -value;
	}

	void updateRoll(){
		if(Input.GetKeyDown (KeyCode.LeftShift) && allowRoll){
			allowRoll = false;
			rollCooldown = 3.0f;
			animator.SetBool("InitiateRoll", true);
		}
		if(Input.GetKeyUp (KeyCode.LeftShift)){
			animator.SetBool("InitiateRoll", false);
		}
		//Prevents players from chaining rolls.
		if(rollCooldown <= 1.5f){
			animator.SetBool("InitiateRoll", false);
		}
		if(rollCooldown <= 0.001){
			allowRoll = true;
		}
	}
	void updateCharge(){
		if(Input.GetKey (KeyCode.LeftControl)){
			if(Time.time - chargeTime > 0.5f){
				particle.startColor = Color.green;
				particle.enableEmission = true;
			}
			if(Time.time - chargeTime > 2.0f){
				particle.startColor = Color.blue;
				particle.enableEmission = true;
			}
			if (Time.time - chargeTime < 0.5){
				particle.enableEmission = false;
			}
		}
	}
	void processBoost(){
		if( mass < 1.3){
			mass += 0.1f;
			scale += 0.1f;
			health++;
			healthMeter.addHeart();
			transform.localScale = new Vector3(scale, scale,scale);
		}
	}
}