using EsperClass.Items;
using EsperClass.Projectiles;
using EsperClass.Buffs;
using EsperClass.UI;
using Microsoft.Xna.Framework;
﻿using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace EsperClass
{
	public class EsperClass : Mod
	{
		public static Mod Instance;
		public static List<int> TKItem = new List<int>();
		public static List<int> TKProjectile = new List<int>();

		public EsperClass()
		{
			Properties = new ModProperties()
			{
				Autoload = true
			};
		}

		public override void AddRecipeGroups()
		{
			RecipeGroup group = new RecipeGroup(() => "Copper or Tin Bar", new int[] { ItemID.CopperBar, ItemID.TinBar }); RecipeGroup.RegisterGroup("EsperClass:Tier1Bar", group);
			group = new RecipeGroup(() => "Silver or Tungsten Bar", new int[] { ItemID.SilverBar, ItemID.TungstenBar }); RecipeGroup.RegisterGroup("EsperClass:Tier3Bar", group);
			group = new RecipeGroup(() => "Gold or Platinum Bar", new int[] { ItemID.GoldBar, ItemID.PlatinumBar }); RecipeGroup.RegisterGroup("EsperClass:Tier4Bar", group);
			group = new RecipeGroup(() => "Demonite or Crimtane Bar", new int[] { ItemID.DemoniteBar, ItemID.CrimtaneBar }); RecipeGroup.RegisterGroup("EsperClass:Tier5Bar", group);
		}

		public override void Load()
		{
			Instance = this;
			//TKItem = TKItem;
			ECUI.Initialize();
		}

		public override void Unload()
		{
			// Unload static references
			// You need to clear static references to assets (Texture2D, SoundEffects, Effects).
			// In addition to that, if you want your mod to completely unload during unload, you need to clear static references to anything referencing your Mod class
			Instance = null;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			ECUI.ModifyInterfaceLayers(layers);
		}

		public override void PostSetupContent()
    {
	    Mod LootBags = ModLoader.GetMod("LootBags");
	    if (LootBags != null)
	    {
				//Player player = Main.player[Main.myPlayer];
				//if (ECPlayer.ModPlayer(player).maxPsychosis > 10)
				//{
		      LootBags.Call(.05, ItemType("TKBoulder"), 20, 30, 1);
			    LootBags.Call(.05, ItemType("MoltenBoulder"), 20, 30, 2);
				//}
	    }
			Main.NewText("Size: " + TKItem.Count, 255, 192, 203);
    }

		//Based on jopojelly's Boss Checklist mod
		public override object Call(params object[] args)
		{
			Player player = Main.player[Main.myPlayer];
			try
			{
				string message = args[0] as string;
				if (message == "IncreaseTelekinesisDamage")
				{
					float damageAmount = Convert.ToSingle(args[1]);
					return ECPlayer.ModPlayer(player).tkDamage += damageAmount;
				}
				else if (message == "IncreaseTelekinesisCrit")
				{
					float critAmount = Convert.ToSingle(args[1]);
					return ECPlayer.ModPlayer(player).tkCrit += (int)critAmount;
				}
				else if (message == "IncreaseTelekinesisVelocity")
				{
					float velAmount = Convert.ToSingle(args[1]);
					return ECPlayer.ModPlayer(player).tkVel += velAmount;
				}
				else if (message == "UseTelekinesisVelocity")
				{
					return Convert.ToSingle(ECPlayer.ModPlayer(player).tkVel);
				}
				else if (message == "AddTKItem")
				{
					float IsTKItem = Convert.ToSingle(args[1]);
					TKItem.Add((int)IsTKItem);
					return "Success";
				}
				else if (message == "AddTKProjectile")
				{
					float IsTKProjectile = Convert.ToSingle(args[1]);
					TKProjectile.Add((int)IsTKProjectile);
					return "Success";
				}
			}
			catch (Exception e)
			{
				ErrorLogger.Log("Esper Class Call Error: " + e.StackTrace + e.Message);
			}
			return "Failure";
		}
	}
}
