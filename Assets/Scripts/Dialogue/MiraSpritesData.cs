using System;
using System.Collections.Generic;
using UnityEngine;

public class MiraSpritesData : MonoBehaviour
{
    [SerializeField] private SpritesDatabase[] spritesDatabaseArray;
    private Dictionary<string, Sprite> spriteDatabase;
    
    private static MiraSpritesData instance;

    void Start() {
        if (instance == null) {
            instance = this;
            spriteDatabase = new Dictionary<string, Sprite>();
            foreach (var i in spritesDatabaseArray) {
                spriteDatabase.Add(i.ID, i.image);
            }
        }
    }

    public static MiraSpritesData GetInstance() {
        return instance;
    }
    public Sprite GetSprite(string spriteID) {
        if (spriteDatabase.TryGetValue(spriteID, out Sprite sprite)) {
            return sprite;
        }
        else {
            return null;
        }
    }





    [Serializable]
    public struct SpritesDatabase {
        public string ID;
        public Sprite image;
    }
}
