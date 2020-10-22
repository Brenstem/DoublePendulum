using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondArm : PhysicsObject
{
    [SerializeField] private float initialAngle;
    [SerializeField] private GameObject massObject;
    [SerializeField] [Range(0, 0.001f)] private float damping;

    public Pendulum thisPendulum;

    private void Awake()
    {
        thisPendulum = new Pendulum();

        this.transform.RotateAround(anchor.transform.position, Vector3.right, initialAngle);

        thisPendulum.mass = massObject.GetComponent<PhysicsObject>().Mass;
        thisPendulum.length = transform.localScale.y;
        thisPendulum.angle = initialAngle * Mathf.Deg2Rad;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        transform.RotateAround(anchor.transform.position, Vector3.right, Mathf.Clamp(thisPendulum.angleVelocity * Mathf.Rad2Deg, -2 * Mathf.PI, 2 * Mathf.PI));
    }
}
