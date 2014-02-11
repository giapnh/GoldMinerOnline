#pragma strict
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
//test linerenderer
var c1 : Color = Color.yellow;
var c2 : Color = Color.red;
var lengthOfLineRenderer : int = 20;
function Start () {

}

function Update () {
	//rotate
	if(state == "IDLE"){
		center_point = transform.parent.transform.position + Vector3(0,-0.1,0);
		if(transform.rotation.z >= 0.5) flag=-1;
		else if(transform.rotation.z <= -0.5) flag=1;
		transform.RotateAround (center_point, Vector3.forward, 40 * Time.deltaTime * flag);
		
		//click
		if(Input.GetMouseButtonDown(0)){
			state = "HOOKING";
			initialPosition = transform.position;
			rotateDirection = transform.position - center_point;
			rigidbody.velocity = rotateDirection*speed;		
		}
		//lineRenderer.SetPosition(0, center_point);
		//lineRenderer.SetPosition(1, transform.position);
	}
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
		Debug.Log(transform.position);
		if(transform.localPosition.y <= -8 || transform.position.x > 385) GoBack();
		if(transform.position.y > initialPosition.y) returnIDLE();
	}
}
function OnTriggerEnter(col : Collider) {
	if(col.gameObject.tag == "Gold") {
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
	rigidbody.velocity = Vector3(0,0,0);
	transform.position = initialPosition;
	state = "IDLE";
}