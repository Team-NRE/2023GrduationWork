using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManagerForTest : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.GameVersion = "Test";
        DontDestroyOnLoad(gameObject);
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        Debug.Log("서버 연결 시도");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("마스터 클라이언트에 연결");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 연결 완료");
        InitailizeRoom();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("게임 룸 생성");
    }

    private void InitailizeRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 6;
        ro.IsVisible = true;
        ro.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("test", ro, TypedLobby.Default);
        Debug.Log($"새로운 방 초기화");
        //SceneManager.LoadScene(""); //초기버전이라 씬 전환이 없음
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log($"방 접속 실패 : {returnCode}, {message}");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방 접속 성공");
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate("Player_net", new Vector3(0, 10, 0), Quaternion.identity);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("새로운 플레이어 입장");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log($"방 입장에 실패");
        //PhotonNetwork.LeaveRoom();
    }
}
