using UnityEngine;

public class GroupColorChanger : MonoBehaviour
{
    public Color losingColor = Color.grey;
    public Color winningColor = Color.white;
    public GameObject parent;
    public int patternValue;


    public void ApplyColor(Color color, GameObject _parent)
    {
        if (_parent == null)
        {
            _parent = parent;
        }

        Renderer[] renderers = _parent.GetComponentsInChildren<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (r.material != null)
            {
                r.material.color = color;
            }
        }
    }

    public void ApplyLosingColor()
    {
        ApplyColor(losingColor, parent);
    }

    public void ApplyWinningColor()
    {
        ApplyColor(winningColor, parent);
    }
}
