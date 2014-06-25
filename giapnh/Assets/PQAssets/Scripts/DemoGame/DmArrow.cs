using UnityEngine;
using System.Collections;

public class DmArrow : MonoBehaviour {
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnClick(){
		GameObject character =  GameObject.Find("Character");
		Character character_info = character.GetComponent<Character>();
		DmHook hook_info = character.gameObject.GetComponentInChildren<DmHook>();
		if(hook_info.state == Hook.IDLE){
			character_info.state = Character.MOVING;
			character_info.target.x = transform.position.x;
		}
	}
}