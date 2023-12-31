﻿using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Fire_Control_Input_01_Desktop_CS : Fire_Control_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE

        public override void Get_Input()
        {
            string key = Key_Bindings_CS.IsFireKeyPressing();
            if (!key.Equals("empty"))
            {
                fireControlScript.Fire(key);
            }
        }
#endif
    }

}