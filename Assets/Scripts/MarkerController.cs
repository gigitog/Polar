using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Polar;
using UnityEngine;

[Serializable]
public class MarkerController : MonoBehaviour
{
    public static MarkerController instance;
    private bool isEmpty;
    public List<AreaSuper> areas;

    private void Awake()
    {
        isEmpty = true;
        SetInstance();
    }

    private void Start()
    {
        ServerController.instance.OnMarkers += OnMarkersRespond;
        ServerController.instance.Markers();
    }

    public List<Area> GetAreasCompressed()
    {
        var result = new List<Area>();
        foreach (var area in areas)
            result.Add(new Area()
            {
                markers = area.GetMarkersCompressed(),
                name = area.name,
                totalMarkers = area.markers.Count
            });

        return result;
    }

    public List<AreaSuper> GetAreas()
    {
        return areas;
    }

    private void OnMarkersRespond(object sender, RespondArgs args)
    {
        if (args.method != RequestMethod.Markers) return;

        var answer = JsonConvert.DeserializeObject<MarkersAnswer>(args.text);

        if (answer == null)
        {
            Debug.Log("INTERNAL ERROR. SERVER GAVE EMPTY ANSWER FOR MARKERS");
            return;
        }

        if (answer.succeeded)
        {
            Debug.Log("OK! Markers\n" + JsonConvert.SerializeObject(answer));
            areas = answer.data;
        }
        else
        {
            Debug.Log("Invalid!");
        }
    }

    private void OnDestroy()
    {
        ServerController.instance.OnMarkers -= OnMarkersRespond;
    }

    private void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}