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
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetDescription(string d){
		description.text = d;
	}

	public void SetAttack(int a){
		attack.text = a + "";
	}

	public void SetHP(int hp){
		HP.text = hp + "";
	}

	public void SetSpeed(int s){
		speed.text = s + "";
	}

	public void ResetInformation(){
		description.text = "";
		attack.text = 000 + "";
		HP.text = 000 + "";
		speed.text = 000 + "";
	}
}
