using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using ProjectEnigma.Cards;

public class CardPackEditorWindow : EditorWindow
{
    private static List<CardBase> cardPackEntries = new List<CardBase>();

    [MenuItem("Window/Card Pack Editor")]
    public static void ShowWindow()
    {
        cardPackEntries = LoadCards();
        GetWindow<CardPackEditorWindow>("Card Pack Editor");
    }

    private void OnGUI()
    {
        // List all existing entries
        for (int i = 0; i < cardPackEntries.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            cardPackEntries[i].Name = EditorGUILayout.TextField("Name", cardPackEntries[i].Name);
            cardPackEntries[i].Rarity = (CardRarity)EditorGUILayout.EnumPopup("Rarity", cardPackEntries[i].Rarity);
            Texture2D texture = null;
            if (cardPackEntries[i].Image.Data != null)
                texture = cardPackEntries[i].Image.GetUIImage();
            texture = new Texture2D(630, 880);
            texture = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), true);
            cardPackEntries[i].Image = new CardImage(texture);
            ((MinionCard)cardPackEntries[i]).Attack = EditorGUILayout.IntField("Attack", ((MinionCard)cardPackEntries[i]).Attack);
            ((MinionCard)cardPackEntries[i]).Defense = EditorGUILayout.IntField("Defense", ((MinionCard)cardPackEntries[i]).Defense);
            EditorGUILayout.EndHorizontal();
          
           


        }

        // Add a new entry button
        if (GUILayout.Button("Add Entry"))
        {
            cardPackEntries.Add(new MinionCard());
        }
        if (GUILayout.Button("Save"))
        {
            SaveCards(cardPackEntries);
        }
    }

private static void SaveCards(List<CardBase> cards)
    {
        using (System.IO.Stream stream = System.IO.File.Open("Assets/Data/Cards/cards.cpk", System.IO.FileMode.Create))
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bin.Serialize(stream, cards);
        }
    }
    public static List<CardBase> LoadCards()
    {
        List<CardBase> cards = new List<CardBase>();
        if (System.IO.File.Exists("Assets/Data/Cards/cards.cpk"))
        {
            using (System.IO.Stream stream = System.IO.File.Open("Assets/Data/Cards/cards.cpk", System.IO.FileMode.Open))
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bin = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                cards = (List<CardBase>)bin.Deserialize(stream);
            }
        }
        return cards;
    }
}
