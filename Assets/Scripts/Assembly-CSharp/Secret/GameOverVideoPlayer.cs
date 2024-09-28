using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class GameOverVideoPlayer : MonoBehaviour
{
    private void OnEnable()
    {
        this.streamingPath = Application.streamingAssetsPath;

        switch(PlayerPrefs.GetInt("gps_familyFriendly", 1))
        {
            case 0:
                this.isCensored = false;
                break;
            case 1:
                this.isCensored = true;
                break;
        }

        StartCoroutine(this.WaitForVideoPlayback());
    }

    private IEnumerator WaitForVideoPlayback()
    {
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(this.streamingPath + "/twitterannouncement", 0, 0);
        
        while (!bundleRequest.isDone)
            yield return null;
        
        AssetBundle bundle = bundleRequest.assetBundle;
        
        switch(this.isCensored)
        {
            case true:
                this.audioClip = bundle.LoadAsset<AudioClip>("twitterAudio_censored");
                break;
            case false:
                this.audioClip = bundle.LoadAsset<AudioClip>("twitterAudio_original");
                break;
        }

        this.player.clip = bundle.LoadAsset<VideoClip>("twitterVideo");
        this.player.Prepare();

        while (!this.player.isPrepared)
            yield return null;

        this.audioDevice.clip = this.audioClip;
        this.audioDevice.Play();
        this.player.Play();
        this.cover.SetActive(false);

        while (this.player.isPlaying)
            yield return null;
        
        Debug.Log("Game Quit");
        Application.Quit();
    }

    [SerializeField] private VideoPlayer player;
    [SerializeField] private AudioSource audioDevice;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject cover;
    [SerializeField] private string streamingPath;
    [SerializeField] private bool isCensored;
}
