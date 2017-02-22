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
  
	private string description;

	private int id;
	private Card card;

	void Start(){
		mCardManager.NextCard (gameObject);
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
		gameObject.transform.SetAsLastSibling();
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
		GameObject.Find ("CoverImage").gameObject.transform.SetAsLastSibling ();
		ResetInformation ();

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
			if (results[0].gameObject.layer == LayerMask.NameToLayer("WorldUI")) 
			{
				if (results[0].gameObject.GetComponent<CharacterIcon> ().ChangeSpy (card.id,
					card.health, card.attack, card.speed, card.attackDistance, card.attackSpeed)) {
					Debug.Log ("Take Effect");

					mCardManager.NextCard (this.gameObject);
					GetComponent<Animator> ().SetTrigger ("Appear");
				}
			}
		}

//        // RayCasting
//        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//        RaycastHit hit;
//
//        if (Physics.Raycast (ray, out hit, 200f)) 
//        {
//			
//			
//        } 
	}

	#endregion

	public void SetInformation(Vector2 pos){
		status.SetInfo (card.description, card.attack, card.health, card.speed, pos);
	}

	public void ResetInformation(){
		status.ResetInformation ();
	}

	public void Init(Card c){
		this.card = c;

		GetComponent<Image> ().sprite = card.image;
	}
}
