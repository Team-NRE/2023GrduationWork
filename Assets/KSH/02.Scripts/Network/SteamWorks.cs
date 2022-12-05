using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Photon.Pun;

public class SteamWorks : MonoBehaviour
{
    [Header("로그인 유저")]
    [SerializeField]
    private string loginNickname;

    void Start()
    {
        loginNickname = SteamFriends.GetPersonaName();
        PlayerPrefs.SetString("Name", loginNickname);
        Debug.Log(loginNickname);
        Debug.Log(PlayerPrefs.GetString("Name"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
