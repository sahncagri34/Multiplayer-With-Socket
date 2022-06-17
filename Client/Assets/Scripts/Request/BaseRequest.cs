using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

/// <summary>
/// Request base class, Request, global singleton
/// Mounted on the game object as a component,
/// When the game object needs to send a request to the server,
/// Call a request method of a request class mounted on it,
///
/// The request class also has a method for processing the message returned by the server,
/// Generally, a method in the Manager is called. If the method is related to the operation of the game object,
/// needs to be executed in the main thread (life cycle function), (asynchronous)
/// Asynchronous operations should be implemented in the Request class as much as possible. If the Request cannot be implemented, asynchronous operations are implemented in the Manager.
/// </summary>
public class BaseRequest : MonoBehaviour {
    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade _facade;

    protected GameFacade facade {
        get {
            if (_facade == null)
                _facade = GameFacade.Instance;
            return _facade;
        }
    }
    public virtual void Awake() {
        facade.AddRequest(actionCode, this);
    }

    protected void SendRequest(string data) {
        facade.SendRequest(requestCode, actionCode, data);
    }

    public virtual void SendRequest() {
        facade.SendRequest(requestCode, actionCode, "r");
    }
    public virtual void OnResponse(string data) { }

    public virtual void OnDestroy() {
        if (facade != null)
            facade.RemoveRequest(actionCode);
    }
}
