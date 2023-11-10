using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isPrologueComplete = false;
    public int deathCount;
    public Vector2 playerRespawnPoint;
    public Vector3 cameraTargetOnReset;
    public SerializableDictionary<string, bool> collectibles;

    public GameData()
    {
        isPrologueComplete = false;
        deathCount = 0;
        playerRespawnPoint = Vector2.zero;
        cameraTargetOnReset = Vector3.zero;
        collectibles = new SerializableDictionary<string, bool>();
    }
}
