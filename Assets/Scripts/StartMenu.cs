using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{

    /*
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _sfxButton;
    [SerializeField] private Button _musicButton;

    [SerializeField] private Sprite _sfxOnImage, _sfxOffImage;
    [SerializeField] private Sprite _musicOnImage, _musicOffImage;

    private bool _sfxOn, _musicOn;
    
     private IEnumerator Start()
    {
        _sfxOn = PlayerPrefs.GetInt("sfxOn", 1) == 1;
        _musicOn = PlayerPrefs.GetInt("musicOn", 1) == 1;
        _sfxButton.image.sprite = _sfxOn ? _sfxOnImage : _sfxOffImage;
        _musicButton.image.sprite = _musicOn ? _musicOnImage : _musicOffImage;

        _startButton.onClick.AddListener(OnStartButtonClicked);
        _sfxButton.onClick.AddListener(OnSfxButtonClicked);
        _musicButton.onClick.AddListener(OnMusicButtonClicked);

        yield return new WaitUntil(() => AudioManager.Instance.IsReady);

        AudioManager.Instance.LoopSound("nightAmbience");

        yield return null;
    }

    private void OnSfxButtonClicked()
    {
        _sfxOn = !_sfxOn;
        _sfxButton.image.sprite = _sfxOn ? _sfxOnImage : _sfxOffImage;
        PlayerPrefs.SetInt("sfxOn", _sfxOn ? 1 : 0);
    }

    private void OnMusicButtonClicked()
    {
        _musicOn = !_musicOn;
        _musicButton.image.sprite = _musicOn ? _musicOnImage : _musicOffImage;
        PlayerPrefs.SetInt("musicOn", _musicOn ? 1 : 0);
        AudioManager.Instance.LoopSound("nightAmbience");
    }
    */
    public void onStartButtonClicked()
    {
        // AudioManager.Instance.StopLoop();
        SceneManager.LoadScene(1); //"Scenes/MainScene"
    }
}

