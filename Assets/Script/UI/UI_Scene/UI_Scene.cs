﻿using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);
    }

    public override void UpdateInit()
    {

    }
}
