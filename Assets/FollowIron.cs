using UnityEngine;

public class FollowIron : MonoBehaviour
{
    public Transform target;
    void Update() => transform.position = new Vector3(target.position.x, target.position.y, 0);
}
