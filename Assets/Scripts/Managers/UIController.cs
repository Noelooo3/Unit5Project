using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button startAsHostButton;
    [SerializeField] private Button startAsServerButton;
    [SerializeField] private Button startAsClientButton;

    private void Start()
    {
        startAsHostButton.onClick.AddListener(StartAsHost);
        startAsServerButton.onClick.AddListener(StartAsServer);
        startAsClientButton.onClick.AddListener(StartAsClient);
    }

    public void StartAsHost()
    {
        NetworkManager.Singleton.StartHost();
        startAsHostButton.gameObject.SetActive(false);
        startAsServerButton.gameObject.SetActive(false);
        startAsClientButton.gameObject.SetActive(false);
    }

    public void StartAsServer()
    {
        NetworkManager.Singleton.StartServer();
        startAsHostButton.gameObject.SetActive(false);
        startAsServerButton.gameObject.SetActive(false);
        startAsClientButton.gameObject.SetActive(false);
    }

    public void StartAsClient()
    {
        NetworkManager.Singleton.StartClient();
        startAsHostButton.gameObject.SetActive(false);
        startAsServerButton.gameObject.SetActive(false);
        startAsClientButton.gameObject.SetActive(false);
    }
}

