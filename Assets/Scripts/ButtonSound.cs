using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour
{
    [SerializeField] private string _nameButtonSound;

    private Button _soundButton;

    private void Awake()
    {
        _soundButton = GetComponent<Button>();
        _soundButton.onClick.AddListener(PlaySound);
    }

    private void OnDestroy()
    {
        _soundButton.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        AudioPlayer.Instance.PlaySound(_nameButtonSound);
    }
}
