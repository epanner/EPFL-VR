using UnityEngine;
using UnityEngine.UI;

public class ArrowMover : MonoBehaviour
{
    public RectTransform arrowTransform;  // drag your arrow image here
    public float moveDistance = 100f;     // how far to move right
    public float moveDuration = 0.5f;     // time for each move
    public int repeatCount = 2;

    private Vector2 originalPos;
    private int currentCycle = 0;
    private bool movingRight = true;
    private float timer = 0f;

    private void Start()
    {
        if (arrowTransform == null)
            arrowTransform = GetComponent<RectTransform>();

        originalPos = arrowTransform.anchoredPosition;
    }

    private void Update()
    {
        if (currentCycle >= repeatCount * 2) return;

        timer += Time.deltaTime;
        float t = timer / moveDuration;

        if (t > 1f)
        {
            // switch direction
            timer = 0f;
            movingRight = !movingRight;
            currentCycle++;
            return;
        }

        // Lerp position
        float offset = Mathf.Lerp(0f, moveDistance, t);
        if (!movingRight) offset = moveDistance - offset;

        arrowTransform.anchoredPosition = originalPos + new Vector2(offset, 0f);
    }
}