using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Status : MonoBehaviour {

	public Text description;
	public Text attack;
	public Text HP;
	public Text speed;

	// Use this for initialization
	void Start () {
		ResetInformation ();
	}

	public void SetInfo(string d, float a, float hp, float s, Vector2 pos){
		gameObject.SetActive(true);
		attack.gameObject.SetActive(true);
		HP.gameObject.SetActive(true);
		speed.gameObject.SetActive(true);

		SetDescription (d);
		SetHP (hp);
		SetAttack (a);
		SetSpeed (s);
		pos += new Vector2 (0, 140);
		GetComponent<Transform> ().position = pos;
	}

	public void SetInfo(string d, float a, float hp, float s){
		gameObject.SetActive(true);
		attack.gameObject.SetActive(true);
		HP.gameObject.SetActive(true);
		speed.gameObject.SetActive(true);

		SetDescription (d);
		SetHP (hp);
		SetAttack (a);
		SetSpeed (s);
	}

	public void SetDescription(string d){
		description.text = d;
	}

	public void SetAttack(float a){
		if (a == 0) {
			attack.gameObject.SetActive(false);
		} else {
			attack.text = "+" + a.ToString () + " ATT";
		}
	}

	public void SetHP(float hp){
		if (hp == 0) {
			HP.gameObject.SetActive(false);
		}
		else if (hp == 999) {
			HP.text = "FULL";
		}else{
			HP.text = "+" + hp.ToString() + " HP";
		}
	}

	public void SetSpeed(float s){
		if (s == 0) {
			speed.gameObject.SetActive (false);
		} else {
			speed.text = "+" + s.ToString () + " SP";
		}
	}

	public void ResetInformation(){
		gameObject.SetActive(false);
	}
}
