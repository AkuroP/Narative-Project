using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomColors : MonoBehaviour
{
    [SerializeField] private int myNumber;
    private List<Material> _materials = new List<Material>();

    private void Awake()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var smr in skinnedMeshRenderers)
        {
            foreach (var mat in smr.materials)
            {
                _materials.Add(mat);
            }
        }
    }

    private void Start()
    {
        GameEvents.Instance.RandomColorModel += DoRandomColorModel;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.RandomColorModel -= DoRandomColorModel;
    }

    private void DoRandomColorModel(int number)
    {
        if (myNumber == number)
        {
            foreach (var mat in _materials)
            {
                mat.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }
}
