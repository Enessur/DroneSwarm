using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fizzbuzz : MonoBehaviour
{

    public struct xgroup
    {
        public string id;
        public int modula;
        
    }
    
    void Start()
    {
        int i;
        for (i = 1; i < 100; i++)
        {
            if ((i % 3 == 0) && (i % 5 == 0))
            {
                Debug.Log("Fizzbuzz");
            }
            else
            {
                if (i % 3 == 0)
                {
                    Debug.Log("fizz");
                }

                if (i % 5 == 0)
                {
                    Debug.Log("buzz");
                }

                if ((i % 3 != 0) && (i % 5 != 0))
                {
                    Debug.Log(i);
                }
            }
        }


        for (i = 1; i < 100; i++)
        {
            string output = "";

            if (i % 3 == 0)
            {
                output += "Fizz";
            }

            if (i % 5 == 0)
            {
                output += "Buzz";
            }

            if (output == "")
            {
                output = "" + i;
            }

            Debug.Log(output);
        }
    }
}