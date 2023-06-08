using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using static UnityEngine.UI.Image;

public class PoolManager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}";

            for (int i = 0; i < count; i++)
            {
                Push(Create());
            }
        }

        public void NetInit(GameObject original, int count = 5)
        {
			Original = original;
			Root = new GameObject().transform;
			Root.name = $"{original.name}";

			for (int i = 0; i < count; i++)
			{
				Push(NetCreate());
			}
		}

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        Poolable NetCreate()
        {
            GameObject go = PhotonNetwork.Instantiate(Original.name, Original.transform.position, Original.transform.rotation);
			go.name = Original.name;
			return go.GetOrAddComponent<Poolable>();
		}

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;
            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            poolable.IsUsing = true;

            return poolable;
        }

        public Poolable NetPop(Transform parent)
        {
			Poolable poolable;
			if (_poolStack.Count > 0)
				poolable = _poolStack.Pop();
			else
				poolable = NetCreate();

			poolable.gameObject.SetActive(true);

			poolable.IsUsing = true;

			return poolable;
		}
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Dictionary<string, Pool> _netPool = new Dictionary<string, Pool>();
    Transform _root;    

    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        //Poolable의 push,pop구조 짜기 
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        //게임 오브젝트의 이름, 해당하는 pool
        _pool.Add(original.name, pool);
    }

    public void NetCreatePool(GameObject original, int count = 5)
    {
		//Poolable의 push,pop구조 짜기 
		Pool pool = new Pool();
		pool.NetInit(original, count);
		pool.Root.parent = _root;

		//게임 오브젝트의 이름, 해당하는 pool
		_pool.Add(original.name, pool);
	}

    public void CreatePool(string original, int count = 5)
    {
        GameObject obj = Managers.Resource.Load<GameObject>($"Prefabs/Projectile/{original}");
        CreatePool(obj, count);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if (_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pool.ContainsKey(original.name) == false)
            CreatePool(original);

        return _pool[original.name].Pop(parent);
    }

    public Poolable NetPop(GameObject original, Transform parent = null)
    {
		if (_pool.ContainsKey(original.name) == false)
			CreatePool(original);

		return _pool[original.name].NetPop(parent);
	}

    public Poolable Pop(string original, Transform parent = null)
    {
        GameObject obj = Managers.Resource.Load<GameObject>($"Prefabs/Reference/AI/Minion/{original}");
        return Pop(obj, parent);
    }

    public Poolable NetPop(GameObject origin, Transform tr, Transform parent = null)
    {
        GameObject obj = PhotonNetwork.Instantiate(origin.name, tr.position, tr.rotation);
		return Pop(obj, parent);
	}
    public Poolable NetPop(string name, Transform tr, Transform parent = null)
    {
        GameObject obj = PhotonNetwork.Instantiate(name, tr.position, tr.rotation);
		return Pop(obj, parent);
	}

    public GameObject GetOriginal(string name)
    {
        if (_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }

    public void Clear()
    {
        foreach (Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }


    //투사체 Pop
    public void Projectile_Pool(string ProjName, Vector3 _shooter = default, Transform _target = null, float bulletSpeed = default, float damage = default, Transform parent = null)
    {
        //Prefab 찾아주기
        GameObject GetObject = GetOriginal(ProjName);

        //못 찾았으면 함수 종료
        if (GetObject == null) return;
 
        Pop(GetObject, parent).GetComponent<Poolable>().Proj_Target_Init(_shooter, _target, bulletSpeed, damage);
    }
}
