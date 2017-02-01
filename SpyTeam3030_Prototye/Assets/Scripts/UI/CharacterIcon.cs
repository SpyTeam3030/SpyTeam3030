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
		mySpy = GameObject.Find (name).GetComponent<CombatController> ();
		if (mySpy == null) {
			return false;
		}
//		if (gs.isServer && mySpy.getID() != 0) {
//			return false;
//		}
//		if (gs.isClient && mySpy.getID() != 1) {
//			return false;
//		}
		GameObject[] obs = GameObject.FindGameObjectsWithTag ("Client");
		if (obs.Length == 0) {
			return false;
		}
		foreach(var gameobject in obs)
		{
			if(gameobject.GetComponent<GameplayClient>().isLocalPlayer){
				gameobject.GetComponent<GameplayClient> ().CmdAttributeChange (name, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
			}
		}

		return true;
		
	}
}
