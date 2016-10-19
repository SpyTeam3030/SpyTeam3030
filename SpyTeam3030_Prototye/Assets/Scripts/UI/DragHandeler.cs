using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {


	public static GameObject itemBeingDragged;
	Vector3 startPosition;
	Transform startParent;

	public Status status;
  
	public string description;

	public float attack;
	public float hp;
    public float speed;


	#region IBeginDragHandler implementation
	public void OnBeginDrag (PointerEventData eventData)
	{
		itemBeingDragged = gameObject;
		startPosition = transform.position;
		GetComponent<CanvasGroup> ().blocksRaycasts = false;
        Color c = GetComponentInParent<Image>().color;
        c.a = 0.25f;
        GetComponentInParent<Image>().color = c;
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
        Color c = GetComponentInParent<Image>().color;
        c.a = 1.0f;
        GetComponentInParent<Image>().color = c;
		ResetInformation ();

        // RayCasting
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast (ray, out hit, 200.0f)) 
        {
            if (hit.transform.gameObject.tag == "CombatObject")
            {
                hit.transform.gameObject.GetComponent<CombatController>().AttributeChange(hp, attack, 0, speed);
                Debug.Log("Take Effect");
            }
        } 
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
