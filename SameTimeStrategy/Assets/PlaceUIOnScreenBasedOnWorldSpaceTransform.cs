using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceUIOnScreenBasedOnWorldSpaceTransform : MonoBehaviour {

    [SerializeField] Transform trackingTransform;
    [SerializeField] Camera CameraToPlaceOnto;
    [SerializeField] RectTransform rectTrans;

    private void LateUpdate()
    {
        Debug.Log(CameraToPlaceOnto.WorldToScreenPoint(trackingTransform.position));


        Rect rect = rectTrans.rect;
        rect.position = CameraToPlaceOnto.WorldToScreenPoint(trackingTransform.position);

        //rectTrans.rect = rect;
    }

}
