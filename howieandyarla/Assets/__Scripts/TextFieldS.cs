using UnityEngine;
using System.Collections;
using Events;

public class TextFieldS : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempScreenPosition = new Vector2(0, 0);
		tempScreenPosition.z = -Camera.main.transform.position.z;
		
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(tempScreenPosition);
		worldPosition.x -= renderer.bounds.size.x * tempScreenPosition.x / Screen.width;
		worldPosition.y += renderer.bounds.size.y * (1 - tempScreenPosition.y / Screen.height);
		transform.position = worldPosition;
	}
}
