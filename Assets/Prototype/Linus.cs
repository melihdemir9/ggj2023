using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linus : MonoBehaviour
{
    public MouseController MouseController;

    public void CeaseSwipe()
    {
        MouseController.CeaseSwipe();
    }
}
