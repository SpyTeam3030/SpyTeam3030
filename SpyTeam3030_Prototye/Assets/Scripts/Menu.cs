using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public AudioClip startAudio;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartButton(){
		GetComponent<AudioSource> ().PlayOneShot (startAudio);
		SceneManager.LoadScene ("Main_l");

	}
}
