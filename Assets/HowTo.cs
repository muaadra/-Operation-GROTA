using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowTo : MonoBehaviour
{
    public GameObject HowToWindow;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("HowTo") == 0) {
            HowToWindow.SetActive(true);
        } else {
            HowToWindow.SetActive(false);
        }
    }


    public void dontShowAgain() {
        PlayerPrefs.SetInt("HowTo", 1);
        close();
    }

    public void close() {
        HowToWindow.SetActive(false);
    }
}
