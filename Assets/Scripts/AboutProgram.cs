using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboutProgram : MonoBehaviour
{
   public void OpenWin()
   {
        Time.timeScale = 0;
        gameObject.SetActive(true);
   }
   public void CloseWin()
   {
        Time.timeScale = 1;
        gameObject.SetActive(false);
   }
}
