using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AreaListComponent : MonoBehaviour
{

    [SerializeField] private List<GameObject> markersObj;
    [SerializeField] private Text markersCountText;
    [SerializeField] private Text areaIDText;
    [SerializeField] private GameObject markerButtonPrefab;
    [SerializeField] private Transform markersBox;
    
    private int areaID;
    private int markersCount;
    private int markersQuantity;

    private bool hasZero;
    private GameObject zero;
    private string areaText = "Area";
    private readonly Color secret = new Color(220/255f, 146/255f, 1f);
    private readonly Color common = new Color(0.88f, 0.88f, 0.88f);

    public void SetDefaultArea(int areaId, Area area, List<List<int>> markerIds)
    {
        hasZero = false;
        areaIDText.text = area.name;
        markersCountText.text = $"{0}/{area.totalMarkers}";
        markersObj = new List<GameObject>();
        
        // create zero object (with question mark) 
        hasZero = true; 
        zero = Instantiate(markerButtonPrefab, Vector3.zero, Quaternion.identity, markersBox.transform);
        zero.name = "Zero";
        
        // create markers button objects
        for (int i = 0; i < area.totalMarkers; i++)
        {
            var m = Instantiate(markerButtonPrefab, Vector3.zero, Quaternion.identity, markersBox.transform);
            
            m.name = $"M_{areaId}_{i}_id={area.markers[i].id}";
            // m.GetComponentInChildren<Text>().text = $"{i+1}";
            m.GetComponentInChildren<Text>().text = $"{area.markers[i].id}";
            m.GetComponentInChildren<Text>().color = area.markers[i].type.Equals("Secret") ? secret : common; // TODO SET COLOR
            m.SetActive(false);
            
            markersObj.Add(m);
        }
    }

    public void UpdateMarkers(Area userArea, Area defaultArea)
    {
        markersCountText.text = $"{userArea.markers.Count}/{userArea.totalMarkers}";
        if (userArea.markers.Count != 0 && hasZero)
        {
            Destroy(zero);
            hasZero = false;
        }
        
        for (int i = 0; i < defaultArea.totalMarkers; i++)
        {
            if (isContains(userArea.markers, defaultArea.markers[i].id))
                markersObj[i].SetActive(true);
        }
    }

    private bool isContains(List<ServerMarker> um, int i)
    {
        List<int> umi = um.Select(marker => marker.id).ToList();
        return umi.Contains(i);
    }
}
