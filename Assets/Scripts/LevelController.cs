using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class LevelController : MonoBehaviour
{
    [SerializeField] private SpriteShapeController bottomSpriteShapeController;
    [SerializeField] private SpriteShapeController topSpriteShapeController;
    [Space]
    [SerializeField] private int levelLength;
    [SerializeField] private float xStepDistance;
    [SerializeField] private float yStepDistance;
    [SerializeField] private float roofDistance;
    [SerializeField] private float noiseStep;
    [SerializeField] private float curveSmoothness;
    [Space]
    [SerializeField] private float bottomThickness;
    [SerializeField] private float topThickness;
    [Space]
    [SerializeField] private GameObject obstaclePrefab;
    [Range(0f, 1f)]
    [SerializeField] private float chanceForObstacle;

    private Spline bottomSpline;
    private Spline topSpline;
    public Vector3 LastPosition { get; private set; }

    private void Awake()
    {
        bottomSpline = bottomSpriteShapeController.spline;
        topSpline = topSpriteShapeController.spline;
    }

    private void OnValidate()
    {
        bottomSpline = bottomSpriteShapeController.spline;
        topSpline = topSpriteShapeController.spline;

        bottomSpline.Clear();
        topSpline.Clear();

        for (int i = 0; i < levelLength; i++)
        {
            LastPosition = transform.position + new Vector3(i * xStepDistance, Mathf.PerlinNoise(0, i * noiseStep) * yStepDistance);
            bottomSpline.InsertPointAt(i, LastPosition);
            topSpline.InsertPointAt(i, LastPosition + Vector3.up * roofDistance);

            if (i > 0 && i < levelLength - 1)
            {
                SetContinuousTangent(i);
            }
        }

        bottomSpline.InsertPointAt(levelLength, new Vector3(LastPosition.x, transform.position.y - bottomThickness));
        bottomSpline.InsertPointAt(levelLength + 1, transform.position + Vector3.down * bottomThickness);

        topSpline.InsertPointAt(levelLength, new Vector3(LastPosition.x, transform.position.y + roofDistance + topThickness));
        topSpline.InsertPointAt(levelLength + 1, transform.position + Vector3.up * roofDistance + Vector3.up * topThickness);

        if (!Application.isPlaying)
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }

    private void SetContinuousTangent(int index)
    {
        bottomSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
        bottomSpline.SetLeftTangent(index, Vector3.left * xStepDistance * curveSmoothness);
        bottomSpline.SetRightTangent(index, Vector3.right * xStepDistance * curveSmoothness);

        topSpline.SetTangentMode(index, ShapeTangentMode.Continuous);
        topSpline.SetLeftTangent(index, Vector3.left * xStepDistance * curveSmoothness);
        topSpline.SetRightTangent(index, Vector3.right * xStepDistance * curveSmoothness);
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AddSegments(1);
    }
#endif

    public void AddSegments(int count)
    {
        for (int i = 0; i < count; i++)
        {
            LastPosition = transform.position + new Vector3(levelLength * xStepDistance, Mathf.PerlinNoise(0, levelLength * noiseStep) * yStepDistance);

            // Add new points at the end of the ground and roof lines
            bottomSpline.InsertPointAt(levelLength, LastPosition);
            topSpline.InsertPointAt(levelLength, LastPosition + Vector3.up * roofDistance);

            SetContinuousTangent(levelLength - 1);

            // Move the bottom and top right corner to match on the x-axis
            bottomSpline.SetPosition(levelLength + 1, new Vector3(LastPosition.x, transform.position.y - bottomThickness));
            topSpline.SetPosition(levelLength + 1, new Vector3(LastPosition.x, transform.position.y + roofDistance + topThickness));

            // Should an obstacle be spawned
            if (Random.Range(0f, 1f) < chanceForObstacle)
            {
                var obstacle = Instantiate(obstaclePrefab);
                var yPos = Random.Range(LastPosition.y, LastPosition.y + roofDistance);
                obstacle.transform.position = new Vector3(LastPosition.x, yPos);
            }

            levelLength++;
        }
    }
}
