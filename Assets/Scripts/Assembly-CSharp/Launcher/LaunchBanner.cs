using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class LaunchBanner : MonoBehaviour
{
    struct UpdateData
    {
        public int[] version;
        public string title;
        public string icon;
        public string buttonType;
        public string infoButtonText;
        public string url;
    }
    UpdateData latestUpdateData;
    [TextArea(1,5)] [SerializeField] string requestURL;
    [SerializeField] int[] curAppVersion;
    [SerializeField] Image bannerBorder;
    [SerializeField] TMP_Text bannerTitle;
    [SerializeField] Button bannerButton;
    [SerializeField] TMP_Text bannerInfoText;
    [SerializeField] RawImage bannerIcon;
    [SerializeField] Launcher launcherController;

    void Start()
    {
        bannerTitle.text = string.Empty;
        bannerInfoText.text = string.Empty;
        bannerButton.gameObject.SetActive(false);
        StartCoroutine(CheckForBanners());
    }

    IEnumerator CheckForBanners()
    {
        UnityWebRequest request = UnityWebRequest.Get(this.requestURL + "beta-6.json");
        request.disposeDownloadHandlerOnDispose = true;
        request.timeout = 7;
        Debug.Log("Sending banner request to server.");

        yield return request.SendWebRequest();

        if (request.isDone)
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                this.latestUpdateData = JsonUtility.FromJson<UpdateData>(request.downloadHandler.text);

                if (IsVersionOutdated())
                {
                    this.bannerBorder.color = Color.green;
                    this.bannerTitle.text = latestUpdateData.title;
                    this.bannerTitle.fontStyle = FontStyles.Underline;
                    this.bannerInfoText.text = latestUpdateData.infoButtonText;
                    this.bannerButton.gameObject.SetActive(true);
                    this.bannerIcon.color = Color.white;
                    StartCoroutine(GetBannerImage());
                }
            }
            else
                Debug.LogError("Banner request failed.");
        }
        request.Dispose();
    }

    IEnumerator GetBannerImage()
    {
        string imageURL = this.requestURL + latestUpdateData.icon;
        var request = new UnityWebRequest(imageURL, UnityWebRequest.kHttpVerbGET);
        request.downloadHandler = new DownloadHandlerTexture();

        yield return request.SendWebRequest();

        if (request.isDone)
        {
            if (request.result == UnityWebRequest.Result.Success)
                this.bannerIcon.texture = DownloadHandlerTexture.GetContent(request);
            else
                Debug.LogError("Banner image request failed.");
        }
        request.Dispose();
    }

    bool IsVersionOutdated()
    {
        if (curAppVersion == latestUpdateData.version)
            return false;
        else
        {
            if (curAppVersion[0] > latestUpdateData.version[0])
                return false;
            else if (curAppVersion[0] == latestUpdateData.version[0])
            {
                if (curAppVersion[1] > latestUpdateData.version[1])
                    return false;
                else if (curAppVersion[1] == latestUpdateData.version[1])
                {
                    if (curAppVersion[2] >= latestUpdateData.version[2])
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }    
            else
                return true;
        }
    }

    public void BannerButtonClick(int type)
    {
        switch(type)
        {
            case 2:
                launcherController.OpenLink(latestUpdateData.url);
                break;
        }
    }
}
