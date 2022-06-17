using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;
public class RegisterPanel : BasePanel {

    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;
    private RegisterRequest registerRequest;

    private void Awake() {
        registerRequest = GetComponent<RegisterRequest>();

        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();

        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);

    }

    protected override void EnterAnimation() {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }

    private void OnRegisterClick() {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text)) {
            msg += "username can't be enmpty\n";
        }
        if (string.IsNullOrEmpty(passwordIF.text)) {
            msg += "\npassword can't be enmpty\n";
        }
        if (passwordIF.text != rePasswordIF.text) {
            msg += "\npassword is not same\n";
        }
        if (msg != "") {
            uiMng.ShowMessage(msg); return;
        }
        registerRequest.SendRequest(usernameIF.text, passwordIF.text);
    }
    public void OnRegisterResponse(ReturnCode returnCode) {
        if (returnCode == ReturnCode.Success) {
            uiMng.ShowMessage("register success");
        } else {
            uiMng.ShowMessage("register fail");
        }
    }

    public override void OnExit() {
        transform.DOScale(0, 0.4f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
        tweener.OnComplete(() => Close());
    }
}
