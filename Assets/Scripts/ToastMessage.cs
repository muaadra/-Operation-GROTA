using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class shows a timed short message to notify player of certain events
/// that do not require feedback
/// </summary>

public class ToastMessage : MonoBehaviour
{
    public Text messg;
    private float timeDuration;
    private float timer = 5;
    private bool show;

    /// <summary>
    /// show the toast
    /// </summary>
    /// <param name="message"></param> the text of the message
    /// <param name="duration"></param> how long to show in seconds
    public void showToast(string message, float duration) {
        messg.text = message;
        timeDuration = duration;
        timer = 0;
        gameObject.SetActive(true);
        show = true;

    }

    private void Update() {
        if (!show) return;

        //start timer
        timer += Time.deltaTime;

        if (timer >= timeDuration) {
            show = false;
            gameObject.SetActive(false);
        }
        
    }
}
