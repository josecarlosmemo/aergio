using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PickUpType
{
    Health,
    Waste,
}

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private PickUpType type;

    [SerializeField]
    private GameObject particle;

    // public Text text;

    // private void Start()
    // {
    //     text.enabled = false;
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (type)
            {
                case PickUpType.Health:
                    other.GetComponent<Health>().health += 10;
                    // text.text = "You picked up some Health!";
                    // text.enabled = true;
                    Destroy(gameObject);

                    GameObject healthPart =
                        Instantiate(particle, transform.position, transform.rotation) as GameObject;
                    Destroy(healthPart, 5);

                    break;

                case PickUpType.Waste:
                    other.GetComponent<Health>().health -= 10;
                    Destroy(gameObject);

                    GameObject wastePart =
                        Instantiate(particle, transform.position, transform.rotation) as GameObject;
                    Destroy(wastePart, 5);
                    break;

                default:
                    break;
            }
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     print(other.tag);

    //     if (other.CompareTag("Player"))
    //     {
    //         // text.enabled = false;
    //     }
    // }
}
