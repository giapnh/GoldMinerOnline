using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	//public GameObject character;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick(){
		GameObject character =  GameObject.Find("Character");
		Character character_info = character.GetComponentInChildren<Character>();
        character_info.state = Character.MOVING;
		character_info.target.x = transform.position.x;
	}
}
