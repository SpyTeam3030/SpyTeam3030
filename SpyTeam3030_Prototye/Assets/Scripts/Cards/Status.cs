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

	public void SetDescription(string d){
		description.text = d;
	}

	public void SetAttack(float a){
        attack.text = a.ToString();
	}

	public void SetHP(float hp){
		if (hp == 999) {
			HP.text = "FULL";
		}else{
			HP.text = hp.ToString();
		}
	}

	public void SetSpeed(float s){
        speed.text = s.ToString();
	}

	public void ResetInformation(){
		description.text = "";
		attack.text = 000 + "";
		HP.text = 000 + "";
		speed.text = 000 + "";
	}
}
