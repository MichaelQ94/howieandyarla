using UnityEngine;
using System.Collections;

public class YarlaTendrilS : MonoBehaviour {

	public TrailRenderer	yarlaGrabRenderer;
	public TrailRenderer	yarlaAttackRenderer;

	public YarlaS yarla;

	// Use this for initialization
	void Start () {
		yarla = GameObject.FindGameObjectWithTag ("YarlaS").GetComponent<YarlaS> ();
	}
	
	// Update is called once per frame
	void Update () {


		if (HowieS.H.isHowieSolo){

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

			if (yarla.launched){
				yarlaGrabRenderer.enabled = true;
			}
			else{
				yarlaGrabRenderer.enabled = false;
			}

		}
	
	}
}
