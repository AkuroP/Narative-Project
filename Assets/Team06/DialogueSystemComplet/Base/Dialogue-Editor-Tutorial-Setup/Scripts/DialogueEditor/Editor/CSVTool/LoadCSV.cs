using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using File = System.IO.File;

namespace Team06.Editor
{


    public class LoadCSV
    {
        private string _csvDirectoryName = "Team06/Resources/CSV";
        private string _csvFileName = "DialogueCSV_Load.csv";

        public void Load()
        {
            string text = File.ReadAllText($"{Application.dataPath}/{_csvDirectoryName}/{_csvFileName}");
            List<List<string>> result = ParseCSV(text);

            List<string> headers = result[0];

            List<DialogueContainerSO> dialogueContainers = Helper.FindAllDialogueContainerSO();

            foreach (var dialogueContainer in dialogueContainers)
            {
                foreach (var nodeData in dialogueContainer.dialogueDatas)
                {
                    foreach (var textData in nodeData.dialogueDataTexts)
                    {
                        LoadInToDialogueNodeText(result, headers, textData);
                    }
                }

                foreach (var nodeData in dialogueContainer.choiceDatas)
                {
                    LoadInToChoiceNode(result, headers, nodeData);
                }

                EditorUtility.SetDirty(dialogueContainer);
                AssetDatabase.SaveAssets();
            }
        }

        private void LoadInToDialogueNodeText(List<List<string>> result, List<string> headers,
            DialogueDataText nodeDataText)
        {
            foreach (List<string> line in result)
            {
                if (line[2] == nodeDataText.guidID.value)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        foreach (var languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            if (headers[i] == languageType.ToString())
                            {
                                nodeDataText.text.Find(x => x.languageType == languageType).languageGenericType =
                                    line[i];
                            }
                        }
                    }
                }
            }
        }

        private void LoadInToChoiceNode(List<List<string>> result, List<string> headers, ChoiceData nodeData)
        {
            foreach (var line in result)
            {
                if (line[1] == nodeData.nodeGuid)
                {
                    for (int i = 0; i < line.Count; i++)
                    {
                        foreach (var languageType in (LanguageType[]) Enum.GetValues(typeof(LanguageType)))
                        {
                            if (headers[i] == languageType.ToString())
                            {
                                nodeData.text.Find(x => x.languageType == languageType).languageGenericType = line[i];
                            }
                        }
                    }
                }
            }
        }



        //Thanks and Made bu furukazu
        // flag for processing a CSV
        public enum ParsingMode
        {
            // default(treat as null)
            None,

            // processing a character which is out of quotes 
            OutQuote,

            // processing a character which is in quotes
            InQuote
        }

        /// <summary>
        /// Parses the CSV string.
        /// </summary>
        /// <returns>a two-dimensional array. first index indicates the row.  second index indicates the column.</returns>
        /// <param name="src">raw CSV contents as string</param>
        public List<List<string>> ParseCSV(string src)
        {
            var rows = new List<List<string>>();
            var cols = new List<string>();
#pragma warning disable XS0001 // Find APIs marked as TODO in Mono
            var buffer = new StringBuilder();
#pragma warning restore XS0001 // Find APIs marked as TODO in Mono

            ParsingMode mode = ParsingMode.OutQuote;
            bool requireTrimLineHead = false;
            var isBlank = new Regex(@"\s");

            int len = src.Length;

            for (int i = 0; i < len; ++i)
            {

                char c = src[i];

                // remove whilespace at beginning of line
                if (requireTrimLineHead)
                {
                    if (isBlank.IsMatch(c.ToString()))
                    {
                        continue;
                    }

                    requireTrimLineHead = false;
                }

                // finalize when c is the last character
                if ((i + 1) == len)
                {
                    // final char
                    switch (mode)
                    {
                        case ParsingMode.InQuote:
                            if (c == '"')
                            {
                                // ignore
                            }
                            else
                            {
                                // if close quote is missing
                                buffer.Append(c);
                            }

                            cols.Add(buffer.ToString());
                            rows.Add(cols);
                            return rows;

                        case ParsingMode.OutQuote:
                            if (c == ',')
                            {
                                // if the final character is comma, add an empty cell
                                // next col
                                cols.Add(buffer.ToString());
                                cols.Add(string.Empty);
                                rows.Add(cols);
                                return rows;
                            }

                            if (cols.Count == 0)
                            {
                                // if the final line is empty, ignore it. 
                                if (string.Empty.Equals(c.ToString().Trim()))
                                {
                                    return rows;
                                }
                            }

                            buffer.Append(c);
                            cols.Add(buffer.ToString());
                            rows.Add(cols);
                            return rows;
                    }
                }

                // the next character
                char n = src[i + 1];

                switch (mode)
                {
                    case ParsingMode.OutQuote:
                        // out quote
                        if (c == '"')
                        {
                            // to in-quote
                            mode = ParsingMode.InQuote;
                            continue;

                        }
                        else if (c == ',')
                        {
                            // next cell
                            cols.Add(buffer.ToString());
                            buffer.Remove(0, buffer.Length);

                        }
                        else if (c == '\r' && n == '\n')
                        {
                            // new line(CR+LF)
                            cols.Add(buffer.ToString());
                            rows.Add(cols);
                            cols = new List<string>();
                            buffer.Remove(0, buffer.Length);
                            ++i; // skip next code
                            requireTrimLineHead = true;

                        }
                        else if (c == '\n' || c == '\r')
                        {
                            // new line
                            cols.Add(buffer.ToString());
                            rows.Add(cols);
                            cols = new List<string>();
                            buffer.Remove(0, buffer.Length);
                            requireTrimLineHead = true;

                        }
                        else
                        {
                            // get one char
                            buffer.Append(c);
                        }

                        break;

                    case ParsingMode.InQuote:
                        // in quote
                        if (c == '"' && n != '"')
                        {
                            // to out-quote
                            mode = ParsingMode.OutQuote;

                        }
                        else if (c == '"' && n == '"')
                        {
                            // get "
                            buffer.Append('"');
                            ++i;

                        }
                        else
                        {
                            // get one char
                            buffer.Append(c);
                        }

                        break;
                }
            }

            return rows;
        }
    }
}
