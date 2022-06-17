using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;

public class ChatRequest : BaseRequest
{
    public InputField messageInput;
    public GameObject chatScreen;
    public GameObject toggleButtonChatScreen;
    public Transform messageItemSpawnParent;
    public MessageItem messageItemPrefab;

    string dataArrived;

    bool isActive;
    bool messageArrived;
     public override void Awake() {
        requestCode = RequestCode.Chat;
        actionCode = ActionCode.SendMessage;
        base.Awake();
    }
    private void Update()
    {
        if(messageArrived)
        {
            var messageItemInstance = Instantiate(messageItemPrefab, messageItemSpawnParent);
            messageItemInstance.DisplayData(dataArrived);
            dataArrived = "";
            messageArrived = false;
        }
    }
    public override void OnResponse(string data)
    {
        dataArrived = data;
        messageArrived = true;
    }
   public void OnToggleChatScreenButtonClicked()
    {
        isActive = !isActive;
        chatScreen.SetActive(isActive);
        toggleButtonChatScreen.SetActive(!isActive);
    }
    public override void SendRequest()
    {
        UserData ud = facade.GetUserData();
        string data = "Player "+ud.Username+" : "+messageInput.text;
        messageInput.text = "";
        base.SendRequest(data);
    }
}
