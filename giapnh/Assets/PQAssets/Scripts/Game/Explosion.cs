using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	//vars for the whole sheet
	public int colCount =  5;
	public int rowCount =  5;
	 
	//vars for animation
	public int  rowNumber  =  0; //Zero Indexed
	public int colNumber = 0; //Zero Indexed
	public int totalCells = 25;
	public int  fps     = 10;
	int j = 0;
	public float damage_radius;
  	//Maybe this should be a private var
    private Vector2 offset;
	float count_time=0;
	bool is_destroyed = false;
	GameObject onlineGameScreen;
	OnlineGamePanel onlineGame_info;
	GameObject dmHook;
	
	string[] items_list = new string[4]{ "Gold", "Diamond", "Stone", "Buff"};
	void Start () {
		onlineGameScreen = GameObject.Find("OnlineGameScreen");
		dmHook = GameObject.Find("Hook");
	}
	
	// Update is called once per frame
	void Update () {
		j = SetSpriteAnimation(colCount,rowCount,rowNumber,colNumber,totalCells,fps);
		if(j==24) {
			Destroy(this.gameObject);
		}
		
		if(!is_destroyed) {
			ExplosionDamage();
			is_destroyed = true;
		}
	}
	
	void ExplosionDamage() {
		//Debug.Log ("a");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damage_radius);
        int i = 0;
        while (i < hitColliders.Length) {
			//if(hitColliders[i].gameObject.tag=="Gold" || hitColliders[i].gameObject.tag=="Diamond" || hitColliders[i].gameObject.tag=="Pig"){
			if(System.Array.IndexOf(items_list,hitColliders[i].gameObject.tag)!=-1){
				Destroy(hitColliders[i].gameObject);
				if(onlineGameScreen){
					//online
					onlineGame_info = onlineGameScreen.gameObject.GetComponent<OnlineGamePanel> ();
					if(hitColliders[i].gameObject.tag!="Buff")
						onlineGame_info.item_count --;
				} else{
					//offline
					DmHook dmHook_info = dmHook.gameObject.GetComponent<DmHook>();
					dmHook_info.item_count --;
				}
			}
            i++;
        }
    }
	
	int SetSpriteAnimation(int colCount ,int rowCount ,int rowNumber ,int colNumber,int totalCells,int fps ){
	    // Calculate index
	    //int index  = (int)(Time.time * fps);
		int index  = (int)(count_time * fps);
	    // Repeat when exhausting all cells
	    index = index % totalCells;
	 
	    // Size of every cell
	    float sizeX = 1.0f / colCount;
	    float sizeY = 1.0f / rowCount;
	    Vector2 size =  new Vector2(sizeX,sizeY);
	 
	    // split into horizontal and vertical index
	    var uIndex = index % colCount;
	    var vIndex = index / colCount;
	 
	    // build offset
	    // v coordinate is the bottom of the image in opengl so we need to invert.
	    float offsetX = (uIndex+colNumber) * size.x;
	    float offsetY = (1.0f - size.y) - (vIndex + rowNumber) * size.y;
	    Vector2 offset = new Vector2(offsetX,offsetY);
	 
	    renderer.material.SetTextureOffset ("_MainTex", offset);
	    renderer.material.SetTextureScale  ("_MainTex", size);
		count_time+=Time.deltaTime;
		return index;
	}
}
