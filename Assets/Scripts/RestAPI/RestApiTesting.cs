using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiTesting : MonoBehaviour
{
    private const string SERVER_URL = "https://unit5-test-default-rtdb.firebaseio.com/";

    [SerializeField] private Button getPlayersDataButton;
    [SerializeField] private TMP_InputField playerNameInputField;

    private void Start()
    {
        getPlayersDataButton.onClick.AddListener(GetPlayersData);
    }

    private void GetPlayersData()
    {
        StartCoroutine(GetPlayersDataCoroutine(playerNameInputField.text));
    }

    private IEnumerator GetPlayersDataCoroutine(string playerName)
    {
        string getPlayerDataUrl = $"{SERVER_URL}{playerName}/.json";
        UnityWebRequest request = UnityWebRequest.Get(getPlayerDataUrl);
        yield return request.SendWebRequest();
        
        string result = request.downloadHandler.text;
        Debug.Log(result);
    }
    
    public void GetAllData()
    {
        StartCoroutine(GetAllDataCoroutine());
    }
    
    private IEnumerator GetAllDataCoroutine()
    {
        string getAllDataUrl = SERVER_URL + ".json";
        UnityWebRequest request = UnityWebRequest.Get(getAllDataUrl);
        yield return request.SendWebRequest();

        string result = request.downloadHandler.text;
        Debug.Log(result);
    }
}
