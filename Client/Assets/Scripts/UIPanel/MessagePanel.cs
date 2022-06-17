using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel :BasePanel {

    private Text text;
    public float showTime = 1;
    private string message = null;

    private void Update() {
        if (message != null) {
            text.CrossFadeAlpha(1, 0.2f, false);
            text.text = message;
            message = null;
            text.enabled = true;
            Invoke("Hide", showTime);
        }
    }

    public override void OnEnter() {
        base.OnEnter();
        text = GetComponent<Text>();
        text.enabled = false;
    }


    public void ShowMessage(string msg) {
        message = msg;
    }
    private void Hide() {
        text.CrossFadeAlpha(0, 1, false);
    }

}
