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
        [Header ("## UI References :")]
        [SerializeField] private GameObject updateCanvas;
        [SerializeField] private Button updateButton;
        [SerializeField] private TextMeshProUGUI descriptionText;

        [Space(20f)]
        [Header ("## Settings")]
        [SerializeField] [TextArea (1,5)] string jsonDataURL;

        GameData latestGameData;

        private void Start()
        {
            StopAllCoroutines();
            updateCanvas.SetActive(false);
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
            updateButton.onClick.AddListener ( () => {
                Application.OpenURL(latestGameData.Url);
            });
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}