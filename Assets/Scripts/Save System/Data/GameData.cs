using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public Vector2 playerRespawnPoint;
    public Vector3 cameraTargetOnReset;
    public bool isPrologueComplete = false;
    public bool isDashUnlocked = false;

    public GameData()
    {
        deathCount = 0;
        playerRespawnPoint = Vector2.zero;
        cameraTargetOnReset = Vector3.zero;
        isPrologueComplete = false;
        isDashUnlocked = false;
    }
}
