using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public Transform bar1;
    public Transform bar2;
    void Start()
    {
        bar1 = transform.Find("Bar");
        bar2 = transform.Find("Other");
    }

    // Update is called once per frame
    void Update()
    {
        //ensure energy bar has a refence
        if(bar2 == null)
        {
            bar2 = transform.Find("Other");
        }
    }
    //methods to set size of health and energy bars according to player object values
    public void SetHealthSize(float sizeNormalized)
    {
        bar1.localScale = new Vector3(sizeNormalized, 1f);
    }
    public void SetEnergySize(float sizeNormalized)
    {
        bar2.localScale = new Vector3(sizeNormalized, 1f);
    }
}
