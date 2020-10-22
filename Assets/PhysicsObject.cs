using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] protected GameObject anchor;
    [SerializeField] protected List<GameObject> anchors;
    [SerializeField] private bool staticObj;
    [SerializeField] protected float mass;

    private GameObject activeAnchor;
    public GameObject Anchor { get { return anchor; } }
    private bool anchored = false;
    public float Mass { get { return mass; } }
    public bool Static { get { return staticObj; } }

    protected virtual void Start()
    {
        if (anchor != null)
            activeAnchor = AnchorObject();
    }

    public void UpdateAnchors() 
    {
        if (anchored)
        {
            transform.position += anchor.transform.position - activeAnchor.transform.position;
            activeAnchor.transform.position = anchor.transform.position;
        }
    }

    private GameObject AnchorObject()
    {
        if (anchors.Count == 0) 
        {
            Debug.LogError("Object has no anchor points"); 
            return null;
        }
        
        GameObject closestAnchor = anchors[0];

        foreach (GameObject myAnchor in anchors)
        {
            if (Vector3.Distance(myAnchor.transform.position, anchor.transform.position) <
                Vector3.Distance(closestAnchor.transform.position, anchor.transform.position))
            {
                closestAnchor = myAnchor;
            }
        }

        anchored = true;
        return closestAnchor;
    }
}
