using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveCSV
{
   private string _csvDirectoryName = "Resources/CSV";
   private string _csvFileName = "DialogueCSV_Save.csv";
   private string _csvSeparator = ",";
   private List<string> _csvHeader;
   private string _nodeId = "Node Guid ID";
   private string _textId = "Text Guid ID";
   private string _dialogueName = "Dialogue Name";

   public void Save()
   {
      List<DialogueContainerSO> dialogueContainers = Helper.FindAllDialogueContainerSO();

      CreateFile();

      foreach (var dialogueContainer in dialogueContainers)
      {
         foreach (var nodeData in dialogueContainer.dialogueDatas)
         {
            foreach (var textData in nodeData.dialogueDataTexts)
            {
               List<string> texts = new List<string>
               {
                  dialogueContainer.name,
                  nodeData.nodeGuid,
                  textData.guidID.value
               };

               foreach (var languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
               {
                  string tmp = textData.text.Find(language => language.languageType == languageType).languageGenericType.Replace("\"", "\"\"");
                  texts.Add($"\"{tmp}\"");
               }

               AppendToFile(texts);
            }
         }

         foreach (ChoiceData nodeData in dialogueContainer.choiceDatas)
         {
            List<string> texts = new List<string>
            {
               dialogueContainer.name,
               nodeData.nodeGuid,
               "Choice Dont have Text ID"
            };

            foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
               string tmp = nodeData.text.Find(language => language.languageType == languageType).languageGenericType.Replace("\"", "\"\"");
               texts.Add($"\"{tmp}\"");
            }

            AppendToFile(texts);
         }
      }
   }

   private void AppendToFile(List<string> strings)
   {
      using (StreamWriter sw = File.AppendText(GetFilePath()))
      {
         string finalString = "";
         foreach (string text in strings)
         {
            if (finalString != "")
            {
               finalString += _csvSeparator;
            }
            finalString += text;
         }
      
         sw.WriteLine(finalString);
      }
   }

   private void CreateFile()
   {
      VerifyDirectory();
      MakeHeader();
      using (StreamWriter sw = File.CreateText(GetFilePath()))
      {
         string finalString = "";
         foreach (string header in _csvHeader)
         {
            if (finalString != "")
            {
               finalString += _csvSeparator;
            }
            finalString += header;
         }

         sw.WriteLine(finalString);
      }
   }

   private void MakeHeader()
   {
      List<string> headerText = new List<string>
      {
         _nodeId,
         _textId,
         _dialogueName
      };

      foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
      {
         headerText.Add(language.ToString());
      }

      _csvHeader = headerText;
   }

   private void VerifyDirectory()
   {
      string directory = GetDirectoryPath();

      if (!Directory.Exists(directory))
      {
         Directory.CreateDirectory(directory);
      }
   }

   private string GetDirectoryPath()
   {
      return $"{Application.dataPath}/{_csvDirectoryName}";
   }

   private string GetFilePath()
   {
      return $"{GetDirectoryPath()}/{_csvFileName}";
   }
}

