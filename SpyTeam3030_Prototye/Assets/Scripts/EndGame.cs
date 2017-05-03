using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public GameObject children;

	public GameObject displayImage;
	public Sprite drawImage;
	public Sprite winImage;
	public Sprite loseImage;

	private bool set;

	// Use this for initialization
	void Start () {
		set = false;
		children.SetActive (false);
	}
	
	// Update is called once per frame
	public void Win(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			displayImage.GetComponent<Image> ().sprite = winImage;
			Debug.Log ("WIN!");
			GameObject.Find ("Canvas").GetComponent<CardSounds> ().PlayWinSound ();
		}
	}

	public void Lose(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			displayImage.GetComponent<Image> ().sprite = loseImage;
			Debug.Log ("Lose!");
			GameObject.Find ("Canvas").GetComponent<CardSounds> ().PlayLoseSound ();
		}
	}

	public void Draw(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			Debug.Log ("Draw!");
			displayImage.GetComponent<Image> ().sprite = drawImage;
		}
	}
}
