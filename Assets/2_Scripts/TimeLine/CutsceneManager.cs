using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    private GameObject _currentCutscene;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(GameObject cutscenePrefab)
    {
        // 컷신 인스턴스 생성
        _currentCutscene = Instantiate(cutscenePrefab);

        // Timeline 재생
        var director = _currentCutscene.GetComponent<PlayableDirector>();
        director.Play();
    }

    public void Stop()
    {
        if (_currentCutscene != null)
        {
            Destroy(_currentCutscene);
            _currentCutscene = null;
        }
    }
}
