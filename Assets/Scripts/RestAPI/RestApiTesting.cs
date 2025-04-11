using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiTesting : MonoBehaviour
{
    private const string SERVER_URL = "https://unit5-test-default-rtdb.firebaseio.com/";

    [SerializeField] private Button getPlayersDataButton;
    [SerializeField] private Button updatePlayersDataButton;
    [SerializeField] private Button deletePlayersDataButton;
    
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TMP_InputField playerIdInputField;
    [SerializeField] private TMP_InputField playerScoreInputField;

    [SerializeField] private TMP_Text leaderboardText;

    private void Start()
    {
        getPlayersDataButton.onClick.AddListener(GetPlayersData);
        updatePlayersDataButton.onClick.AddListener(UpdatePlayersData);
        deletePlayersDataButton.onClick.AddListener(DeletePlayersData);
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

        Player player = JsonConvert.DeserializeObject<Player>(result);
        Debug.Log(player.Id);
        Debug.Log(player.Score);
        
        // Using Unity Json library
        //Player player = JsonUtility.FromJson<Player>(result);
    }
    
    public void GetAllData()
    {
        StartCoroutine(GetLeaderBoardCoroutine());
    }

    private IEnumerator GetLeaderBoardCoroutine()
    {
        string getAllDataUrl = SERVER_URL + ".json";
        UnityWebRequest request = UnityWebRequest.Get(getAllDataUrl);
        yield return request.SendWebRequest();
        
        string result = request.downloadHandler.text;
        Dictionary<string, Player> players = JsonConvert.DeserializeObject<Dictionary<string, Player>>(result);
        
        List<Player> playerList = new List<Player>();
        foreach (Player player in players.Values)
        {
            playerList.Add(player);
        }

        playerList.Sort((playerA, playerB) => playerB.Score.PlayerScore.CompareTo(playerA.Score.PlayerScore));
        
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var player in playerList)
        {
            stringBuilder.Append($"PlayerId: {player.Id}, Score: {player.Score.PlayerScore}");
            stringBuilder.Append("<br>");
        }
        leaderboardText.text = stringBuilder.ToString();
    }
    
    private IEnumerator GetAllDataCoroutine()
    {
        string getAllDataUrl = SERVER_URL + ".json";
        UnityWebRequest request = UnityWebRequest.Get(getAllDataUrl);
        yield return request.SendWebRequest();

        string result = request.downloadHandler.text;
        Debug.Log(result);

        Dictionary<string, Player> players = JsonConvert.DeserializeObject<Dictionary<string, Player>>(result);
        
        // This one is not working:
        //Dictionary<string, Player> players = JsonUtility.FromJson<Dictionary<string, Player>>(result);

        foreach (var player in players)
        {
            Debug.Log(player.Key + " : " + player.Value.Id + " : " + player.Value.Score);
        }
    }

    public void UpdatePlayersData()
    {
        string playerIdString = playerIdInputField.text;
        int.TryParse(playerIdString, out int playerId);
        
        string playerScoreString = playerScoreInputField.text;
        int.TryParse(playerScoreString, out int playerScore);
        
        string playerName = playerNameInputField.text;
        
        StartCoroutine(UpdatePlayersDataCoroutine(playerId, playerScore, playerName));
    }
    
    private IEnumerator UpdatePlayersDataCoroutine(int id, int score, string playerName)
    {
        // string updatePlayersDataUrl = $"{SERVER_URL}/{playerName}/.json";
        //
        // Player player = new Player()
        // {
        //     Id = id,
        //     Score = score,
        // };
        //
        // string json = JsonConvert.SerializeObject(player);
        // Debug.Log(json);
        //
        // UnityWebRequest request = UnityWebRequest.Put(updatePlayersDataUrl, json);
        // yield return request.SendWebRequest();
        
        Score scoreObj = new Score()
        {
            PlayerScore = score,
        };
        
        string json = JsonConvert.SerializeObject(scoreObj);
        Debug.Log(json);

        string updatePlayersDataUrl = $"{SERVER_URL}/{playerName}/Score/.json";
        UnityWebRequest request = UnityWebRequest.Put(updatePlayersDataUrl, json);
        yield return request.SendWebRequest();
    }

    public void DeletePlayersData()
    {
        string playerName = playerNameInputField.text;
        StartCoroutine(DeletePlayersDataCoroutine(playerName));
    }

    private IEnumerator DeletePlayersDataCoroutine(string playerName)
    {
        string deletePlayersDataUrl = $"{SERVER_URL}/{playerName}.json";
        
        UnityWebRequest request = UnityWebRequest.Delete(deletePlayersDataUrl);
        yield return request.SendWebRequest();
    }
    

    // private IEnumerator PostPlayer(int id, int score, string playerName)
    // {
    //     string postPlayerUrl = $"{SERVER_URL}/{playerName}/.json";
    //
    //     Score scoreObj = new Score()
    //     {
    //         PlayerScore = score,
    //     };
    //     Player player = new Player()
    //     {
    //         Id = id,
    //         Score = scoreObj,
    //     };
    //
    //     string json = JsonConvert.SerializeObject(player);
    //     Debug.Log(json);
    //
    //     UnityWebRequest request = new UnityWebRequest(postPlayerUrl, "POST");
    //     byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
    //     request.uploadHandler = new UploadHandlerRaw(jsonToSend);
    //     request.downloadHandler = new DownloadHandlerBuffer();
    //     request.SetRequestHeader("Content-Type", "application/json");
    //     
    //     yield return request.SendWebRequest();
    // }
}
