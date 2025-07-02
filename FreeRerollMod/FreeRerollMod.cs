using Landfall.Haste;
using Landfall.Modding;
using MonoMod.RuntimeDetour;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization;
using Zorro.Settings;

namespace FreeRerollMod
{
    [LandfallPlugin]
    public class FreeRerollMod
    {
        public static int initialRerollCost;
        public static int incrementRerollCost;
        public static int maxRerollCost;
        
        static FreeRerollMod()
        {
 
            On.GM_Shop.Awake += (orig, self) =>
            {
                orig(self);
                GM_Shop.instance.rerollCost = initialRerollCost;
                

            };
            
            On.RerollShop.Interact += (orig, self) => 
            {
                orig(self);
                if(Player.localPlayer.data.resource >= (float)GM_Shop.instance.rerollCost) {
                    
                    GM_Shop.instance.rerollCost = Mathf.Min(GM_Shop.instance.rerollCost + incrementRerollCost, maxRerollCost);
                    var text = self.priceTextParnet.GetComponentInChildren<TextMeshPro>();
                    text.text = GM_Shop.instance.rerollCost.ToString();
                }

            };
        }

        public static void SetInitialRerollCost(int value) => initialRerollCost = value;
        public static void SetIncrementRerollCost(int value) => incrementRerollCost = value;
        public static void SetMaxRerollCost(int value) => maxRerollCost = value;
    }


   

    // The HasteSetting attribute is equivalent to
    // GameHandler.Instance.SettingsHandler.AddSetting(new HelloSetting());
    [HasteSetting]
    public class InitialRerollCostSetting : IntSetting, IExposedSetting
    {
        public override void ApplyValue() => FreeRerollMod.SetInitialRerollCost(Value);
        protected override int GetDefaultValue() => 0;
        public LocalizedString GetDisplayName() => new UnlocalizedString("Initial Reroll Cost");
        public string GetCategory() => SettingCategory.General;
    }

    [HasteSetting]
    public class IncrementRerollCostSetting : IntSetting, IExposedSetting
    {
        public override void ApplyValue() => FreeRerollMod.SetIncrementRerollCost(Value);
        protected override int GetDefaultValue() => 150;
        public LocalizedString GetDisplayName() => new UnlocalizedString("Reroll Cost Increase");
        public string GetCategory() => SettingCategory.General;
    }

    [HasteSetting]
    public class MaxRerollCostSetting : IntSetting, IExposedSetting
    {
        public override void ApplyValue() => FreeRerollMod.SetMaxRerollCost(Value);
        protected override int GetDefaultValue() => 1000;
        public LocalizedString GetDisplayName() => new UnlocalizedString("Max Reroll Cost");
        public string GetCategory() => SettingCategory.General;
    }
}
