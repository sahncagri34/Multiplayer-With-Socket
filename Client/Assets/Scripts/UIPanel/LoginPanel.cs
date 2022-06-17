﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;
public class LoginPanel : BasePanel {

    private InputField usernameIF;
    private InputField passwordIF;
    private LoginRequest loginRequest;

    private void Awake() {
        loginRequest = GetComponent<LoginRequest>();
        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();

        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }

    private void OnLoginClick() {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text)) {
            msg += "username can't be enmpty\n";
        }
        if (string.IsNullOrEmpty(passwordIF.text)) {
            msg += "password can't be enmpty\n";
        }
        if (msg != "") {
            uiMng.ShowMessage(msg); return;
        }
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    private void OnRegisterClick() {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }


    protected override void EnterAnimation() {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }
    protected override void HideAnimation() {
        transform.DOScale(0, 0.3f);
        transform.DOLocalMoveX(1000, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }

    protected override void OnCloseClick() {
        base.OnCloseClick();
        GameFacade.Instance.GetManager<ClientManager>().Close();
    }
}
