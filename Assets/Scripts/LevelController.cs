using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelController : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;
    [SerializeField] private int levelLength;
    [SerializeField] private float xStepDistance;
    [SerializeField] private float yStepDistance;
    [SerializeField] private float noiseStep;
    [SerializeField] private float curveSmoothness;

    private Vector3 lastPosition;

    private void OnValidate()
    {
        spriteShapeController.spline.Clear();

        for (int i = 0; i < levelLength; i++)
        {
            lastPosition = transform.position + new Vector3(i * xStepDistance, Mathf.PerlinNoise(0, i * noiseStep) * yStepDistance);
            spriteShapeController.spline.InsertPointAt(i, lastPosition);

            // if not first or last point
            if (i > 0 && i < levelLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xStepDistance * curveSmoothness);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xStepDistance * curveSmoothness);
            }
        }

        spriteShapeController.spline.InsertPointAt(levelLength, new Vector3(lastPosition.x, transform.position.y - 10f));
        spriteShapeController.spline.InsertPointAt(levelLength + 1, transform.position + Vector3.down * 10f);
    }
}
