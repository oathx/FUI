﻿using FUI.Test;
using FUI.UGUI.ValueConverter;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace FUI.Editor.Drawer
{
    [CustomEditor(typeof(Binding))]
    public class BindingPropertyEditor : UnityEditor.Editor
    {
        
        public override VisualElement CreateInspectorGUI()
        {
            return CreateBinding();
        }

        VisualElement CreateBinding()
        {
            var binding = target as Binding;
            binding.config.viewName = binding.gameObject.name;

            var rootElement = new VisualElement();
            var saveButton = new Button();
            saveButton.text = "Save";
            saveButton.clicked += () =>
            {
                var path = EditorUtility.SaveFilePanel("Save", Application.dataPath, binding.gameObject.name, "binding");
                if (!string.IsNullOrEmpty(path))
                {
                    var json = JsonUtility.ToJson(binding.config, true);
                    File.WriteAllText(path, json);
                }
            };
            rootElement.Add(saveButton);

            var contextList = new ListView
            {
                itemHeight = 200,
                makeItem = () => CreateContextItem(),
            };
            contextList.style.height = 400;
            contextList.bindItem = (e, i) =>
            {
                var itemData = contextList.itemsSource[i] as BindingContext;
                RefreshContextItem(itemData, e);
            };
            contextList.itemsSource = binding.config.contexts;
            contextList.selectionType = SelectionType.None;
            rootElement.Add(contextList);

            var addContextBtn = new Button();
            addContextBtn.text = "AddBindingContext";
            addContextBtn.clicked += () =>
            {
                binding.config.contexts.Add(new BindingContext());
                contextList.itemsSource = binding.config.contexts;
            };
            rootElement.Add(addContextBtn);
            return rootElement;
        }

        void SetChoices<T>(PopupField<T> popupField, List<T> choices)
        {
            var prop = popupField.GetType().GetField("m_Choices", BindingFlags.NonPublic | BindingFlags.Instance);
            prop.SetValue(popupField, choices);
        }

        void RefreshContextItem(BindingContext itemData, VisualElement itemView)
        {
            var viewModels = GetAssignableTypes<ViewModel>();
            var viewModelSelector = itemView.Q<PopupField<Type>>("ViewModelSelector");
            SetChoices(viewModelSelector, viewModels);
            var selected = viewModels.Find((x)=>x.FullName == itemData.type);
            if (selected != null)
            {
                viewModelSelector.value = selected;
            }
            viewModelSelector.RegisterCallback<ChangeEvent<Type>>((evt) =>
            {
                itemData.type = evt.newValue.FullName;
                if (evt.newValue != null)
                {
                    RefreshContextItem(itemData, itemView);
                }
            });

            var propertyList = itemView.Q<ListView>("PropertyList");
            propertyList.itemsSource = itemData.properties;
            propertyList.style.height = propertyList.itemsSource.Count * propertyList.itemHeight;

            var addPropertyBtn = itemView.Q<Button>("AddPropertyBtn");
            addPropertyBtn.visible = viewModelSelector.value != null && (propertyList.itemsSource == null || propertyList.itemsSource.Count == 0);
            addPropertyBtn.clicked += () =>
            {
                itemData.properties.Add(new BindingProperty());
                RefreshContextItem(itemData, itemView);
            };

            var addContextBtn = itemView.Q<Button>("AddContextBtn");
            addContextBtn.clicked += () =>
            {

            };

            var removeContextBtn = itemView.Q<Button>("RemoveContextBtn");
            removeContextBtn.clicked += () =>
            {
            };
        }

        static readonly List<string> Titles = new List<string> { "Property", "ValueConverter", "Element", "ElementType" };
        VisualElement CreateContextItem()
        {
            var element = new VisualElement();
            {
                var toolbar = new VisualElement();
                {
                    toolbar.style.justifyContent = Justify.FlexStart;
                    toolbar.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                    toolbar.style.width = 680;
                    toolbar.style.backgroundColor = new Color(38f / 255f, 38f / 255f, 38f / 255f, 1f);
                    var viewModelSelector = new PopupField<Type>
                    {
                        name = "ViewModelSelector",
                        formatSelectedValueCallback = (x) => x == null ? string.Empty : x.Name,
                        formatListItemCallback= (x) => x == null ? string.Empty : x.FullName,
                    };
                    viewModelSelector.style.width = 620;
                    toolbar.Add(viewModelSelector);


                    var addContextBtn = new Button
                    {
                        text = "+",
                        name = "AddContextBtn"
                    };
                    toolbar.Add(addContextBtn);

                    var removeContextBtn = new Button
                    {
                        text = "-",
                        name = "RemoveContextBtn"
                    };
                    toolbar.Add(removeContextBtn);
                }
                element.Add(toolbar);

                var title = new VisualElement();
                {
                    title.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
                    title.style.width = 620;
                    foreach(var t in Titles)
                    {
                        var titleItem = new TextElement { text = t };
                        titleItem.style.width = 155;
                        titleItem.style.left = 5;
                        title.Add(titleItem);
                    }
                }
                element.Add(title);

                var propertyList = new ListView
                {
                    itemHeight = 20,
                    name = "PropertyList",
                    makeItem = () => CreatePropertyItem()
                };
                propertyList.bindItem = (e, i) =>
                {
                    RefreshPropertyItem(propertyList.itemsSource[i] as BindingProperty, e, propertyList, i);
                };
                propertyList.style.height = 200;
                propertyList.selectionType = SelectionType.None;
                element.Add(propertyList);

                var addBtn = new Button
                {
                    text = "+",
                    name = "AddPropertyBtn"
                };
                element.Add(addBtn);
            }
            return element;
        }

        void RefreshPropertyItem(BindingProperty itemData, VisualElement itemView, ListView list, int index)
        {
            var viewModelType = list.parent.Q<PopupField<Type>>("ViewModelSelector").value;
            //要绑定的属性选择
            var propertySelector = itemView.Q<PopupField<PropertyInfo>>("PropertySelector");
            var properties = new List<PropertyInfo>();
            properties.AddRange(viewModelType.GetProperties());
            SetChoices(propertySelector, properties);
            propertySelector.RegisterCallback<ChangeEvent<PropertyInfo>>((evt) =>
            {
                itemData.name = evt.newValue.Name;
                itemData.type = evt.newValue.PropertyType.FullName;
            });
            var selectedProperty = properties.Find((p) => p.Name == itemData.name);
            if(selectedProperty != null)
            {
                propertySelector.value = selectedProperty;
            }

            //值转换器
            var valueConverterSelector = itemView.Q<PopupField<Type>>("ValueConverterSelector");
            var valueConverters = new List<Type>
            {
                null,
            };
            
            var converters = GetAssignableTypes<IValueConverter>();
            foreach (var converter in converters)
            {
                if (!converter.IsAbstract)
                {
                    valueConverters.Add(converter);
                }
            }
            SetChoices(valueConverterSelector, valueConverters);
            valueConverterSelector.RegisterCallback<ChangeEvent<Type>>((evt) =>
            {
                if(evt.newValue == null)
                {
                    itemData.converterType = evt.newValue.FullName;
                    itemData.converterValueType = string.Empty;
                    itemData.converterTargetType = string.Empty;
                }
                else
                {
                    var (valueType, targetType) = GetConverterArgumentType(evt.newValue);
                    itemData.converterType = evt.newValue.FullName;
                    itemData.converterValueType = valueType.FullName;
                    itemData.converterTargetType = targetType.FullName;
                }
            });
            var selectedType = valueConverters.Find((t) => t?.FullName == itemData.converterType);
            valueConverterSelector.value = selectedType; 

            //绑定的Element路径
            var objectSelector = itemView.Q<ObjectField>("ObjectSelector");
            objectSelector.RegisterCallback<ChangeEvent<UnityEngine.Object>>((evt) =>
            {
                var obj = evt.newValue;
                itemData.elementPath = GetChildPath(evt.newValue as GameObject);
            });
            var root = (target as MonoBehaviour).gameObject;
            if (!string.IsNullOrEmpty(itemData.elementPath))
            {
                var obj = root.transform.Find(itemData.elementPath);
                if(obj != null)
                {
                    objectSelector.value = obj.gameObject;
                }
            }

            //绑定的Element类型
            var elements = new List<Type>();
            if (!string.IsNullOrEmpty(itemData.elementPath) && root.transform.Find(itemData.elementPath) != null)
            {
                foreach (var childElement in root.transform.Find(itemData.elementPath).GetComponents<IVisualElement>())
                {
                    elements.Add(childElement.GetType());
                }
            }
            var elementSelector = itemView.Q<PopupField<Type>>("ElementSelector");
            SetChoices(elementSelector, elements);
            elementSelector.SetEnabled(!string.IsNullOrEmpty(itemData.elementPath));
            elementSelector.RegisterCallback<ChangeEvent<Type>>((evt) =>
            {
                itemData.elementType = evt.newValue.FullName;
                itemData.elementValueType = GetElementValueType(evt.newValue).FullName;
            });
            var selectedElement = elements.Find((t) => t.FullName == itemData.elementType);
            if (selectedElement != null)
            {
                elementSelector.value = selectedElement;
            }

            //添加属性按钮
            var addPropertyBtn = itemView.Q<Button>("AddPropertyBtn");
            addPropertyBtn.clicked += () =>
            {
                list.itemsSource.Insert(index, new BindingProperty());
                list.style.height = list.itemsSource.Count * list.itemHeight;
                list.Refresh();
            };

            //删除属性按钮
            var removePropertyBtn = itemView.Q<Button>("RemovePropertyBtn");
            removePropertyBtn.clicked += () =>
            {
                list.itemsSource.RemoveAt(index);
                list.style.height = list.itemsSource.Count * list.itemHeight;
                list.Refresh();
            };
        }

        VisualElement CreatePropertyItem()
        {
            var element = new VisualElement();
            element.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            const float width = 150;

            //属性选择框
            var propertySelector = new PopupField<PropertyInfo>
            {
                name = "PropertySelector",
                formatListItemCallback = (e) =>
                {
                    return e == null ? string.Empty : e.Name;
                },
                formatSelectedValueCallback = (e) =>
                {
                    return e == null ? string.Empty : e.Name;
                }
            };
            propertySelector.style.width = width;
            element.Add(propertySelector);

            //值转换器选择框
            var valueConverterSelector = new PopupField<Type>
            {
                name = "ValueConverterSelector",
                formatListItemCallback = (e) =>
                {
                    return e == null ? "None" : e.FullName;
                },
                formatSelectedValueCallback = (e) =>
                {
                    return e == null ? "None" : e.Name.Replace("Converter", string.Empty);
                },
            };
            valueConverterSelector.style.width = width;
            element.Add(valueConverterSelector);

            //Element选择框
            var objectSelector = new ObjectField
            {
                name = "ObjectSelector",
                objectType = typeof(GameObject)
            };
            objectSelector.style.width = width;
            element.Add(objectSelector);

            //Element类型选择框
            var elementSelector = new PopupField<Type>
            {
                name = "ElementSelector",
                formatSelectedValueCallback = (e) =>
                {
                    return e == null ? string.Empty : e.Name;
                },
                formatListItemCallback = (e) =>
                {
                    return e == null ? string.Empty : e.FullName;
                }
            };
            elementSelector.style.width = width;
            element.Add(elementSelector);

            //添加属性按钮
            var addPropertyBtn = new Button
            {
                text = "+",
                name = "AddPropertyBtn"
            };
            element.Add(addPropertyBtn);

            //删除属性按钮
            var removePropertyBtn = new Button
            {
                text = "-",
                name = "RemovePropertyBtn"
            };
            element.Add(removePropertyBtn);
            return element;
        }

        string GetChildPath(GameObject child)
        {
            var root = (target as MonoBehaviour).gameObject;
            var result = string.Empty;
            result += child.name;
            var current = child.transform;

            while (true)
            {
                current = current.transform.parent;
                if(current == null || current.gameObject == root)
                {
                    break;
                }

                result = $"{current.name}/{result}";
            }
            return result;
        }

        /// <summary>
        /// 获取一个Element的值类型
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        Type GetElementValueType(Type elementType)
        {
            foreach(var @interface in elementType.GetInterfaces())
            {
                if(@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IVisualElement<>))
                {
                    return @interface.GetGenericArguments()[0];
                }
            }
            return typeof(object);
        }

        /// <summary>
        /// 获取值转换器的参数类型
        /// </summary>
        /// <param name="converterType"></param>
        /// <returns></returns>
        (Type valueType, Type targetType) GetConverterArgumentType(Type converterType)
        {
            foreach(var @interface in converterType.GetInterfaces())
            {
                if(@interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IValueConverter<,>))
                {
                    return (@interface.GetGenericArguments()[0], @interface.GetGenericArguments()[1]);
                }
            }
            return (typeof(object), typeof(object));
        }

        /// <summary>
        /// 获取派生自某个类型的所有类型
        /// </summary>
        List<Type> GetAssignableTypes<T>()
        {
            var t = typeof(T);
            var result = new List<Type>();  

            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                result.AddRange(assembly.GetTypes().Where(x => t.IsAssignableFrom(x)));
            }

            return result;
        }
    }
}
