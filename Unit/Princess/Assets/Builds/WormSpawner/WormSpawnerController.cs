using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawnerController : MonoBehaviour
{
    public Animator animator;
    public float radio = 5f;
    public Transform spawnPosition;
    public float hardPosibility = 0.15f;
    public float mediumPosibility = 0.25f;
    public GameObject wormBuild;
    private float countDownNewSpawn = 2f;
    private bool spawning = false;

    private HealthController player;




    void Start()
    {
        player = (GameObject.FindGameObjectWithTag("Player")).GetComponent<HealthController>();
    }



    void FixedUpdate()
    {
        Debug.Log(Vector3.Distance(player.transform.position, transform.position).ToString() + " <= " + radio.ToString());

        if (Vector3.Distance(player.transform.position, transform.position) <= radio)
        {
            animator.SetBool("start", true);
            spawning = true;
        }


        if (spawning){
            countDownNewSpawn -= Time.deltaTime;
            if (countDownNewSpawn <= 0){
                countDownNewSpawn = 4f + Random.Range(0f, 2f);

                GameObject newWorm = GameObject.Instantiate(wormBuild, spawnPosition.transform.position, spawnPosition.transform.rotation);
                WormAttackController newWormAttackController = newWorm.GetComponent<WormAttackController>();
                if (newWormAttackController != null){
                    float r = Random.Range(0f, 1f);
                    if (r > (1f - hardPosibility)){
                        newWormAttackController.SetLevel(2);
                    }
                    else if (r > (1f - hardPosibility - mediumPosibility)){
                        newWormAttackController.SetLevel(1);
                    }else{
                        newWormAttackController.SetLevel(0);
                    }
                }
            }
        }
    }

    public void StartToSpawn()
    {
        spawning = true;
    }
}
