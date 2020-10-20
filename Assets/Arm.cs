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
    [SerializeField] private Arm otherPendulum;
    [SerializeField] [Range(0, 0.001f)]private float damping;
    [SerializeField] private bool Modifier;

    public Pendulum thisPendulum;

    private void Awake()
    {
        thisPendulum = new Pendulum();

        this.transform.RotateAround(anchor.transform.position, Vector3.right, initialAngle);

        thisPendulum.mass = massObject.GetComponent<PhysicsObject>().Mass;
        thisPendulum.length = transform.localScale.y;
        thisPendulum.angle = initialAngle * Mathf.Deg2Rad;

        Time.timeScale *= 0.3f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
         
        if (Modifier)
        {
            UpdatePendulumAngles();
            Damping();
        }

        print(otherPendulum.thisPendulum.angleVelocity * Mathf.Rad2Deg);
        transform.RotateAround(anchor.transform.position, Vector3.right, thisPendulum.angleVelocity * Mathf.Rad2Deg);
    }

    private void UpdatePendulumAngles()
    {
        float pend1a = Physics.Variables.DoublePendAngleAcc(thisPendulum, otherPendulum.thisPendulum);
        float pend2a = Physics.Variables.DoublePendleAngleAcc2(thisPendulum, otherPendulum.thisPendulum);

        thisPendulum.angleVelocity += pend1a;
        otherPendulum.thisPendulum.angleVelocity += pend2a;

        thisPendulum.angle += thisPendulum.angleVelocity;
        otherPendulum.thisPendulum.angle += otherPendulum.thisPendulum.angleVelocity;
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

        if (otherPendulum.thisPendulum.angleVelocity > 0)
        {
            otherPendulum.thisPendulum.angleVelocity -= damping;
        }
        else
        {
            otherPendulum.thisPendulum.angleVelocity += damping;
        }
    }
}
