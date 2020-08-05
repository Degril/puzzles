using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameLoader : UtilMonoBehaviour
{
    public GameLoaderData GameLoaderData;

    public void Init(GameLoaderData gameLoaderData)
    {
        GameLoaderData = gameLoaderData;
    }
}
