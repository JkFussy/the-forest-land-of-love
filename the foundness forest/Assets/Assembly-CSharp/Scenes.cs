using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void level1(int _SceneLoader)
    {
        SceneManager.LoadScene(_SceneLoader);
    }
}
