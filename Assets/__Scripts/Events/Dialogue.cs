using UnityEngine;
using System.Collections;

namespace Events {
public class Dialogue : MonoBehaviour {
	public const KeyCode TALK_KEY = KeyCode.L;
	public const string TEXT_FIELD = "TextField";
	public static bool shouldTalk() {
		return (Environment.isComputer () && Input.GetKeyDown (TALK_KEY)); 
	}
	public static void printMessage(string message) {
		GameObject textfield;
		if (GameObject.FindGameObjectsWithTag (TEXT_FIELD).Length > 0) {
						textfield = GameObject.FindGameObjectsWithTag (TEXT_FIELD) [0];
						textfield.GetComponent<TextMesh> ().text = message;
		} 
		else throw new MissingComponentException("TextField not found!");
		
	}
}
}
