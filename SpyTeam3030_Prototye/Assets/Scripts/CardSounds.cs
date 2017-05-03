using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

using UnityEngine;

public class CardSounds : NetworkBehaviour {

	public AudioSource source;
	public AudioClip[] clips;
	public AudioClip tapSound;
	public AudioClip newcardSound;
	public AudioClip winSound;
	public AudioClip loseSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayCardSound(int id){
		source.PlayOneShot (clips [id-1]);
	}

	public void PlayTapSound(){
		source.PlayOneShot (tapSound);
	}

	public void PlayNewcardSound(){
		source.PlayOneShot (newcardSound);
	}

	public void PlayWinSound(){
		source.PlayOneShot (winSound);
	}

	public void PlayLoseSound(){
		source.PlayOneShot (loseSound);
	}

	[ClientRpc]
	public void RpcPlayCardSound(int id){
		PlayCardSound (id);
	}
}
