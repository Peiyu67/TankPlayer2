using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Name;
    void Start()
    {
        Name = GameObject.Find("TankName").GetComponent<Text>();
        Name.text = GameManager.localPlayer.name;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NamePos = Camera.main.WorldToScreenPoint(GameManager.localPlayer.transform.position);
        Vector3 NamePoss = new Vector3(NamePos.x, NamePos.y + 200f, NamePos.z);
        Name.transform.position = NamePoss;
    }
}
