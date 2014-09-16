using UnityEngine;
using System.Collections;

public class YarlaTendrilS : MonoBehaviour {

	public TrailRenderer	yarlaGrabRenderer;
	public TrailRenderer	yarlaAttackRenderer;
	public HowieS howie;

	// Use this for initialization
	void Start () {
		howie = GameObject.FindGameObjectsWithTag ("Player") [0].GetComponent<HowieS> ();
	}
	
	// Update is called once per frame
	void Update () {


		if (howie.isHowieSolo){

			yarlaGrabRenderer.enabled = false;
			yarlaAttackRenderer.enabled = false;

		}
		else{

			if (NewChompS.N.attacking){
				yarlaAttackRenderer.enabled = true;
			}
			else{
				yarlaAttackRenderer.enabled = false;
			}

			if (YarlaS.Y.launched){
				yarlaGrabRenderer.enabled = true;
			}
			else{
				yarlaGrabRenderer.enabled = false;
			}

		}
	
	}
}
