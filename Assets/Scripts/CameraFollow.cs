using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followGO;

    [SerializeField]
    private Vector3 setOffset = Vector3.zero;
    private Vector3 finalPos = Vector3.zero;
    [SerializeField]
    private bool shouldFollow = true;

    bool faceRight = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shouldFollow)
            FollowGOMethod();
    }

    void FollowGOMethod()
    {
        if(faceRight)
            finalPos = followGO.position + setOffset;
        else
        {
            finalPos = followGO.position + setOffset;
            finalPos.x -= setOffset.x * 2;
        }
        transform.position = Vector3.Lerp(transform.position, finalPos, 0.1f);
    }

    public void ShouldFaceRight(bool should)
    {
        faceRight = should;
    }

    public void ShouldCameraFollow(bool should)
    {
        shouldFollow = should;
    }
}
