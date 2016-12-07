using UnityEngine;
using System.Collections;

public class Backgroundmusic : MonoBehaviour {

	public AudioClip loopClip;
	public float length;

	// Use this for initialization
	void Start () {
		length = GetComponent<AudioSource> ().clip.length;
//		GetComponent<AudioSource> ().time = 50f;
		Invoke ("Loop", length - 0.04f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Loop(){
		GetComponent<AudioSource> ().clip = loopClip;
		GetComponent<AudioSource> ().Play ();
	}
}
