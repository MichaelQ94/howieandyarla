using UnityEngine;
using System.Collections;

public class EventHandlers : MonoBehaviour {
	public const KeyCode TALK_KEY = KeyCode.L;
	public static bool shouldTalk() {
		return (isComputer () && Input.GetKeyDown (TALK_KEY)); 
	}
	public static bool isComputer() {
		return isMac () || isWindows ();
	}
	public static bool isMac() {
		return (Application.platform == RuntimePlatform.OSXEditor || 
			Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.OSXDashboardPlayer);
	}
	public static bool checkIfPlayerCollision(Collider other) {
		return other.gameObject.tag == "Player";
	}
	public static bool isWindows() {
		return (Application.platform == RuntimePlatform.WindowsEditor || 
						Application.platform == RuntimePlatform.WindowsPlayer ||
						Application.platform == RuntimePlatform.WindowsWebPlayer);
	}
}
