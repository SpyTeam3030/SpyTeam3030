using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour {

//	public Text AttackText;
//	public Text HealthText;
//	public Text SpeedText;
	public Sprite defaultSprite;
	public Sprite currentSprite;
    public GameplayClient gc;
	public string name;

	public CombatController mySpy;

	// Use this for initialization
	void Start () {
		mySpy = null;
		currentSprite = defaultSprite;
        gc = null;
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

	public bool ChangeSpy(int cardID, float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f){
        if (gc == null)
        {
            GameObject[] gcs = GameObject.FindGameObjectsWithTag("LocalClient");
            foreach(GameObject g in gcs)
            {
                if (g.GetComponent<GameplayClient>().isLocalPlayer)
                {
                    gc = g.GetComponent<GameplayClient>();
                    break;
                }
            }
        }

        if (mySpy == null)
        {
            return false;
        }
        else
        {
            gc.CmdAttributeChange(mySpy.name, cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
            return true;
        }
	}
}
