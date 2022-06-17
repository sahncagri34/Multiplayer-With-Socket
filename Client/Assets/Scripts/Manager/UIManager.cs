using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public enum UIPanelType
{
    None,
    Message,
    Start,
    Login,
    Register,
    RoomList,
    Room,
    Game
}

public class UIManager : BaseManager
{

    private Transform canvasTransform;
    private Transform CanvasTransform {
        get {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }
    private Dictionary<UIPanelType, string> panelPathDict = new Dictionary<UIPanelType, string>();
    public Dictionary<UIPanelType, BasePanel> panelDict = new Dictionary<UIPanelType, BasePanel>();
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();
    private MessagePanel msgPanel;
    private UIPanelType panelTypeToPush = UIPanelType.None;
    private UIPanelType panelTypeToSetActive = UIPanelType.None;

    public UserData ud1;
    public UserData ud2;

    public string uiPanelPathPrefix = "UIPanel/";


    public UIManager(GameFacade facade) : base(facade)
    {
        foreach (var item in Enum.GetValues(typeof(UIPanelType)))
        {
            panelPathDict.Add((UIPanelType)item, uiPanelPathPrefix + item + "Panel");
        }
    }
    public override void OnInit()
    {
        msgPanel = NewPanel<MessagePanel>();
    }

    public override void Update()
    {
        if (panelTypeToPush != UIPanelType.None)
        {
            PushPanel(panelTypeToPush);
            panelTypeToPush = UIPanelType.None;
        }
    }



    public P NewPanel<P>() where P : BasePanel
    {
        P p = GetPanel<P>();
        p.OnEnter();
        return p;
    }


    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }
    public P PushPanel<P>() where P : BasePanel
    {
        return (P)PushPanel(ScriptExtends.TypeToEnum<UIPanelType, P>("Panel"));
    }
    public void PushPanelAsync(UIPanelType panelType)
    {
        panelTypeToPush = panelType;
    }


    public void PopPanel()
    {
        if (panelStack != null && panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Pop();
            topPanel.OnExit();
            if (panelStack.Count > 0)
            {
                BasePanel topPanel2 = panelStack.Peek();
                topPanel2.OnResume();
            }
        }
        else
        {
            Debug.LogError("");
        }
    }

    public T GetPanel<T>() where T : BasePanel
    {
        return (T)GetPanel(ScriptExtends.TypeToEnum<UIPanelType, T>("Panel"));
    }

    
    private BasePanel GetPanel(UIPanelType panelType)
    {
        BasePanel panel = panelDict.TryGet(panelType);
        if (panel == null)
        {
            string path = panelPathDict[panelType];
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            instPanel.transform.SetParent(CanvasTransform, false);
            instPanel.GetComponent<BasePanel>().uiMng = this;
            instPanel.GetComponent<BasePanel>().facade = facade;
            instPanel.GetComponent<BasePanel>().uIPanelType = panelType;
            panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
            return instPanel.GetComponent<BasePanel>();
        }
        else
        {
            return panel;
        }
    }

    public void ShowMessage(string msg)
    {
        Debug.Log("ShowMessage: " + msg);
        GetPanel<MessagePanel>().ShowMessage(msg);
    }
}
