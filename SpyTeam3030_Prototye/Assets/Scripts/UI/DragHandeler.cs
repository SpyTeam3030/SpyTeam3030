using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;

	public Status status;

	public string description;

	public int attack;
	public int hp;
	public int speed;

	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}
	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		GetComponent<Transform>().position = eventData.position;
		SetInformation ();
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
		GetComponent<Transform>().position = startPosition;
		ResetInformation ();


	}

	#endregion

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
