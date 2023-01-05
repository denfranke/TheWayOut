using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountUnitsManager : MonoBehaviour
{
    public static AccountUnitsManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance" + transform + " " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public List<UnitCard> GetUnitsOnAccount()
    {
        List<UnitCard> unitCards = new List<UnitCard>();
        unitCards.Add(CreateUnitCard("Warrior"));
        unitCards.Add(CreateUnitCard("Shooter"));
        unitCards.Add(CreateUnitCard("Warrior"));
        unitCards.Add(CreateUnitCard("Shooter"));
        unitCards.Add(CreateUnitCard("Warrior"));
        unitCards.Add(CreateUnitCard("Warrior"));
        unitCards.Add(CreateUnitCard("Shooter"));
        unitCards.Add(CreateUnitCard("Warrior"));
        unitCards.Add(CreateUnitCard("Shooter"));
        unitCards.Add(CreateUnitCard("Warrior"));

        return unitCards;
    }

    private Sprite CreateSprite(string unitName)
    {
        string pathToImage = "UnitCardImages/" + unitName;
        Texture2D texture = Resources.Load<Texture2D>(pathToImage);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        return sprite;
    }

    private UnitCard CreateUnitCard(string unitName)
    {
        UnitCard unitCard = new UnitCard
        {
            name = unitName,
            sprite = CreateSprite(unitName)
        };

        return unitCard;
    }
}
