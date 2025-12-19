using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSwitcher : MonoBehaviour
{
    public GameObject VFX1;
    public GameObject VFX2;
    public GameObject VFX3;
    public GameObject VFX4;
    public GameObject VFX5;
    public GameObject VFX6;
    public GameObject VFX7;
    public GameObject VFX8;
    public GameObject VFX9;
    public GameObject VFX10;
    public GameObject VFX11;
    public GameObject VFX12;

    public GameObject currentVFX;
    public int V;

    // Start is called before the first frame update
    void Start()
    {
        V = 1;
        currentVFX = VFX1;
        currentVFX.SetActive(true);
    }

    //Select vfx
    private void Update()
    {
        if (V > 12) { V = 1; }
        if (V < 1) { V = 12; }

        if (V == 1) { currentVFX = VFX1; }
        if (V == 2) { currentVFX = VFX2; }
        if (V == 3) { currentVFX = VFX3; }
        if (V == 4) { currentVFX = VFX4; }
        if (V == 5) { currentVFX = VFX5; }
        if (V == 6) { currentVFX = VFX6; }
        if (V == 7) { currentVFX = VFX7; }
        if (V == 8) { currentVFX = VFX8; }
        if (V == 9) { currentVFX = VFX9; }
        if (V == 10) { currentVFX = VFX10; }
        if (V == 11) { currentVFX = VFX11; }
        if (V == 12) { currentVFX = VFX12; }
    }

    //Trigger Next vfx
    public void NextVFX()
    {
        currentVFX.SetActive(false);

        V += 1;

        currentVFX.SetActive(true);
    }

    //Trigger Previous vfx
    public void PreviousVFX()
    {
        currentVFX.SetActive(false);

        V -= 1;

        currentVFX.SetActive(true);
    }
}
