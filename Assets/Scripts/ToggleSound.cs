using UnityEngine;
using System.Collections;

public class ToggleSound : MonoBehaviour {

	AudioSource audioSource;

	// Use this for initialization
	void Start () 
	{
		audioSource = GameObject.Find ("MusicHandler").GetComponent<AudioSource>();
	}

	public void ToggleMusic()
	{
		if (!PlayerPrefs.HasKey("Sound") || (PlayerPrefs.GetInt("Sound") == 1)) 
		{
			audioSource.mute = true;
			PlayerPrefs.SetInt("Sound", 0);
		}
		else
		{
			PlayerPrefs.SetInt ("Sound", 1);
			audioSource.mute = false;
			audioSource.Play ();
		}
	}
}
