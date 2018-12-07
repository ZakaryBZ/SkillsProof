using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollView : ScrollRect
{
    private IEnumerator scrollCoroutine;

    private float pixelsAmount = 200f;

    private float standardTime = 1f;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (this.scrollCoroutine != null) //stops autoscroll if user started new interation with scrollview
        {
            this.StopCoroutine(this.scrollCoroutine);
            this.scrollCoroutine = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        this.StopMovement();

        Button[] buttons = this.content.GetComponentsInChildren<Button>();
        //search for closest button to the screen center
        Button closestButton = buttons[0];
        float minDist = Vector3.Distance(closestButton.transform.position, this.transform.position);
        for (int i = 1; i < buttons.Length; i++)
        {
            float dist = Vector3.Distance(buttons[i].transform.position, this.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestButton = buttons[i];
            }
        }

        //determines which side the button is located
        Vector3 targetPos;
        if (closestButton.transform.position.x > this.transform.position.x)
        {
            targetPos = this.content.position - new Vector3(minDist, 0);
        }
        else
        {
            targetPos = this.content.position + new Vector3(minDist, 0);
        }

        float time = (this.standardTime / this.pixelsAmount) * minDist; //calc scrolltime based on speed that equals 200 pixels per second
        this.scrollCoroutine = this.ScrollToClosest(targetPos, time);
        this.StartCoroutine(this.scrollCoroutine);
    }

    //used Lerp inside coroutine for smooth scrolling
    private IEnumerator ScrollToClosest(Vector3 targetPos, float time)
    {
        float elapsedTime = 0;
        Vector3 startPos = this.content.position;
        while (elapsedTime < time)
        {
            this.content.position = Vector3.Lerp(startPos, targetPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        this.scrollCoroutine = null;
    }
}