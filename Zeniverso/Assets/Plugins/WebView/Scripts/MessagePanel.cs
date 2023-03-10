using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MessagePanel : MonoBehaviour
{
    public enum MessageType
    {
        Loading,
        NetworkError,
        NotSupported
    }

    private Dictionary<MessageType, string> messageDict = new Dictionary<MessageType, string>()
    {
        {MessageType.Loading, "Carregando..." },
        {MessageType.NetworkError, "Lamentamos, a sua Internet está instável." },
        {MessageType.NotSupported, "Está aplicação somente tem suporte em dispositivos Android e iOS." }
    };

    [SerializeField] private Text messageLabel = null;
    
    // Set text message to the Message panel
    public void SetMessage(string message)
    {
        messageLabel.text = message;
    }

    public void SetMessage(MessageType type)
    {
        messageLabel.text = messageDict[type];
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    public void SetMargins(int left, int top, int right, int bottom)
    {
        RectTransform rect = transform as RectTransform;
        rect.offsetMax = new Vector2(-right, -top);
        rect.offsetMin = new Vector2(left, bottom);
    }
}
