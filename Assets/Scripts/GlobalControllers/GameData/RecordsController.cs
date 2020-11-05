using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RecordsController : MonoBehaviour
{
    [SerializeField] private GameStats gameStats;
    [SerializeField] private RecordsPanelRendererController panelRendererController;

    private List<SessionRecord> records = new List<SessionRecord>();
    private string filename;

    private void Awake()
    {
        filename = Path.Combine(Application.persistentDataPath, "records.dat");
        gameStats = GetComponent<GameStats>();
    }

    private void Start()
    {
        Load();
    }
    private void Load()
    {
        if (!File.Exists(filename))
        {
            return;
        }
        Dictionary<string, object> gamestate;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(filename, FileMode.Open);
        gamestate = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();

        records = (List<SessionRecord>)gamestate["records"];
        if(panelRendererController != null)
        {
            panelRendererController.Render();
        }
    }

    private void AddRecord()
    {
        SessionRecord sessionRecord = new SessionRecord(gameStats);
        records.Add(sessionRecord);
    }
    public void Save()
    {
        AddRecord();
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        gamestate.Add("records", records);
        FileStream stream = File.Create(filename);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);
        stream.Close();
    }

    public List<SessionRecord> GetRecords()
    {
        return records;
    }
}
