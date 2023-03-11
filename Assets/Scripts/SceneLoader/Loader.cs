using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Loader
{
    private class LoadingMonoBehaviour : MonoBehaviour {}

    public enum Scene
    {
        StartMenu,
        Main,
        Loading
    }

    private static Action onLoaderCallback;

    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            GameObject loading = new GameObject();
            loading.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while(!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
            return loadingAsyncOperation.progress;
        else
            return 1f;
    }

    public static void LoadCallback()
    {
        if(onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
