using EsperClass.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace EsperClass
{
	public class ECWorld : ModWorld
	{
		// Adapted from Example Mod's code
		public override void PostWorldGen()
		{
			int skyPlaced = 0;
			//int shadowPlaced = 0;
			int[] itemsToPlaceInLockedGoldenChests = { mod.ItemType("DungeonSawblade"), mod.ItemType("DungeonCanister") };
			int itemsToPlaceInGoldenChestsChoice = 0;
			for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
			{
				Chest chest = Main.chest[chestIndex];
				if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers)
				{
					// Golden Chests
					if (Main.tile[chest.x, chest.y].frameX == 1 * 36 && Main.rand.Next(3) == 0)
					{
						for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
						{
							if (chest.item[inventoryIndex].type == 0)
							{
								chest.item[inventoryIndex].SetDefaults(mod.ItemType("BatJar"));
								break;
							}
						}
					}
					// Skyware Chests
					if (Main.tile[chest.x, chest.y].frameX == 13 * 36 && skyPlaced < 3)
					{
						for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
						{
							if (chest.item[inventoryIndex].type == 0)
							{
								chest.item[inventoryIndex].SetDefaults(mod.ItemType("FeatherGust"));
								skyPlaced++;
								break;
							}
						}
					}
					// Locked Golden Chests
					if (Main.tile[chest.x, chest.y].frameX == 2 * 36 && Main.rand.Next(2) == 0)
					{
						for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
						{
							if (chest.item[inventoryIndex].type == 0)
							{
								chest.item[inventoryIndex].SetDefaults(itemsToPlaceInLockedGoldenChests[itemsToPlaceInGoldenChestsChoice]);
								itemsToPlaceInGoldenChestsChoice = (itemsToPlaceInGoldenChestsChoice + 1) % itemsToPlaceInLockedGoldenChests.Length;
								break;
							}
						}
					}
					// Locked Shadow Chests
					if (Main.tile[chest.x, chest.y].frameX == 4 * 36 && Main.rand.Next(2) == 0)
					{
						for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
						{
							if (chest.item[inventoryIndex].type == 0)
							{
								chest.item[inventoryIndex].SetDefaults(mod.ItemType("HellbatJar"));
								break;
							}
						}
					}
				}
			}
		}
	}
}
