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
		}
	}

	public void Lose(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			displayImage.GetComponent<Image> ().sprite = loseImage;
		}
	}

	public void Draw(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			displayImage.GetComponent<Image> ().sprite = drawImage;
		}
	}
}
