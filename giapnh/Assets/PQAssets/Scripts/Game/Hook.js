#pragma strict
var textures : Texture[];
var center_point: Vector3;
var flag:int = 1;
var isRotating:boolean = true;
var isCatching:boolean = false;
var isHooking:boolean = false;
var state:String = "IDLE"; //HOOKING, CATCHING, 
var rotateDirection: Vector3;
var initialPosition: Vector3;
var speed:float = 10;
var gold:Object;
function Start () {
	transform.localRotation.z = 0;
}

function Update () {
	//rotate
	if(state == "IDLE"){
		center_point = transform.parent.transform.position + Vector3(0,-0.15,0);
		//center_point = Vector3(0,0.1,0);
		if(transform.localRotation.z >= 0.5) flag=1;
		else if(transform.localRotation.z <= -0.5) flag=-1;
		transform.RotateAround (center_point, Vector3.forward, 60 * Time.deltaTime * flag);
		
		//click
		if(Input.GetMouseButtonDown(0) || ( Input.touchCount >0 && Input.GetTouch(0).phase == TouchPhase.Began)){
			var mouse_pos = Input.mousePosition;
			if(mouse_pos.y <480){
				state = "HOOKING";
				initialPosition = transform.position;
				rotateDirection = transform.position - center_point;
				rigidbody.velocity = rotateDirection*speed;
			}	
		}
	}//
	//catching
	if(state == "CATCHING"){
		//Debug.Log(gold.transform.position);
		if(transform.position.y > initialPosition.y){
			Destroy(gold);
			returnIDLE();
		}
	}
	//hooking
	if(state == "HOOKING"){
		if(transform.position.y <= -0.9 || transform.position.x < -1.6 || transform.position.x > 1.6) GoBack();
		if(transform.position.y > initialPosition.y) returnIDLE();
	}
}
function OnTriggerEnter(col : Collider) {
	if(col.gameObject.tag == "Gold") {
		renderer.material.mainTexture = textures[1];
		GoBack();
		col.rigidbody.velocity = rigidbody.velocity;
		gold = col.gameObject;
		state = "CATCHING";
	}
}

function GoBack(){
	rigidbody.velocity = -rotateDirection*speed;
}
function returnIDLE(){	
	renderer.material.mainTexture = textures[0];
	rigidbody.velocity = Vector3(0,0,0);
	transform.position = initialPosition;
	state = "IDLE";
}