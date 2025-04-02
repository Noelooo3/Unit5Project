using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance => _instance;
    private static UIController _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    [SerializeField] private Button startAsHostButton;
    [SerializeField] private Button startAsClientButton;

    [SerializeField] private GameObject gameMenuPanel;
    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject gameOverPanel;
    
    [SerializeField] private TMP_InputField nameInputField;
    
    private void Start()
    {
        startAsHostButton.onClick.AddListener(StartAsHost);
        startAsClientButton.onClick.AddListener(StartAsClient);
    }

    public void StartAsHost()
    {
        NetworkManager.Singleton.StartHost();
        gameMenuPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public void StartAsClient()
    {
        NetworkManager.Singleton.StartClient();
        gameMenuPanel.SetActive(false);
        inGamePanel.SetActive(true);
    }

    public string GetName()
    {
        return nameInputField.text;
    }
}

