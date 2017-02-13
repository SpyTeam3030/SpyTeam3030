using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour {

//	public Text AttackText;
//	public Text HealthText;
//	public Text SpeedText;
	public Sprite defaultSprite;
	public Sprite currentSprite;
	public GameplayServer gs;
	public string name;

	public CombatController mySpy;

	// Use this for initialization
	void Start () {
		mySpy = null;
		currentSprite = defaultSprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (mySpy == null) {
			GameObject[] obs = GameObject.FindGameObjectsWithTag ("CombatObject");
			if (obs.Length != 0) {
				mySpy = GameObject.Find (name).GetComponent<CombatController> ();
			}
		}

		if (mySpy != null){
			int id = mySpy.cardID;
			if (id != 0) {
				currentSprite = GameObject.Find ("CardManager").GetComponent<CardManager> ().images [id - 1];
			} else
				currentSprite = defaultSprite;
		}else{
			currentSprite = defaultSprite;
		}
		GetComponent<Image> ().sprite = currentSprite;
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

	public bool ChangeSpy(int cardID, float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f){
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

		bool result = !mySpy.card;
		foreach(var gameobject in obs)
		{
			if(gameobject.GetComponent<GameplayClient>().isLocalPlayer){
				gameobject.GetComponent<GameplayClient> ().CmdAttributeChange (result, name, cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
			}
		}

		return result;
		
	}
}
