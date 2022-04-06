using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private List<Transform> _segments = new List<Transform>();
    private int score = 0;

    public Transform segmentPrefab;
    public int initialSize = 3;
    public Text scoreText;
    public AudioSource AudioSource;
    public AudioClip ScoreSound, DeathSound;

    private void Start()
    {
        ResetState();
    }


    // Update is called once per frame
    void Update()
    {
        if (direction.x != 0)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector2.up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector2.down;
            }
        }
       
        if (direction.y != 0)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector2.right;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector2.left;
            }
        }
        
    }

    private void FixedUpdate() 
    {
        for (int i = _segments.Count - 1; i > 0; i--)
        {
            _segments[i].position = _segments[i - 1].position;
        }


        // Makes Snake move the way it's facing
        // Rounding the values to make a grid
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y,
            0.0f
            );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
    }

    private void ResetState()
    {
        

        // Start at 1 to skip destroying the head
        for (int i = 1; i < _segments.Count; i++)
        {
            Destroy(_segments[i].gameObject);
            
        }

        // Clear list but add back the head
        _segments.Clear();
        _segments.Add(this.transform);
        
        // Makes the Snake start at a determened size
        for (int i = 1; i < initialSize; i++)
        {
            _segments.Add(Instantiate(this.segmentPrefab));
        }

        score = 0;
        scoreText.text = score.ToString();

        this.transform.position = Vector3.zero;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            score++;
            scoreText.text = score.ToString();

            AudioSource.PlayOneShot(ScoreSound);
            
        } else if (other.gameObject.CompareTag("Obstacle"))
        {
            ResetState();
            AudioSource.PlayOneShot(DeathSound);
        }

    }
}
