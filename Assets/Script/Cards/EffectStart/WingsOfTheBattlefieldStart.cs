using Photon.Pun;
using Stat;
using System.Collections;
using UnityEngine;

public class WingsOfTheBattlefieldStart : BaseEffect
{
    protected PhotonView _pv;
    PlayerStats _stats;
    float speed = default;
    float effectTime = default;
    int _playerId;

    [PunRPC]
    public override IEnumerator CardEffectInit(int userId, float time)
    {
        _pv = GetComponent<PhotonView>();
        base.CardEffectInit(userId);
        _stats = Managers.game.RemoteTargetFinder(userId).GetComponent<PlayerStats>();
        _playerId = userId;
        speed = 2.0f;
        effectTime = 1.1f;

        _stats.speed += speed;
        yield return new WaitForSeconds(effectTime);
        _stats.speed -= speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PLAYER" && other.gameObject.layer == _stats.playerArea)
        {
            Debug.Log(other.gameObject.name);

            GameObject Wing = PhotonNetwork.Instantiate($"Particle/Effect_WingsoftheBattlefield", other.transform.position, Quaternion.Euler(-90, 0, 0));
            //Wing.transform.localPosition = new Vector3(0, 1.0f, 0);
            Wing.GetComponent<PhotonView>().RPC("CardEffectInit", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);

        }
    }
}
