//#if !UNITY_EDITOR

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
 
//[Preserve]
public class SkipUnityLogo
{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void BeforeSplashScreen()
    {
        //System.Threading.Tasks.Task.Run(AsyncSkip);
        AsyncSkip();
// #if UNITY_WEBGL
//         Application.focusChanged += Application_focusChanged;
// #else
//         System.Threading.Tasks.Task.Run(AsyncSkip);
// #endif
    }
 

    private static void Application_focusChanged(bool obj)
    {
        Application.focusChanged -= Application_focusChanged;
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }

    private static async void AsyncSkip()
    {
        await Task.Yield();
        SplashScreen.Stop(SplashScreen.StopBehavior.StopImmediate);
    }

}
//#endif