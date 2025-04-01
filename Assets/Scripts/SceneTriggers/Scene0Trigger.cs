using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Scene0Trigger : MonoBehaviour
{
    PostProcessVolume post;
    void Start()
    {
        post = GetComponent<PostProcessVolume>();
    }

    void Update()
    {
        if(post.weight > 0)
        {
            post.weight -= 0.05f * Time.deltaTime;
        }
    }
}
