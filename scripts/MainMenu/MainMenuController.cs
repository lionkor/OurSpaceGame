using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

public class MainMenuController : MonoBehaviour 
{
	public string PlaySceneName;
	public string SettingsSceneName;
	
	public void Play ()
	{
		SceneManager.LoadScene (PlaySceneName);
	}

	public void Exit ()
	{
        Application.Quit ();
	}

	public void Settings ()
	{
        // PLANNED implement
        Info.WIP ("Settings Menu Option");
		SceneManager.LoadScene (SettingsSceneName);
	}
}
