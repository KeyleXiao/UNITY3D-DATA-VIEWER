//
//    Copyright 2017 KeyleXiao.
//    Contact to Me : Keyle_xiao@hotmail.com 
//
//      Licensed under the Apache License, Version 2.0 (the "License");
//      you may not use this file except in compliance with the License.
//      You may obtain a copy of the License at
//
//          http://www.apache.org/licenses/LICENSE-2.0
//
//          Unless required by applicable law or agreed to in writing, software
//          distributed under the License is distributed on an "AS IS" BASIS,
//          WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//          See the License for the specific language governing permissions and
//          limitations under the License.
//

//TODO bug fix: 删除文件之后加载CodeGen直接报错 然而直接加载detail编辑 再切回来就正常了 主要是由于生成脚本编译引起的 现在CodeGen之后直接把界面关闭

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using SmartDataViewer.Helpers;

namespace SmartDataViewer.Editor.BuildInEditor
{
	/// <summary>
	/// 泛化数据编辑器
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public partial class ConfigEditorSchema<T> : IMultipleWindow where T : new()
	{
		
        /// <summary>
        /// 设置基础的控件
        /// </summary>
        /// <param name="width"></param>
        /// <param name="enable"></param>
        /// <param name="value"></param>
        /// <param name="setValue"></param>
        public virtual void RenderBaseControl(int width, bool enable, object value, Action<object> setValue)
        {
            if (value is Enum)
            {
                if (enable)
                {
                    value = EditorGUILayout.EnumPopup(value as Enum, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField((value as Enum).ToString(),
                        new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Bounds)
            {
                if (enable)
                {
                    value = EditorGUILayout.BoundsField((Bounds) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.BoundsField((Bounds) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Color)
            {
                if (enable)
                {
                    value = EditorGUILayout.ColorField((Color) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.ColorField((Color) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is AnimationCurve)
            {
                if (enable)
                {
                    value = EditorGUILayout.CurveField((AnimationCurve) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.CurveField((AnimationCurve) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is string)
            {
                if (enable)
                {
                    value = EditorGUILayout.TextField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is float)
            {
                if (enable)
                {
                    value = EditorGUILayout.FloatField((float) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is int)
            {
                if (enable)
                {
                    value = EditorGUILayout.IntField((int) value, new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(value as string, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is bool)
            {
                if (enable)
                {
                    value = EditorGUILayout.Toggle((bool) value, EditorGUIStyle.GetTogleStyle(),
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.LabelField(((bool) value).ToString(),
                        new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Vector2)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector2Field("", (Vector2) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector2Field("", (Vector2) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Vector3)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector3Field("", (Vector3) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector3Field("", (Vector3) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
            else if (value is Vector4)
            {
                if (enable)
                {
                    value = EditorGUILayout.Vector4Field("", (Vector4) value,
                        new GUILayoutOption[] {GUILayout.Width(width)});
                    setValue(value);
                }
                else
                {
                    EditorGUILayout.Vector4Field("", (Vector4) value, new GUILayoutOption[] {GUILayout.Width(width)});
                }
            }
        }

	}
}