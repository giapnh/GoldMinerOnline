using UnityEngine;
using System.Collections;

public class DmHook : MonoBehaviour {
	public GameObject controller;
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
	Object caught_item;

	//draw line
	public Color c1 = Color.black;
	public Color c2 = Color.red;
	// Use this for initialization
	string[] items_list = new string[3]{ "Gold", "Diamond", "Stone"};
	LineRenderer lineRenderer;

	//check finish
	public int item_count=0;
	public int score = 0;
	public UILabel score_label;

	public GameObject offline_result;
	public int map_id = 0;
	void OnEnable(){
		foreach(string item in items_list){
			GameObject[] items = GameObject.FindGameObjectsWithTag(item);
			item_count += items.Length;	
		}
		state = IDLE;
		score_label.text = "Score: "+ score;
	}
	void Start () {
		transform.localRotation.Set(transform.localRotation.x, transform.localRotation.y, 0, 0);

		//draw line
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.02f,0.02f);
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));

	}
	
	// Update is called once per frame
	void Update () {
		//back button
		if (Input.GetKeyDown(KeyCode.Escape)) {
			//reset map
			GameObject map = GameObject.Find("Map"+map_id+"(Clone)");
			Destroy(map);
			item_count = 0;
			score = 0;
			controller.SendMessage("ShowPanel" , ScreenManager.PN_CAMPAIN_MAP);
			controller.SendMessage("HidePanel" , ScreenManager.PN_CAMPAIN_ONGAME);
		}
		//rotate
		if(state == IDLE){
			//check finish			
			//if(!is_ended) check_finish ();
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
			if(transform.position.y > initialPosition.y){
				if(caught_item) Destroy(caught_item);
				score_label.text = "Score: "+ score;
				returnIDLE();
			}
		}
		//hooking
		if(state == HOOKING){
			if(transform.position.y <= -0.9 || transform.position.x < -1.6 || transform.position.x > 1.6) GoBack();
			if(transform.position.y > initialPosition.y) returnIDLE();
		}
		//draw line
		if (state == HOOKING || state == CATCHING) {					
			lineRenderer = GetComponent<LineRenderer>();
			lineRenderer.SetVertexCount(2);
			lineRenderer.SetPosition(0, initialPosition);
			Vector3 pos = transform.position;
			lineRenderer.SetPosition(1, pos);
		}
	}
	
	void OnTriggerEnter(Collider col) {
		//catch gold
		if(state == HOOKING){
			//catch item
			if(System.Array.IndexOf(items_list,col.gameObject.tag)!=-1){
				renderer.material.mainTexture = textures[1];
				col.transform.position = transform.position + rotateDirection * 1.8f;
				col.rigidbody.velocity = rigidbody.velocity;
				
				Item item_info = col.GetComponent<Item>();
				int item_score = item_info.item_score;
				score +=item_score;
				
				GoBack();
				rigidbody.velocity = new Vector3(rigidbody.velocity.x*item_info.speed_rate, rigidbody.velocity.y*item_info.speed_rate,0);
				col.rigidbody.velocity = rigidbody.velocity;
				
				caught_item = col.gameObject;
				state = CATCHING;
				item_count --;
			}
			
			//catch bomb
			if(col.gameObject.tag == "Bomb") {
				state = CATCHING;
				score -= 100;
				score_label.text = "Score: "+ score;
				GoBack();
				col.GetComponent<Bomb>().state = Bomb.HOOKED;
			}
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
		lineRenderer.SetVertexCount (0);
		check_finish ();
	}
	
	void check_finish(){
		if (item_count == 0) {
			Debug.Log ("end game");
			controller.SendMessage("HidePanel" , ScreenManager.PN_CAMPAIN_ONGAME);
			controller.SendMessage("ShowPanel" , ScreenManager.PN_OFFLINE_GAME_RESULT);
			offline_result.SendMessage("Score", score);
			//reset map
			GameObject map = GameObject.Find("Map"+map_id+"(Clone)");
			Destroy(map);
			item_count = 0;
			score = 0;
		}
	}
}