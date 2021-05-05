using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Stats : MonoBehaviour
{
    [SerializeField]
    private Image health_Stats, stamina_Stats;
    
    public void DisplayHealthStats(float health_Value)
    {
        health_Value /= 100;
        health_Stats.fillAmount = health_Value;
    }
    
    public void DisplayStaminaStats(float stamina_Value)
    {
        stamina_Value /= 100;
        stamina_Stats.fillAmount = stamina_Value;
    }
}
