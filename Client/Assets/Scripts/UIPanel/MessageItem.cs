using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageItem : MonoBehaviour
{
    public Text messageText;
    public void DisplayData(string data)
    {
        messageText.text = data;
    }
}
