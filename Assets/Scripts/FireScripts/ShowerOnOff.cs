using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerOnOff : MonoBehaviour
{
    public List<GameObject> showerSystem;

    private void Start()
    {
        var countChild = transform.childCount;
        for (int i = 0; i < countChild; i++)
        {
            showerSystem.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ShowerOn()
    {
        foreach (GameObject showers in showerSystem)
        {
            showers.GetComponent<ParticleSystem>().Play();
        }
    }
    public void ShowerOff()
    {
        foreach (GameObject showers in showerSystem)
        {
            showers.GetComponent<ParticleSystem>().Stop();
        }
    }
}
