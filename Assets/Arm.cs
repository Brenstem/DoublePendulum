using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum
{
    public float mass;
    public float length;

    public float angle;
    public float angleVelocity;

    public void Print(string name)
    {
        Debug.Log("<color=red> ---------------------------------- </color>");
        Debug.Log(name + " Mass: " + mass);
        Debug.Log(name + " Length: " + length);
        Debug.Log(name + " Angle: " + angle);
        Debug.Log(name + " Velocity: " + angleVelocity * Mathf.Rad2Deg);
        Debug.Log("<color=red> ---------------------------------- </color>");
    }
}

public class Arm : PhysicsObject
{
    [SerializeField] private float initialAngle1;
    [SerializeField] private float initialAngle2;
    [SerializeField] private GameObject massObject1;
    [SerializeField] private GameObject massObject2;
    [SerializeField] private GameObject otherArm;
    [SerializeField] [Range(0, 0.001f)] private float damping;

    private Pendulum thisPendulum;
    private Pendulum otherPendulum;
    private PhysicsObject otherArmPhysObj;
    private PhysicsObject mass1PhysObj;
    private PhysicsObject mass2PhysObj;

    private void Awake()
    {
        thisPendulum = new Pendulum();
        otherPendulum = new Pendulum();
    }

    protected override void Start()
    {
        base.Start();

        otherArmPhysObj = otherArm.GetComponent<PhysicsObject>();
        mass1PhysObj = massObject1.GetComponent<PhysicsObject>();
        mass2PhysObj = massObject2.GetComponent<PhysicsObject>();

        transform.RotateAround(anchor.transform.position, Vector3.right, initialAngle1);
        otherArm.transform.RotateAround(otherArmPhysObj.Anchor.transform.position, Vector3.right, initialAngle2);

        thisPendulum.mass = massObject1.GetComponent<PhysicsObject>().Mass;
        otherPendulum.mass = massObject2.GetComponent<PhysicsObject>().Mass;

        thisPendulum.length = transform.localScale.y * 100;
        otherPendulum.length = otherArm.transform.localScale.y * 100;

        thisPendulum.angle = initialAngle1 * Mathf.Deg2Rad;
        otherPendulum.angle = initialAngle2 * Mathf.Deg2Rad;
    }

    protected void FixedUpdate()
    {
        Vector2 pendA = CalcAccelleration();

        transform.RotateAround(anchor.transform.position, Vector3.right, thisPendulum.angleVelocity * Mathf.Rad2Deg);
        otherArm.transform.RotateAround(otherArmPhysObj.Anchor.transform.position, Vector3.right, otherPendulum.angleVelocity * Mathf.Rad2Deg);
        UpdateAllAnchors();

        UpdatePendulumAngles(pendA.x, pendA.y);
        Damping();
    }

    private void UpdateAllAnchors()
    {
        mass2PhysObj.UpdateAnchors();
        otherArmPhysObj.UpdateAnchors();
        mass1PhysObj.UpdateAnchors();
        UpdateAnchors();
        mass1PhysObj.UpdateAnchors();
        otherArmPhysObj.UpdateAnchors();
        mass2PhysObj.UpdateAnchors();
    }

    private Vector2 CalcAccelleration()
    {
        Vector2 pendA;

        pendA.x = Physics.Variables.DoublePendAngleAcc(thisPendulum, otherPendulum);
        pendA.y = Physics.Variables.DoublePendleAngleAcc2(thisPendulum, otherPendulum);

        return pendA;
    }

    private void UpdatePendulumAngles(float a, float b)
    {
        thisPendulum.angleVelocity += a;
        otherPendulum.angleVelocity += b;

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
