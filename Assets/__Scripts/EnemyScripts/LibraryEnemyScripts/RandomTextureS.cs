using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTextureS : MonoBehaviour {

	// for zombie proj
	// randomize look

	public List<Texture> possibleTextures;

	// Use this for initialization
	void Start () {

		renderer.material.SetTexture("_MainTex",possibleTextures[Mathf.RoundToInt(Random.Range(0,possibleTextures.Count))]);
	
	}

}
