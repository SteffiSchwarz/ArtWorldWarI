using UnityEngine;
using System.Collections;

public class ChangeMusic : MonoBehaviour {
	
	public AudioClip levelMusic;
	public AudioClip menuMusic;
	private AudioSource source;


	// Use this for initialization
	void Awake () 
	{
		source = GetComponent<AudioSource>();
		if (!PlayerPrefs.HasKey("Sound")) {
			source.Play ();
			PlayerPrefs.SetInt ("Sound", 1);
		} else if (PlayerPrefs.GetInt ("Sound") == 1) {
			source.Play ();
		}
	}

	void OnLevelWasLoaded(int level)
	{
		//TODO: make level detection better - not with int?!
		if (level >= 5) 
		{
			if (PlayerPrefs.GetInt ("Sound") == 1) {
				source.clip = levelMusic;
				source.Play ();
			}
		} 

		else if (level >= 0 && level <= 7) 
		{
			if(source.isPlaying && source.clip != menuMusic)
			{
				source.clip = menuMusic;
				source.Play ();
			}
		}
	}
}