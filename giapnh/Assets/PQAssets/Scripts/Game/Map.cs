using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
	public float[] clone_pig_times;
	public GameObject pig;
	GameObject pig_clone;
	int i=0;
	Vector3 random_position;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log(Time.realtimeSinceStartup);
		if(i<clone_pig_times.Length && Time.realtimeSinceStartup > clone_pig_times[i]){
			random_position = new Vector3(-2f,0.3f,0.42f);
			pig_clone = (GameObject)Instantiate(pig, random_position, Quaternion.identity);			
			pig_clone.transform.parent = transform;
			pig_clone.GetComponent<Pig>().path =i%2;
			//Debug.Log (pig_clone.transform.position);
			i++;
		}
		
	}
}
