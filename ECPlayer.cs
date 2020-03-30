using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EsperClass
{
	public class ECPlayer : ModPlayer
	{
		public float tkDamage = 1f;
		public int tkCrit = 0;
		public float tkVel = 1f;
		public bool willfulPotion = false;
		public bool alertPotion = false;
		public bool focusedPotion = false;
		public float psychosis = 10;
		public int maxPsychosis = 10;
		public int maxPsychosis2 = 0;
		public float psychosisRec = 1f;
		public bool psychosisBlock = false;
		public int psychosisDelay = 0;
		public int psychosisDelay2 = 0; //Used to force display the psychosis bar when full
		public float tkDodge = 0f;
		public bool overPsychosis = false;
		public bool psychicEyeMagnet = false;
		public bool psychosisWarning = false;

		public bool fireVial = false;
		public bool frostburnVial = false;
		public bool poisonVial = false;
		public bool midasVial = false;
		public bool cursedFlamesVial = false;
		public bool ichorVial = false;
		public bool shadowflameVial = false;
		public bool venomVial = false;

		public bool accMaxPsychosis1 = false;
		public bool accPsychosisRec1 = false;
		public bool accTkDodge1 = false;
		public bool accMaxPsychosis2 = false;
		public bool accPsychosisRec2 = false;
		public bool accTkDodge2 = false;
		public bool accMaxPsychosis3 = false;
		public bool accPsychosisRec3 = false;
		public bool accTkDodge3 = false;
		public bool accMaxPsychosis4 = false;
		public bool accPsychosisRec4 = false;
		public bool accTkDodge4 = false;
		public bool accMaxPsychosis5 = false;
		public bool accPsychosisRec5 = false;
		public bool accTkDodge5 = false;

		public static ECPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<ECPlayer>();
		}

		public override void ResetEffects()
		{
			ResetEsperVariables();
		}

		public override void UpdateDead()
		{
			ResetEsperVariables();
		}

		private void ResetEsperVariables()
		{
			tkDamage = 1f;
			tkCrit = 0;
			tkVel = 1f;
			willfulPotion = false;
			alertPotion = false;
			focusedPotion = false;
			psychosisBlock = false;
			maxPsychosis2 = 0;
			psychosisRec = 1f;
			tkDodge = 0f;
			fireVial = false;
			frostburnVial = false;
			poisonVial = false;
			midasVial = false;
			cursedFlamesVial = false;
			ichorVial = false;
			shadowflameVial = false;
			venomVial = false;
			//overPsychosis = false;
			psychicEyeMagnet = false;

			accMaxPsychosis1 = false;
			accPsychosisRec1 = false;
			accTkDodge1 = false;
			accMaxPsychosis2 = false;
			accPsychosisRec2 = false;
			accTkDodge2 = false;
			accMaxPsychosis3 = false;
			accPsychosisRec3 = false;
			accTkDodge3 = false;
			accMaxPsychosis4 = false;
			accPsychosisRec4 = false;
			accTkDodge4 = false;
			accMaxPsychosis5 = false;
			accPsychosisRec5 = false;
			accTkDodge5 = false;
		}

		public override void clientClone(ModPlayer clientClone)
		{
			ECPlayer clone = clientClone as ECPlayer;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			//packet.Write(exampleLifeFruits);
			packet.Send(toWho, fromWho);
		}

		public override TagCompound Save()
		{
			return new TagCompound { {"psychosis", psychosis}, {"maxPsychosis", maxPsychosis}, };
		}

		public override void Load(TagCompound tag)
		{
			psychosis = tag.GetFloat("psychosis");
			maxPsychosis = tag.GetInt("maxPsychosis");
		}

		public override void PostUpdateMiscEffects()
		{
			if (willfulPotion)
				tkDamage += 0.2f;
			if (alertPotion)
				tkVel += 0.3f;
			if (focusedPotion)
				tkCrit += 12;
			if (accMaxPsychosis1)
				maxPsychosis2 += 3;
			if (accMaxPsychosis2)
				maxPsychosis2 += 6;
			if (accMaxPsychosis3)
				maxPsychosis2 += 9;
			if (accMaxPsychosis4)
				maxPsychosis2 += 12;
			if (accMaxPsychosis5)
				maxPsychosis2 += 15;
			if (accPsychosisRec1)
				psychosisRec += 0.2f;
			if (accPsychosisRec2)
				psychosisRec += 0.4f;
			if (accPsychosisRec3)
				psychosisRec += 0.6f;
			if (accPsychosisRec4)
				psychosisRec += 0.8f;
			if (accPsychosisRec5)
				psychosisRec += 1f;
			if (accTkDodge1)
				tkDodge += 0.05f;
			if (accTkDodge2)
				tkDodge += 0.1f;
			if (accTkDodge3)
				tkDodge += 0.15f;
			if (accTkDodge4)
				tkDodge += 0.2f;
			if (accTkDodge5)
				tkDodge += 0.25f;
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (Main.rand.NextFloat() <= tkDodge && !player.HasBuff(mod.BuffType("LessStepsAhead")))
			{
				TKDodge();
				return false;
			}
			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (overPsychosis && hitDirection == 0 && damageSource.SourceOtherIndex == 8)
			{
				switch (Main.rand.Next(5))
				{
					case 0:
						damageSource = PlayerDeathReason.ByCustomReason(player.name + " has ascended to a higher plane.");
						break;
					case 1:
						damageSource = PlayerDeathReason.ByCustomReason(player.name + " succumbed to madness.");
						break;
					case 2:
						damageSource = PlayerDeathReason.ByCustomReason(player.name + " was consumed by visions.");
						break;
					case 3:
						damageSource = PlayerDeathReason.ByCustomReason(player.name + "s' head inexplicably exploded.");
						break;
					case 4:
						damageSource = PlayerDeathReason.ByCustomReason(player.name + " has seen too much.");
						break;
				}
			}
			return true;
		}

		public void TKDodge()
		{
			player.AddBuff(mod.BuffType("LessStepsAhead"), 300);
			player.immune = true;
			player.immuneTime = 80;
			if (player.longInvince)
			{
				player.immuneTime += 40;
			}
			for (int i = 0; i < player.hurtCooldowns.Length; i++)
			{
				player.hurtCooldowns[i] = player.immuneTime;
			}
			for (int j = 0; j < 20; j++)
			{
				int num = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 86, 0f, 0f, 100, default(Color), 2f);
				Dust dust = Main.dust[num];
				dust.position.X = dust.position.X + (float)Main.rand.Next(-20, 21);
				Dust dust2 = Main.dust[num];
				dust2.position.Y = dust2.position.Y + (float)Main.rand.Next(-20, 21);
				Main.dust[num].velocity *= 0.4f;
				Main.dust[num].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(player.cWaist, player);
				Main.dust[num].noGravity = true;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num].scale *= 1f + (float)Main.rand.Next(40) * 0.01f;
				}
			}
			if (player.whoAmI == Main.myPlayer)
			{
				NetMessage.SendData(62, -1, -1, null, player.whoAmI, 1f, 0f, 0f, 0, 0, 0);
			}
		}

		//A way to make global damage and critical increases work
		public override void PostUpdate()
		{
			float addedDamage = 1000f;
			if (addedDamage > Main.player[player.whoAmI].meleeDamage)
				addedDamage = Main.player[player.whoAmI].meleeDamage;
			if (addedDamage > Main.player[player.whoAmI].rangedDamage)
				addedDamage = Main.player[player.whoAmI].rangedDamage;
			if (addedDamage > Main.player[player.whoAmI].thrownDamage)
				addedDamage = Main.player[player.whoAmI].thrownDamage;
			if (addedDamage > Main.player[player.whoAmI].magicDamage)
				addedDamage = Main.player[player.whoAmI].magicDamage;
			if (addedDamage > Main.player[player.whoAmI].minionDamage)
				addedDamage = Main.player[player.whoAmI].minionDamage;
			if (addedDamage < 1f)
				addedDamage = 1f;
			tkDamage += (addedDamage - 1f);

			int addedCrit = 1000;
			if (addedCrit > Main.player[player.whoAmI].meleeCrit)
				addedCrit = Main.player[player.whoAmI].meleeCrit;
			if (addedCrit > Main.player[player.whoAmI].rangedCrit)
				addedCrit = Main.player[player.whoAmI].rangedCrit;
			if (addedCrit > Main.player[player.whoAmI].thrownCrit)
				addedCrit = Main.player[player.whoAmI].thrownCrit;
			if (addedCrit > Main.player[player.whoAmI].magicCrit)
				addedCrit = Main.player[player.whoAmI].magicCrit;
			if (addedCrit < 0)
				addedCrit = 0;
			tkCrit += addedCrit;
			if (psychosis > TotalPsychosis())
			{
				psychosis = TotalPsychosis();
			}
			if (!psychosisBlock)
			{
				if (psychosisDelay > 0)
				{
					psychosisDelay--;
				}
				if (psychosisDelay <= 0 && !PsychosisFull())
				{
					PsychosisRegen();
					if (PsychosisFull())
					{
						psychosisDelay2 = 60;
					}
				}
			}
			if (psychosis < -1f && !player.HasBuff(mod.BuffType("PsychedOut")))
			{
				player.AddBuff(mod.BuffType("PsychedOut"), 600);
				psychosis = TotalPsychosis();
				psychosisDelay = 0;
				psychosisWarning = false;
			}
			if (psychosisDelay2 > 0)
				psychosisDelay2--;
			if (overPsychosis)
			{
				int newDust = Dust.NewDust(new Vector2(player.position.X, player.position.Y - (player.gravDir * 20)), player.width, player.height, 86, 0f, -2f * player.gravDir, 50, default(Color), 1.5f);
				Main.dust[newDust].noGravity = true;
				Main.dust[newDust].velocity *= 1.4f;
			}
		}

		public int TotalPsychosis()
		{
			return maxPsychosis + maxPsychosis2;
		}

		public void PsychosisRegen()
		{
			psychosis = Math.Min(TotalPsychosis(), psychosis += ((TotalPsychosis() / 10) / 20f) * psychosisRec);
			if (psychosis >= 0f && psychosisWarning)
				psychosisWarning = false;
		}

		public bool PsychosisFull()
		{
			return psychosis >= TotalPsychosis();
		}

		public override void UpdateBadLifeRegen()
		{
			if (overPsychosis)
			{
				if (player.lifeRegen > 0)
					player.lifeRegen = 0;
				player.lifeRegenTime = 0;
				player.lifeRegen -= 20;
			}
		}

		//This function should not be used to reduce psychosis
		public void PsychosisRestore(float amount, bool showNum = true)
		{
			int numDisplay = (int)Math.Ceiling(amount);
			if (amount < 0f)
			{
				if (!PsychosisFull())
				{
					psychosisDelay2 = 60;
				}
				psychosis = TotalPsychosis();
				numDisplay = TotalPsychosis();
			}
			else
			{
				if (psychosis + amount >= TotalPsychosis())
				{
					psychosisDelay2 = 60;
				}
				psychosis = Math.Min(TotalPsychosis(), psychosis += amount);
			}
			if (Main.myPlayer == player.whoAmI && showNum)
			{
				player.HealEffect(numDisplay, true);
			}
			if (psychosis >= 0f && psychosisWarning)
				psychosisWarning = false;
		}

		//Amount should be drain per second, not per frame
		public void PsychosisDrain(float amount, bool delayRegen = true, bool bypassDebuff = false)
		{
			if (player.HasBuff(mod.BuffType("PsychedOut")) && !bypassDebuff)
				return;
			if (delayRegen)
				psychosisDelay = 60;
			psychosisBlock = true;
			psychosis -= amount / 60f;
			if (psychosis <= 0f && !psychosisWarning)
			{
				Main.PlaySound(23, (int)player.position.X, (int)player.position.Y);
				psychosisWarning = true;
			}
		}

		public override void CatchFish(Item fishingRod, Item bait, int power, int liquidType, int poolSize, int worldLayer, int questFish, ref int caughtType, ref bool junk)
		{
			if (junk)
				return;
			if (liquidType == 0 && !(caughtType >= ItemID.WoodenCrate && caughtType <= ItemID.GoldenCrate)
			&& !(caughtType >= ItemID.CorruptFishingCrate && caughtType <= ItemID.JungleFishingCrate))
			{
				int[] chance = {30, 1500};
				if (player.ZoneSandstorm)
				{
					chance[0] = 15;
					chance[1] = 750;
				}
				if (Main.rand.Next(Math.Max(chance[0], chance[1] / power)) == 0 && player.ZoneDesert && worldLayer == 1 && !(player.position.X / 16 < 340 || player.position.X / 16 > Main.maxTilesX - 340))
				{
					caughtType = mod.ItemType("SpittingSandfish");
				}
			}
		}
	}
}
