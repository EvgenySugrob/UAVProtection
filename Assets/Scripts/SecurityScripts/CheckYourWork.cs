using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System;
using System.Linq;
/*
 * This script should be attached on camera
 * in script have methods for working thief and police spawn/respawn elements and time control fragment 
 */
public class CheckYourWork : MonoBehaviour
{
    [SerializeField] GameObject Thief, policeCar;
    public GameObject realThief, timeField, statsField, realPoliceCar; //realThief and realPoliceCar must be empty in editor
    private DateTime time;
    private int countSteelObjects;
    public void SpawnThief(List<Transform> steelObjects) //steelObjects - all objects with SteelScript component
    {
        if (!realThief)
        {
            realThief = Instantiate(Thief);
            realThief.GetComponent<Walk>().steelObjects = steelObjects;
            statsField.SetActive(true); //active vision field with count of steeling object and all object with SteelScript count
            statsField.transform.GetChild(1).GetComponent<TMP_Text>().text = "0/" + steelObjects.Count;
            countSteelObjects = steelObjects.Count;
        }
    }
    public void RemoveThief() //Re-writing method which use in enother scripts 
    {
        TimerOff();
    }

    public void AddCount() //when thief stealling object -> count of steeling object + 1
    {
        string tmpStr = statsField.transform.GetChild(1).GetComponent<TMP_Text>().text;
        statsField.transform.GetChild(1).GetComponent<TMP_Text>().text = (int.Parse(string.Concat(tmpStr.TakeWhile((chr) => !(chr == '/')))) + 1).ToString() + "/" + countSteelObjects;
    }

    /*Ниже представлены скрипты для таймера при обнаружении вора*/
    public void TimerOn() //when thief was detected instantiate this method
    {
        StopAllCoroutines();
        time = new DateTime(2022, 9, 11, 0, 3, 0);
        timeField.SetActive(true);
        timeField.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = time.Minute.ToString() + ":0" + time.Second.ToString();
        StartCoroutine(Timer());
    }
    public void TimerOff() //when thief steal all objects or timer was gone to 0 instantiate this method
    {
        StopCoroutine(Timer());
        time = new DateTime(2022, 9, 11, 0, 3, 0);
        timeField.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = time.Minute.ToString() + ":0" + time.Second.ToString();
        timeField.SetActive(false);
        realThief.GetComponent<Walk>().EndOfCheck();
        Destroy(realPoliceCar);
    }
    public void TimeCheck(GameObject escapeZone) //after each theft, the thief checks if it is time for him to escape
    {
        float distance = Vector3.Distance(escapeZone.transform.position, realThief.transform.position);
        if (time.Minute == 0 && time.Second <= (distance / realThief.GetComponent<NavMeshAgent>().speed))
            realThief.GetComponent<Walk>().EscapeThief();
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        if (time.Minute == 0 && time.Second == 0)
        {
            TimerOff();
        }
        else
        {
            if (time.Minute == 0 && time.Second == 10)
                realPoliceCar = Instantiate(policeCar);
            time = time.AddSeconds(-1);
            string checkTime = time.Second.ToString();
            if (time.Second <= 9)
                checkTime = "0" + time.Second;
            timeField.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = time.Minute.ToString() + ":" + checkTime;
            StartCoroutine(Timer());
        }
    }
}
