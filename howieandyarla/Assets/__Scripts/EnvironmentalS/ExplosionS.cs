using UnityEngine;
using System.Collections;

public class ExplosionS : MonoBehaviour {
	
	public float explosionLifeTime = 0.8f;
	public float halfExploTime;

	
	// Use this for initialization
	void Start () {
		
		halfExploTime = explosionLifeTime*0.25f; 
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (explosionLifeTime > 0){
			explosionLifeTime -= Time.deltaTime;
		}
		else{
			Destroy(gameObject);
		}
		
		if (explosionLifeTime > halfExploTime){
			renderer.material.color = Color.black;
		}
		else{
			renderer.material.color = Color.white;
		}
		
	}
}
