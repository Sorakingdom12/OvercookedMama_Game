using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extintor : MonoBehaviour
{
    private Vector3 relativePos;
    private ParticleSystem ps;
    public AudioManager au;
    private bool playing;
    // Start is called before the first frame update
    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (relativePosRaw != new Vector3(0, 0, 0))
        {
            relativePos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        }
        relativePos.Normalize();

        if (transform.parent.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) { ps.Play(); }
        if (ps.isPlaying && Input.GetKeyUp(KeyCode.E)) { ps.Stop(); }

        //si mi padre es el player y me activa -> colisiono
        if (transform.parent.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            if (!playing)
            {
                au.Play("Extintor");
                playing = true;
            }

            Ray raig = new Ray(transform.position, relativePos); //segon parametre es la direccio del raig
            RaycastHit colisio;
            Debug.DrawRay(transform.position, relativePos * 9.6f, Color.red);
            if (Physics.Raycast(raig, out colisio, 9.6f))
            {
                if (colisio.collider.tag.Equals("fogon") && colisio.collider.GetComponent<Fogon>().onFire())
                {
                    colisio.collider.GetComponent<Fogon>().extinguishFire();
                }
                else if (colisio.collider.tag.Equals("Horno") && colisio.collider.GetComponent<Oven>().onFire())
                {
                    colisio.collider.GetComponent<Oven>().extinguishFire();
                }
            }
        }
        else
        {
            au.Stop("Extintor");
            playing = false;
        }
    }
}
