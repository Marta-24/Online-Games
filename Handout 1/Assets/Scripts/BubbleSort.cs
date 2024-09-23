using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class BubbleSort : MonoBehaviour
{
    float[] array;
    List<GameObject> mainObjects;
    public GameObject prefab;

    void Start()
    {
        mainObjects = new List<GameObject>();
        array = new float[30000];
        for (int i = 0; i < 30000; i++)
        {
            array[i] = (float)Random.Range(0, 1000) / 100;
        }

        spawnObjs();
        Thread sortThread = new Thread(bubbleSort);
        sortThread.Start();
    }

    void Update()
    {
        if (updateHeights())
        {
            ChangeHeights();
        }
    }

    void bubbleSort()
    {
        int i, j;
        int n = array.Length;
        bool swapped;
        for (i = 0; i < n - 1; i++)
        {
            swapped = false;
            for (j = 0; j < n - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    swapped = true;
                }
            }

            logArray();
            if (swapped == false)
                break;
        }
        logArray();
    }

    void logArray()
    {
        string text = string.Join(", ", array);
        Debug.Log(text);
    }

    void spawnObjs()
    {
        for (int i = 0; i < array.Length; i++)
        {
            GameObject obj = Instantiate(prefab, new Vector3((float)i / 1000,
                this.gameObject.GetComponent<Transform>().position.y, 0), Quaternion.identity);
            mainObjects.Add(obj);
        }
    }

    bool updateHeights()
    {
        bool changed = false;
        for (int i = 0; i < mainObjects.Count; i++)
        {
            float newHeight = array[i];
            Vector3 scale = mainObjects[i].transform.localScale;
            if (scale.y != newHeight)
            {
                scale.y = newHeight;
                mainObjects[i].transform.localScale = scale;
                changed = true;
            }
        }
        return changed;
    }

    void ChangeHeights()
    {

    }
}
