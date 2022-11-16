using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        DontDestroyOnLoad(gameObject);
        ConnectToServer();
    }

    public void ConnectToServer()
    {
        Debug.Log("서버에 연결을 시도합니다.");
        PhotonNetwork.ConnectUsingSettings(); // 서버연결

        if (PlayerPrefs.HasKey("Name"))
            PhotonNetwork.NickName = PlayerPrefs.GetString("Name");
        //나중에 이 부분을 로그인 아이디에서 받아오도록 해야함

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터서버와 연결되었습니다");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    //여기서 여러 방으로 들어가는 내용을 분기해줘야함
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby 함수 작동");
        base.OnJoinedLobby();
        Debug.Log("대기실에 입장하였습니다.");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방이 만들어졌습니다");
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        //3 : 3으로 한 방에 6명만 들어갈 수 있도록 룸 옵션 지정
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        
        PhotonNetwork.JoinOrCreateRoom("testRoom", roomOptions, TypedLobby.Default);
        //Lobby 입장
        #warning RoomName을 채워야합니다
        SceneManager.LoadScene("");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("방 입장에 실패하였습니다.");
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("새로운 플레이어가 입장하였습니다.");
        //base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnJoinedRoom()
    { 
        Debug.Log("룸에 접속 완료하였습니다");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("OnLeftRoom() 콜백 호출");
    }


    #region options
    //나가기 버튼을 눌렀을때 발생해야할 이벤트
    public void OnRoomExitClick()
    {
        Debug.Log("ExitButton Click");
        PhotonNetwork.LeaveRoom();
        #warning 나가기 로딩 씬이 개발되지 않았습니다
        SceneManager.LoadScene("Exit");
    }

    //RPC를 사용한 인스턴스 삭제 -> 현재 사용하지 않음
    [PunRPC]
    private void LocalDestroy()
    {
        GameObject.Destroy(photonView);
    }
    [PunRPC]
    private void RemoteDestroy(int viewID)
    {
        GameObject.Destroy(PhotonView.Find(viewID).gameObject);
    }
    #endregion
}
