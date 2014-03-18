using UnityEngine;
using System.Collections;

public class Pig : MonoBehaviour {
	public int state;
	public static int HOOKED = 0;
	public static int MOVING = 1;
	float percentsPerSecond = 0.1f; // %2 of the path moved per second
    float currentPathPercent = 0.0f; //min 0, max 1
	public int path;
	// Use this for initialization
	void Start () {
		state=MOVING;
	}
	
	// Update is called once per frame
	void Update () {
		if(state==MOVING){
			currentPathPercent += percentsPerSecond * Time.deltaTime;
			iTween.PutOnPath(this.gameObject, iTweenPath.GetPath("pig_path"+path), currentPathPercent);
		}
		if(transform.position.x > 2) Destroy (gameObject);
	}
}
