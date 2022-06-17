using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RequestManager : BaseManager {
    public RequestManager(GameFacade facade) : base(facade) { }

    private Dictionary<ActionCode, BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public void AddRequest(ActionCode actionCode, BaseRequest request) {
        requestDict.Add(actionCode, request);
    }
    public void RemoveRequest(ActionCode actionCode) {
        requestDict.Remove(actionCode);
    }

    public void HandleReponse(ActionCode actionCode, string data) {
        BaseRequest request = requestDict.TryGet(actionCode);
        if (request == null) {
            Debug.LogWarning("ActionCode[" + actionCode + "Request"); return;
        }
        request.OnResponse(data);
    }
}
