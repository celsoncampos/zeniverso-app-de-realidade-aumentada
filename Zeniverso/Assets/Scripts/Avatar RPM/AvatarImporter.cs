using UnityEngine;
using UnityEngine.Events;
using Wolf3D.ReadyPlayerMe.AvatarSDK;

public class AvatarImporter : MonoBehaviour
{
    [SerializeField] private WebView webView;

    private GameObject importedAvatar;
    public UnityEvent OnAvatarStored;

    public GameObject ImportedAvatar
    {
        get
        {
            return importedAvatar;
        }
        set
        {
            importedAvatar = value;
        }
    }

    void Start()
    {
        webView.CreateWebView();
        webView.OnAvatarCreated = ImportAvatar;
    }

    private void ImportAvatar(string url)
    {
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(url, null, StoreAvatar);
    }

    private void StoreAvatar(GameObject avatar, AvatarMetaData meta)
    {
        importedAvatar = avatar;
        // A escala original é 0.2
        importedAvatar.transform.localScale = Vector3.one * 0.9f;
        importedAvatar.SetActive(false);
        OnAvatarStored.Invoke();
    }

    public void CloseApp()
    {
        Application.Quit();
    }

}
