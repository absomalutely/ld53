using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint;

    [SerializeField] 
    private Transform rayPoint;

    [SerializeField]
    private float rayDistance;

    [SerializeField]
    private PlayerInput player;

    [SerializeField]
    public float throwImpulse = 650;

    private GameObject grabbedObject;
    

    private int layerIndex;
    private int directionModifier = 1;

    
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("Objects");
    }

    void Update()
    {
        var hitRaycast = GetRayCastLine();
        RaycastHit2D hitInfo = Physics2D.Raycast(hitRaycast.Item1, hitRaycast.Item2, hitRaycast.Item3);

        if (player.FaceLeft) {
            directionModifier = -1;
        } else {
            directionModifier = 1;
        }


            if (Input.GetKeyUp(KeyCode.Space)) {
                Debug.Log("Space Pressed");
                if(hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex && grabbedObject == null) {
                    Debug.Log("Pickup");
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    grabbedObject.GetComponent<BoxCollider2D>().enabled = false;
                    //grabbedObject.GetComponent<Rigidbody2D>().simulated = false;
                    grabbedObject.transform.position = grabPoint.position;
                    grabbedObject.transform.SetParent(transform);
                    player.IsCarrying = true;
                } else if(grabbedObject != null){
                    Debug.Log("Drop");
                    grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    grabbedObject.GetComponent<BoxCollider2D>().enabled = true;
                    grabbedObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.GetComponent<Rigidbody2D>().velocity.x + (throwImpulse * directionModifier), player.GetComponent<Rigidbody2D>().velocity.y + throwImpulse));
                    grabbedObject.transform.SetParent(null);
                    grabbedObject = null;
                    player.IsCarrying = false;
                }
            }
        

        //Debug.DrawRay(rayPoint.position, transform.right * rayDistance);
    }

    private Tuple<Vector2, Vector2, float> GetRayCastLine() {
        //var playerOffset = player.GetComponent<SpriteRenderer>().bounds.size.x * 100;
        //if (player.FaceLeft && rayDistance < 0)
        //    rayDistance *= -1;
        //Debug.Log(player.FaceLeft + ": " + rayDistance);

        if (player.FaceLeft) {
            if(rayDistance > 0) {
                rayDistance *= -1;
                //rayPoint.transform.Rotate(0, 0, 180);
                rayPoint.transform.position = new Vector3(player.transform.position.x - player.GetComponent<SpriteRenderer>().bounds.size.x / 2 - .83f, player.transform.position.y, player.transform.position.z);

                //playerOffset *= -1;
            }
        } else {
            if(rayDistance < 0) {
                rayDistance *= -1;
                //rayPoint.transform.Rotate(180, 180, 0);
                rayPoint.transform.position = new Vector3(player.transform.position.x + player.GetComponent<SpriteRenderer>().bounds.size.x/2, player.transform.position.y, player.transform.position.z);
                //playerOffset *= -1;
            }
        }

        
        return new Tuple<Vector2, Vector2, float>(
            new Vector3(rayPoint.position.x,
                        rayPoint.position.y,
                        rayPoint.position.z),
            
            transform.right, rayDistance);
    }
    private void OnDrawGizmos() {
        var grabObjectRayCast = GetRayCastLine();
        Gizmos.DrawLine(grabObjectRayCast.Item1, new Vector3(grabObjectRayCast.Item1.x + grabObjectRayCast.Item2.x, grabObjectRayCast.Item1.y + grabObjectRayCast.Item2.y, 0));
    }
}
