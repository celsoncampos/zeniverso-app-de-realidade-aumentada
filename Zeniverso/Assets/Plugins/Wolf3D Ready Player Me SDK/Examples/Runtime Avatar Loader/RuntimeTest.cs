using UnityEngine;
using Wolf3D.ReadyPlayerMe.AvatarSDK;

public class RuntimeTest : MonoBehaviour
{
    [SerializeField] private string AvatarURL = "https://d1a370nemizbjq.cloudfront.net/209a1bc2-efed-46c5-9dfd-edc8a1d9cbe4.glb";

    private void Start()
    {
        Debug.Log($"Começou a carregar o avatar. [{Time.timeSinceLevelLoad:F2}]");
        AvatarLoader avatarLoader = new AvatarLoader();
        avatarLoader.LoadAvatar(AvatarURL, OnAvatarImported, OnAvatarLoaded);
    }

    private void OnAvatarImported(GameObject avatar)
    {
        Debug.Log($"Avatar importado. [{Time.timeSinceLevelLoad:F2}]");
    }

    private void OnAvatarLoaded(GameObject avatar, AvatarMetaData metaData)
    {
        Debug.Log($"Avatar carregado. [{Time.timeSinceLevelLoad:F2}]\n\n{metaData}");
    }
}
