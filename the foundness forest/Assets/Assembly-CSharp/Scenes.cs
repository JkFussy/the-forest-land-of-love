using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void level(int _SceneLoader)
    {
        NextScene.SwitchToScene(_SceneLoader);
    }
}
