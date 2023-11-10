using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueCompleter : MonoBehaviour, IPersistentData
{
    [Header("Next Scene")]
    [SerializeField] private SceneObject _chapterSelectScene;
    [Header("Input to disable")]
    [SerializeField] private InputReader _inputReader;

    bool _isPrologueComplete = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            _isPrologueComplete = true;
            SceneManager.LoadSceneAsync(_chapterSelectScene);
        }
    }

    public void LoadData(GameData data)
    {
        _isPrologueComplete = data.isPrologueComplete;
    }

    public void SaveData(ref GameData data)
    {
        data.isPrologueComplete = _isPrologueComplete;
    }
}
