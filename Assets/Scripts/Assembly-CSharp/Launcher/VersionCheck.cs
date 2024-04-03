using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

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
        [SerializeField] [TextArea(1,5)] string jsonDataURL;
        [SerializeField] [TextArea(1,5)] string stableURL;
        [SerializeField] [TextArea(1,5)] string nightlyURL;

        GameData latestGameData;

        private void Start()
        {
            StopAllCoroutines();
            stableCanvas.SetActive(false);
            nightlyCanvas.SetActive(false);

            if (isNightly) jsonDataURL = nightlyURL;
            else jsonDataURL = stableURL;

            StartCoroutine(CheckForUpdates());
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
                    if (!string.IsNullOrEmpty(latestGameData.Version) && !Application.version.Equals (latestGameData.Version))
                    {
                        descriptionText.text = latestGameData.Description;
                        ShowPopup();
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

            if (isNightly) nightlyCanvas.SetActive(true);
            else stableCanvas.SetActive(true);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}