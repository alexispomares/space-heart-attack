using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollableFit : MonoBehaviour {

    public Text TargetSize;

    void Start()
    {
        var rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, TargetSize.preferredHeight);
    }

}
