using UnityEngine;
using System.Collections;

public class Backgroundmusic : MonoBehaviour {

	public AudioClip winMusic;
	public AudioClip loseMusic;

	public void PlayMusic(){
		GetComponent<AudioSource> ().Play ();
	}

	public void PlayWinLoseMusic(bool win){
		if (win) {
			GetComponent<AudioSource> ().PlayOneShot (winMusic);
		} else {
			GetComponent<AudioSource> ().PlayOneShot (loseMusic);
		}
	}
}
