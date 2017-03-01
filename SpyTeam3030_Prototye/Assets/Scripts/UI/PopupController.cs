using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour
{
    private static GameObject canvas = null;
    private static PopupText mPopup = null;
    
    public static void DisplayPopup(string value, Vector3 pos, Color c)
    {
        if(mPopup == null)
        {
            mPopup = Resources.Load<PopupText>("Prefabs/DamagePopup");
        }
        if(canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }

        PopupText instance = Instantiate(mPopup);
        Vector2 screenPos = Camera.main.WorldToScreenPoint(pos);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = screenPos;
        instance.SetDamageText(value, c);
    }
}
