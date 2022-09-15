using System.Collections;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private float _destroyTime;

    [SerializeField]
    private int _id;

    public int Id
    {
        get
        {
            return _id;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.GameEnded += OnGameEnded;
        GameManager.Instance.ScoreReset += ScoreReset;
        GameManager.Instance.ScoreSet += ScoreSet;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameEnded -= OnGameEnded;
        GameManager.Instance.ScoreReset -= ScoreReset;
        GameManager.Instance.ScoreSet -= ScoreSet;
    }


    public void OnGameEnded()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(Rescale());
    }

    private IEnumerator Rescale()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        Vector3 scaleOffset = endScale - startScale;
        float timeElapsed = 0f;
        float speed = 1 / _destroyTime;
        var updateTime = new WaitForFixedUpdate();
        while (timeElapsed < 1f)
        {
            timeElapsed += speed * Time.fixedDeltaTime;
            transform.localScale = startScale + timeElapsed * scaleOffset;
            yield return updateTime;
        }

        Destroy(gameObject);
    }

    [SerializeField] private SpriteRenderer _sr;

    [SerializeField] private Color _activeColor, _inactiveColor;
    private void ScoreReset()
    {
        _sr.color = _inactiveColor;
    }

    private void ScoreSet(int scoreId)
    {
        if(scoreId != Id)
        {
            return;
        }

        _sr.color = _activeColor;
    }

    [SerializeField] private GameObject _effect;
    [SerializeField] private Animator _effectAC;
    [SerializeField] private AnimationClip _effectClip;

    public void PlayScoreAnimation()
    {
        StartCoroutine(ScoreAnimation());
    }

    private IEnumerator ScoreAnimation()
    {
        StopCoroutine(ScoreAnimation());
        _effect.SetActive(true);

        _effectAC.Play(_effectClip.name,-1,0f);
        yield return new WaitForSeconds(_effectClip.length);

        _effect.SetActive(false);
    }
}
