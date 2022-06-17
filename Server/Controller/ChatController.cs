using Common;
using GameServer.Controller;
using GameServer.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Controller
{
    public  class ChatController: BaseController
    {
        public ChatController(ControllerManager cm) : base(RequestCode.Chat, cm) { }

        public string SendMessage(string data, Client client)
        {
            Room room = client.room;
            if (room != null) room.roomController.server.Broadcast(ActionCode.SendMessage, data);
            return null;
        }
    }
}
