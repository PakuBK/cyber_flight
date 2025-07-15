using UnityEngine;

namespace CF.Player {
public class BoosterController : MonoBehaviour
{
    public GameObject BoosterLeft;
    public GameObject BoosterRight;

    public GameObject Player;

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = Player.transform.position;
    }

    public void StartBooster()
    {
        BoosterLeft.GetComponent<ParticleSystem>().Play();
        BoosterRight.GetComponent<ParticleSystem>().Play();
    }

    public void StopBooster()
    {
        BoosterLeft.GetComponent<ParticleSystem>().Stop();
        BoosterRight.GetComponent<ParticleSystem>().Stop();
    }
}
}