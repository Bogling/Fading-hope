using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float Damage;
    [SerializeField] private float Speed;
    [SerializeField] private GameObject impactObject;

    public void Launch(Vector3 direction) {
        GetComponent<Rigidbody>().AddForce(direction * Speed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.GetComponent<IDamageable>() != null) {
            other.gameObject.GetComponent<IDamageable>().DealDamage(Damage);
        }
        Debug.Log("Collision");
        var impObj = Instantiate(impactObject, transform.position, transform.rotation);
        impObj.SetActive(true);
        Destroy(gameObject);
    }
}
