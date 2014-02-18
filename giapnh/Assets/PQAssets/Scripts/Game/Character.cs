using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public int state;
	public static int IDLE = 0;
	public static int MOVING = 1;
	public Vector3 target;
	public float speed;
	// Use this for initialization
	void Start () {
		state = IDLE;
		target = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == MOVING){
			Move(target);
		}
	}
	
	void Move(Vector3 target){
		float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
		if(transform.position == target){
			state = IDLE;
		}
	}
}
