using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DetailShuffler : MonoBehaviour
{    
    [SerializeField] private Image _blackBackground;

    public void Shuffle(IEnumerable<AbstractDetail> details)
    {        
        _blackBackground.gameObject.SetActive(true);
        var duration = 0.4f;
        _blackBackground.color = new Color(0, 0, 0, 0);
        Game.Get<Coroutine>().Animate(duration, (time, isAnimate) =>
        {
            _blackBackground.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time / duration));
            if (isAnimate == false)
            {
                RandomShuffle(details);
                AfterBlackScreen(duration);
            }
        });
    }

    private void RandomShuffle(IEnumerable<AbstractDetail> details)
    {
        foreach (var abstractDetail in details)
        {
            var detail = (DetailComponent)abstractDetail;
            var chance = Game.Get<GameLoader>().GameLoaderData == null ? 1 :
                Game.Get<GameLoader>().GameLoaderData?.defaultSqueres;

            if (Random.Range(0f, 1f) > chance)
                detail.MoveToTable();
            else detail.Fix();
        }
    }

    private void AfterBlackScreen(float duration)
    {
        _blackBackground.color = new Color(0, 0, 0, 1);
        Game.Get<Coroutine>().Animate(duration, (time2, isAnimate2) =>
        {
            _blackBackground.color = new Color(0, 0, 0, 1 - Mathf.Lerp(0, 1, time2 / duration));

            if (isAnimate2 == false)
            {
                _blackBackground.gameObject.SetActive(false);
            }
        });
    }
}