using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : NetworkBehaviour
{
    [SerializeField] private TMP_Text chatText;
    [SerializeField] private Button sendButton;
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    private void SendMessage()
    {
        string inputMessage = inputField.text;
        SendMessageServerRpc(inputMessage, new ServerRpcParams());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendMessageServerRpc(string message, ServerRpcParams serverRpcParams)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        string newMessage = $"{clientId} : {message}";
        SendMessageClientRpc(newMessage);
    }

    [ClientRpc]
    private void SendMessageClientRpc(string message)
    {
        string currentText = chatText.text;
        currentText += message;
        currentText += "<br>";
        chatText.text = currentText;
    }
}
