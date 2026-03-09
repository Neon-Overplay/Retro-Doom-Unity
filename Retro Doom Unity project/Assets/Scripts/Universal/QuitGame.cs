using System;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private KeyCode quitButton = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKey(quitButton))
        {
            QuitApplication();
        }
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
