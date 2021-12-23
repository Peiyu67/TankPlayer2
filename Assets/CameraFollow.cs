using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public Transform targetTr;
	public float dist = 10f;
	public float height = 3.0f;
	public float dampTrace = 20.0f;

	public Transform tr;
	// Use this for initialization
	void Start()
	{
		targetTr = GameObject.Find("TankPlayer(Clone)").transform;
		tr = GetComponent<Transform>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		tr.position = Vector3.Lerp(tr.position, targetTr.position - (targetTr.forward * dist) + (Vector3.up * height), Time.deltaTime * dampTrace);
		tr.LookAt(targetTr.position);
	}
}
