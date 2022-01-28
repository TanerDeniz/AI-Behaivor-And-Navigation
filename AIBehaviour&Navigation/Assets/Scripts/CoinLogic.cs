using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinLogic : MonoBehaviour
{

    AudioSource audioSource;
    Collider collider;
    MeshRenderer renderer;
    [SerializeField]
    AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 1.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (audioSource && audioClip)
                audioSource.PlayOneShot(audioClip);
            if (collider)
                collider.enabled = false;
            if (renderer)
                renderer.enabled = false;
        }
    }
}
