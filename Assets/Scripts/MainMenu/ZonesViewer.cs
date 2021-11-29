using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZonesViewer : MonoBehaviour
{
    [SerializeField] private Sprite onImage;
    [SerializeField] private Sprite offImage;
    [SerializeField] private GameObject polyObj;
    [SerializeField] private GameObject zoneObj;
    [SerializeField] private Mesh polyPref;
    [SerializeField] private Mesh polyPrefZoned;
    
    private Image image;
    
    
    private bool isOn;
    
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        
    }

    public void Swap()
    {
        if (isOn)
        {
            isOn = false;
            image.sprite = offImage;
            zoneObj.SetActive(false);
        }
        else
        {
            isOn = true;
            image.sprite = onImage;
            zoneObj.SetActive(true);
        }
    }
}
