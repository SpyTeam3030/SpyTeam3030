using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

	public GameObject children;

	public Text myText;
	public Text enemyText;
	public Text drawText;
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
			myText.text = "WINNER!";
			enemyText.text = "LOSER!";
		}
	}

	public void Lose(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			myText.text = "LOSER!";
			enemyText.text = "WINNER!";
		}
	}

	public void Draw(){
		if (set == false) {
			set = true;
			children.SetActive (true);
			myText.gameObject.SetActive (false);
			enemyText.gameObject.SetActive (false);
			drawText.gameObject.SetActive (true);
		}
	}
}
