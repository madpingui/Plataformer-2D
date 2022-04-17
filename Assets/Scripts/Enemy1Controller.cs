using UnityEngine;

public class Enemy1Controller : MonoBehaviour
{
    private bool movingToLeft;
    public GameObject[] Enemy1Parts = new GameObject[6];
    private SpriteRenderer[] RotableParts = new SpriteRenderer[6];
    private Transform sword;
    private int direction;

    public bool coward;
    private Transform player;

    void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;

        for(int i = 0;i<Enemy1Parts.Length;i++)
        {
            RotableParts[i] = Enemy1Parts[i].GetComponent<SpriteRenderer>();
        }

        sword = Enemy1Parts[3].GetComponent<Transform>();
    }

    void Start(){
        movingToLeft = true;
    }

    void Update()
    {
        if (coward)
        {
            if (transform.position.x < player.transform.position.x)
            {
                movingToLeft = true;
            }
            else
            {
                movingToLeft = false;
            }


            if (Vector3.Distance(transform.position, player.position) < 5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -3 * Time.deltaTime);
            }
        }
        else
        {
            if (transform.position.x < player.transform.position.x)
            {
                movingToLeft = false;
            }
            else
            {
                movingToLeft = true;
            }


            if (Vector3.Distance(transform.position, player.position) > 1f && Vector3.Distance(transform.position, player.position) < 5f)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 3 * Time.deltaTime);
            }
        }

        if (!movingToLeft)
        {
            ChangeDirection();
        }
        else if (movingToLeft)
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        direction = movingToLeft ? 1 : -1;
        SpritesFlip();
    }

    void SpritesFlip()
    {
        for(int j = 0; j<Enemy1Parts.Length; j++)
        {
            RotableParts[j].flipX = !movingToLeft;
        }
        sword.eulerAngles = new Vector3 (0f, 0f, (direction * 70f));
    }
}
