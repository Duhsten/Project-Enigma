using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using ProjectEnigma.Util;
using ProjectEnigma.Cards;
using UnityEngine.UI;
using System.IO;
using System;

namespace ProjectEnigma.Rooms
{
    public abstract class RoomBiome
    {
        private string Id { get; set; }
        private string Name { get; set; }
        public abstract string Biome {get;}
        public abstract float Probability { get; }
        public Room NewRoom(Tilemap world)
        {
            return NewRoom(world, new Vector2Int(500, 500), new Vector2Int(Random.Range(0, 1), Random.Range(0, 1)));
        }
        public Room NewRoom(Tilemap world, Vector2Int startPos, Vector2Int direction)
        {

            string id = System.Guid.NewGuid().ToString();
            startPos += direction;
            var floorPoints = GenerateFloorShape(world, startPos, direction);
            var wallPoints = GenerateWalls(world, floorPoints);
            var room  = new Room()
            {
                Id = id,
                Name = id.Split('-')[0],
                FloorPoints = floorPoints,
                WallPoints = wallPoints,
            };
            room.DoorPoints = GenerateDoors(world, wallPoints,room);
            return room;
         
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Room))
            {
                return false;
            }

            var other = (Room)obj;
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Id, Name);
        }

        public abstract List<TileVector> GenerateFloorShape(Tilemap world, Vector2Int startPos, Vector2Int direction);

        public List<TileVector> GenerateWalls(Tilemap world, List<TileVector> floorPoints)
        {
            List<TileVector> points = new List<TileVector>();
            foreach (var p in floorPoints)
            {
                Vector3Int[] directions = new Vector3Int[]
                {
                    new Vector3Int(p.x+1, p.y, 0),
                    new Vector3Int(p.x-1, p.y, 0),
                    new Vector3Int(p.x, p.y+1, 0),
                    new Vector3Int(p.x, p.y-1, 0),
                    new Vector3Int(p.x+1, p.y+1, 0),
                    new Vector3Int(p.x-1, p.y-1, 0),
                    new Vector3Int(p.x+1, p.y-1, 0),
                    new Vector3Int(p.x-1, p.y+1, 0),

                };
                foreach (var d in directions)
                {
                    if (world.GetTile(d) != Tiles.FLOOR_TILE && world.GetTile(d) != Tiles.WALL_TILE && world.GetTile(d) is not InteractableTile)
                    {

                        world.SetTile(d, Tiles.WALL_TILE);
                        points.Add(new TileVector(d.x, d.y));
                    }
                }
            }
            return points;
        }
        public Dictionary<TileVector, TileVector> GenerateDoors(Tilemap world, List<TileVector> wallPoints, ProjectEnigma.Rooms.Room room)
        {
            Dictionary<TileVector, TileVector> dict = new Dictionary<TileVector, TileVector>();

            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                TileVector point = new TileVector(0, 0);
                Vector2Int direction = new Vector2Int(0, 0);
                bool validPoint = false;
                bool validDirection = false;
                while (!validPoint && !validDirection)
                {
                    point = wallPoints[Random.RandomRange(0, wallPoints.Count - 1)];
                    Vector3Int[] directions = new Vector3Int[]
                    {
                    new Vector3Int(point.x-1, point.y, 0),
                    new Vector3Int(point.x+1, point.y, 0),
                    new Vector3Int(point.x, point.y-1, 0),
                    new Vector3Int(point.x, point.y+1, 0),
                    };
                    foreach (var d in directions)
                    {
                        if (world.GetTile(d) == Tiles.FLOOR_TILE)
                        {
                            Debug.Log($"Floor Found {d}");
                            validPoint = true;
                        }
                        if (world.GetTile(d) != Tiles.WALL_TILE && world.GetTile(d) != Tiles.FLOOR_TILE && world.GetTile(d) != Tiles.EMPTY_TILE)
                        {
                            Debug.Log($"Air Found {d}");
                            direction = new Vector2Int(d.x - point.x, d.y - point.y);
                            validDirection = true;
                        }
                    }
                }
                if (validDirection && validPoint)
                {
                    world.SetTile(new Vector3Int(point.x, point.y), Tiles.DOOR_TILE(new Vector2Int(point.x, point.y), direction, room));
                    dict.Add(new TileVector(point.x,point.y), new TileVector(direction.x, direction.y));
                }
                else
                {
                    return GenerateDoors(world, wallPoints, room);
                }

            }
            return dict;
        }
       public List<Vector2Int> GetValidNeighbors(Vector2Int currPos, int[,] shape, int width, int height, Vector2Int direction)
        {
            List<Vector2Int> neighbors = new List<Vector2Int>();

            // Prioritize the desired direction
            Vector2Int up = new Vector2Int(currPos.x, currPos.y + 1);
            if (up.x >= 0 && up.x < width && up.y >= 0 && up.y < height && shape[up.x, up.y] == 0)
            {
                neighbors.Add(up);
            }

            Vector2Int right = new Vector2Int(currPos.x + 1, currPos.y);
            if (right.x >= 0 && right.x < width && right.y >= 0 && right.y < height && shape[right.x, right.y] == 0)
            {
                neighbors.Add(right);
            }

            Vector2Int down = new Vector2Int(currPos.x, currPos.y - 1);
            if (down.x >= 0 && down.x < width && down.y >= 0 && down.y < height && shape[down.x, down.y] == 0)
            {
                neighbors.Add(down);
            }
            Vector2Int left = new Vector2Int(currPos.x - 1, currPos.y);
            if (left.x >= 0 && left.x < width && left.y >= 0 && left.y < height && shape[left.x, left.y] == 0)
            {
                neighbors.Add(left);
            }

            // Check if the desired direction is valid and add it to the list
            Vector2Int desiredDirection = new Vector2Int(currPos.x + direction.x, currPos.y + direction.y);
            if (desiredDirection.x >= 0 && desiredDirection.x < width && desiredDirection.y >= 0 && desiredDirection.y < height && shape[desiredDirection.x, desiredDirection.y] == 0)
            {
                if (neighbors.Count > 0 && neighbors[0] != desiredDirection)
                {
                    neighbors.Insert(0, desiredDirection);
                }
                else
                {
                    neighbors.Add(desiredDirection);
                }
            }

            return neighbors;
        }
    }
    
    [System.Serializable]
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<TileVector, ProjectEnigma.Rooms.Room> ConnectedRooms { get; set; }
        public List<TileVector> FloorPoints { get; set; }
        public List<TileVector> WallPoints { get; set; }
        public Dictionary<TileVector, TileVector> DoorPoints { get; set; }

    
        public override bool Equals(object obj)
        {
            if (!(obj is Room))
            {
                return false;
            }

            var other = (Room)obj;
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(Id, Name);
        }

       
        public void AddConnectedRoom(TileVector doorPos, ProjectEnigma.Rooms.Room newRoom)
        {
            if (ConnectedRooms == null)
                ConnectedRooms = new Dictionary<TileVector, ProjectEnigma.Rooms.Room>();
            ConnectedRooms.Add(doorPos, newRoom);
        }
   
    }
}
namespace ProjectEnigma.Util
{
    [System.Serializable]
    public struct TileVector
    {
        public int x;
        public int y;
        public TileVector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static TileVector ZERO = new TileVector(0, 0);
        public static TileVector LEFT = new TileVector(-1, 0);
        public static TileVector TOP = new TileVector(0, 1);
        public static TileVector RIGHT = new TileVector(1, 0);
        public static TileVector BOTTOM = new TileVector(0, -1);
        
        public static Vector2Int ToVector2Int(TileVector v)
        {
            return new Vector2Int(v.x, v.y);
        }
        public static Vector3Int ToVector3Int(TileVector v)
        {
            return new Vector3Int(v.x, v.y,0);
        }
        public static TileVector operator + (TileVector a, TileVector b)
        {
            return new TileVector { x = a.x + b.x, y = a.y + b.y };
        }
    }
    public class ColorCodes
    {
        public static Dictionary<CardRarity, Color> RarityColors = new Dictionary<CardRarity, Color>()
        {
            {CardRarity.Common, Color.white},
            {CardRarity.Uncommon, new Color(0,.97f, 0.12f)},
            {CardRarity.Rare, new Color(0,.78f, 0.97f)},
            {CardRarity.Legendary, new Color(.78f,0, .97f)},
            {CardRarity.Mythic, new Color(0.97f,.72f, 0)},
        };
    }
}
namespace ProjectEnigma.Cards
{
    [System.Serializable]

    public abstract class CardBase
    {
        public string Id;
        public string Name;
        public string Description;
        public CardImage Image;
        public CardRarity Rarity;
        public abstract CardType Type { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CardBase))
            {
                return false;
            }

            var other = (CardBase)obj;
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }

    public abstract class PlayableCard : CardBase
    {

    }

    public class Deck
    {
        public string Name;

    }

    [System.Serializable]
    public struct CardImage
    {
        public byte[] Data;
        public CardImage(Texture2D image)
        {
            Data = image.GetRawTextureData();
        }
        public Texture2D GetUIImage()
        {
            Texture2D texture = new Texture2D(630, 880);
            texture.LoadRawTextureData(Data);
            return texture;
        }
    }

    [System.Serializable]
    [Obsolete()]
public class CardPack
    {
        public List<MinionCard> MinionCards;
        public List<SpellCard> SpellCards;
        public List<KeyCard> KeyCards;
        public List<MagicCard> MagicCards;
    }
    [System.Serializable]
    public class MinionCard : PlayableCard
    {
        public bool JustPlaced;
        public int Attack;
        public int Defense;
        public List<CardAbility> Abilities;

        public override CardType Type { get => CardType.Hero; set => Type = value; }
    }
    [System.Serializable]
    public class SpellCard : PlayableCard
    {
        public override CardType Type { get => CardType.Spell; set => Type = value; }
        public List<CardAbility> Abilities;
    }
    [System.Serializable]
    public class KeyCard : CardBase
    {
        public override CardType Type { get => CardType.Key; set => Type = value; }
        public string DoorId;
    }
    [System.Serializable]
    public class MagicCard : CardBase
    {
        public override CardType Type { get => CardType.Magic; set => Type = value; }
        public string Id;
        public string Name;
    }
    [System.Serializable]
    public enum CardType
    {
        Hero,
        Spell,
        Key,
        Magic
    }

    [System.Serializable]
    public enum CardRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Mythic,
        Illusory
    }
    
    [System.Serializable]
    public enum AbilityTarget
    {
        Self_Person,
        Self_Card,
        Opponent_Person,
        Opponent_Card
    }
    public interface IModifer
    { 

    }

    [System.Serializable]
    public class AbilityModifier
    {

    }
    [System.Serializable]
    public class CardAbility
    {
        public string AbilityName;
        public string Description;
        public AbilityTarget Target;
        public List<AbilityModifier> AbilityModifiers;

    }
}

namespace ProjectEnigma.Info
{
    public class Config
    {
        public static string GAME_NAME = "Project Enigma";
        public static string GAME_DIR = Application.dataPath + "/";
        public static string SAVES_DIR = $"{GAME_DIR}Saves";
        public static string DATA_DIR = $"{GAME_DIR}Data";
        public static string CARDS_DIR = $"{DATA_DIR}/Cards";
        public static System.Version GAME_VERSION = new System.Version(0, 0, 1, 0);
    }
    public class Story
    { }

}
namespace ProjectEnigma.Data
{

    public class LaunchManager
    {
        public static void DataCheck()
        {
            if (!System.IO.Directory.Exists(Info.Config.GAME_DIR))
                System.IO.Directory.CreateDirectory(Info.Config.GAME_DIR);
            if (!System.IO.Directory.Exists(Info.Config.CARDS_DIR))
                System.IO.Directory.CreateDirectory(Info.Config.CARDS_DIR);
            if (!System.IO.Directory.Exists(Info.Config.SAVES_DIR))
                System.IO.Directory.CreateDirectory(Info.Config.SAVES_DIR);
        }
    }
    [System.Serializable]
    public class SaveData
    {
        public List<ProjectEnigma.Rooms.Room> rooms;

    }
    [System.Serializable]
    public class Save
    {
        public string Name;
        public System.DateTime LastPlayed;
        public SaveData Data;

    }
    [System.Serializable]
    public class SaveManager
    {
        public static List<Save> Saves;
        public static Save CurrentSave;

        public static List<Save> GetSaves()
        {
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(ProjectEnigma.Info.Config.SAVES_DIR);
            if (Saves is null)
                Saves = new List<Save>();
            foreach (var file in d.GetFiles("*.save"))
            {
                Saves.Add(ReadSaveFile(file.FullName));
            }
            return Saves;
        }
        public static Save ReadSaveFile(string filePath)
        {
            Save save = new Save();
            using (System.IO.Stream stream = System.IO.File.Open(filePath, System.IO.FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                save = (Save)bin.Deserialize(stream);
            }

            return save;
        }
        public static void SaveGame()
        {
            WriteSaveFile(CurrentSave);
        }
        public static void WriteSaveFile(Save save)
        {
            using (System.IO.Stream stream = System.IO.File.Open($"{Info.Config.SAVES_DIR}/{save.Name}.save", System.IO.FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, save);
            }
        }
    }
}

