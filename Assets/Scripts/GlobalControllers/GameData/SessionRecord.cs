using System;
[Serializable]
public class SessionRecord
{
    public string prefabShipName;
    public float sessionLenght;
    public float score;
    public DateTime sessionTime;

    public SessionRecord(GameStats stats)
    {
        prefabShipName = stats.GetCurrentPlayerShipPrefabName();
        score = stats.GetScore();
        sessionLenght = stats.GetSessionTime();
        sessionTime = DateTime.Now;
    }
}
