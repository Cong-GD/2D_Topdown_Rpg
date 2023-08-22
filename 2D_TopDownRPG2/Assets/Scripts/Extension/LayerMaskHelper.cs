using UnityEngine;

public static class LayerMaskHelper
{
    public static readonly LayerMask FigherMask = LayerMask.GetMask(new string[] { "Fighter" });

    public static readonly LayerMask ObstacleMask = LayerMask.GetMask(new string[] { "Obstacle" });
}