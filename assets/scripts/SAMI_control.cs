using UnityEngine;
using System.Collections;

public class SAMI_control : MonoBehaviour {

	public float moveSpeed;
	public float rotateSpeed;
	public float jumpHeight;
	public bool isGrounded;
	private CharacterController cControl;
	private bool cam;
	public GameObject Cam;


	// Use this for initialization
	void Start () {
		cControl = GetComponent<CharacterController> ();
		GetComponent<Animation>().CrossFade("SAMI_IDLE");

	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs (Input.GetAxisRaw ("Vertical")) > 0.2 || Mathf.Abs (Input.GetAxisRaw("Horizontal")) > 0.2 ) {
			GetComponent<Animation> ().CrossFade ("SAMI_RUN");
			transform.Rotate(0, Input.GetAxisRaw("Horizontal") * rotateSpeed, 0);
			cControl.SimpleMove(transform.TransformDirection(Vector3.forward) * moveSpeed * (Input.GetAxisRaw("Vertical")));

		} else {
			GetComponent<Animation>().CrossFade("SAMI_IDLE");
		}
			
	
	}
}
