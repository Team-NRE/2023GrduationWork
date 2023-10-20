using Photon.Pun;
using Stat;
using System.Collections;
using UnityEngine;

public class WingsOfTheBattlefieldStart : BaseEffect
{
    float effectTime;
    float startEffect = 0.01f;

    protected PhotonView _pv;
    protected PhotonView _playerPV;

    PlayerStats pStat;

    float shieldValue = default;
    float shieldRatioPerHealth = 0.4f;


    void Start()
    {
        ///�ʱ�ȭ
        player = transform.parent.gameObject;
        pStat = player.GetComponent<PlayerStats>();
        _playerPV = player.GetComponent<PhotonView>();

        ///���� ���� �ð�
        effectTime = 4.0f;
        _speed = 2.0f;

        ///�� ī���� �� Max �� - ���� ī���� Max ��
        _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", _speed);
    }

    private void Update()
    {
        startEffect += Time.deltaTime;

        ///���� ���� ����
        if (startEffect > effectTime - 0.01f)
        {
            ///�� ī���� �� Max �� - ���� ī���� Max ��
            _playerPV.RPC("photonStatSet", RpcTarget.All, "speed", -_speed);

            //���� ī�� ����
            Destroy(gameObject);

            return;
        }
    }
}
