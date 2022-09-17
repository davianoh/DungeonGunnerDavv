using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{
    void Initialise(int coinsAmount);
    GameObject GetGameObject();
}
