
using UnityEngine;
using System.Collections;

public class Controll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR_WIN
		if(Input.GetMouseButtonDown(0)){
			Debug.Log("Touch");
		}
#elif UNITY_ANDROID
		if(Input.touchCount > 0){
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				Debug.Log("Touch!!");
			}
		}
#endif
	}
}
