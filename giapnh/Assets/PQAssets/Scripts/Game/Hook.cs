using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {	
	public int state;
	public static int IDLE = 0;
	public static int MOVING = 1;
	public static int ROTATING = 2;
	public static int CATCHING = 3;
	public static int HOOKING = 4;
	
	public Texture[] textures;
	public Vector3 center_point;
	public int flag=1;
	public float hook_speed;
	Vector3 rotateDirection;
	Vector3 initialPosition;
	Object gold;
	
	// Use this for initialization
	void Start () {
		state = IDLE;
		transform.localRotation.Set(transform.localRotation.x, transform.localRotation.y, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		//rotate
		if(state == IDLE){
			center_point = transform.parent.transform.position + new Vector3(0,-0.15f,0);
			//center_point = Vector3(0,0.1,0);
			if(transform.localRotation.z >= 0.5) flag=1;
			else if(transform.localRotation.z <= -0.5) flag=-1;
			transform.RotateAround (center_point, Vector3.forward, 60 * Time.deltaTime * flag);
			
			//click
			if(Input.GetMouseButtonDown(0) || ( Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began)){
				var mouse_pos = Input.mousePosition;
				if(mouse_pos.y<400 && transform.parent.GetComponent<Character>().state== Character.IDLE){
					state = HOOKING;
					initialPosition = transform.position;
					rotateDirection = transform.position - center_point;
					rigidbody.velocity = rotateDirection*hook_speed;
				}	
			}
		}//
		//catching
		if(state == CATCHING){
			//Debug.Log(gold.transform.position);
			if(transform.position.y > initialPosition.y){
				Destroy(gold);
				returnIDLE();
			}
		}
		//hooking
		if(state == HOOKING){
			if(transform.position.y <= -0.9 || transform.position.x < -1.6 || transform.position.x > 1.6) GoBack();
			if(transform.position.y > initialPosition.y) returnIDLE();
		}
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Gold") {
			renderer.material.mainTexture = textures[1];
			GoBack();
			col.rigidbody.velocity = rigidbody.velocity;
			gold = col.gameObject;
			state = CATCHING;
		}
	}

	void GoBack(){
		rigidbody.velocity =  -rotateDirection * hook_speed;
	}
	void returnIDLE(){
		renderer.material.mainTexture = textures[0];
		rigidbody.velocity = new Vector3(0,0,0);
		transform.position = initialPosition;
		state = IDLE;
	}
}
