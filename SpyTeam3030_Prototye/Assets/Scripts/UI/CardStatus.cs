using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardStatus : MonoBehaviour {

	public Text AttackText;
	public Text HealthText;
	public Text SpeedText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetStats(float attack, float health, float speed){
		AttackText.text = "Attack: " + attack;
		HealthText.text = "Health: " + health;
		SpeedText.text = "Speed: " + speed;
	}
}
