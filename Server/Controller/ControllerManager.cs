using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;
namespace GameServer.Controller {
    public class ControllerManager {
        public Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        public Servers.Server server;

        public ControllerManager(Servers.Server server) {
            this.server = server;
            InitController();
        }
        
        void InitController() {
            foreach (var item in Enum.GetValues(typeof(RequestCode))) {
                if ((RequestCode)item!=RequestCode.None) {
                    controllerDict.Add((RequestCode)item,
                        (BaseController)Activator.CreateInstance(
                            Type.GetType("GameServer.Controller." + item.ToString() + "Controller"),
                            new object[] { this }
                        )
                    );
                }
            }
        }

        public C GetController<C>()where C : BaseController {
            try {
                string typeName = typeof(C).ToString();
                string valueStr = typeName.Substring(0, typeName.LastIndexOf("Controller"));
                valueStr = valueStr.Substring(valueStr.LastIndexOf('.')+1);
                return (C)controllerDict[(RequestCode)Enum.Parse(typeof(RequestCode), valueStr)];
            } catch {
                return null;
            }
        }


     
        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client) {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false) {
                Console.WriteLine("Can't found " + requestCode + "Controller"); return;
            }

            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null) {
                Console.WriteLine("Can't found" + controller.GetType() + "a method name called : " + methodName + "]"); return;
            }

            object[] parameters = new object[] { data, client };
            object o = mi.Invoke(controller, parameters);
            if (o == null || string.IsNullOrEmpty(o as string)) {
                return;
            }

            server.SendResponse(client, actionCode, o as string);
        }

    }
}
