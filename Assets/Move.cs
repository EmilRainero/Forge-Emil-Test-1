using UnityEngine;
using BeardedManStudios.Network;

public class Move : NetworkedMonoBehavior
{
    private void Start()
    {
        if (IsOwner)
        {
            GetComponentInChildren<Renderer>().material.color = Color.red;
        }

    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (Input.GetKey(KeyCode.W))
            transform.position += Vector3.forward * 5.0f * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position += Vector3.back * 5.0f * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
            transform.position += Vector3.left * 5.0f * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            transform.position += Vector3.right * 5.0f * Time.deltaTime;
    }
}
