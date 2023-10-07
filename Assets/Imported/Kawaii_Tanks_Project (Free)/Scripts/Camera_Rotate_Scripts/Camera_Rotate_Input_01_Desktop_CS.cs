using UnityEngine;


namespace ChobiAssets.KTP
{

    public class Camera_Rotate_Input_01_Desktop_CS : Camera_Rotate_Input_00_Base_CS
    {
#if !UNITY_ANDROID && !UNITY_IPHONE || PLATFORM_WEBGL

        public override void Get_Input()
        {
            //Debug.Log("Camera_Rotate_Input_01_Desktop_CS ()()");
            /*
            // Cancel while the cursor is displayed.
            if (Cursor.lockState == CursorLockMode.None)
            {
                cameraRotateScript.rotationInput = Vector3.zero;
                return;
            }
            */

            // Get the input.
            cameraRotateScript.rotationInput = Key_Bindings_CS.GetCameraRotationAxis() * General_Settings_CS.cameraRotationSensibility;
        }

#endif
    }

}