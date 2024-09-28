using TMPro;
using UnityEngine;

public class CreditLink : MonoBehaviour
{
    public void Underline()
    {
        this.text.fontStyle = FontStyles.Underline;
    }

    public void Ununderline()
    {
        this.text.fontStyle = FontStyles.Normal;
    }

    public void OpenLink()
    {
        Application.OpenURL(this.link);
    }

    [SerializeField] [TextArea(1,5)] private string link;
    [SerializeField] private TMP_Text text;
}
