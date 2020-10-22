using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum
{
    public float mass;
    public float length;

    public float angle;
    public float angleVelocity;
}

public class Arm : PhysicsObject
{
    [SerializeField] private float initialAngle;
    [SerializeField] private GameObject massObject;
    [SerializeField] private PhysicsObject otherArm;
    [SerializeField] [Range(0, 0.001f)]private float damping;

    private Pendulum thisPendulum;
    private Pendulum otherPendulum;

    private void Awake()
    {
        thisPendulum = new Pendulum();

        this.transform.RotateAround(anchor.transform.position, Vector3.right, initialAngle);

        thisPendulum.mass = massObject.GetComponent<PhysicsObject>().Mass;
        thisPendulum.length = transform.localScale.y;
        thisPendulum.angle = initialAngle * Mathf.Deg2Rad;
    }

    private void Start()
    {
        otherPendulum = otherArm.GetComponent<SecondArm>().thisPendulum;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
         
        UpdatePendulumAngles();
        Damping();

        print(Mathf.Clamp(otherPendulum.angleVelocity * Mathf.Rad2Deg, 2 * -Mathf.PI, 2 * Mathf.PI));
        
        transform.RotateAround(anchor.transform.position, Vector3.right, Mathf.Clamp(thisPendulum.angleVelocity * Mathf.Rad2Deg, -2 * Mathf.PI, 2 * Mathf.PI));
    }

    private void UpdatePendulumAngles()
    {
        float pend1a = Physics.Variables.DoublePendAngleAcc(thisPendulum, otherPendulum);
        float pend2a = Physics.Variables.DoublePendleAngleAcc2(thisPendulum, otherPendulum);

        thisPendulum.angleVelocity += pend1a;
        otherPendulum.angleVelocity += pend2a;


        thisPendulum.angle += thisPendulum.angleVelocity;
        otherPendulum.angle += otherPendulum.angleVelocity;
    }

    private void Damping()
    {
        if (thisPendulum.angleVelocity > 0)
        {
            thisPendulum.angleVelocity -= damping;
        }
        else
        {
            thisPendulum.angleVelocity += damping;
        }

        if (otherPendulum.angleVelocity > 0)
        {
            otherPendulum.angleVelocity -= damping;
        }
        else
        {
            otherPendulum.angleVelocity += damping;
        }
    }
}
