using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public Vector2 playerRespawnPoint;
    public Transform cameraTargetOnReset;
    public bool isDashUnlocked = false;

    public GameData()
    {
        deathCount = 0;
        playerRespawnPoint = Vector2.zero;
        cameraTargetOnReset = null;
        isDashUnlocked = false;
    }
}
