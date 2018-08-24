using System.Collections;
using UnityEngine;

public class StartAnim : MonoBehaviour
{
    public Animator anim;
    public Animation _animation;
    public GameObject fadePlane;
    public SpriteRenderer sprR;
    public int fadeTime;
    public string animatorState;

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animatorState))
        {
            fadePlane.SetActive(true);
            StartCoroutine(Fade(fadeTime));
        }
    }

    private IEnumerator Fade(float _time)
    {

        Color _col = sprR.color;
        int i = 0;
        while (sprR.color.a >= 0 && i < 1000)
        {
            sprR.color = new Color(_col.r, _col.g, _col.b, (_col.a - 0.1f));
            Debug.Log(sprR.color.a);
            i++;
        }

        this.gameObject.SetActive(false);

        yield return null;
    }
}
