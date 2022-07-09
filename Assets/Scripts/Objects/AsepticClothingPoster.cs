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

    public void HighlightText(ClothingType type) {
        if (type == ClothingType.HeadCover) headCoverText.color = highlightedColor;
        else if (type == ClothingType.FaceMask) faceMaskText.color = highlightedColor;
        else if (type == ClothingType.LabCoat) labCoatText.color = highlightedColor;
        else if (type == ClothingType.SleeveCovers) sleeveCoversText.color = highlightedColor;
        else if (type == ClothingType.ProtectiveGloves) protectiveGlovesText.color = highlightedColor;
        else if (type == ClothingType.ShoeCovers) shoeCoversText.color = highlightedColor;
    }
}
