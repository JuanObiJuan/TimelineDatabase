/**MIT License

Copyright (c) 2020 JuanObiJuan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TouchInputController : MonoBehaviour
{
    public static TouchInputController instance;


    [SerializeField]
    public Dictionary<int, Vector2> myTouchesDirection = new Dictionary<int, Vector2>();
    [SerializeField]
    public Dictionary<int, Vector2> timeLineDrag = new Dictionary<int, Vector2>();

    

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

 
    
    // Update is called once per frame
    void Update()
    {
        Touch[] thisFrameTouches = Input.touches;
        for (int i = 0; i < thisFrameTouches.Length; i++)
        {
            Vector3 mainPos = Camera.main.ScreenToWorldPoint(thisFrameTouches[i].position);
            Ray ray = Camera.main.ScreenPointToRay(thisFrameTouches[i].position);
            RaycastHit hitInfo;
            mainPos.z = -4f;
            if (Physics.Raycast(ray, out hitInfo, 100f))
            {
                Debug.Log("hit : " + hitInfo.collider.transform.parent.name+ " "+ thisFrameTouches[i].phase);
                hitInfo.transform.SendMessage("Touch", thisFrameTouches[i]);
                /*
                //Finger On
                if (thisFrameTouches[i].phase == TouchPhase.Began)
                {

                    print("Obj Touched!!");
                }
                */
            }
            else
            {
                Debug.Log(thisFrameTouches[i].phase+" "+thisFrameTouches[i].fingerId+ "is not hitting");
            }
        }
        /*
        //For every finger
        for (int i = 0; i < Input.touchCount; i++)
        



            if (!myTouchesDirection.ContainsKey(thisFrameTouches[i].fingerId))
            {
                myTouchesDirection.Add(thisFrameTouches[i].fingerId,Vector3.zero);
                
                //Debug.Log("id "+ thisFrameTouches[i].fingerId.ToString()+" "+thisFrameTouches[i].phase);
                //Debug.Log("id " + thisFrameTouches[i].fingerId.ToString() + " "+thisFrameTouches[i].deltaPosition);
                //Debug.Log("id " + thisFrameTouches[i].fingerId.ToString() + " " + thisFrameTouches[i].tapCount);


            }
            else
            {
                
                if (thisFrameTouches[i].phase == TouchPhase.Moved)
                {
                    myTouchesDirection[thisFrameTouches[i].fingerId] += thisFrameTouches[i].deltaPosition;
                }
                if (thisFrameTouches[i].phase == TouchPhase.Ended)
                {
                    Debug.Log("myTouchesDirection size" + myTouchesDirection.Count);
                    Debug.Log("acu" + myTouchesDirection[thisFrameTouches[i].fingerId]);
                    myTouchesDirection.Remove(thisFrameTouches[i].fingerId);     
                }
            }
            /*
            Vector3 mainPos = Camera.main.ScreenToWorldPoint(myTouches[i].position);
            Ray ray = Camera.main.ScreenPointToRay(myTouches[i].position);
            RaycastHit hit;
            mainPos.z = -4f;
            if (Physics.Raycast(ray, out hit, 20f))
            {
                //Finger On
                if (myTouches[i].phase == TouchPhase.Began)
                {
                    
                    print("Obj Touched!!");
                }
            }
            */
            //OnMouseDrag
            /*
            if (touchedObj[ID] != null)
            {
                //touchedObj[ID].transform.position = mainPos;
                print("Obj Dragged!!");
            }
            */
            //OnMouseUp()
          
            
        }
    }



