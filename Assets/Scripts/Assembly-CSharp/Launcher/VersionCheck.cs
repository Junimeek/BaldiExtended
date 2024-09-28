using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

// This code is based on code by Hamza Herbou: https://www.youtube.com/watch?v=z-H37N6Mjlk

namespace UpgradeSystem
{
    struct GameData
    {
        public string Description;
        public string Version;
        public string Url;
    }

    public class VersionCheck : MonoBehaviour
    {
        [Header ("UI")]
        [SerializeField] private GameObject stableCanvas;
        [SerializeField] private GameObject nightlyCanvas;
        [SerializeField] private Button updateButton;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Space(20f)]
        [Header ("Settings")]
        public bool isNightly;
        public string stableBuild;
        public string nightlyBuild;
        [SerializeField] private string curVersion;
        [TextArea(1,5)] string jsonDataURL;
        [SerializeField] [TextArea(1,5)] string stableURL;
        [SerializeField] [TextArea(1,5)] string nightlyURL;

        GameData latestGameData;

        private void Start()
        {
            StopAllCoroutines();
            this.stableCanvas.SetActive(false);
            this.nightlyCanvas.SetActive(false);

            if (this.isNightly)
                this.jsonDataURL = this.nightlyURL;
            else
                this.jsonDataURL = this.stableURL;

            if (this.isNightly)
                this.curVersion = this.nightlyBuild;
            else
                this.curVersion = this.stableBuild;

            StartCoroutine(this.CheckForUpdates());
        }

        private IEnumerator CheckForUpdates()
        {
            UnityWebRequest request = UnityWebRequest.Get (jsonDataURL);
            request.chunkedTransfer = false;
            request.disposeDownloadHandlerOnDispose = true;
            request.timeout = 60;

            yield return request.Send();

            if (request.isDone)
            {
                if (!request.isNetworkError)
                {
                    latestGameData = JsonUtility.FromJson<GameData> (request.downloadHandler.text);
                    if (!string.IsNullOrEmpty(latestGameData.Version) && curVersion != latestGameData.Version)
                    {
                        this.descriptionText.text = latestGameData.Description;
                        this.ShowPopup();
                    }
                }
            }
            request.Dispose();
        }

        private void ShowPopup()
        {
            updateButton.onClick.AddListener (() => {
                Application.OpenURL(latestGameData.Url);
            });

            if (isNightly)
                this.nightlyCanvas.SetActive(true);
            else
                this.stableCanvas.SetActive(true);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}