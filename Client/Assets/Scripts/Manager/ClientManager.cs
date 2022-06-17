using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using Common;


public class ClientManager : BaseManager {

    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

    private Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private Message msg = new Message();

    public ClientManager(GameFacade facade) : base(facade) { }

    public bool Connect() {
        try {
            clientSocket.Connect(IP, PORT);
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
            return true;
        } catch (Exception e) {
            facade.ShowMessage("Can't Connect.");
            Debug.LogError(e);
            return false;
        }
    }
    private void ReceiveCallback(IAsyncResult ar) {
        try {
            if (clientSocket == null || clientSocket.Connected == false) return;
            int count = clientSocket.EndReceive(ar);

            msg.ReadMessage(count, OnProcessDataCallback);

            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
        } catch (Exception e) {
            Debug.LogError(e);
            facade.ShowMessage("can't receive data.");
        }
    }

    private void OnProcessDataCallback(ActionCode actionCode, string data) {
        facade.HandleReponse(actionCode, data);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data) {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }

    public void Close() {
        try {
            clientSocket.Close();
        } catch (Exception e) {
            Debug.LogWarning(e);
        }

    }

    public override void OnDestroy() {
        Close();
    }
}