using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //Camera's target SET IN EDITOR
    public Transform target;
    //Camera's offset SET IN EDITOR
    public Vector3 offset;
    
	// Update is called once per frame
	void Update () {

        //Follow target using offset
        if (target)
            transform.position = Vector3.Lerp(transform.position, target.position + offset, 15 * Time.deltaTime);

        //If no target is set, find one with the tag "Player"
        else
            target = GameObject.FindWithTag("Player").transform;
	}
}
