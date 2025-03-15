using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace EnglishPatch;

/// <summary>
/// Put dicey stuff in here that might crash the plugin - so it doesnt crash the existing plugins
/// </summary>
[BepInPlugin($"{MyPluginInfo.PLUGIN_GUID}.Debug", "DebugGame", MyPluginInfo.PLUGIN_VERSION)]
internal class DebugPlugin: BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        Logger = base.Logger;

        Harmony.CreateAndPatchAll(typeof(DebugPlugin));
        Logger.LogWarning($"Debug Game Plugin should be patched!");
    }

    private void Test()
    {
        /*
         * [Error  :XUnity.Common] An error occurred while invoking AssetLoaded event.
System.NullReferenceException
  at (wrapper managed-to-native) UnityEngine.Object.GetName(UnityEngine.Object)
  at UnityEngine.Object.get_name () [0x00001] in <c6f9c541975c45798261d77d99bb6eb2>:0 
  at EnglishPatch.Sprites.SpriteReplacerPlugin.ReplaceSpriteInAsset (System.String parentAssetName, UnityEngine.UI.Image child) [0x00020] in <f89debbaca4946c798a611fd74046fa9>:0 
  at EnglishPatch.Sprites.SpriteReplacerPlugin.OnAssetLoaded (XUnity.ResourceRedirector.AssetLoadedContext context) [0x000f6] in <f89debbaca4946c798a611fd74046fa9>:0 
  at XUnity.ResourceRedirector.ResourceRedirection.FireAssetLoadedEvent (XUnity.ResourceRedirector.AssetLoadedParameters parameters, UnityEngine.AssetBundle assetBundle, UnityEngine.Object[]& assets) [0x001db] in <2aa225b7d50341e7b2dc1bfd1a8d4bf7>:0 
         */

        /*
         
        Stack trace:
UnityEngine.Debug:LogException(Exception)
QuestPrototype:DMD<QuestPrototype::GetInfoById>(Int64)
SweetPotato.PlayerQuest:IsQuestFinished(Int64)
AnswerViewNew:CheckConditionTask(AnswerCondition)
AnswerViewNew:MatchCount(AnswerCondition[], List`1, Boolean)
AnswerViewNew:IsMatchAnswerExtraConditionsAll(AnswerCondition[], List`1)
AnswerViewNew:IsMatchAnswerExtraConditions(String, Int32, List`1)
ShopStoreData:CheckConditionItemAndRefresh()
ShopStoreSystem:OnEnterShopMode(Int64, Int64)
SweetPotato.GrowInfo:DMD<SweetPotato.GrowInfo::OnEntrustFinish>(GrowInfo, VirtualEvent, Boolean)
GrowManager:OnEntrustFinish(VirtualEvent, Boolean)
VirtualEvent:DoFinish(Boolean)
VirtualEventManager:FinishVirtualEvent(VirtualEvent, Boolean)
VirtualEvent:Update()
VirtualEventManager:ManagerUpdate()
SweetPotato.WorldManager:Update()

         */
    }

    //// Opening Screen
    //[HarmonyPrefix, HarmonyPatch(typeof(SweetPotato.LoginViewNew), "OnButtonClick")]
    //public static void Postfix_OnButtonClick()
    //{
    //    Logger.LogWarning($"Hooked POSTFIX OnButtonClick!");
    //}

    ////[HarmonyPostfix, HarmonyPatch(typeof(SweetPotato.LoginViewNew), "OnButtonClick")]
    ////public static void Postfix_OnButtonClick(IEnumerable<CodeInstruction> __instructions)
    ////{
    ////    InstructionLogger.LogInstructions(__instructions);
    ////}

    //[HarmonyPostfix, HarmonyPatch(typeof(SweetPotato.LoginViewNew), "OnButtonNewGame")]
    //public static void Postfix_LoginViewNew_OnButtonNewGame()
    //{
    //    Logger.LogWarning("Hooked OnButtonNewGame!");
    //}
}

public class InstructionLogger
{
    public static void LogInstructions(MethodBase method)
    {
        DebugPlugin.Logger.LogFatal($"Logging instructions for method: {method.Name}");

        //var instructions = PatchProcessor.GetOriginalInstructions(method);
        var instructions = PatchProcessor.GetCurrentInstructions(method);
        foreach (var instruction in instructions)
        {
            DebugPlugin.Logger.LogFatal($"Instruction: {instruction.opcode} {instruction.operand}");
        }
    }

    public static void LogInstructions(IEnumerable<CodeInstruction> instructions)
    {
        DebugPlugin.Logger.LogFatal("Logging modified instructions:");
        foreach (var instruction in instructions)
        {
            DebugPlugin.Logger.LogFatal($"Instruction: {instruction.opcode} {instruction.operand}");
        }
    }
}
