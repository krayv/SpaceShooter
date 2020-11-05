using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour
{
    private ParticleSystem.Particle[] points;
    private float starDistanceSqr;
    private float starClipDistanceSqr;

    [SerializeField] private Color starColor;
    [SerializeField] private int starsMax = 600;
    [SerializeField] float starSize = .35f;
    [SerializeField] float starDistance = 60f;

    private int currentStarsMax;


    private void Awake()
    {
        starDistanceSqr = starDistance * starDistance;
        currentStarsMax = starsMax;
    }

    private void Start()
    {
        Messenger.AddListener<float>(GameEvents.CHANGE_QUALITY, ChangeQuality);
        StartCoroutine(GetCurrentQualityValue());
    }


    private void FixedUpdate()
    {
        if(points == null)
        {
            CreateStars();
        }
        int localCurrentStartMax = currentStarsMax;
        for (int i = 0; i < localCurrentStartMax; i++)
        {
            if((points[i].position - transform.position).sqrMagnitude > starDistanceSqr)
            {
                Vector3 position = Random.insideUnitSphere.normalized * starDistance + transform.position;
                points[i].position = new Vector3(position.x, position.y, transform.position.z);
                points[i].startSize = Random.Range(starSize / 3f, starSize);
            }
        }
        GetComponent<ParticleSystem>().SetParticles(points, points.Length);
    }

    private void CreateStars()
    {
        int localCurrentStartMax = currentStarsMax;
        points = new ParticleSystem.Particle[localCurrentStartMax];    
        for (int i = 0; i < localCurrentStartMax; i++)
        {
            Vector3 position = Random.insideUnitSphere.normalized * starDistance + transform.position;
            points[i].position = new Vector3(position.x, position.y, transform.position.z);
            points[i].startColor = starColor;
            points[i].startSize = Random.Range(starSize / 3f, starSize);
        }
    }

    private IEnumerator GetCurrentQualityValue()
    {
        yield return new WaitForEndOfFrame();
        ChangeQuality(GameObject.FindGameObjectWithTag(LayersAndTags.GAMECONTROLLER_TAG).GetComponent<SettingsController>().GetCurrentQualityValue());
    }

    private void ClearStars()
    {
        points = null;
    }

    private void ChangeQuality(float value)
    {
        ClearStars();
        currentStarsMax = (int)(starsMax * value);
    }
}
