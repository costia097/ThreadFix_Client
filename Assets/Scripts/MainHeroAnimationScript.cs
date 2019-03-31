using UnityEngine;

public class MainHeroAnimationScript : MonoBehaviour
{
    private Animator _animator;
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsSlashing = Animator.StringToHash("isSlashing");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            _animator.SetBool(IsRunning, true);     
        }
        else
        {
            _animator.SetBool(IsRunning, false);
        }

        _animator.SetBool(IsSlashing, Input.GetKey(KeyCode.LeftControl));
    }
}
