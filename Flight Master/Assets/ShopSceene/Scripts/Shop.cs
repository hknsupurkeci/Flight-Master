﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Shop : MonoBehaviour
{
    #region Singlton:Shop

    public static Shop Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    [System.Serializable] public class ShopItem
	{
		public int id;
		public string name;
		public Sprite Image;
		public int Price;
		public bool IsPurchased = false;
		public int speedXStartPrice;
		public int fuelStartPrice;
		public float speedX;
		public float fuel;
	}

	public List<ShopItem> ShopItemsList;
	[SerializeField] Animator NoCoinsAnim;
 
	[SerializeField] GameObject ItemTemplate;
	GameObject g;
	[SerializeField] Transform ShopScrollView;
	[SerializeField] GameObject ShopPanel;
	Button buyBtn;

	void Start ()
	{
		//PlayerPrefs.DeleteAll();
        foreach (var item in ShopItemsList)
        {
			
			if(PlayerPrefs.GetInt("IsPurchased"+item.id, 0) == 1)
            {
				Profile.Instance.AddAvatar(item.Image);
				item.IsPurchased = true;
			}
        }
		int len = ShopItemsList.Count;
		for (int i = 0; i < len; i++) {
			g = Instantiate (ItemTemplate, ShopScrollView);
			g.transform.GetChild (0).GetComponent <Image> ().sprite = ShopItemsList [i].Image;
			g.transform.GetChild (1).GetChild (0).GetComponent <Text> ().text = ShopItemsList [i].Price.ToString ();
			buyBtn = g.transform.GetChild (2).GetComponent <Button> ();
			if (ShopItemsList [i].IsPurchased) {
				DisableBuyButton ();
			}
			buyBtn.AddEventListener (i, OnShopItemBtnClicked);
		}
	}

	void OnShopItemBtnClicked (int itemIndex)
	{
		if (Game.Instance.HasEnoughCoins (ShopItemsList [itemIndex].Price)) {
			Game.Instance.UseCoins (ShopItemsList [itemIndex].Price);
			//purchase Item
			ShopItemsList [itemIndex].IsPurchased = true;
			PlayerPrefs.SetInt("IsPurchased" + ShopItemsList[itemIndex].id, 1);

			PlayerPrefs.SetFloat(ShopItemsList[itemIndex].name + "speedX", ShopItemsList[itemIndex].speedX);
			PlayerPrefs.SetFloat(ShopItemsList[itemIndex].name + "maxValue", ShopItemsList[itemIndex].fuel);
			PlayerPrefs.SetFloat(ShopItemsList[itemIndex].name + "speedMoney", ShopItemsList[itemIndex].speedXStartPrice);
			PlayerPrefs.SetFloat(ShopItemsList[itemIndex].name + "fuelMoney", ShopItemsList[itemIndex].fuelStartPrice);

			Debug.Log("IsPurchased" + ShopItemsList[itemIndex].id);
			//disable the button
			buyBtn = ShopScrollView.GetChild (itemIndex).GetChild (2).GetComponent <Button> ();
			DisableBuyButton ();
			//change UI text: coins
			Game.Instance.UpdateAllCoinsUIText ();

			//add avatar
			Profile.Instance.AddAvatar (ShopItemsList [itemIndex].Image);
		} else {
			NoCoinsAnim.SetTrigger ("NoCoins");
			Debug.Log ("You don't have enough coins!!");
		}
	}

	void DisableBuyButton ()
	{
		buyBtn.interactable = false;
		buyBtn.transform.GetChild (0).GetComponent <Text> ().text = "PURCHASED";
	}
	/*-------------2--------Open & Close shop--------------------------*/
	public void OpenShop ()
	{
		ShopPanel.SetActive (true);
	}

	public void CloseShop ()
	{
		ShopPanel.SetActive (false);
	}

}
