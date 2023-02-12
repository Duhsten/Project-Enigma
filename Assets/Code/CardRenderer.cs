using ProjectEnigma.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardRenderer : MonoBehaviour
{
    [Header("Color Modifiers")]
    public Image cardImage;
    public TMPro.TMP_Text cardTitleText;
    public TMPro.TMP_Text cardDescText;
    public RawImage cardPortrait;
    public GameObject cardBack;
    [Header("Hero Card")]
    [Space(5)]
    public GameObject MinionCard;
    public TMPro.TMP_Text MinionCardAttack;
    public TMPro.TMP_Text MinionCardDefense;
    public CardRenderer()
    {

    }
    public void Start()
    {
    }

    public void RenderCard(CardBase card)
    {
        cardImage.color = ProjectEnigma.Util.ColorCodes.RarityColors[card.Rarity];
        if(cardDescText != null)
        {
            cardDescText.color = ProjectEnigma.Util.ColorCodes.RarityColors[card.Rarity];
            cardDescText.color = ProjectEnigma.Util.ColorCodes.RarityColors[card.Rarity + 1];
        }
        cardTitleText.text = card.Name;
        if(cardPortrait != null)
            cardPortrait.texture = card.Image.GetUIImage();
        switch (card.Type)
        {
            case CardType.Hero:
                RenderMinionCard((MinionCard)card);
                break;
            case CardType.Spell:
                RenderSpellCard(card);
                break;
            case CardType.Key:
                RenderKeyCard((KeyCard)card);
                break;
            case CardType.Magic:
                RenderMagicCard(card);
                break;
        }
        cardBack.SetActive(false);
    }

    private void RenderMinionCard(MinionCard card)
    {
        MinionCard.SetActive(true);
        MinionCardAttack.text = card.Attack.ToString();
        MinionCardDefense.text = card.Defense.ToString();
    }
    private void RenderSpellCard(CardBase card)
    {
    }
    private void RenderKeyCard(KeyCard card)
    {

        cardTitleText.alignment = TMPro.TextAlignmentOptions.Center;
        cardDescText.alignment = TMPro.TextAlignmentOptions.Center;
    }
    private void RenderMagicCard(CardBase card)
    {

    }
}
