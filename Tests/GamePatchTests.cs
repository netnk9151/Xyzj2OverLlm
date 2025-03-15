using HarmonyLib;
using System;
using System.Reflection;
using Xunit;

namespace Translate.Tests;

public class MainGameAwakePatchTests : IDisposable
{
    private Assembly _gameAssembly;
    private Harmony _harmony;

    public MainGameAwakePatchTests()
    {
        // Setup - runs before each test
        _gameAssembly = Assembly.LoadFrom(@"G:/SteamLibrary/steamapps/common/下一站江湖Ⅱ/下一站江湖Ⅱ/下一站江湖Ⅱ_Data/Managed/Assembly-CSharp.dll");
        _harmony = new Harmony("com.mytest.awake.patch");

        // Apply patches from our assembly (will find all Harmony attributes)
        _harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    public void Dispose()
    {
        // Cleanup - runs after each test
        _harmony.UnpatchAll("com.mytest.awake.patch");
    }

    //[Fact]
    //public void Patch_MainGame_Awake_Method()
    //{
    //    // Create instance and test
    //    object mainGameInstance = Activator.CreateInstance(_mainGameType);

    //    // Call the method (this would trigger our patches)
    //    _awakeMethod.Invoke(mainGameInstance, null);

    //    // Verify our patch was executed
    //    Assert.True(_awakeWasCalled, "The Awake method patch was not called");
    //}

    // Patch class using Harmony attributes
    [HarmonyPatch]
    public static class AwakePatch
    {
        private static bool _awakeWasCalled;

        // Define which method to patch
        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPatch("MainGame", "Awake")]
        [HarmonyPrefix]
        public static bool AwakePrefix(object __instance)
        {
            Console.WriteLine("Awake method is about to be called!");
            return true; // Continue to the original method
        }

        [HarmonyPatch(MethodType.Normal)]
        [HarmonyPatch("MainGame", "Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(object __instance)
        {
            Console.WriteLine("Awake method was called!");
            _awakeWasCalled = true;
        }
    }
}
