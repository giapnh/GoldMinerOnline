using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public float speed_rate;
	public int item_id;
	public int item_score;
	// Use this for initialization
	float running_time = 0;
	public int state = 0;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (state == 0){
			running_time += Time.deltaTime;
			//TODO fix runnign time theo server
			if (running_time > 15)
					Destroy (this.gameObject);
		}
	}
}
