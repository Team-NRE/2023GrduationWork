using System.Collections.Generic;
using UnityEngine;
using Stat;
using Define;

public abstract class ObjectController : MonoBehaviour
{
    //외부 namespace Stat 참조
    public ObjectStats _oStats { get; set; }

    //외부 namespace Define의 State 참조
    public ObjectAction action { get; set; }

    //모든 오브젝트의 Transform이 담긴 배열
    public List<Transform> _allEnemyTransforms;

    private void OnEnable() 
    {
        AddAllEnemyObjectTransform();
    }

    protected void ExecuteObjectAction()
    {
        switch (action)
        {
            case ObjectAction.Attack:
                Attack();
                break;
            case ObjectAction.Death:
                Death();
                break;
            case ObjectAction.Move:
                Move();
                break;
            case ObjectAction.Summon:
                Summon();
                break;
            case ObjectAction.None:
                break;
        }
    }

    protected virtual void Attack() { }
    protected virtual void Death() { }
    protected virtual void Move() { }
    protected virtual void Summon() { }

    private void AddAllEnemyObjectTransform()
    {
        if (_allEnemyTransforms.Count > 0) return;

        var _enemyLayer = (gameObject.layer.Equals(Layer.Human) ? Layer.Cyborg : Layer.Human);
        
        var _allTransform = FindObjectsOfType(typeof(Transform)) as Transform[];

        for (int i=0; i<_allTransform.Length; i++)
        {
            if (_allTransform[i].gameObject.layer == 1 << (int)_enemyLayer)
            {
                _allEnemyTransforms.Add(_allTransform[i]);
            }
        }
    }
}