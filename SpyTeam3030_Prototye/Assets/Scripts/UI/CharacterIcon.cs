using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour {

//	public Text AttackText;
//	public Text HealthText;
//	public Text SpeedText;
	public GameplayServer gs;
	public string name;

	public CombatController mySpy;

	// Use this for initialization
	void Start () {
		mySpy = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetSpy(CombatController s){
//		mySpy = s;
		mySpy = GameObject.Find (name).GetComponent<CombatController> ();
	}

//	public void SetStats(float attack, float health, float speed){
//		AttackText.text = "Attack: " + attack;
//		HealthText.text = "Health: " + health;
//		SpeedText.text = "Speed: " + speed;
//	}

	public bool ChangeSpy(float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f){
		Debug.Log ("Trying to Change");
		mySpy = GameObject.Find (name).GetComponent<CombatController> ();
		if (mySpy == null) {
			return false;
		}
		if (gs.isServer && mySpy.getID() != 0) {
			return false;
		}
		if (gs.isClient && mySpy.getID() != 1) {
			return false;
		}
		return (mySpy.AttributeChange (maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed));

//		return true;
		
	}
}
