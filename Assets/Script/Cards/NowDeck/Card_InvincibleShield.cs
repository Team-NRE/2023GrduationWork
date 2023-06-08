using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_InvincibleShield : UI_Card
{
    Transform _Player = null;
    LayerMask _layer = default;
    private bool isShieldOn = false;
    public override void Init()
    {
        _cost = 3;
        _rangeType = "None";
        _rangeScale = 6.0f;

        _CastingTime = 0.3f;
        _effectTime = 1.1f;
    }

    public override void InitCard()
    {
        Debug.Log($"{this.gameObject.name} is called");
        Debug.Log($"마나 {_cost} 사용 ");
        Debug.Log($"{_rangeScale}내에 팀원들 버프");
    }

    public override void UpdateInit()
    {
        if (_Player != null && _layer != default && !isShieldOn){
            ShieldOn(_Player, _layer);
            isShieldOn = true; // 호출 상태를 true로 변경
        }
    }

    public override GameObject cardEffect(Transform Ground = null, Transform Player = null, LayerMask layer = default)
    {
        _effectObject = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield", Player);
        _Player = Player;
        _layer = layer;

        return _effectObject;
    }

    public void ShieldOn(Transform Player, LayerMask layer)
    {
        Collider[] cols = Physics.OverlapSphere(Player.position, _rangeScale, layer);
        foreach (Collider col in cols)
        {
            //col.transform -> Police, 미니언
            GameObject shield = Managers.Resource.Instantiate($"Particle/Effect_InvincibleShield_1", col.transform);

            Destroy(shield, 3.0f);
        }
    }


    public override void DestroyCard(GameObject Particle = null, float delay = default)
    {
        Destroy(this.gameObject, delay);
    }
}
