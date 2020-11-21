using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ActivateTooltip : MonoBehaviour
{
    [SerializeField]
    GameObject toolTip = null;

    public GameObject ToolTip { get { return toolTip; } private set { toolTip = value; } }

    public void SetTooltip(bool active)
    {
        ToolTip.SetActive(active);
    }
}
