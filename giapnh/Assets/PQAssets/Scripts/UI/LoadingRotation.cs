using UnityEngine;
using System.Collections;

public class LoadingRotation : MonoBehaviour {
	Vector3 rotation;
	// Use this for initialization
	void Start () {
		rotation = new Vector3(0,0,-4);
	}
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(rotation);
	}
}
