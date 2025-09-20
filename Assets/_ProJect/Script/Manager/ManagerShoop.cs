using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum PowerUp {None,DoubleLife, Shield }

[Serializable]
public class ButtonShop
{
    public Button button;
    public TextMeshProUGUI textCost;
    public int cost;
    public GameObject imageCost;

    public PowerUp powerUp;
    public bool hasPowerUp;
}

public class ManagerShoop : GenericSingleton<ManagerShoop>
{
    [Header("Setting")]
    [SerializeField] private List<ButtonShop> shops;
    [SerializeField] private AudioClip soundButton;

    private void Start()
    {
        foreach (ButtonShop shop in shops)
        {
            shop.textCost.text = shop.cost.ToString();
            shop.hasPowerUp = ManagerGame.Instance.GetSaveData().hasPowerUp(shop.powerUp);
            if (shop.hasPowerUp) DisableShopItem(shop);
        }
    }

    public void DoubleLife() => Buy(PowerUp.DoubleLife);
    public void Shield() => Buy(PowerUp.Shield);

    private void Buy(PowerUp powerUp)
    {
        ManagerAudio.Instance.PlayFXSound(soundButton, 0.1f);
        ButtonShop shop = shops.Find(x => x.powerUp == powerUp);
        if (shop == null) return;

        if (ManagerGame.Instance.GetMoney() >= shop.cost)
        {
            SelectPowerUp(shop);
            DisableShopItem(shop);
        }
    }

    private void SelectPowerUp(ButtonShop shop)
    {
        ManagerGame.Instance.UpDateCoin(-shop.cost);

        ManagerGame.Instance.SavePowerUp(shop.powerUp);
    }

    private void DisableShopItem(ButtonShop shop)
    {
        shop.button.interactable = false;
        shop.imageCost.SetActive(false);
    }
}
