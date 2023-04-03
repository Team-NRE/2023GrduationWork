using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    protected UIManager _manager;
	
	public void Init(UIManager manager)
	{
		_manager = manager;
	}
	
}
