using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUnlockItem
{
    Sprite UnlockImage { get; }
    string Name { get; }
}
