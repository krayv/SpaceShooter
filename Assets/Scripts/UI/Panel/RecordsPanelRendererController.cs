using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RecordsPanelRendererController : MonoBehaviour
{

    [SerializeField] Transform containerTranfsorm;
    [SerializeField] RecordsController recordsController;
    [SerializeField] GameObject recordRowPrefab;
    [SerializeField] int maxCount = 100;

    public void Render()
    {
        List<SessionRecord> sessionRecords = recordsController.GetRecords();
        sessionRecords = sessionRecords.OrderByDescending(s => s.score).ToList();
        int iteration = 1;
        foreach (SessionRecord sessionRecord in sessionRecords)
        {
            
            GameObject newRow = Instantiate(recordRowPrefab);
            newRow.transform.parent = containerTranfsorm;
            RecordRow row = newRow.GetComponent<RecordRow>();
            row.SetInfo(sessionRecord);
            if(iteration >= maxCount)
            {
                break;
            }
            ++iteration;
            
        }
    }
}
