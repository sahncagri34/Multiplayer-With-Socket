using UnityEngine;
using System.Collections;

public class BasePanel : MonoBehaviour {
    public UIManager uiMng;
    public GameFacade facade;
    public UIPanelType uIPanelType;

    protected void PlayClickSound() {
        facade.PlayNormalSound(AudioManager.Sound_ButtonClick);
    }

    public virtual void OnEnter() {
        EnterAnimation();
    }


    public virtual void OnPause() {
        HideAnimation();
    }


    public virtual void OnResume() {
        EnterAnimation();
    }


    public virtual void OnExit() {
        HideAnimation();
        Close();
    }

    protected virtual void EnterAnimation() {
        gameObject.SetActive(true);
    }

    protected virtual void HideAnimation() {
        gameObject.SetActive(false);
    }


    protected virtual void Close() {
        uiMng.panelDict.Remove(uIPanelType);
        Destroy(this.gameObject);
    }


    protected virtual void OnCloseClick() {
        PlayClickSound();
        uiMng.PopPanel();
    }
}
