using ProjectEnigma.Cards;
using ProjectEnigma.Rooms;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameRegistry : MonoBehaviour
{
    public static List<RoomBiome> RoomBiomes;
    public static CardPack Cards;
    public static void RegisterRoomBiomes()
    {
        RegisterRoomBiome(new GeneralBiome());
        RegisterRoomBiome(new HiddenTempleBiome());
    }

    private static void RegisterRoomBiome(RoomBiome biome)
    {
        if(RoomBiomes == null)
            RoomBiomes = new List<RoomBiome>();
        RoomBiomes.Add(biome);
    }
    public static void LoadCardPacks()
    {
        CardPack cards = new CardPack();
        cards.MinionCards = new List<MinionCard>();
        cards.SpellCards = new List<SpellCard>();
        cards.MagicCards = new List<MagicCard>();
        cards.KeyCards = new List<KeyCard>();
        System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(ProjectEnigma.Info.Config.CARDS_DIR);

        foreach (var file in d.GetFiles("*.cpk"))
        {
            CardPack temp = new CardPack();
            using (Stream stream = File.Open(file.FullName, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                temp = (CardPack)bin.Deserialize(stream);
            }
            foreach (var card in temp.MinionCards)
            {
                cards.MinionCards.Add(card);
            }
            foreach (var card in temp.SpellCards)
            {
                cards.SpellCards.Add(card);
            }
            foreach (var card in temp.KeyCards)
            {
                cards.KeyCards.Add(card);
            }
            foreach (var card in temp.MagicCards)
            {
                cards.MagicCards.Add(card);
            }

        }
        

        Cards = cards;
    }
}
