using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoolDownTimer 
{
   [SerializeField] private float coolDownTimeIcy;
   [SerializeField] private float coolDownTimeSnowBall;
   private float nextFireTime;

   public bool isCoolingDown => Time.time < nextFireTime;
   public void StartCoolDownIcy() => nextFireTime = Time.time + coolDownTimeIcy;
   public void StartCoolDownSnowBall() => nextFireTime = Time.time + coolDownTimeSnowBall;
}