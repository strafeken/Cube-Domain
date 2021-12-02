using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Checks if scene has been loaded
/// </summary>
public class SceneLoadManager : MonoBehaviour
{
    public event Action OnSceneFinishedLoading;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene.name));
    }

    void Start()
    {
        OnSceneFinishedLoading?.Invoke();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
