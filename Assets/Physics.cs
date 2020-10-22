using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{
    private static Physics _variables;
    public static Physics Variables
    {
        get
        {
            if (_variables == null)
            {
                _variables = GameObject.FindObjectOfType<Physics>();
            }
            return _variables;
        }
    }

    [SerializeField] private float gravity;
    
    public float Gravity { get { return gravity; } }

    public float SinglePendAngleAcc(Pendulum pend)
    {
        float angleAcc;

        angleAcc = -Gravity / pend.length * Mathf.Sin(pend.angle );

        return angleAcc;
    }

    public float DoublePendAngleAcc(Pendulum pend1, Pendulum pend2)
    {
        float angleAcc;

        float num1 = -gravity * (2 * pend1.mass + pend2.mass) * Mathf.Sin(pend1.angle);
        float num2 = -pend2.mass * gravity * Mathf.Sin(pend1.angle - 2 * pend2.angle);
        float num3 = -2 * Mathf.Sin(pend1.angle - pend2.angle) * pend2.mass;
        float num4 = Mathf.Pow(pend2.angleVelocity, 2) * pend2.length + Mathf.Pow(pend1.angleVelocity, 2) * pend1.length * Mathf.Cos(pend1.angle - pend2.angle );
        float den = pend1.length * (2 * pend1.mass + pend2.mass - (pend2.mass * Mathf.Cos(2 * pend1.angle - 2 * pend2.angle)));

        angleAcc = (num1 + num2 + num3 * num4) / den;

        return angleAcc;
    }

    public float DoublePendleAngleAcc2(Pendulum pend1, Pendulum pend2)
    {
        float angleAcc;

        float num1 = 2 * Mathf.Sin(pend1.angle - pend2.angle);
        float num2 = (Mathf.Pow(pend1.angleVelocity, 2) * pend1.length * (pend1.mass + pend2.mass));
        float num3 = gravity * (pend1.mass + pend2.mass) * Mathf.Cos(pend1.angle);
        float num4 = Mathf.Pow(pend2.angleVelocity, 2) * pend2.length * pend2.mass * Mathf.Cos(pend1.angle - pend2.angle);
        float den = pend2.length * (2 * pend1.mass + pend2.mass - (pend2.mass * Mathf.Cos(2 * pend1.angle - 2 * pend2.angle)));

        angleAcc = num1 * (num2 + num3 + num4) / den;

        return angleAcc;
    }
}
