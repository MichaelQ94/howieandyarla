using UnityEngine;
using System.Collections;

namespace Events {
public class Environment : MonoBehaviour {
	public const string PLAYER = "Player";

		public static string reloadScene;

	public static bool isComputer() {
		return isMac () || isWindows ();
	}
	public static string getPlatform() {
			if (isMac ()) {
				return "Mac";
			}
			else if (isWindows ()) { 
				return "PC";
			}
			else { 
				return "Default";
			}
	}
	public static bool isMac() {
		return (Application.platform == RuntimePlatform.OSXEditor || 
			Application.platform == RuntimePlatform.OSXPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer || 
				Application.platform == RuntimePlatform.OSXDashboardPlayer);
	}
	public static bool checkIfPlayerCollision(Collider other) {
		return other.gameObject.tag == PLAYER;
	}
	public static bool isWindows() {
		return (Application.platform == RuntimePlatform.WindowsEditor || 
						Application.platform == RuntimePlatform.WindowsPlayer ||
						Application.platform == RuntimePlatform.WindowsWebPlayer);
	}
	public static void pause(bool isPaused) {
			Time.timeScale = (isPaused) ? 0f : 1f;
	}
}
}
