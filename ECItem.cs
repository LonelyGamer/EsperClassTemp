using System;
﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Events;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace EsperClass
{
	public abstract class ECItem : ModItem
	{
		public bool onlyOne = true;

		public override bool CloneNewInstances
		{
			get
			{
				return true;
			}
		}

		public override void SetDefaults()
		{
			item.channel = true;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;
			item.noUseGraphic = true;
			item.noMelee = true;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			//damage = (int)(damage * ECPlayer.ModPlayer(player).tkDamage + 5E-06f);
			mult *= ECPlayer.ModPlayer(player).tkDamage;
		}

		public override void GetWeaponKnockback(Player player, ref float knockback)
		{
			knockback = knockback;
		}

		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit = crit + ECPlayer.ModPlayer(player).tkCrit;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " telekinesis " + damageWord;
			}
		}

		public override int ChoosePrefix(UnifiedRandom rand)
		{
			int prefix = 0;
		  if (item.maxStack > 1)
		  {
		    return -1;
		  }
			int rando = Main.rand.Next(14);
			if (rando == 0)
			{
				prefix = 36;
			}
			if (rando == 1)
			{
				prefix = 37;
			}
			if (rando == 2)
			{
				prefix = 38;
			}
			if (rando == 3)
			{
				prefix = 53;
			}
			if (rando == 4)
			{
				prefix = 54;
			}
			if (rando == 5)
			{
				prefix = 55;
			}
			if (rando == 6)
			{
				prefix = 39;
			}
			if (rando == 7)
			{
				prefix = 40;
			}
			if (rando == 8)
			{
				prefix = 56;
			}
			if (rando == 9)
			{
				prefix = 41;
			}
			if (rando == 10)
			{
				prefix = 57;
			}
			if (rando == 11)
			{
				prefix = 59;
			}
			if (rando == 12)
			{
				prefix = 60;
			}
			if (rando == 13)
			{
				prefix = 61;
			}
			return prefix;
		}

		public override void HoldStyle(Player player)
		{
			ECPlayer.ModPlayer(player).overPsychosis = false;
			if (player.channel)
			{
				//if (ECPlayer.ModPlayer(player).psychosis > 0f)
				//{
				float amount = 1f;
				if (!onlyOne)
					amount = 3f;
				ECPlayer.ModPlayer(player).PsychosisDrain(amount);
				//}
				if (player.HasBuff(mod.BuffType("PsychedOut")))
				{
					//player.channel = false;
					ECPlayer.ModPlayer(player).overPsychosis = true;
				}
			}
		}

    public override bool CanUseItem(Player player)
		{
			//if (item.useTime == 15)
			if (onlyOne)
			{
				for (int m = 0; m < 1000; m++)
				{
					if (Main.projectile[m].active && Main.projectile[m].owner == player.whoAmI && Main.projectile[m].type == item.shoot)
						return false;
				}
			}
			/*if (player.HasBuff(mod.BuffType("PsychedOut")))
			{
				return false;
			}*/
			return base.CanUseItem(player);
		}

		/*public override bool UseItem(Player player)
		{
			ECPlayer.ModPlayer(player).psychosis--;
			return base.UseItem(player);
		}*/
	}

	public class ECItem2 : GlobalItem
	{
		public override void SetDefaults(Item item)
		{
			if (DetectPositives(item))
			{
				item.melee = false;
				item.ranged = false;
				item.magic = false;
				item.summon = false;
				item.thrown = false;
				item.sentry = false;
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
		{
			if (DetectPositives(item))
			{
				mult *= ECPlayer.ModPlayer(player).tkDamage;
			}
		}

		public override void GetWeaponKnockback(Item item, Player player, ref float knockback)
		{
			if (DetectPositives(item))
				knockback = knockback;
		}

		public override void GetWeaponCrit(Item item, Player player, ref int crit)
		{
			if (DetectPositives(item))
			{
				crit = crit + ECPlayer.ModPlayer(player).tkCrit;
			}
		}

		public override void HoldStyle(Item item, Player player)
		{
			if (DetectPositives(item))
			{
				ECPlayer.ModPlayer(player).overPsychosis = false;
				if (player.channel)
				{
					ECPlayer.ModPlayer(player).PsychosisDrain(1f);
					if (player.HasBuff(mod.BuffType("PsychedOut")))
						ECPlayer.ModPlayer(player).overPsychosis = true;
				}
			}
		}

    public override bool CanUseItem(Item item, Player player)
		{
			if (DetectPositives(item))
			{
				/*if (player.HasBuff(mod.BuffType("PsychedOut")))
				{
					return false;
				}*/
			}
			return base.CanUseItem(item, player);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (DetectPositives(item))
			{
				TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
				if (tt != null)
				{
					string[] splitText = tt.text.Split(' ');
					string damageValue = splitText.First();
					string damageWord = splitText.Last();
					tt.text = damageValue + " telekinesis " + damageWord;
				}
			}
		}

		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			if (DetectPositives(item))
			{
				int prefix = 0;
			  if (item.maxStack > 1)
			  {
			    return -1;
			  }
				int rando = Main.rand.Next(14);
				if (rando == 0)
				{
					prefix = 36;
				}
				if (rando == 1)
				{
					prefix = 37;
				}
				if (rando == 2)
				{
					prefix = 38;
				}
				if (rando == 3)
				{
					prefix = 53;
				}
				if (rando == 4)
				{
					prefix = 54;
				}
				if (rando == 5)
				{
					prefix = 55;
				}
				if (rando == 6)
				{
					prefix = 39;
				}
				if (rando == 7)
				{
					prefix = 40;
				}
				if (rando == 8)
				{
					prefix = 56;
				}
				if (rando == 9)
				{
					prefix = 41;
				}
				if (rando == 10)
				{
					prefix = 57;
				}
				if (rando == 11)
				{
					prefix = 59;
				}
				if (rando == 12)
				{
					prefix = 60;
				}
				if (rando == 13)
				{
					prefix = 61;
				}
				return prefix;
			}
			return base.ChoosePrefix(item, rand);
		}

		public bool DetectPositives(Item item)
		{
			if (item.type == ItemID.FlyingKnife)
				return true;

			List<int> TKItem = EsperClass.TKItem;
			foreach (var IsTKItem in EsperClass.TKItem)
			{
				if (item.type == IsTKItem)
					return true;
			}

			//if (item.type == (float)mod.Call("IsTKItem"))
			//	return true;
			return false;
		}
	}
}
