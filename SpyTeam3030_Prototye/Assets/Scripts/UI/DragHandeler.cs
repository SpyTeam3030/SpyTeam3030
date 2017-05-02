using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public CardManager mCardManager;
	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;

	public Status status;
	public Status overlayStatus;
  
	private string description;

	private int id;
	private Card card;

	public bool clicked;
	public CardStatPanel cardStatPanel;

	void Start(){
		mCardManager.NextCard (gameObject, -1);
		clicked = false;
	}

	void Update(){
		if (clicked) {
			overlayStatus.gameObject.SetActive (true);
		}
		else{
			overlayStatus.gameObject.SetActive (false);
		}
	}

	public void Clicked(){
		overlayStatus.SetInfo (card.description, card.attack, card.health, card.speed);
		GameObject.Find ("Canvas").GetComponent<CardSounds> ().PlayTapSound ();
		clicked = !clicked;
	}

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
        Color c = GetComponentInParent<Image>().color;
        c.a = 0.5f;
        GetComponentInParent<Image>().color = c;
		clicked = false;
		cardStatPanel.HideCardStats ();
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		GetComponent<Transform>().position = eventData.position;
		SetInformation (eventData.position);
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GetComponent<Transform>().position = startPosition;
        Color c = GetComponentInParent<Image>().color;
        c.a = 1.0f;
        GetComponentInParent<Image>().color = c;
		ResetInformation ();

		if (card.type == "SE") {
			//Code to be place in a MonoBehaviour with a GraphicRaycaster component
			GraphicRaycaster gr = GameObject.Find ("Canvas").GetComponent<GraphicRaycaster> ();
			//Create the PointerEventData with null for the EventSystem
			PointerEventData ped = new PointerEventData (null);
			//Set required parameters, in this case, mouse position
			ped.position = Input.mousePosition;
			//Create list to receive all results
			List<RaycastResult> results = new List<RaycastResult> ();
			//Raycast it
			gr.Raycast (ped, results);

			if (results.Count == 0) {
				Debug.Log ("Card type SE");
				if (card.id == 25) {
					//right now it just kills all enemy spies
					if (results [0].gameObject != null) {
						int m_id = -1;
						GameplayClient gc = null;
						GameplayClient[] clients = GameObject.FindObjectsOfType<GameplayClient> ();
						foreach (GameplayClient cobj in clients) {
							if (cobj.isLocalPlayer) {
								gc = cobj;
								m_id = cobj.teamID;
								break;
							}
						}

						GameObject[] obs = GameObject.FindGameObjectsWithTag ("Icon");
						for (int i = 0; i < obs.Length; i++) {
							CharacterIcon ci = obs [i].GetComponent<CharacterIcon> ();
							if (ci != null && !ci.IsSameTeam (m_id)) {
								gc.CmdAttributeChange (ci.mySpy.name, card.id, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 999.0f);
								GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

								mCardManager.NextCard (this.gameObject, card.id);
								GetComponent<Animator> ().SetTrigger ("Appear");
							}
						}
					}
				} else if (card.id == 26) {

				} else if (card.id == 27) {
					GameplayClient gc = null;
					GameplayClient[] clients = GameObject.FindObjectsOfType<GameplayClient> ();
					foreach(GameplayClient cobj in clients)
					{
						if(cobj.isLocalPlayer)
						{
							Debug.Log ("local player id " + cobj.teamID);
							gc = cobj;
							break;
						}
					}
					if (gc != null) {
						gc.CmdDisableHUD (gc.teamID);
						GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

						mCardManager.NextCard (this.gameObject, card.id);
						GetComponent<Animator> ().SetTrigger ("Appear");
					}
				} else if (card.id == 28) {

				} else if (card.id == 29) {

				} else if (card.id == 30) {
					if (results [0].gameObject != null) {
						int m_id = -1;
						GameplayClient gc = null;
						GameplayClient[] clients = GameObject.FindObjectsOfType<GameplayClient> ();
						foreach (GameplayClient cobj in clients) {
							if (cobj.isLocalPlayer) {
								gc = cobj;
								m_id = cobj.teamID;
								break;
							}
						}

						GameObject[] obs = GameObject.FindGameObjectsWithTag ("Icon");
						for (int i = 0; i < obs.Length; i++) {
							CharacterIcon ci = obs [i].GetComponent<CharacterIcon> ();
							if (ci != null && !ci.IsSameTeam (m_id)) {
								gc.CmdAttributeChange (ci.mySpy.name, card.id, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, card.attack);
								GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

								mCardManager.NextCard (this.gameObject, card.id);
								GetComponent<Animator> ().SetTrigger ("Appear");
							}
						}
					}
				}
			}

			return;
		} else if (card.type == "SP" && card.id == 23) {
			//deal 30 damage to tower in the lane
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				TowerController tc = hit.transform.GetComponent<TowerController> ();
				if (tc == null) {
					return;
				}
				int myid = tc.getMyID();
				GameplayClient gc = null;
				GameplayClient[] clients = GameObject.FindObjectsOfType<GameplayClient> ();
				foreach(GameplayClient cobj in clients)
				{
					if(cobj.isLocalPlayer)
					{
						gc = cobj;
						break;
					}
				}
				if (gc != null && !tc.IsSameTeam(gc.teamID)) {
					gc.CmdDoDamageToSingleTower (card.attack, myid);
					Debug.Log ("do damage to tower " + myid);

					GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

					mCardManager.NextCard (this.gameObject, card.id);
					GetComponent<Animator> ().SetTrigger ("Appear");
				}
			}
		}
		else{
			//Code to be place in a MonoBehaviour with a GraphicRaycaster component
			GraphicRaycaster gr = GameObject.Find("CardHitCanvas").GetComponent<GraphicRaycaster>();
			//Create the PointerEventData with null for the EventSystem
			PointerEventData ped = new PointerEventData(null);
			//Set required parameters, in this case, mouse position
			ped.position = Input.mousePosition;
			//Create list to receive all results
			List<RaycastResult> results = new List<RaycastResult>();
			//Raycast it
			gr.Raycast(ped, results);

			if (results.Count > 0) {
				if (card.type == "ST") {
					Debug.Log ("Card type ST");
					if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI") && 
						results [0].gameObject.GetComponent<CharacterIcon> ().ChangeSpy (card.id, card.health, card.attack, card.speed, card.attackDistance, card.attackSpeed)) {
						Debug.Log ("Take Effect");
						GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

						mCardManager.NextCard (this.gameObject, card.id);
						GetComponent<Animator> ().SetTrigger ("Appear");
					}
				}
				else if (card.type == "SP") 
				{
					Debug.Log ("Card type SP");
					//Sniper rifle
					if (card.id == 17) {
						if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI") && 
							results [0].gameObject.GetComponent<CharacterIcon> ().ChangeSpy (card.id, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, card.attack)) {
							Debug.Log ("Take Effect");
							GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

							mCardManager.NextCard (this.gameObject, card.id);
							GetComponent<Animator> ().SetTrigger ("Appear");
						}				
					}
					else if (card.id == 18) {
						if (results [0].gameObject != null) {
							int m_id = -1;
							GameplayClient gc = null;
							GameplayClient[] clients = GameObject.FindObjectsOfType<GameplayClient> ();
							foreach(GameplayClient cobj in clients)
							{
								if(cobj.isLocalPlayer)
								{
									gc = cobj;
									m_id = cobj.teamID;
									break;
								}
							}

							GameObject[] obs = GameObject.FindGameObjectsWithTag ("Icon");
							for (int i = 0; i < obs.Length; i++) {
								CharacterIcon ci = obs [i].GetComponent<CharacterIcon> ();
								if (ci != null && !ci.IsSameTeam(m_id)) {
									gc.CmdAttributeChange(ci.mySpy.name, card.id, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, card.attack);
									GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);
								}
							}
						}
					}
					else if (card.id == 19) 
					{
						if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI") && 
							results [0].gameObject.GetComponent<CharacterIcon> ().ChangeSpy (card.id, card.health, card.attack, card.speed, card.attackDistance, card.attackSpeed)) {
							Debug.Log ("Take Effect");

							GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

							mCardManager.NextCard (this.gameObject, card.id);
							GetComponent<Animator> ().SetTrigger ("Appear");
						}
					}else if (card.id == 20) {
						if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI")) {
							results [0].gameObject.GetComponent<CharacterIcon> ().mySpy.Flash (100.0f);
							Debug.Log ("Take Effect");

							GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

							mCardManager.NextCard (this.gameObject, card.id);
							GetComponent<Animator> ().SetTrigger ("Appear");
						}
					}
					else if (card.id == 21 || card.id == 22 || card.id == 24) {
						if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI") && 
							results [0].gameObject.GetComponent<CharacterIcon> ().ChangeSpy (card.id, card.health, card.attack, card.speed, card.attackDistance, card.attackSpeed)) {
							Debug.Log ("Take Effect");

							GameObject.Find ("Canvas").GetComponent<CardSounds> ().RpcPlayCardSound (card.id);

							mCardManager.NextCard (this.gameObject, card.id);
							GetComponent<Animator> ().SetTrigger ("Appear");
						}
					}
				}
			}
		}

	}

	#endregion

	public void SetInformation(Vector2 pos){
		status.SetInfo (card.description, card.attack, card.health, card.speed, pos);
		overlayStatus.SetInfo (card.description, card.attack, card.health, card.speed);
	}

	public void ResetInformation(){
		status.ResetInformation ();
	}

	public void Init(Card c){
		this.card = c;

		GetComponent<Image> ().sprite = card.image;
	}
}
