using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Vector3 position;
    public GameObject ball;

    // Start is called before the first frame update
    public void Start()
    {
        PickRandomPosition();
        SpawnBall();
    }

    public void PickRandomPosition()
	{
        int yPosition = Random.Range(100, 400);
        int zPosition = Random.Range(-400, 100);

        position = new Vector3(-1100, yPosition, zPosition);
    }

    public void SpawnBall()
	{
        Instantiate(ball, position, Quaternion.identity);
	}
}