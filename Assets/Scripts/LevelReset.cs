using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelReset
{
    public static void Reset()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
