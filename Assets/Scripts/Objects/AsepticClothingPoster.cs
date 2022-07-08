using TMPro;
using UnityEngine;

public class AsepticClothingPoster : MonoBehaviour {

    public TextMeshPro headCoverText;
    public TextMeshPro faceMaskText;
    public TextMeshPro labCoatText;
    public TextMeshPro sleeveCoversText;
    public TextMeshPro protectiveGlovesText;
    public TextMeshPro shoeCoversText;
    public Color highlightedColor;

    public void HighlightText(string text) {
        if (text.Equals("Suojapäähine")) headCoverText.color = highlightedColor;
        else if (text.Equals("Kasvomaski")) faceMaskText.color = highlightedColor;
        else if (text.Equals("Laboratoriotakki")) labCoatText.color = highlightedColor;
        else if (text.Equals("Hihasuojat")) sleeveCoversText.color = highlightedColor;
        else if (text.Equals("Suojakäsineet")) protectiveGlovesText.color = highlightedColor;
        else if (text.Equals("Kengänsuojat")) shoeCoversText.color = highlightedColor;
    }
}
