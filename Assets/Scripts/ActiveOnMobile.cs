using System.Runtime.InteropServices;
using UnityEngine;

public class ActiveOnMobile : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool IsMobileBrowser();
#endif
    void Start()
    {
        bool isMobile = false;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            isMobile = GetMobileBrowserStatusJS();
        }
        else
        {
            isMobile = Application.platform == RuntimePlatform.Android ||
                   Application.platform == RuntimePlatform.IPhonePlayer;
        }
        if (isMobile)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private bool GetMobileBrowserStatusJS()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            return IsMobileBrowser();
        #else
            return false;
        #endif
    }

}
