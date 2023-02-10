using UnityEngine;
using UnityEditor;
using ProjectEnigma.Cards;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class GameEditor : MonoBehaviour
{
    [MenuItem("Debug/GenerateRoom")]
    static void DebugOption1()
    {
        NormalRoom room = new NormalRoom();
        room.DebugPrintRoomData(room.GenerateRoomData());
    }

    [MenuItem("Debug/Debug Option 2")]
    static void DebugOption2()
    {
        NormalRoom room = new NormalRoom();
        room.PrintRoom(room.GenerateRoom(new int[10,10]));
    }

    [MenuItem("Debug/CreateCardPack")]
    static void DebugOption3()
    {
        var cards = new CardPack()
        {
            HeroCards = new List<HeroCard>()
            {
                new HeroCard()
                {
                    Id = GUID.Generate().ToString(),
                    Name = "Test",
                    Attack = 7,
                    Defense = 10,
                }
            },
            SpellCards = new List<SpellCard>(),
            KeyCards = new List<KeyCard>(),
            MagicCards = new List<MagicCard>()
        };
            using (Stream stream = File.Open("Assets/Data/Cards/cards.cpk", FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, cards);
            }
       
    }
}
