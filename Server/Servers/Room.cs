﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Threading;
using GameServer.Controller;
using GameServer.Model;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    public class Room
    {
        private const int MAX_HP = 100;
        private List<Client> clientList = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;
        public RoomController roomController;

        public Room(RoomController roomController) {
            this.roomController = roomController;
        }

        public bool IsWaitingJoin() {
            return state == RoomState.WaitingJoin;
        }
        public void AddClient(Client client) {
            clientList.Add(client);
            client.room = this;
            if (clientList.Count >= 2) {
                state = RoomState.WaitingBattle;
            }
        }
        public void RemoveClient(Client client) {
            client.room = null;
            clientList.Remove(client);

            if (clientList.Count >= 2) {
                state = RoomState.WaitingBattle;
            } else {
                state = RoomState.WaitingJoin;
            }
        }
        public string GetHouseOwnerData() {
            return clientList[0].GetUserData();
        }

        public int GetId() {
            if (clientList.Count > 0) {
                return clientList[0].GetUserId();
            }
            return -1;
        }

       
        public String GetRoomData() {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientList) {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0) {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        
        public void BroadcastMessage(ActionCode actionCode, Client excludeClient = null, string data = "r") {
            foreach (Client client in clientList) {
                if (client != excludeClient) {
                    roomController.server.SendResponse(client, actionCode, data);
                }
            }
        }
       
        public bool IsHouseOwner(Client client) {
            return client == clientList[0];
        }
        public void QuitRoom(Client client) {
            if (client == clientList[0]) {
                Close();
            } else
                clientList.Remove(client);
        }
        public void Close() {
            foreach (Client client in clientList) {
                client.room = null;
            }
            roomController.roomList.Remove(this);
        }

  
        public void StartGame() {
            int maxHp = roomController.controllerManager.GetController<GameController>().maxHp;
            for (int i = 0; i < clientList.Count; i++) {
                clientList[i].player = new Player(maxHp,(RoleType)i);
            }

            BroadcastMessage(ActionCode.StartGame,null, ((int)ReturnCode.Success).ToString());
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--) {
                BroadcastMessage( ActionCode.ShowTimer, null, i.ToString());
                Thread.Sleep(1000);
            }
            BroadcastMessage(ActionCode.StartPlay);
        }

        
        public void TakeDamage(RoleType rt,int damage, Client excludeClient) {
            foreach (Client client in clientList) {
                if (client.player.roleType == rt) {
                    client.player.TakeDamage(damage);
                    if (client.player.IsDie()) {
                        EndGame();
                    }
                }
            }
        }


        private void EndGame() {
            foreach (Client client in clientList) {
                if (client.player.IsDie()) {
                    client.UpdateResult(false);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                } else {
                    client.UpdateResult(true);
                    client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                }
            }
            Close();
        }
    }
}
