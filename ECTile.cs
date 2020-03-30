using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace EsperClass
{
	public class ECTile : GlobalTile
	{
		public override bool Drop(int i, int j, int type)
		{
			if (type == TileID.ShadowOrbs && Main.tile[i, j].frameX >= 0 && Main.tile[i, j].frameX <= 16
			&& (Main.tile[i, j].frameY >= 0 && Main.tile[i, j].frameY <= 16 || Main.tile[i, j].frameY >= 32 && Main.tile[i, j].frameY <= 48)
			&& WorldGen.shadowOrbSmashed && Main.rand.Next(2) == 0)
			{
				Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("ShadowOrbit"));
			}
			if (type == TileID.ShadowOrbs && Main.tile[i, j].frameX >= 32 && Main.tile[i, j].frameX <= 48
			&& (Main.tile[i, j].frameY >= 0 && Main.tile[i, j].frameY <= 16 || Main.tile[i, j].frameY >= 32 && Main.tile[i, j].frameY <= 48)
			&& WorldGen.shadowOrbSmashed && Main.rand.Next(2) == 0)
			{
				Item.NewItem(i * 16, j * 16, 32, 32, mod.ItemType("ClotBomber"));
			}
			return true;
		}
	}
}
