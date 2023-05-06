/// ksPark
/// 
/// 초기 오브젝트를 Pooling해주는 스크립트

using UnityEngine;

public class InitPoolingManager : MonoBehaviour
{
    /// <summary>
    /// Pooling 될 오브젝트들과 그 개수가 담긴 배열
    /// </summary>
    public poolItem[] poolList;

    private void Awake() 
    {
        for (int i=0; i<poolList.Length; i++)
        {
            poolItem nowItem = poolList[i]; // 현재 PoolItem 가져오기

            if (nowItem.obj == null) continue;  // 만약 Pool할 오브젝트가 없을 시, continue

            // 이미 풀이 되어있지 않은 오브젝트일 경우 풀링 (중복 풀링 방지)
            if (Managers.Pool.GetOriginal(nowItem.obj.name) == null)
                Managers.Pool.CreatePool(nowItem.obj, nowItem.count);
        }
    }
}
