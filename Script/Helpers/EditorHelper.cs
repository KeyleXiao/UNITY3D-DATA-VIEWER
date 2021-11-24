using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

namespace SmartDataViewer.Helpers
{
    public class EditorHelper
    {
        // *****************************************************************************
        // Handle Support
        // *****************************************************************************

        public static void DrawWireSphere(Vector3 rPosition, float rRadius, Color rColor)
        {
            Color lHandlesColor = Handles.color;
            Handles.color = rColor;

            Handles.DrawWireArc(rPosition, Vector3.forward, Vector3.up, 360f, rRadius);
            Handles.DrawWireArc(rPosition, Vector3.up, Vector3.forward, 360f, rRadius);
            Handles.DrawWireArc(rPosition, Vector3.right, Vector3.up, 360f, rRadius);

            Handles.color = lHandlesColor;
        }

        /// <summary>
        /// Draws a circle that always faces the scene camera
        /// </summary>
        /// <param name="rPosition"></param>
        /// <param name="rRadius"></param>
        /// <param name="rColor"></param>
        public static void DrawCircle(Vector3 rPosition, float rRadius, Color rColor)
        {
            Camera lCamera = SceneView.lastActiveSceneView.camera;

            Color lHandleColor = Handles.color;
            Handles.color = rColor;

            Vector3 lNormal = -lCamera.transform.forward;
            Handles.DrawSolidDisc(rPosition, lNormal, rRadius);

            Handles.color = lHandleColor;
        }

        /// <summary>
        /// Draws text on the scene
        /// </summary>
        /// <param name="rText"></param>
        /// <param name="rPosition"></param>
        /// <param name="rColor"></param>
        public static void DrawText(string rText, Vector3 rPosition, Color rColor)
        {
            Color lGUIColor = GUI.color;
            GUI.color = rColor;
            Handles.color = rColor;

            Handles.Label(rPosition, rText);

            GUI.color = lGUIColor;
            Handles.color = lGUIColor;
        }

        // *****************************************************************************
        // Field Support
        // *****************************************************************************

        public static string LastPath = "";

        public static string FieldStringValue = "";

        public static bool FieldBoolValue = false;

        public static float FieldFloatValue = 0f;

        public static int FieldIntValue = 0;

        public static Vector2 FieldVector2Value = Vector2.zero;

        public static Vector3 FieldVector3Value = Vector3.zero;

        public static Quaternion FieldQuaternionValue = Quaternion.identity;

        public static UnityEngine.Object FieldObjectValue = null;

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static void LabelField(string rTitle, string rTip, float rMinWidth = 0)
        {
            if (rMinWidth <= 0)
            {
                EditorGUILayout.LabelField(new GUIContent(rTitle, rTip));
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent(rTitle, rTip), GUILayout.Width(rMinWidth));
            }
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool TextField(string rTitle, string rTip, string rValue, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldStringValue = EditorGUILayout.TextField(new GUIContent(rTitle, rTip), rValue);
            }
            else
            {
                FieldStringValue = EditorGUILayout.TextField(new GUIContent(rTitle, rTip), rValue, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool BoolField(bool rValue, string rTitle = "", UnityEngine.Object rRecorder = null, float rWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rWidth <= 0)
            {
                FieldBoolValue = EditorGUILayout.Toggle(rValue);
            }
            else
            {
                FieldBoolValue = EditorGUILayout.Toggle(rValue, GUILayout.Width(rWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rTitle.Length > 0 && rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool BoolField(string rTitle, string rTip, bool rValue, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldBoolValue = EditorGUILayout.Toggle(new GUIContent(rTitle, rTip), rValue);
            }
            else
            {
                FieldBoolValue = EditorGUILayout.Toggle(new GUIContent(rTitle, rTip), rValue, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool FloatField(string rTitle, string rTip, float rValue, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldFloatValue = EditorGUILayout.FloatField(new GUIContent(rTitle, rTip), rValue);
            }
            else
            {
                FieldFloatValue = EditorGUILayout.FloatField(new GUIContent(rTitle, rTip), rValue, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool FloatField(float rValue, string rTitle = "", UnityEngine.Object rRecorder = null, float rWidth = 0, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rWidth <= 0)
            {
                if (rMinWidth <= 0)
                {
                    FieldFloatValue = EditorGUILayout.FloatField(rValue);
                }
                else
                {
                    FieldFloatValue = EditorGUILayout.FloatField(rValue, GUILayout.MinWidth(rMinWidth));
                }
            }
            else
            {
                FieldFloatValue = EditorGUILayout.FloatField(rValue, GUILayout.Width(rWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rTitle.Length > 0 && rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool IntField(string rTitle, string rTip, int rValue, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldIntValue = EditorGUILayout.IntField(new GUIContent(rTitle, rTip), rValue);
            }
            else
            {
                FieldIntValue = EditorGUILayout.IntField(new GUIContent(rTitle, rTip), rValue, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool IntField(int rValue, string rTitle = "", UnityEngine.Object rRecorder = null, float rWidth = 0, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rWidth <= 0)
            {
                if (rMinWidth <= 0)
                {
                    FieldIntValue = EditorGUILayout.IntField(rValue);
                }
                else
                {
                    FieldIntValue = EditorGUILayout.IntField(rValue, GUILayout.MinWidth(rMinWidth));
                }
            }
            else
            {
                FieldIntValue = EditorGUILayout.IntField(rValue, GUILayout.Width(rWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rTitle.Length > 0 && rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool PopUpField(string rTitle, string rTip, int rValue, string[] rValues, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldIntValue = EditorGUILayout.Popup(rTitle, rValue, rValues);
            }
            else
            {
                FieldIntValue = EditorGUILayout.Popup(rTitle, rValue, rValues, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool Vector2Field(string rTitle, string rTip, Vector2 rValue, UnityEngine.Object rRecorder = null)
        {
            bool lIsDirty = false;

            FieldVector2Value = rValue;

            EditorGUILayout.BeginHorizontal();

            EditorHelper.LabelField(rTitle, rTip + " Values as x, y.", EditorGUIUtility.labelWidth - 4f);

            EditorGUI.BeginChangeCheck();
            FieldVector2Value.x = EditorGUILayout.FloatField(FieldVector2Value.x, GUILayout.MinWidth(31));
            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                lIsDirty = true;
            }

            EditorGUI.BeginChangeCheck();
            FieldVector2Value.y = EditorGUILayout.FloatField(FieldVector2Value.y, GUILayout.MinWidth(31));
            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                lIsDirty = true;
            }

            EditorGUILayout.EndHorizontal();

            return lIsDirty;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool Vector3Field(string rTitle, string rTip, Vector3 rValue, UnityEngine.Object rRecorder = null)
        {
            bool lIsDirty = false;

            FieldVector3Value = rValue;

            EditorGUILayout.BeginHorizontal();

            EditorHelper.LabelField(rTitle, rTip + " Values as x, y, z.", EditorGUIUtility.labelWidth - 4f);

            EditorGUI.BeginChangeCheck();
            FieldVector3Value.x = EditorGUILayout.FloatField(FieldVector3Value.x, GUILayout.MinWidth(28));
            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                lIsDirty = true;
            }

            EditorGUI.BeginChangeCheck();
            FieldVector3Value.y = EditorGUILayout.FloatField(FieldVector3Value.y, GUILayout.MinWidth(28));
            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                lIsDirty = true;
            }

            EditorGUI.BeginChangeCheck();
            FieldVector3Value.z = EditorGUILayout.FloatField(FieldVector3Value.z, GUILayout.MinWidth(28));
            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                lIsDirty = true;
            }

            EditorGUILayout.EndHorizontal();

            return lIsDirty;
        }
        
        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool QuaternionField(string rTitle, string rTip, Quaternion rValue, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            Vector3 lValue = rValue.eulerAngles;

            if (rMinWidth <= 0)
            {
                Vector3 lEuler = EditorGUILayout.Vector3Field(new GUIContent(rTitle, rTip), lValue);
                FieldVector3Value = lEuler;
                FieldQuaternionValue = Quaternion.Euler(lEuler);
            }
            else
            {
                Vector3 lEuler = EditorGUILayout.Vector3Field(new GUIContent(rTitle, rTip), lValue, GUILayout.MinWidth(rMinWidth));
                FieldVector3Value = lEuler;
                FieldQuaternionValue = Quaternion.Euler(lEuler);
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool ObjectField<T>(string rTitle, string rTip, T rValue, UnityEngine.Object rRecorder = null, float rMinWidth = 0) where T : UnityEngine.Object
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldObjectValue = EditorGUILayout.ObjectField(new GUIContent(rTitle, rTip), rValue, typeof(T), true);
            }
            else
            {
                FieldObjectValue = EditorGUILayout.ObjectField(new GUIContent(rTitle, rTip), rValue, typeof(T), true, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Simplified way to create a text field
        /// </summary>
        /// <returns></returns>
        public static bool ObjectField(string rTitle, string rTip, UnityEngine.Object rValue, Type rValueType, UnityEngine.Object rRecorder = null, float rMinWidth = 0)
        {
            EditorGUI.BeginChangeCheck();

            if (rMinWidth <= 0)
            {
                FieldObjectValue = EditorGUILayout.ObjectField(new GUIContent(rTitle, rTip), rValue, rValueType, true);
            }
            else
            {
                FieldObjectValue = EditorGUILayout.ObjectField(new GUIContent(rTitle, rTip), rValue, rValueType, true, GUILayout.MinWidth(rMinWidth));
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Primarily used for interfaces where we know the interface will be a ScriptableObject or other Unity object
        /// </summary>
        /// <returns>Determines if a change has occurred</returns>
        public static bool ScriptableObjectField(string rTitle, string rTip, object rValue, Type rValueType, UnityEngine.Object rRecorder = null)
        {
            if (rValue != null && !(rValue is ScriptableObject)) { return false; }

            ScriptableObject lTemplate = rValue as ScriptableObject;

            EditorGUI.BeginChangeCheck();

            FieldObjectValue = EditorGUILayout.ObjectField(new GUIContent(rTitle, rTip), lTemplate, rValueType, true);

            if (FieldObjectValue != null && !ReflectionHelper.IsAssignableFrom(rValueType, FieldObjectValue.GetType()))
            {
                FieldObjectValue = null;
            }

            if (EditorGUI.EndChangeCheck())
            {
                if (rRecorder != null) { Undo.RecordObject(rRecorder, "Set " + rTitle); }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Shows a file selection field and button
        /// </summary>
        /// <param name="rLabel"></param>
        /// <param name="rValue"></param>
        /// <returns></returns>
        public static string FileSelect(GUIContent rLabel, string rValue, string rFileTypes)
        {
            EditorGUILayout.BeginHorizontal();

            string lNewValue = EditorGUILayout.TextField(rLabel, rValue);
            if (lNewValue != rValue)
            {
                rValue = lNewValue;
            }

            if (GUILayout.Button(new GUIContent("...", "Select resource"), EditorStyles.miniButton, GUILayout.Width(20)))
            {
                lNewValue = EditorUtility.OpenFilePanel("Select the file", LastPath, rFileTypes);
                if (lNewValue.Length != 0)
                {
                    LastPath = lNewValue;

                    int lStartResource = lNewValue.IndexOf("Resources");
                    if (lStartResource >= 0)
                    {
                        lNewValue = lNewValue.Substring(lStartResource + 10);
                    }

                    lStartResource = lNewValue.IndexOf("Assets");
                    if (lStartResource >= 0)
                    {
                        lNewValue = lNewValue.Substring(lStartResource + 7);
                    }

                    int lStartExtension = lNewValue.LastIndexOf(".");
                    if (lStartExtension > 0)
                    {
                        lNewValue = lNewValue.Substring(0, lStartExtension);
                    }

                    if (lNewValue != rValue)
                    {
                        rValue = lNewValue;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            return rValue;
        }

        /// <summary>
        /// Renders out the editor GUI field based on the type
        /// </summary>
        public static object ObjectField(GUIContent rLabel, object rValue, Type rType)
        {
            object lNewValue = rValue;

            if (rType == typeof(bool))
            {
                lNewValue = EditorGUILayout.Toggle(rLabel, (bool)rValue);
            }
            else if (rType == typeof(int))
            {
                lNewValue = EditorGUILayout.IntField(rLabel, (int)rValue);
            }
            else if (rType == typeof(float))
            {
                lNewValue = EditorGUILayout.FloatField(rLabel, (float)rValue);
            }
            else if (rType == typeof(string))
            {
                lNewValue = EditorGUILayout.TextField(rLabel, (string)rValue);
            }
            else if (rType == typeof(Vector2))
            {
                lNewValue = EditorGUILayout.Vector2Field(rLabel, (Vector2)rValue);
            }
            else if (rType == typeof(Vector3))
            {
                lNewValue = EditorGUILayout.Vector3Field(rLabel, (Vector3)rValue);
            }
            else if (rType == typeof(Vector4))
            {
                lNewValue = EditorGUILayout.Vector4Field(rLabel.text, (Vector4)rValue);
            }
            else if (rType == typeof(Quaternion))
            {
                Vector3 lEuler = ((Quaternion)rValue).eulerAngles;
                lEuler = EditorGUILayout.Vector3Field(new GUIContent(rLabel.text, rLabel.tooltip + " Use pitch(x), yaw(y), and roll(z)."), lEuler);

                lNewValue = Quaternion.Euler(lEuler);
            }
            else if (rType == typeof(Transform))
            {
                lNewValue = EditorGUILayout.ObjectField(rLabel, (Transform)rValue, typeof(Transform), true) as Transform;
            }
            else if (rType == typeof(GameObject))
            {
                lNewValue = EditorGUILayout.ObjectField(rLabel, (GameObject)rValue, typeof(GameObject), true) as GameObject;
            }
            else if (rType == typeof(UnityEngine.Object))
            {
                lNewValue = EditorGUILayout.ObjectField(rLabel, (UnityEngine.Object)rValue, typeof(UnityEngine.Object), true) as object;
            }
            else
            {
                lNewValue = rValue;
            }

            return lNewValue;
        }

        /// <summary>
        /// Allows us to show an object field that supports interfaces
        /// </summary>
        /// <returns>Returns an object that is of the valid type or null</returns>
        public static GameObject InterfaceOwnerField<T>(GUIContent rLabel, GameObject rValue, bool rAllowSceneObjects, params GUILayoutOption[] rOptions)
        {
            GameObject lGameObject = EditorGUILayout.ObjectField(rLabel, rValue, typeof(GameObject), rAllowSceneObjects, rOptions) as GameObject;
            if (lGameObject != null)
            {
                if (InterfaceHelper.GetComponent<T>(lGameObject) == null)
                {
                    lGameObject = null;
                }
            }

            return lGameObject;
        }

        /// Holds the list of all the current layers
        private static string[] sLayerNames = null;
        private static int[] sLayerValues = null;

        /// <summary>
        /// Renders out the layer mask selection box for us
        /// 
        /// Props to Bunny83
        /// http://answers.unity3d.com/questions/60959/mask-field-in-the-editor.html
        /// </summary>
        /// <param name="rGUIContent"></param>
        /// <param name="rMask"></param>
        /// <param name="rOptions"></param>
        /// <returns></returns>
        public static int LayerMaskField(GUIContent rGUIContent, int rMask, params GUILayoutOption[] rOptions)
        {
            int lValue = rMask;
            int lMaskValue = 0;

            if (sLayerNames == null) { RefreshLayers(); }

            for (int i = 0; i < sLayerNames.Length; i++)
            {
                if (sLayerValues[i] != 0)
                {
                    if ((lValue & sLayerValues[i]) == sLayerValues[i])
                        lMaskValue |= 1 << i;
                }
                else if (lValue == 0)
                    lMaskValue |= 1 << i;
            }

            int lNewMaskVal = EditorGUILayout.MaskField(rGUIContent, lMaskValue, sLayerNames, rOptions);
            int lChanges = lMaskValue ^ lNewMaskVal;

            for (int i = 0; i < sLayerValues.Length; i++)
            {
                if ((lChanges & (1 << i)) != 0)            // has this list item changed?
                {
                    if ((lNewMaskVal & (1 << i)) != 0)     // has it been set?
                    {
                        if (sLayerValues[i] == 0)           // special case: if "0" is set, just set the val to 0
                        {
                            lValue = 0;
                            break;
                        }
                        else
                            lValue |= sLayerValues[i];
                    }
                    else                                  // it has been reset
                    {
                        lValue &= ~sLayerValues[i];
                    }
                }
            }

            return lValue;
        }

        /// <summary>
        /// Reload the layer list. We may need to do this every so often
        /// </summary>
        public static void RefreshLayers()
        {
            List<string> lLayerNames = new List<string>();
            List<int> lLayerValues = new List<int>();

            for (int i = 0; i < 32; i++)
            {
                try
                {
                    string lName = LayerMask.LayerToName(i);
                    if (lName != "")
                    {
                        lLayerNames.Add(lName);
                        lLayerValues.Add(1 << i);
                    }
                }
                catch { }
            }

            sLayerNames = lLayerNames.ToArray();
            sLayerValues = lLayerValues.ToArray();
        }
       
        
        // *****************************************************************************
        // Style Support
        // *****************************************************************************
        public static Texture2D InheritedBackground;
        public static Texture2D InstanceBackground;
        public static Texture2D GreenPlusButton;
        public static Texture2D BluePlusButton;

        public static GUIStyle InstanceStyle;
        public static GUIStyle InheritedStyle;
        public static GUIStyle IndexStyle;
        public static GUIStyle GreenPlusButtonStyle;
        public static GUIStyle BlueXButtonStyle;
        public static GUIStyle BluePlusButtonStyle;

        public static Color Border = new Color(84f / 255f, 84f / 255f, 84f / 255f, 1f);
        public static Color LightBorder = new Color(124f / 255f, 124f / 255f, 124f / 255f, 1f);
        public static Color LightOrange = new Color(241f / 255f, 156f / 255f, 117f / 255f, 1f);
        public static Color OverrideBlue = new Color(202f / 255f, 209f / 255f, 220f / 255f, 1f);
        public static Color InheritedGray = new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);

        /// <summary>
        /// Renders the inspector title for our asset
        /// </summary>
        public static void DrawSmallTitle(string rTitle)
        {
            EditorGUILayout.BeginHorizontal(EditorHelper.SmallBox);

            GUILayout.Space(5);

            EditorGUILayout.LabelField(rTitle, EditorHelper.SmallTitle);

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Renders the inspector title for our asset
        /// </summary>
        public static void DrawInspectorTitle(string rTitle)
        {
            EditorGUILayout.BeginHorizontal(EditorHelper.TitleBox);

            EditorGUILayout.LabelField("", ootiiIcon, GUILayout.Width(24f), GUILayout.Height(24f));

            GUILayout.Space(5);

            EditorGUILayout.LabelField(rTitle, EditorHelper.Title);

            GUILayout.FlexibleSpace();

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Renders the inspector title for our asset
        /// </summary>
        public static void DrawInspectorDescription(string rDescription, MessageType rMessageType)
        {
            Color lGUIColor = GUI.color;

            GUI.color = CreateColor(242f, 164f, 130f, 0.8f);
            EditorGUILayout.HelpBox(rDescription, rMessageType);

            GUI.color = lGUIColor;
        }

        /// <summary>
        /// Renders the link in our inspector
        /// </summary>
        public static void DrawLink(string rTitle, string rURL)
        {
            Color lGUIColor = GUI.color;

            if (GUILayout.Button(rTitle, LinkLabel))
            {
                Application.OpenURL(rURL);
            }

            GUI.color = lGUIColor;
        }

        /// <summary>
        /// Renders a simple line to the inspector
        /// </summary>
        public static void DrawLine()
        {
            EditorGUILayout.BeginHorizontal(EditorHelper.Line);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Label with small text
        /// </summary>
        private static GUIStyle mReadOnlyLabel = null;
        public static GUIStyle ReadOnlyLabel
        {
            get
            {
                if (mReadOnlyLabel == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/ClearBox" : "Editor/ClearBox");

                    mReadOnlyLabel = new GUIStyle(GUI.skin.label);
                    mReadOnlyLabel.normal.background = lTexture;
                    mReadOnlyLabel.border = new RectOffset(3, 3, 2, 2);
                }

                return mReadOnlyLabel;
            }
        }

        /// <summary>
        /// Label with small text
        /// </summary>
        private static GUIStyle mSmallText = null;
        public static GUIStyle SmallText
        {
            get
            {
                if (mSmallText == null)
                {
                    mSmallText = new GUIStyle(GUI.skin.label);
                    mSmallText.fontSize = 9;
                }

                return mSmallText;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mWindow = null;
        public static GUIStyle Window
        {
            get
            {
                if (mWindow == null)
                {
                    mWindow = new GUIStyle(GUI.skin.window);
                    mWindow.fontStyle = FontStyle.Bold;
                    mWindow.alignment = TextAnchor.UpperLeft;
                }

                return mWindow;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mBox = null;
        public static GUIStyle Box
        {
            get
            {
                if (mBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/OrangeGrayBox_pro" : "Editor/OrangeGrayBox");

                    mBox = new GUIStyle(GUI.skin.box);
                    mBox.normal.background = lTexture;
                    mBox.padding = new RectOffset(2, 2, 6, 6);
                    //mBox.margin = new RectOffset(0, 0, 0, 0);
                }

                return mBox;
            }
        }

        /// <summary>
        /// Waning box that contains warnings. White with red border
        /// </summary>
        private static GUIStyle mWarningBox = null;
        private static GUIStyle WarningBox
        {
            get
            {
                if (mWarningBox == null)
                {
                    Texture2D lTexture = EditorHelper.CreateTexture(16, 16, Color.white, Color.red);

                    mWarningBox = new GUIStyle(GUI.skin.box);
                    mWarningBox.normal.background = lTexture;
                    mWarningBox.padding = new RectOffset(0, 0, 0, 0);
                    mWarningBox.margin = new RectOffset(0, 0, 0, 0);
                }

                return mWarningBox;
            }
        }
        
        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mGoalAtomic = null;
        public static GUIStyle GoalAtomic
        {
            get
            {
                if (mGoalAtomic == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GoalAtomicSimple" : "Editor/GoalAtomicSimple");

                    mGoalAtomic = new GUIStyle(GUI.skin.label);
                    mGoalAtomic.normal.background = lTexture;
                    mGoalAtomic.border = new RectOffset(3, 3, 3, 3);
                    mGoalAtomic.normal.textColor = Color.black;
                    mGoalAtomic.alignment = TextAnchor.MiddleCenter;
                    mGoalAtomic.padding = new RectOffset(0, 0, -2, 0);
                }

                return mGoalAtomic;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mGoalComposite = null;
        public static GUIStyle GoalComposite
        {
            get
            {
                if (mGoalAtomic == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GoalCompositeSimple" : "Editor/GoalCompositeSimple");

                    mGoalComposite = new GUIStyle(GUI.skin.label);
                    mGoalComposite.normal.background = lTexture;
                    mGoalComposite.border = new RectOffset(3, 3, 3, 3);
                    mGoalComposite.normal.textColor = Color.black;
                    mGoalComposite.alignment = TextAnchor.MiddleCenter;
                    mGoalComposite.padding = new RectOffset(0, 0, -2, 0);
                }

                return mGoalComposite;
            }
        }

        /// <summary>
        /// Label with small text
        /// </summary>
        private static GUIStyle mSmallTitle = null;
        public static GUIStyle SmallTitle
        {
            get
            {
                if (mSmallTitle == null)
                {
                    Font lFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    if (lFont == null) { lFont = EditorStyles.standardFont; }

                    mSmallTitle = new GUIStyle(GUI.skin.label);
                    mSmallTitle.font = lFont;
                    mSmallTitle.fontSize = 12;
                    mSmallTitle.fontStyle = FontStyle.Bold;
                    mSmallTitle.normal.textColor = Color.white;
                    mSmallTitle.fixedHeight = 18f;
                    mSmallTitle.fixedWidth = 200f;
                    mSmallTitle.padding = new RectOffset(0, 0, 1, 0);
                }

                return mSmallTitle;
            }
        }

        /// <summary>
        /// Label with small text
        /// </summary>
        private static GUIStyle mTitle = null;
        public static GUIStyle Title
        {
            get
            {
                if (mTitle == null)
                {
                    Font lFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                    if (lFont == null) { lFont = EditorStyles.standardFont; }

                    mTitle = new GUIStyle(GUI.skin.label);
                    mTitle.font = lFont;
                    mTitle.fontSize = 14;
                    mTitle.fontStyle = FontStyle.Bold;
                    mTitle.normal.textColor = Color.white;
                    mTitle.fixedHeight = 22f;
                    mTitle.fixedWidth = 200f;
                    mTitle.padding = new RectOffset(0, 0, 4, 0);
                }

                return mTitle;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mSmallBox = null;
        public static GUIStyle SmallBox
        {
            get
            {
                if (mSmallBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/TitleBox" : "Editor/TitleBoxLight");

                    mSmallBox = new GUIStyle(GUI.skin.box);
                    mSmallBox.normal.background = lTexture;
                    mSmallBox.padding = new RectOffset(0, 0, 0, 0);
                }

                return mSmallBox;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mTitleBox = null;
        public static GUIStyle TitleBox
        {
            get
            {
                if (mTitleBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/TitleBox" : "Editor/TitleBox");

                    mTitleBox = new GUIStyle(GUI.skin.box);
                    mTitleBox.normal.background = lTexture;
                    mTitleBox.padding = new RectOffset(3, 3, 3, 3);
                }

                return mTitleBox;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mThinGroupBox = null;
        public static GUIStyle ThinGroupBox
        {
            get
            {
                if (mThinGroupBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GroupBox_pro" : "Editor/GroupBox");

                    mThinGroupBox = new GUIStyle(GUI.skin.box);
                    mThinGroupBox.normal.background = lTexture;
                    mThinGroupBox.padding = new RectOffset(2, 2, 2, 2);
                }

                return mThinGroupBox;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mGroupBox = null;
        public static GUIStyle GroupBox
        {
            get
            {
                if (mGroupBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GroupBox_pro" : "Editor/GroupBoxLight");

                    mGroupBox = new GUIStyle(GUI.skin.box);
                    mGroupBox.normal.background = lTexture;
                    mGroupBox.padding = new RectOffset(3, 3, 3, 3);
                }

                return mGroupBox;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mRedBox = null;
        public static GUIStyle RedBox
        {
            get
            {
                if (mRedBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/RedBox_pro" : "Editor/RedBox");

                    mRedBox = new GUIStyle(GUI.skin.box);
                    mRedBox.normal.background = lTexture;
                }

                return mRedBox;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mGreenBox = null;
        public static GUIStyle GreenBox
        {
            get
            {
                if (mGreenBox == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GreenBox_pro" : "Editor/GreenBox");

                    mGreenBox = new GUIStyle(GUI.skin.box);
                    mGreenBox.normal.background = lTexture;
                }

                return mGreenBox;
            }
        }

        /// <summary>
        /// Draws a rectangle in the specified color
        /// </summary>
        //private static Texture2D mWhitePixel = null;
        //private static GUIStyle mRectStyle = null;
        public static void DrawRect(Rect rRect, Color rColor)
        {
            EditorGUI.DrawRect(rRect, rColor);

            //if (mWhitePixel == null) { mWhitePixel = Resources.Load<Texture2D>("Editor/WhitePixel"); }
            //if (mRectStyle == null) { mRectStyle = new GUIStyle(); }

            //Color lColor = GUI.color;

            //GUI.color = rColor;
            //mRectStyle.normal.background = mWhitePixel;
            //GUI.Box(rPosition, GUIContent.none, mRectStyle);
            //GUI.color = lColor;
        }

        /// <summary>
        /// Draws a rectangle in the specified color
        /// </summary>
        public static void DrawBox(Rect rRect, Color rFillColor, Color rBorderColor)
        {
            EditorGUI.DrawRect(rRect, rFillColor);

            Rect lTop = new Rect(rRect.x, rRect.y, rRect.width, 1f);
            EditorGUI.DrawRect(lTop, rBorderColor);

            Rect lBottom = new Rect(rRect.x, rRect.y + rRect.height - 1f, rRect.width, 1f);
            EditorGUI.DrawRect(lBottom, rBorderColor);

            Rect lLeft = new Rect(rRect.x, rRect.y, 1f, rRect.height);
            EditorGUI.DrawRect(lLeft, rBorderColor);

            Rect lRight = new Rect(rRect.x + rRect.width - 1f, rRect.y, 1f, rRect.height);
            EditorGUI.DrawRect(lRight, rBorderColor);
        }

        /// <summary>
        /// Draws a rectangle in the specified color
        /// </summary>
        public static void DrawRoundedBox(Rect rRect, Color rFillColor, Color rBorderColor)
        {
            Rect lFill = new Rect(rRect.x + 1, rRect.y + 1, rRect.width - 2, rRect.height - 2);
            EditorGUI.DrawRect(lFill, rFillColor);

            Rect lTop = new Rect(rRect.x + 1, rRect.y, rRect.width - 2, 1f);
            EditorGUI.DrawRect(lTop, rBorderColor);

            Rect lBottom = new Rect(rRect.x + 1, rRect.y + rRect.height - 1f, rRect.width - 2, 1f);
            EditorGUI.DrawRect(lBottom, rBorderColor);

            Rect lLeft = new Rect(rRect.x, rRect.y + 1, 1f, rRect.height - 2);
            EditorGUI.DrawRect(lLeft, rBorderColor);

            Rect lRight = new Rect(rRect.x + rRect.width - 1f, rRect.y + 1, 1f, rRect.height - 2);
            EditorGUI.DrawRect(lRight, rBorderColor);
        }

        /// <summary>
        /// Status bar for the bottom window
        /// </summary>
        private static GUIStyle mStatusBar = null;
        public static GUIStyle StatusBar
        {
            get
            {
                if (mStatusBar == null)
                {
                    mStatusBar = new GUIStyle(GUI.skin.FindStyle("Toolbar"));
                    mStatusBar.fixedHeight = 21;
                }

                return mStatusBar;
            }
        }

        /// <summary>
        /// Box used to draw a solid line
        /// </summary>
        private static GUIStyle mLine = null;
        public static GUIStyle Line
        {
            get
            {
                if (mLine == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/Line_pro" : "Editor/Line");

                    mLine = new GUIStyle(GUI.skin.box);
                    mLine.border.top = 0;
                    mLine.border.left = 0;
                    mLine.border.right = 0;
                    mLine.border.bottom = 0;
                    mLine.padding.top = 0;
                    mLine.padding.left = 0;
                    mLine.padding.right = 0;
                    mLine.padding.bottom = 0;
                    mLine.fixedHeight = 8f;
                    mLine.normal.background = lTexture;
                }

                return mLine;
            }
        }

        /// <summary>
        /// Gray select target button
        /// </summary>
        private static GUIStyle mTinyButton = null;
        public static GUIStyle TinyButton
        {
            get
            {
                if (mTinyButton == null)
                {
                    mTinyButton = new GUIStyle(EditorStyles.miniButton);
                    mTinyButton.padding = new RectOffset(0, 0, 0, 0);
                    mTinyButton.margin = new RectOffset(0, 0, 0, 0);
                    mTinyButton.fixedHeight = 14;
                }

                return mTinyButton;
            }
        }

        /// <summary>
        /// Gray select target button
        /// </summary>
        private static GUIStyle mOrangeGearButton = null;
        public static GUIStyle OrangeGearButton
        {
            get
            {
                if (mOrangeGearButton == null)
                {
                    mOrangeGearButton = new GUIStyle();
                    mOrangeGearButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GearButtonOrange_pro" : "Editor/GearButtonOrange");
                    mOrangeGearButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mOrangeGearButton;
            }
        }

        /// <summary>
        /// Gray select target button
        /// </summary>
        private static GUIStyle mBlueGearButton = null;
        public static GUIStyle BlueGearButton
        {
            get
            {
                if (mBlueGearButton == null)
                {
                    mBlueGearButton = new GUIStyle();
                    mBlueGearButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/GearButtonBlue_pro" : "Editor/GearButtonBlue");
                    mBlueGearButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mBlueGearButton;
            }
        }

        /// <summary>
        /// Gray select target button
        /// </summary>
        private static GUIStyle mGraySelectButton = null;
        public static GUIStyle GraySelectButton
        {
            get
            {
                if (mGraySelectButton == null)
                {
                    mGraySelectButton = new GUIStyle();
                    mGraySelectButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/SelectButtonGray" : "Editor/SelectButtonGray");
                    mGraySelectButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mGraySelectButton;
            }
        }

        /// <summary>
        /// Gray add button
        /// </summary>
        private static GUIStyle mGrayAddButton = null;
        public static GUIStyle GrayAddButton
        {
            get
            {
                if (mGrayAddButton == null)
                {
                    mGrayAddButton = new GUIStyle();
                    mGrayAddButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/AddButtonGray" : "Editor/AddButtonGray");
                    mGrayAddButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mGrayAddButton;
            }
        }

        /// <summary>
        /// Gray delete button
        /// </summary>
        private static GUIStyle mGrayXButton = null;
        public static GUIStyle GrayXButton
        {
            get
            {
                if (mGrayXButton == null)
                {
                    mGrayXButton = new GUIStyle();
                    mGrayXButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/DeleteButtonGray" : "Editor/DeleteButtonGray");
                    mGrayXButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mGrayXButton;
            }
        }

        /// <summary>
        /// Blue select target button
        /// </summary>
        private static GUIStyle mBlueSelectButton = null;
        public static GUIStyle BlueSelectButton
        {
            get
            {
                if (mBlueSelectButton == null)
                {
                    mBlueSelectButton = new GUIStyle();
                    mBlueSelectButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/SelectButton" : "Editor/SelectButton");
                    mBlueSelectButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mBlueSelectButton;
            }
        }

        /// <summary>
        /// Blue add button
        /// </summary>
        private static GUIStyle mBlueAddButton = null;
        public static GUIStyle BlueAddButton
        {
            get
            {
                if (mBlueAddButton == null)
                {
                    mBlueAddButton = new GUIStyle();
                    mBlueAddButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/AddButtonBlue" : "Editor/AddButtonBlue");
                    mBlueAddButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mBlueAddButton;
            }
        }

        /// <summary>
        /// Blue select target button
        /// </summary>
        private static GUIStyle mBlueXButton = null;
        public static GUIStyle BlueXButton
        {
            get
            {
                if (mBlueXButton == null)
                {
                    mBlueXButton = new GUIStyle();
                    mBlueXButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/DeleteButtonBlue" : "Editor/DeleteButtonBlue");
                    mBlueXButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mBlueXButton;
            }
        }

        /// <summary>
        /// Red delete button
        /// </summary>
        public static GUIStyle mRedXButton = null;
        public static GUIStyle RedXButton
        {
            get
            {
                if (mRedXButton == null)
                {
                    mRedXButton = new GUIStyle();
                    mRedXButton.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/DeleteButton" : "Editor/DeleteButton");
                    mRedXButton.margin = new RectOffset(0, 0, 2, 0);
                }

                return mRedXButton;
            }
        }

        /// <summary>
        /// Blue select target button
        /// </summary>
        private static GUIStyle mLongButtonGreen = null;
        public static GUIStyle LongButtonGreen
        {
            get
            {
                if (mLongButtonGreen == null)
                {
                    mLongButtonGreen = new GUIStyle();
                    mLongButtonGreen.normal.background = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/LongButtonGreen" : "Editor/LongButtonGreen");
                    mLongButtonGreen.border = new RectOffset(3, 3, 3, 3);
                }

                return mLongButtonGreen;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        public static GUIStyle mLabel = null;
        public static GUIStyle Label
        {
            get
            {
                if (mLabel == null)
                {
                    mLabel = new GUIStyle(GUI.skin.label);
                }

                return mLabel;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        public static GUIStyle mWrapLabel = null;
        public static GUIStyle WrapLabel
        {
            get
            {
                if (mWrapLabel == null)
                {
                    mWrapLabel = new GUIStyle(GUI.skin.label);
                    mWrapLabel.wordWrap = true;
                }

                return mWrapLabel;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        public static GUIStyle mSelectedLabel = null;
        public static GUIStyle SelectedLabel
        {
            get
            {
                if (mSelectedLabel == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/BlueSelected" : "Editor/BlueSelected");

                    mSelectedLabel = new GUIStyle(GUI.skin.label);
                    mSelectedLabel.normal.textColor = Color.white;
                    mSelectedLabel.normal.background = lTexture;
                }

                return mSelectedLabel;
            }
        }

        /// <summary>
        /// Disabled Label
        /// </summary>
        public static GUIStyle mDisabledLabel = null;
        public static GUIStyle DisabledLabel
        {
            get
            {
                if (mDisabledLabel == null)
                {
                    mDisabledLabel = new GUIStyle(GUI.skin.label);
                    mDisabledLabel.normal.textColor = Color.gray;
                }

                return mDisabledLabel;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        public static GUIStyle mSmallLabel = null;
        public static GUIStyle SmallLabel
        {
            get
            {
                if (mSmallLabel == null)
                {
                    mSmallLabel = new GUIStyle(GUI.skin.label);
                    mSmallLabel.fontSize = 9;
                }

                return mSmallLabel;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        public static GUIStyle mSmallBoldLabel = null;
        public static GUIStyle SmallBoldLabel
        {
            get
            {
                if (mSmallBoldLabel == null)
                {
                    mSmallBoldLabel = new GUIStyle(GUI.skin.label);
                    mSmallBoldLabel.fontSize = 10;
                    mSmallBoldLabel.fontStyle = FontStyle.Bold;
                }

                return mSmallBoldLabel;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        public static GUIStyle mLinkLabel = null;
        public static GUIStyle LinkLabel
        {
            get
            {
                if (mLinkLabel == null)
                {
                    mLinkLabel = new GUIStyle(GUI.skin.label);
                    mLinkLabel.normal.textColor = Color.blue;
                    mLinkLabel.fontSize = 9;
                }

                return mLinkLabel;
            }
        }

        /// <summary>
        /// Label
        /// </summary>
        private static GUIStyle mOptionLabel = null;
        public static GUIStyle OptionLabel
        {
            get
            {
                if (mOptionLabel == null)
                {
                    mOptionLabel = new GUIStyle(GUI.skin.label);
                    mOptionLabel.wordWrap = true;
                    mOptionLabel.padding.top = 11;
                }

                return mOptionLabel;
            }
        }

        private static GUIStyle mValueStyle = null;
        public static GUIStyle ValueStyle
        {
            get
            {
                if (mValueStyle == null)
                {
                    mValueStyle = new GUIStyle(GUI.skin.label);
                    mValueStyle.alignment = TextAnchor.MiddleRight;
                }

                return mValueStyle;
            }
        }

        /// <summary>
        /// Scroll region
        /// </summary>
        public static GUIStyle mScrollArea = null;
        public static GUIStyle ScrollArea
        {
            get
            {
                if (mScrollArea == null)
                {
                    mScrollArea = new GUIStyle(GUI.skin.box);
                    mScrollArea.margin.left = 0;
                    mScrollArea.margin.right = 0;
                    mScrollArea.margin.top = 0;
                    mScrollArea.margin.bottom = 0;
                }

                return mScrollArea;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mootiiIcon = null;
        private static GUIStyle ootiiIcon
        {
            get
            {
                if (mootiiIcon == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "Editor/ootii_Icon" : "Editor/ootii_Icon");

                    mootiiIcon = new GUIStyle(GUI.skin.box);
                    mootiiIcon.normal.background = lTexture;
                    mootiiIcon.padding = new RectOffset(0, 0, 0, 0);
                    mootiiIcon.margin = new RectOffset(0, 0, 0, 0);
                }

                return mootiiIcon;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mBasicIcon = null;
        public static GUIStyle BasicIcon
        {
            get
            {
                if (mBasicIcon == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "BasicIcon_pro" : "BasicIcon");

                    mBasicIcon = new GUIStyle(GUI.skin.button);
                    mBasicIcon.normal.background = lTexture;
                    mBasicIcon.padding = new RectOffset(0, 0, 0, 0);
                    mBasicIcon.margin = new RectOffset(0, 0, 1, 0);
                    mBasicIcon.border = new RectOffset(0, 0, 0, 0);
                    mBasicIcon.stretchHeight = false;
                    mBasicIcon.stretchWidth = false;

                }

                return mBasicIcon;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mAdvancedIcon = null;
        public static GUIStyle AdvancedIcon
        {
            get
            {
                if (mAdvancedIcon == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "AdvancedIcon_pro" : "AdvancedIcon");

                    mAdvancedIcon = new GUIStyle(GUI.skin.button);
                    mAdvancedIcon.normal.background = lTexture;
                    mAdvancedIcon.padding = new RectOffset(0, 0, 0, 0);
                    mAdvancedIcon.margin = new RectOffset(0, 0, 1, 0);
                    mAdvancedIcon.border = new RectOffset(0, 0, 0, 0);
                    mAdvancedIcon.stretchHeight = false;
                    mAdvancedIcon.stretchWidth = false;

                }

                return mAdvancedIcon;
            }
        }

        /// <summary>
        /// Box used to group standard GUI elements
        /// </summary>
        private static GUIStyle mDebugIcon = null;
        public static GUIStyle DebugIcon
        {
            get
            {
                if (mDebugIcon == null)
                {
                    Texture2D lTexture = Resources.Load<Texture2D>(EditorGUIUtility.isProSkin ? "DebugIcon_pro" : "DebugIcon");

                    mDebugIcon = new GUIStyle(GUI.skin.button);
                    mDebugIcon.normal.background = lTexture;
                    mDebugIcon.padding = new RectOffset(0, 0, 0, 0);
                    mDebugIcon.margin = new RectOffset(0, 0, 1, 0);
                    mDebugIcon.border = new RectOffset(0, 0, 0, 0);
                    mDebugIcon.stretchHeight = false;
                    mDebugIcon.stretchWidth = false;

                }

                return mDebugIcon;
            }
        }

        /// <summary>
        /// Initialize the styles we use
        /// </summary>
        public static void InitializeStyles()
        {
            // Load the resources
            InheritedBackground = CreateTexture(2, 2, new Color(0.5f, 0.5f, 0.5f, 0.25f));
            InstanceBackground = CreateTexture(2, 2, new Color(0.5f, 0.5f, 0.5f, 0.75f));
            GreenPlusButton = Resources.Load<Texture2D>("AddButton");
            BluePlusButton = Resources.Load<Texture2D>("AddButtonBlue");

            InstanceStyle = new GUIStyle(GUI.skin.textField);
            InstanceStyle.normal.background = InstanceBackground;

            InheritedStyle = new GUIStyle(GUI.skin.textField);
            InheritedStyle.normal.background = InheritedBackground;

            IndexStyle = new GUIStyle(GUI.skin.textField);
            IndexStyle.alignment = TextAnchor.MiddleRight;

            GreenPlusButtonStyle = new GUIStyle();
            GreenPlusButtonStyle.normal.background = GreenPlusButton;
            GreenPlusButtonStyle.margin = new RectOffset(0, 0, 2, 0);

            BluePlusButtonStyle = new GUIStyle();
            BluePlusButtonStyle.normal.background = BluePlusButton;
            BluePlusButtonStyle.margin = new RectOffset(0, 0, 2, 0);
        }

        /// <summary>
        /// Creates a texture given the specified color
        /// </summary>
        /// <param name="rWidth">Width of the texture</param>
        /// <param name="rHeight">Height of the texture</param>
        /// <param name="rColor">Color of the texture</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int rWidth, Color rColor)
        {
            int lHeight = rWidth;
            int lRow = lHeight / 2;
            Color lColor = new Color(0f, 0f, 0f, 0f);

            Color[] lPixels = new Color[rWidth * lHeight];
            for (int i = 0; i < lPixels.Length; i++)
            {
                lPixels[i] = lColor;
            }

            // Set the border (top, left and right, bottom)
            for (int x = 0; x < rWidth; x++) { lPixels[(lRow * rWidth) + x] = rColor; }

            Texture2D lResult = new Texture2D(rWidth, lHeight);
            lResult.SetPixels(lPixels);
            lResult.Apply();

            return lResult;
        }

        /// <summary>
        /// Creates a texture given the specified color
        /// </summary>
        /// <param name="rWidth">Width of the texture</param>
        /// <param name="rHeight">Height of the texture</param>
        /// <param name="rColor">Color of the texture</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int rWidth, int rHeight, Color rColor)
        {
            Color[] lPixels = new Color[rWidth * rHeight];
            for (int i = 0; i < lPixels.Length; i++)
            {
                lPixels[i] = rColor;
            }

            Texture2D lResult = new Texture2D(rWidth, rHeight);
            lResult.SetPixels(lPixels);
            lResult.Apply();

            return lResult;
        }

        /// <summary>
        /// Creates a texture given the specified color
        /// </summary>
        /// <param name="rWidth">Width of the texture</param>
        /// <param name="rHeight">Height of the texture</param>
        /// <param name="rColor">Color of the texture</param>
        /// <returns></returns>
        public static Texture2D CreateTexture(int rWidth, int rHeight, Color rColor, Color rBorderColor)
        {
            Color[] lPixels = new Color[rWidth * rHeight];
            for (int i = 0; i < lPixels.Length; i++)
            {
                lPixels[i] = rColor;
            }

            // Set the border (top, left and right, bottom)
            for (int x = 0; x < rWidth; x++) { lPixels[x] = rBorderColor; }
            for (int y = 0; y < rHeight; y++) { lPixels[y * rWidth] = rBorderColor; lPixels[(y * rWidth) + (rWidth - 1)] = rBorderColor; }
            for (int x = lPixels.Length - rWidth; x < lPixels.Length; x++) { lPixels[x] = rBorderColor; }

            Texture2D lResult = new Texture2D(rWidth, rHeight);
            lResult.SetPixels(lPixels);
            lResult.Apply();

            return lResult;
        }

        /// <summary>
        /// Create a color using 0-255 values
        /// </summary>
        /// <param name="rRed"></param>
        /// <param name="rGreen"></param>
        /// <param name="rBlue"></param>
        /// <returns></returns>
        public static Color CreateColor(float rRed, float rGreen, float rBlue)
        {
            Color lColor = new Color(rRed / 255f, rGreen / 255f, rBlue / 255f);
            return lColor.gamma;
        }

        /// <summary>
        /// Create a color using 0-255 values
        /// </summary>
        /// <param name="rRed"></param>
        /// <param name="rGreen"></param>
        /// <param name="rBlue"></param>
        /// <returns></returns>
        public static Color CreateColor(float rRed, float rGreen, float rBlue, float rAlpha)
        {
            Color lColor = new Color(rRed / 255f, rGreen / 255f, rBlue / 255f, rAlpha);
            return lColor.gamma;
        }
    }
}

#endif
