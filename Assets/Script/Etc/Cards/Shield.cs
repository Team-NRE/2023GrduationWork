using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseController
{
    //CardStats _cardStats;
    //PlayerStats _playerStat;

	public int _cost;
	public float _damage;
	public float _defence;
	public float _debuff;
	public float _buff;
	public float _range;
	public float _time;

    public override void Init()
    {
        Debug.Log($"{this.gameObject.name} is called");
    }

	public void cardEffect()
    {
        SetStat();
    }

    public override void SetStat()
    {
        
    }

    public override void LoadEffect()
    {
        
    }
}
