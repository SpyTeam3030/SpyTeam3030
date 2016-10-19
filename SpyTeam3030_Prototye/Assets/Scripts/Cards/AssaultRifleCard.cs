using UnityEngine;
using System.Collections;

public class AssaultRifleCard : MonoBehaviour {

	public Status status;

	public string description;

	public int attack;
	public int hp;
	public int speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetInformation(){
		status.SetDescription (description);
		status.SetAttack (attack);
		status.SetHP (hp);
		status.SetSpeed (speed);
	}

	public void ResetInformation(){
		status.ResetInformation ();
	}
}
