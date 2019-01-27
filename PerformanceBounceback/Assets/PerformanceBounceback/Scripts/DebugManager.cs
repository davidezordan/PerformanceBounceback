using UnityEngine;

public class DebugManager {
	public static bool isDebugging = true;

	public static void Info(string message) {
		if(!isDebugging) {
			return;
		} else {
			Debug.Log(message);
		}
	}
}
