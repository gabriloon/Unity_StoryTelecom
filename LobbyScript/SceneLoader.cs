﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public CanvasGroup fadeCg;
    [Range(0.5f, 2.0f)]
    public float fadeDuration = 1.0f;

    public Dictionary<string, LoadSceneMode> loadScenes = new Dictionary<string, LoadSceneMode>
();
    void InitSceneInfo() {
        loadScenes.Add("MainScene", LoadSceneMode.Additive);
        loadScenes.Add("RoomScene", LoadSceneMode.Additive);


    }

    IEnumerator Start() {
        InitSceneInfo();
        fadeCg.alpha = 1.0f;

        foreach (var _loadScene in loadScenes) {

            yield return StartCoroutine(LoadScene(_loadScene.Key, _loadScene.Value));
        }
        StartCoroutine(Fade(0.0f));
    }


    IEnumerator LoadScene(string sceneName, LoadSceneMode mode) {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        Scene loadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(loadedScene);

    }

    IEnumerator Fade(float finalAlpha) {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("RoomScene"));
        fadeCg.blocksRaycasts = true;

        float fadeSpeed = Mathf.Abs(fadeCg.alpha - finalAlpha) / fadeDuration;


        while (!Mathf.Approximately(fadeCg.alpha, finalAlpha)) {

            fadeCg.alpha = Mathf.MoveTowards(fadeCg.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        fadeCg.blocksRaycasts = false;


        SceneManager.UnloadSceneAsync("LoadingScene1");
    }
}
