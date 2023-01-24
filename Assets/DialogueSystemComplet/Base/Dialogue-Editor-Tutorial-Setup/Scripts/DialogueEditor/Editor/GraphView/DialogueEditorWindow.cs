using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;


public class DialogueEditorWindow : EditorWindow
{
    private DialogueContainerSO _currentDialogueContainer; //Current open dialogue container
    private DialogueGraphView _graphView;  // Ref graphview class
    private DialogueSaveAndLoad _saveAndLoad;  // Ref save and load class

    public LanguageType selectedLanguage = LanguageType.French;
    public LanguageType LanguageType { get => selectedLanguage; set => selectedLanguage = value; }
    
    private ToolbarMenu _toolbarMenu;
    private Label _nameOfDialogueContainer;
    private string _graphViewStyleSheet = "USS/EditorWindow/EditorWindowStyle";

    // CallBack attribute for opening an asset in Unity (fired when double clicking an asset in the project browser)
    [OnOpenAsset(0)]
    public static bool ShowWindow(int instanceId, int line)
    {
        UnityEngine.Object item = EditorUtility.InstanceIDToObject(instanceId); //Find Unity Object with this instanceID and load it in

        if (item is DialogueContainerSO) //Check if item is a DialogueContainerSO
        { 
            
            DialogueEditorWindow window = GetWindow<DialogueEditorWindow>();        //Make a unity editor window of type DialogueEditorWindow
            window.titleContent = new GUIContent("Dialogue Editor");            //Name of editor window
            window._currentDialogueContainer = item as DialogueContainerSO;         // The DialogueContainerSo we will load in to the editor window
            window.minSize = new Vector2(500, 250);                            //Starter size of the editor window
            window.Load();                                                        //Load in DialogueContainerSo data in to the editor window
        }

        return false;
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolBar();
        Load();
    }
    
    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
    
    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this);
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);

        _saveAndLoad = new DialogueSaveAndLoad(_graphView);
    }
    
    private void GenerateToolBar()
    {
        rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>(_graphViewStyleSheet));
        Toolbar toolbar = new Toolbar();
        
        //SaveButton
        toolbar.Add(new Button(() => Save()){text = "Save"});
        toolbar.Add(new Button(() => Load()){text = "Load"});
        
        //Dropdown menu for language
        _toolbarMenu = new ToolbarMenu();
        foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
        {
            _toolbarMenu.menu.AppendAction(language.ToString(), new Action<DropdownMenuAction>(x => Language(language)));
        }
        toolbar.Add(_toolbarMenu);
        
        //Name of current DialogueContainer open
        _nameOfDialogueContainer = new Label("");
        toolbar.Add(_nameOfDialogueContainer);
        _nameOfDialogueContainer.AddToClassList("nameOfDialogueContainer");
        
        rootVisualElement.Add(toolbar);
    }
    
    /// <summary>
    /// Will save the current changes to dialogue container.
    /// </summary>
    private void Save()
    {
        //Save the window
        Debug.Log("Save");
        if (_currentDialogueContainer != null)
        {
            _saveAndLoad.Save(_currentDialogueContainer);
        }
    }
    
    /// <summary>
    /// Will load in current selected dialogue container.
    /// </summary>
    private void Load()
    {
        if (_currentDialogueContainer != null)
        {
            Debug.Log("Load");
            Language(LanguageType.French);
            _nameOfDialogueContainer.text = "Name:   " + _currentDialogueContainer.name;
            _saveAndLoad.Load(_currentDialogueContainer);
        }
    }

    /// <summary>
    /// Will change the language in the dialogue editor window.
    /// </summary>
    /// <param name="language">Language that you want to change to</param>
    private void Language(LanguageType language)
    {
        _toolbarMenu.text = "Language: " + language.ToString();
        selectedLanguage = language;
        _graphView.ReloadLanguage();
    }
}
