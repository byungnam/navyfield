using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosion;
    public float lasting_time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreateExplosion() {
        for (int i = 0; i < 20; i++) {
            for (int j = 0; j < UnityEngine.Random.Range(1, 3); j++) {
                Vector3 p = transform.forward * UnityEngine.Random.Range(-70, 100) +
                transform.right * UnityEngine.Random.Range(-10, 10);
                GameObject i_explosion = Instantiate(explosion, transform.position + p, Quaternion.identity);
                float scale = UnityEngine.Random.Range(1f, 10f);
                i_explosion.transform.localScale = new Vector3(scale, scale, scale);
                Destroy(i_explosion, lasting_time);
            }
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.05f));
        }
    }
}
