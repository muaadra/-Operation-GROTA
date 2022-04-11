using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// this handles the money animation for the harvesters or anything that makes 
/// money.
/// </summary>
public class MoneyAnimi : MonoBehaviour
{
    public Animator animator;
    public TextMeshPro text;

   
    /// <summary>
    /// show the animation
    /// </summary>
    /// <param name="amount"></param> the amount to show on the animation
    public void showMoneyAnimi(int amount) {
        text.text = "+ $" + amount;
        gameObject.SetActive(true);
        animator.SetTrigger("Play");
    }

    public void hideMoneyAnimi() {
       gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(LevelManager.manager.mainCam.transform.position);

    }
}
