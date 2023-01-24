using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNode : Node
{
   protected string nodeGuid;
   protected DialogueGraphView graphView;
   protected DialogueEditorWindow editorWindow;
   protected Vector2 defaultNodeSize = new Vector2(200, 250);

   private List<LanguageGenericHolderText> _languageGenericsListTexts = new List<LanguageGenericHolderText>();
   private List<LanguageGenericHolderAudioClip> _languageGenericsListAudioClips = new List<LanguageGenericHolderAudioClip>();
   
   public string NodeGuid { get => nodeGuid; set => nodeGuid = value; }

   public BaseNode()
   {
      StyleSheet styleSheet = Resources.Load<StyleSheet>("USS/Nodes/NodeStyle");
      styleSheets.Add(styleSheet);
   }
   
   #region Get New Field ------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Get a new Label
        /// </summary>
        /// <param name="labelName">Text in the label</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected Label GetNewLabel(string labelName, string uss01 = "", string uss02 = "")
        {
            Label labelTexts = new Label(labelName);

            // Set uss class for stylesheet.
            labelTexts.AddToClassList(uss01);
            labelTexts.AddToClassList(uss02);

            return labelTexts;
        }

        /// <summary>
        /// Get a new Button
        /// </summary>
        /// <param name="btnText">Text in the button</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected Button GetNewButton(string btnText, string uss01 = "", string uss02 = "")
        {
            Button btn = new Button()
            {
                text = btnText,
            };

            // Set uss class for stylesheet.
            btn.AddToClassList(uss01);
            btn.AddToClassList(uss02);

            return btn;
        }

        // Value's --------------------------------------------------------------------------

        /// <summary>
        /// Get a new IntegerField.
        /// </summary>
        /// <param name="inputValue">ContainerInt that need to be set in to the IntegerField</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected IntegerField GetNewIntegerField(ContainerInt inputValue, string uss01 = "", string uss02 = "")
        {
            IntegerField integerField = new IntegerField();

            // When we change the variable from graph view.
            integerField.RegisterValueChangedCallback(value =>
            {
                inputValue.value = value.newValue;
            });
            integerField.SetValueWithoutNotify(inputValue.value);

            // Set uss class for stylesheet.
            integerField.AddToClassList(uss01);
            integerField.AddToClassList(uss02);

            return integerField;
        }

        /// <summary>
        /// Get a new FloatField.
        /// </summary>
        /// <param name="inputValue">ContainerFloat that need to be set in to the FloatField</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected FloatField GetNewFloatField(ContainerFloat inputValue, string uss01 = "", string uss02 = "")
        {
            FloatField floatField = new FloatField();

            // When we change the variable from graph view.
            floatField.RegisterValueChangedCallback(value =>
            {
                inputValue.value = value.newValue;
            });
            floatField.SetValueWithoutNotify(inputValue.value);

            // Set uss class for stylesheet.
            floatField.AddToClassList(uss01);
            floatField.AddToClassList(uss02);

            return floatField;
        }

        /// <summary>
        /// Get a new TextField.
        /// </summary>
        /// <param name="inputValue">ContainerString that need to be set in to the TextField</param>
        /// <param name="placeholderText"></param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected TextField GetNewTextField(ContainerString inputValue, string placeholderText, string uss01 = "", string uss02 = "")
        {
            TextField textField = new TextField();

            // When we change the variable from graph view.
            textField.RegisterValueChangedCallback(value =>
            {
                inputValue.value = value.newValue;
            });
            textField.SetValueWithoutNotify(inputValue.value);

            // Set uss class for stylesheet.
            textField.AddToClassList(uss01);
            textField.AddToClassList(uss02);

            // Set Place Holder
            SetPlaceholderText(textField, placeholderText);

            return textField;
        }

        /// <summary>
        /// Get a new Image.
        /// </summary>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected Image GetNewImage(string uss01 = "", string uss02 = "")
        {
            Image imagePreview = new Image();

            // Set uss class for stylesheet.
            imagePreview.AddToClassList(uss01);
            imagePreview.AddToClassList(uss02);

            return imagePreview;
        }

        /// <summary>
        /// Get a new ObjectField with a Sprite as the Object.
        /// </summary>
        /// <param name="inputSprite">ContainerSprite that need to be set in to the ObjectField</param>
        /// <param name="imagePreview">Image that need to be set as preview image</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected ObjectField GetNewObjectFieldSprite(ContainerSprite inputSprite, Image imagePreview, string uss01 = "", string uss02 = "")
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = inputSprite.value,
            };

            // When we change the variable from graph view
            objectField.RegisterValueChangedCallback(value =>
            {
                inputSprite.value = value.newValue as Sprite;

                imagePreview.image = (inputSprite.value != null ? inputSprite.value.texture : null);
            });
            imagePreview.image = (inputSprite.value != null ? inputSprite.value.texture : null);

            // Set uss class for stylesheet.
            objectField.AddToClassList(uss01);
            objectField.AddToClassList(uss02);

            return objectField;
        }

        /// <summary>
        /// Get a new ObjectField with a ContainerDialogueEventSO as the Object.
        /// </summary>
        /// <param name="inputDialogueEventSo">ContainerDialogueEventSO that need to be set in to the ObjectField</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected ObjectField GetNewObjectFieldDialogueEvent(ContainerDialogueEventSo inputDialogueEventSo, string uss01 = "", string uss02 = "")
        {
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(DialogueEventSO),
                allowSceneObjects = false,
                value = inputDialogueEventSo.dialogueEventSo,
            };

            // When we change the variable from graph view.
            objectField.RegisterValueChangedCallback(value =>
            {
                inputDialogueEventSo.dialogueEventSo = value.newValue as DialogueEventSO;
            });
            objectField.SetValueWithoutNotify(inputDialogueEventSo.dialogueEventSo);

            // Set uss class for stylesheet.
            objectField.AddToClassList(uss01);
            objectField.AddToClassList(uss02);

            return objectField;
        }

        // Enum's --------------------------------------------------------------------------

        /// <summary>
        /// Get a new EnumField where the emum is ChoiceStateType.
        /// </summary>
        /// <param name="enumType">ContainerChoiceStateType that need to be set in to the EnumField</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected EnumField GetNewEnumFieldChoiceStateType(ContainerChoiceStateType enumType, string uss01 = "", string uss02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.value
            };
            enumField.Init(enumType.value);

            // When we change the variable from graph view.
            enumField.RegisterValueChangedCallback((value) =>
            {
                enumType.value = (ChoicesStateType)value.newValue;
            });
            enumField.SetValueWithoutNotify(enumType.value);

            // Set uss class for stylesheet.
            enumField.AddToClassList(uss01);
            enumField.AddToClassList(uss02);

            enumType.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField where the emum is EndNodeType.
        /// </summary>
        /// <param name="enumType">ContainerEndNodeType that need to be set in to the EnumField</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected EnumField GetNewEnumFieldEndNodeType(ContainerEndNodeType enumType, string uss01 = "", string uss02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.value
            };
            enumField.Init(enumType.value);

            // When we change the variable from graph view.
            enumField.RegisterValueChangedCallback((value) =>
            {
                enumType.value = (EndNodeType)value.newValue;
            });
            enumField.SetValueWithoutNotify(enumType.value);

            // Set uss class for stylesheet.
            enumField.AddToClassList(uss01);
            enumField.AddToClassList(uss02);

            enumType.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField where the emum is StringEventModifierType.
        /// </summary>
        /// <param name="enumType">ContainerStringEventModifierType that need to be set in to the EnumField</param>
        /// <param name="action"></param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected EnumField GetNewEnumFieldStringEventModifierType(ContainerStringEventModifierType enumType, Action action, string uss01 = "", string uss02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.value
            };
            enumField.Init(enumType.value);

            // When we change the variable from graph view.
            enumField.RegisterValueChangedCallback((value) =>
            {
                enumType.value = (StringEventModifierType)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.value);

            // Set uss class for stylesheet.
            enumField.AddToClassList(uss01);
            enumField.AddToClassList(uss02);

            enumType.EnumField = enumField;
            return enumField;
        }

        /// <summary>
        /// Get a new EnumField where the emum is StringEventConditionType.
        /// </summary>
        /// <param name="enumType">ContainerStringEventConditionType that need to be set in to the EnumField</param>
        /// <param name="action">A Action that is use to hide/show depending on if a FloatField is needed</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected EnumField GetNewEnumFieldStringEventConditionType(ContainerStringEventConditionType enumType, Action action, string uss01 = "", string uss02 = "")
        {
            EnumField enumField = new EnumField()
            {
                value = enumType.value
            };
            enumField.Init(enumType.value);

            // When we change the variable from graph view.
            enumField.RegisterValueChangedCallback((value) =>
            {
                enumType.value = (StringEventConditionType)value.newValue;
                action?.Invoke();
            });
            enumField.SetValueWithoutNotify(enumType.value);

            // Set uss class for stylesheet.
            enumField.AddToClassList(uss01);
            enumField.AddToClassList(uss02);

            enumType.EnumField = enumField;
            return enumField;
        }

        // Custom-made's --------------------------------------------------------------------------

        /// <summary>
        /// Get a new TextField that use a List<LanguageGeneric<string>> text.
        /// </summary>
        /// <param name="Text">List of LanguageGeneric<string> Text</param>
        /// <param name="placeholderText">The text that will be displayed if the text field is empty</param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected TextField GetNewTextFieldTextLanguage(List<LanguageGeneric<string>> Text, string placeholderText = "", string uss01 = "", string uss02 = "")
        {
            // Add languages
            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                Text.Add(new LanguageGeneric<string>
                {
                    languageType = language,
                    languageGenericType = ""
                });
            }

            // Make TextField.
            TextField textField = new TextField("");

            // Add it to the reload current language list.
            _languageGenericsListTexts.Add(new LanguageGenericHolderText(Text, textField, placeholderText));

            // When we change the variable from graph view.
            textField.RegisterValueChangedCallback(value =>
            {
                Text.Find(text => text.languageType == editorWindow.selectedLanguage).languageGenericType = value.newValue;
            });
            textField.SetValueWithoutNotify(Text.Find(text => text.languageType == editorWindow.selectedLanguage).languageGenericType);

            // Text field is set to be multiline.
            textField.multiline = true;

            // Set uss class for stylesheet.
            textField.AddToClassList(uss01);
            textField.AddToClassList(uss02);

            return textField;
        }


        /// <summary>
        /// Get a new ObjectField that use List<LanguageGeneric<AudioClip>>.
        /// </summary>
        /// <param name="audioClips"></param>
        /// <param name="uss01">USS class add to the UI element</param>
        /// <param name="uss02">USS class add to the UI element</param>
        /// <returns></returns>
        protected ObjectField GetNewObjectFieldAudioClipsLanguage(List<LanguageGeneric<AudioClip>> audioClips, string uss01 = "", string uss02 = "")
        {
            // Add languages.
            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                audioClips.Add(new LanguageGeneric<AudioClip>
                {
                    languageType = language,
                    languageGenericType = null
                });
            }

            // Make ObjectField.
            ObjectField objectField = new ObjectField()
            {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                value = audioClips.Find(audioClip => audioClip.languageType == editorWindow.selectedLanguage).languageGenericType,
            };

            // Add it to the reload current language list.
            _languageGenericsListAudioClips.Add(new LanguageGenericHolderAudioClip(audioClips, objectField));

            // When we change the variable from graph view.
            objectField.RegisterValueChangedCallback(value =>
            {
                audioClips.Find(audioClip => audioClip.languageType == editorWindow.selectedLanguage).languageGenericType = value.newValue as AudioClip;
            });
            objectField.SetValueWithoutNotify(audioClips.Find(audioClip => audioClip.languageType == editorWindow.selectedLanguage).languageGenericType);

            // Set uss class for stylesheet.
            objectField.AddToClassList(uss01);
            objectField.AddToClassList(uss02);

            return objectField;
        }

        #endregion

   /// <summary>
   /// Add a port to the inputContainer.
   /// </summary>
   /// <param name="name">The name of port.</param>
   /// <param name="capacity">Can it accept multiple or a single one.</param>
   /// <returns>Get the port that was just added to the inputContainer.</returns>
   public Port AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
   {
      Port inputPort = GetPortInstance(Direction.Input, capacity);
      inputPort.portName = name;
      inputContainer.Add(inputPort);
      return inputPort;
   }

   /// <summary>
   /// Add a port to the outputContainer.
   /// </summary>
   /// <param name="name">The name of port.</param>
   /// <param name="capacity">Can it accept multiple or a single one.</param>
   /// <returns>Get the port that was just added to the outputContainer.</returns>
   public Port AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
   {
      Port outputPort = GetPortInstance(Direction.Output, capacity);
      outputPort.portName = name;
      outputContainer.Add(outputPort);
      return outputPort;
   }

   public Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
   {
      return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
   }

   public virtual void LoadValueInToField()
   {
      
   }

   /// <summary>
   /// Reload languages to the current selected language.
   /// </summary>
   public virtual void ReloadLanguage()
   {
      foreach (LanguageGenericHolderText textHolder in _languageGenericsListTexts)
      {
         ReloadTextLanguage(textHolder.inputText, textHolder.textField, textHolder.placeholderText);
      }
      foreach (LanguageGenericHolderAudioClip audioHolder in _languageGenericsListAudioClips)
      {
         ReloadAudioClipLanguage(audioHolder.inputAudioClip, audioHolder.objectField);
      }
   }
   
   /// <summary>
   /// Reload all the text in the TextField to the current selected language.
   /// </summary>
   /// <param name="inputText">List of LanguageGeneric<string></param>
   /// <param name="textField">The TextField that is to be reload</param>
   /// <param name="placeholderText">The text that will be displayed if the text field is empty</param>
   protected void ReloadTextLanguage(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "")
   {
      // Reload Text
      textField.RegisterValueChangedCallback(value =>
      {
         inputText.Find(text => text.languageType == editorWindow.selectedLanguage).languageGenericType = value.newValue;
      });
      textField.SetValueWithoutNotify(inputText.Find(text => text.languageType == editorWindow.selectedLanguage).languageGenericType);

      SetPlaceholderText(textField, placeholderText);
   }

   /// <summary>
   /// Reload all the AudioClip in the ObjectField to the current selected language.
   /// </summary>
   /// <param name="inputAudioClip">List of LanguageGeneric<AudioClip></param>
   /// <param name="objectField">The ObjectField that is to be reload</param>
   protected void ReloadAudioClipLanguage(List<LanguageGeneric<AudioClip>> inputAudioClip, ObjectField objectField)
   {
      // Reload Text
      objectField.RegisterValueChangedCallback(value =>
      {
         inputAudioClip.Find(text => text.languageType == editorWindow.selectedLanguage).languageGenericType = value.newValue as AudioClip;
      });
      objectField.SetValueWithoutNotify(inputAudioClip.Find(text => text.languageType == editorWindow.selectedLanguage).languageGenericType);
   }
   
   /// <summary>
   /// Set a placeholder text on a TextField.
   /// </summary>
   /// <param name="textField">TextField that need a placeholder</param>
   /// <param name="placeholder">The text that will be displayed if the text field is empty</param>
   protected void SetPlaceholderText(TextField textField, string placeholder)
   {
      string placeholderClass = TextField.ussClassName + "__placeholder";

      CheckForText();
      OnFocusOut();
      textField.RegisterCallback<FocusInEvent>(evt => OnFocusIn());
      textField.RegisterCallback<FocusOutEvent>(evt => OnFocusOut());

      void OnFocusIn()
      {
         if (textField.ClassListContains(placeholderClass))
         {
            textField.value = string.Empty;
            textField.RemoveFromClassList(placeholderClass);
         }
      }

      void OnFocusOut()
      {
         if (string.IsNullOrEmpty(textField.text))
         {
            textField.SetValueWithoutNotify(placeholder);
            textField.AddToClassList(placeholderClass);
         }
      }

      void CheckForText()
      {
         if (!string.IsNullOrEmpty(textField.text))
         {
            textField.RemoveFromClassList(placeholderClass);
         }
      }
   }
   
      /// <summary>
        /// Add String Modifier Event to UI element.
        /// </summary>
        /// <param name="stringEventModifier">The List<EventData_StringModifier> that EventDataStringModifier should be added to.</param>
        /// <param name="stringEvent">EventDataStringModifier that should be use.</param>
        protected void AddStringModifierEventBuild(List<EventDataStringModifier> stringEventModifier, EventDataStringModifier stringEvent = null)
        {
            EventDataStringModifier tmpStringEventModifier = new EventDataStringModifier();

            // If we paramida value is not null we load in values.
            if (stringEvent != null)
            {
                tmpStringEventModifier.stringEventText.value = stringEvent.stringEventText.value;
                tmpStringEventModifier.number.value = stringEvent.number.value;
                tmpStringEventModifier.stringEventModifierType.value = stringEvent.stringEventModifierType.value;
            }

            stringEventModifier.Add(tmpStringEventModifier);

            // Container of all object.
            Box boxContainer = new Box();
            Box boxfloatField = new Box();
            boxContainer.AddToClassList("StringEventBox");
            boxfloatField.AddToClassList("StringEventBoxfloatField");

            // Text.
            TextField textField = GetNewTextField(tmpStringEventModifier.stringEventText, "String Event", "StringEventText");

            // ID number.
            FloatField floatField = GetNewFloatField(tmpStringEventModifier.number, "StringEventInt");

            // TODO: Delete maby?
            // Check for StringEventType and add the proper one.
            //EnumField enumField = null;

            // String Event Modifier
            Action tmp = () => ShowHideStringEventModifierType(tmpStringEventModifier.stringEventModifierType.value, boxfloatField);
            // EnumField String Event Modifier
            EnumField enumField = GetNewEnumFieldStringEventModifierType(tmpStringEventModifier.stringEventModifierType, tmp, "StringEventEnum");
            // Run the show and hide.
            ShowHideStringEventModifierType(tmpStringEventModifier.stringEventModifierType.value, boxfloatField);

            // Remove button.
            Button btn = GetNewButton("X", "removeBtn");
            btn.clicked += () =>
            {
                stringEventModifier.Remove(tmpStringEventModifier);
                DeleteBox(boxContainer);
            };

            // Add it to the box
            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxfloatField.Add(floatField);
            boxContainer.Add(boxfloatField);
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// Add String Condition Event to UI element.
        /// </summary>
        /// <param name="stringEventCondition">The List<EventDataStringCondition> that EventDataStringCondition should be added to.</param>
        /// <param name="stringEvent">EventDataStringCondition that should be use.</param>
        protected void AddStringConditionEventBuild(List<EventDataStringCondition> stringEventCondition, EventDataStringCondition stringEvent = null)
        {
            EventDataStringCondition tmpStringEventCondition = new EventDataStringCondition();

            // If we paramida value is not null we load in values.
            if (stringEvent != null)
            {
                tmpStringEventCondition.stringEventText.value = stringEvent.stringEventText.value;
                tmpStringEventCondition.number.value = stringEvent.number.value;
                tmpStringEventCondition.stringEventText.value = stringEvent.stringEventText.value;
            }

            stringEventCondition.Add(tmpStringEventCondition);

            // Container of all object.
            Box boxContainer = new Box();
            Box boxfloatField = new Box();
            boxContainer.AddToClassList("StringEventBox");
            boxfloatField.AddToClassList("StringEventBoxfloatField");

            // Text.
            TextField textField = GetNewTextField(tmpStringEventCondition.stringEventText, "String Event", "StringEventText");

            // ID number.
            FloatField floatField = GetNewFloatField(tmpStringEventCondition.number, "StringEventInt");

            // Check for StringEventType and add the proper one.
            EnumField enumField = null;
            // String Event Condition
            Action tmp = () => ShowHideStringEventConditionType(tmpStringEventCondition.stringEventConditionType.value, boxfloatField);
            // EnumField String Event Condition
            enumField = GetNewEnumFieldStringEventConditionType(tmpStringEventCondition.stringEventConditionType, tmp, "StringEventEnum");
            // Run the show and hide.
            ShowHideStringEventConditionType(tmpStringEventCondition.stringEventConditionType.value, boxfloatField);

            // Remove button.
            Button btn = GetNewButton("X", "removeBtn");
            btn.clicked += () =>
            {
                stringEventCondition.Remove(tmpStringEventCondition);
                DeleteBox(boxContainer);
            };

            // Add it to the box
            boxContainer.Add(textField);
            boxContainer.Add(enumField);
            boxfloatField.Add(floatField);
            boxContainer.Add(boxfloatField);
            boxContainer.Add(btn);

            mainContainer.Add(boxContainer);
            RefreshExpandedState();
        }

        /// <summary>
        /// hid and show the UI element
        /// </summary>
        /// <param name="value">StringEventModifierType</param>
        /// <param name="boxContainer">The Box that will be hidden or shown</param>
        private void ShowHideStringEventModifierType(StringEventModifierType value, Box boxContainer)
        {
            if (value == StringEventModifierType.SetTrue || value == StringEventModifierType.SetFalse)
            {
                ShowHide(false, boxContainer);
            }
            else
            {
                ShowHide(true, boxContainer);
            }
        }

        /// <summary>
        /// hid and show the UI element
        /// </summary>
        /// <param name="value">StringEventConditionType</param>
        /// <param name="boxContainer">The Box that will be hidden or shown</param>
        private void ShowHideStringEventConditionType(StringEventConditionType value, Box boxContainer)
        {
            if (value == StringEventConditionType.True || value == StringEventConditionType.False)
            {
                ShowHide(false, boxContainer);
            }
            else
            {
                ShowHide(true, boxContainer);
            }
        }
        
        
        /// <summary>
        /// Add or remove the USS Hide tag.
        /// </summary>
        /// <param name="show">true = show - false = hide</param>
        /// <param name="boxContainer">which container box to add the desired USS tag to</param>
        protected void ShowHide(bool show, Box boxContainer)
        {
            string hideUssClass = "Hide";
            if (show == true)
            {
                boxContainer.RemoveFromClassList(hideUssClass);
            }
            else
            {
                boxContainer.AddToClassList(hideUssClass);
            }
        }

        /// <summary>
        /// Remove box container.
        /// </summary>
        /// <param name="boxContainer">desired box to delete and remove</param>
        protected virtual void DeleteBox(Box boxContainer)
        {
            mainContainer.Remove(boxContainer);
            RefreshExpandedState();
        }
   
   #region LanguageGenericHolder Class ------------------------------------------------------------------------------------------------------------------------------------------------

   class LanguageGenericHolderText
   {
      public LanguageGenericHolderText(List<LanguageGeneric<string>> inputText, TextField textField, string placeholderText = "placeholderText")
      {
         this.inputText = inputText;
         this.textField = textField;
         this.placeholderText = placeholderText;
      }
      public List<LanguageGeneric<string>> inputText;
      public TextField textField;
      public string placeholderText;
   }

   class LanguageGenericHolderAudioClip
   {
      public LanguageGenericHolderAudioClip(List<LanguageGeneric<AudioClip>> inputAudioClip, ObjectField objectField)
      {
         this.inputAudioClip = inputAudioClip;
         this.objectField = objectField;
      }
      public List<LanguageGeneric<AudioClip>> inputAudioClip;
      public ObjectField objectField;
   }

   #endregion
}
