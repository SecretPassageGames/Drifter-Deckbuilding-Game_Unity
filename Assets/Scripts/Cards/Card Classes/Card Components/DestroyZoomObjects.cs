﻿using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyZoomObjects : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData) =>
        Managers.U_MAN.DestroyZoomObjects();
}
