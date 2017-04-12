using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHUD : MonoBehaviour {
	private bool disabled;
	private float timer;

	public GameObject cardHUD;

	// Use this for initialization
	void Start () {
		disabled = false;
		timer = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (disabled == true) {
			timer -= Time.deltaTime;
		}
		if (timer < 0.0f) {
			disabled = false;
			timer = 5.0f;
			cardHUD.SetActive (true);
		}
	}

	public void DisableHUD(){
		disabled = true;
		cardHUD.SetActive (false);
	}
}
