using UnityEngine;
using System;
using System.Collections.Generic;

public class OpenCovers : Task {
    public enum Conditions { OpenedAllCovers }
    private bool openedFilterCover;
    private bool openedScalpelCover;
    private bool openedTweezersCover;
    private CabinetBase laminarCabinet;

    public OpenCovers() : base(TaskType.OpenCovers, true, false) {
        SetCheckAll(true);
        Subscribe();
        AddConditions((int[])Enum.GetValues(typeof(Conditions)));
    }

    public override void Subscribe() {
        base.SubscribeEvent(ScalpelCoverOpened, EventType.ScalpelCoverOpened);
        base.SubscribeEvent(TweezersCoverOpened, EventType.TweezersCoverOpened);
        base.SubscribeEvent(FilterCoverOpened, EventType.FilterCoverOpened);
        base.SubscribeEvent(WrongSpotOpened, EventType.WrongSpotOpened);
        base.SubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
    }

    private void SetCabinetReference(CallbackData data) {
        CabinetBase cabinet = (CabinetBase)data.DataObject;
        if (cabinet.type == CabinetBase.CabinetType.Laminar) {
            laminarCabinet = cabinet;
            base.UnsubscribeEvent(SetCabinetReference, EventType.ItemPlacedForReference);
        }
    }

    private void ScalpelCoverOpened(CallbackData data) {
        var scalpel = (data.DataObject as Scalpel);
        openedScalpelCover = true;
        CheckIfInsideLaminarCabinet(scalpel);
        CheckAllConditions();
    }

    private void TweezersCoverOpened(CallbackData data) {
        var tweezers = (data.DataObject as Tweezers);
        openedTweezersCover = true;
        CheckIfInsideLaminarCabinet(tweezers);
        CheckAllConditions();
    }
    private void FilterCoverOpened(CallbackData data) {
        var filter = (data.DataObject as FilterInCover);
        openedFilterCover = true;
        CheckIfInsideLaminarCabinet(filter);
        CheckAllConditions();
    }

    private void CheckIfInsideLaminarCabinet(Interactable interactable) {
        if (laminarCabinet.GetContainedItems().Contains(interactable)) {
            return;
        } else {
            CreateTaskMistake("Avasit suojamuovin laminaarikaapin ulkopuolella!!!", 1);
        }
        
        if (laminarCabinet.GetContainedItems() == null) {
            CreateTaskMistake("Avasit suojamuovin laminaarikaapin ulkopuolella!!!", 1);
        }
    }
    private void CheckAllConditions() {
        if (openedFilterCover && openedScalpelCover && openedTweezersCover) {
            EnableCondition(Conditions.OpenedAllCovers);
            CompleteTask();
        }
    }

    public void WrongSpotOpened(CallbackData data) {
        CreateTaskMistake("Avasit suojamuovin väärästä päästä!", 1);
    }

    public override void CompleteTask() {
        base.CompleteTask();
        if (Completed) {
            Popup(base.success, MsgType.Done, base.Points);
        }
    }
}
