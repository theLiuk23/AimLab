using UnityEngine;

public class Shooter : MonoBehaviour
{
    #region variables
    public Camera POV;
    public BallSpawner ballSpawner;
    public AudioSource shootSound;
    public AudioSource backgroundMusic;

    public GameObject game;

    public float fireRate = 0.01f;
    public float currentTime = 10f;
    public int count = 0;
    public int miss = 0;
    public float accuracy = 100f;
    public bool hasStarted = false;
    public float timeValue = 30f;
    public int finalPoints = 0;
    public int goalPoints = 10000;
	#endregion

	private void Start()
	{
        ballSpawner = FindObjectOfType(typeof(BallSpawner)) as BallSpawner;
        backgroundMusic.Play();
	}

	void Update()
    {
        if (timeValue > 0 && hasStarted && game.gameObject.activeSelf)
		{
            currentTime += Time.deltaTime;
            timeValue -= Time.deltaTime;
        }
        // stop!
		else if (timeValue <= 0)
		{
            finalPoints = (int)((float)accuracy / 100f * count * 100);
            timeValue = 0f;
        }

        if (Input.GetButtonDown("Fire1") && currentTime >= fireRate && timeValue > 0 && game.gameObject.activeSelf)
		{
            hasStarted = true;
            currentTime = 0f;
            Shoot();
		}

        if (Cursor.lockState == CursorLockMode.None)
		{
            backgroundMusic.UnPause();
        }
        else if (Cursor.lockState == CursorLockMode.Locked)
		{
            backgroundMusic.Pause();
		}
    }

    public void Shoot()
	{
        shootSound.Play();

        RaycastHit rayCast;
        if(Physics.Raycast(POV.transform.position, POV.transform.forward, out rayCast))
		{
            string target = rayCast.transform.name;
            Debug.Log(target);

            //target == "Ball(Clone)"
            if (rayCast.transform.gameObject.tag == "Ball")
			{
                count++;

                Destroy(GameObject.FindGameObjectWithTag("Ball"));
                ballSpawner.PickRandomPosition();
                ballSpawner.SpawnBall();
			}
			else
			{
                miss++;
			}

            if (count+miss != 0)
			{
                accuracy = ((float)count / (float)(count + miss)) * 100f;
            }
		}
	}
}