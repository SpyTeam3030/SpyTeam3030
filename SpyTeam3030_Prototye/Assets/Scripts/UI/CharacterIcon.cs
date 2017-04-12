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
	public string m_name;

	public CombatController mySpy;

	private float originalAtkSpeed = 0.0f;
	private float m_timer = 0.0f;
	private bool onTime = false;

	// Use this for initialization
	void Start () {
		mySpy = null;
		currentSprite = defaultSprite;
        gc = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (mySpy == null) {
			GameObject[] objs = GameObject.FindGameObjectsWithTag ("CombatObject");
			foreach(GameObject obj in objs)
			{
				CombatController objCombatController = obj.GetComponent<CombatController> ();
				if(m_name == objCombatController.m_name)
				{
					mySpy = objCombatController;
					Debug.Log (mySpy.m_name);
					break;
				}
			}
		}

		if (onTime) {
			m_timer -= Time.deltaTime;
			if(m_timer < 0.0f)
			{
				ChangeSpy (19, 0.0f, 0.0f, 0.0f, 0.0f, originalAtkSpeed);
				mySpy.cardID = 0;
				onTime = false;
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
		mySpy = GameObject.Find (m_name).GetComponent<CombatController> ();
	}

	public bool ChangeSpy(int cardID, float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f, float damage = 0.0f){
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
			if (cardID == 19) {
				originalAtkSpeed = mySpy.attackSpeed;
				newAttackSpeed = originalAtkSpeed / 2.0f;
				onTime = true;
				m_timer = 5.0f;
			}
            gc.CmdAttributeChange(mySpy.name, cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed, damage);
            return true;
        }
	}

	public bool IsSameTeam(int id){
		return mySpy.IsSameTeam(id);
	}

}
