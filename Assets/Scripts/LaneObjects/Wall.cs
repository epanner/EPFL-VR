using UnityEngine;

public class Wall : LaneObject
{
    private GameObject puzzle;
    private void Awake()
    {
        int puzzleChoice = Random.Range(0, transform.childCount);
        puzzle = transform.GetChild(puzzleChoice).gameObject;
        puzzle.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TargetsAreAllTouched())
        {
            GameManager.Instance.UnlockWall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerHit(3);
            DestroyLaneObject();
        }
    }

    private bool TargetsAreAllTouched()
    {
        bool result = true;
        int i = 0;
        while (result && i < puzzle.transform.childCount)
        {
            PuzzleTarget target = puzzle.transform.GetChild(i).gameObject.GetComponent<PuzzleTarget>();
            result = target.isActivated;
            i++;
        }
        return result;
    }
}
