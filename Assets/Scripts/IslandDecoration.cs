using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandDecoration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Show()
    {
        gameObject.SetActive(true);
        var animator = GetComponent<Animator>();
        animator.SetTrigger("show");
        //float length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
