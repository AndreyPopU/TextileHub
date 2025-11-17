using UnityEngine;

public class FloatUp : MonoBehaviour
{
    public float speed = 2;
    public float rotSpeed = 1;
    public float height = .5f;

    private Vector3 startPos;
    private Vector3 startRot;

    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        float newRot = startRot.z + Mathf.Sin(Time.time * rotSpeed);
        transform.localRotation = Quaternion.Euler(startRot.x, startRot.y, newRot);
    }
}
