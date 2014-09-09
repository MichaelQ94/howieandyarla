using UnityEngine;
using System.Collections;

public class TeleporterS : MonoBehaviour {

	public GameObject	transportTarget;

	void OnCollisionEnter (Collision other){

		if (other.gameObject.tag == "Player"){
			if (transportTarget != null){

				Vector3 transportPos = transportTarget.transform.position;
				transportPos.z = HowieS.H.transform.position.z;

				other.gameObject.transform.position = transportPos;

				//Vector3 cameraTransportPos = transportTarget.transform.position;
				//cameraTransportPos.z = CameraShakeS.C.transform.position.z;
				//CameraShakeS.C.transform.position = cameraTransportPos;
			}
		}

	}
}
