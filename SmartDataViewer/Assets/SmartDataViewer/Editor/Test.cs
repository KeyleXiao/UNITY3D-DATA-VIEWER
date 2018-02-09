//using UnityEditor;
//using UnityEngine;
//
//namespace SmartDataViewer.Editor
//{
//    public class Test:EditorWindow
//    {
//        
//        [MenuItem("Help/ShowTestPanel")]
//        public static void ShowTestPanel()
//        {
//            Test t = new Test();
//            t.ShowUtility();
//        }
//
//        private GameObject o = null;
//        private void OnGUI()
//        {
//            o = EditorGUILayout.ObjectField("TestShowTarget",o,typeof(GameObject)) as GameObject;
//
//            if (o)
//            {
//                
//                if (GUILayout.Button(AssetPreview.GetAssetPreview(o)))
//                {
//                    
//                } 
//            }
//           
//        }
//    }
//}