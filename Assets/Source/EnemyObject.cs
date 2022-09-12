using System;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    public Action<EnemyObject> OnDestroying;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MovableObj")
        {
            OnDestroying.Invoke(this);
            Destroy(gameObject);
        }
    }
}
