using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader;
    [Header("Audio")]
    [SerializeField] private AudioEventChannel _switchChannel;
    [SerializeField] private AudioClip _switchAudio;
    [Header("Camera")]
    [SerializeField] private ChapterSelectCameraController _cameraController;
    [SerializeField] private Transform[] _cameraPositions;
    [Header("Scenes")]
    [SerializeField] private SceneObject _prologueScene;
    [SerializeField] private SceneObject _chapterOneScene;

    private int _currentSelectedIndex = 0;
    private bool _canSwitch = false;

    private void Awake()
    {
        _inputReader.OnChapterSelectSwitch += SwitchSelected;
        _canSwitch = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(StartDelayed());

        IEnumerator StartDelayed()
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            transform.GetChild(0).GetComponent<Button>().Select();
            _canSwitch = true;
        }
    }

    private void SwitchSelected(int direction)
    {
        if (!_canSwitch) return;

        if (direction == 0) return;

        if (_currentSelectedIndex + direction >= 0 && _currentSelectedIndex + direction < _cameraPositions.Length)
        {
            _currentSelectedIndex += direction;
            _cameraController.SetCameraTarget(_cameraPositions[_currentSelectedIndex]);
            _switchChannel.RequestPlayAudio(_switchAudio);
        }
    }
}
