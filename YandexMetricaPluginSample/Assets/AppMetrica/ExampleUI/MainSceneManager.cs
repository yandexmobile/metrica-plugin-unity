using UnityEngine;
using System.Collections;

public class MainSceneManager : MonoBehaviour 
{
	private GameObject nullGameObject = null;

	private static bool isReportsEnabled = true;
	private static bool isTrackLocationEnabled = true;
	private PopUp popupWindow = new PopUp ();
	private static int counter = 1;

	private void initGUI()
	{
		GUI.skin.button.fontSize = 40;
		GUI.skin.textField.fontSize = 35;
		GUI.contentColor = Color.white;
		GUI.skin.label.fontSize = 40;
	}

	private void OnGUI ()
	{
		initGUI ();

		popupWindow.onGUI ();

		var metrica = AppMetrica.Instance;
		if (Button("Report Test")) {
			string report = "Test" + counter++;
			metrica.ReportEvent(report);
			popupWindow.showPopup("Report: " + report);
		}
		if (Button("Force Send Reports")) {
			metrica.SendEventsBuffer();
		}
		if (Button("Track Location Enabled: " + isTrackLocationEnabled)) {
			isTrackLocationEnabled = !isTrackLocationEnabled;
			metrica.TrackLocationEnabled = isTrackLocationEnabled;
		}
		if (Button("Reports Enabled: " + isReportsEnabled)) {
			isReportsEnabled = !isReportsEnabled;
			metrica.ReportsEnabled = isReportsEnabled;
		}
		if (Button("[CRASH] NullReference")) {
			nullGameObject.SendMessage("");
		}
		if (Button("LOG Library Version")) {
			popupWindow.showPopup("Version: " + metrica.LibraryVersion);
		}
		if (Button("LOG Library API Level")) {
			popupWindow.showPopup("Level: " + metrica.LibraryApiLevel);
		}
		if (Button("[SCENE] Load")) {
			Application.LoadLevel("AnotherScene");
		}

		if (Button("Exit")) {
			Application.Quit();
		}
	}

	private bool Button(string title)
	{
		return GUILayout.Button(title, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height / 12));
	}
}
