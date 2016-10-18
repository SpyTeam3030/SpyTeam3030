using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupText : MonoBehaviour {

    public Animator animator;
    private Text damgeText;

	void OnEnable()
    {
        float animationLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        Destroy(gameObject, animationLength);
    }

    public void SetDamageText(string value)
    {

        damgeText = animator.GetComponent<Text>();
        damgeText.text = value;
    }
}
