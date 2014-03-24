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
	int i = 0;
	public float damage_radius;
  	//Maybe this should be a private var
    private Vector2 offset;
	float count_time=0;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		i = SetSpriteAnimation(colCount,rowCount,rowNumber,colNumber,totalCells,fps);
		if(i==0) {
			Debug.Log ("a");
			ExplosionDamage();
		}
		if(i==24) {
			Destroy(this.gameObject);
		}
	}
	
	void ExplosionDamage() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damage_radius);
        int i = 0;
        while (i < hitColliders.Length) {
            i++;
			Debug.Log (hitColliders[i].gameObject.tag);
			if(hitColliders[i].gameObject.tag=="Gold" || hitColliders[i].gameObject.tag=="Pig")
				Destroy(hitColliders[i].gameObject);
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
		//Debug.Log (index);
		count_time+=Time.deltaTime;
		return index;
	}
}
