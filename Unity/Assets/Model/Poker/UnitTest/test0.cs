using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class test0 : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[13];
    public GameObject image;
    public int index;

    void Start()
    {
        ChangeSprtie();
    }

    private async void ChangeSprtie()
    {
        while (true)
        {
            image.GetComponent<Image>().sprite =sprites[index];
            index++;
            if (index > 12) break;
            await Task.Delay(1000);
        }
   
    }
   
}
