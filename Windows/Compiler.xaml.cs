using Antlr4.Runtime;
using Laba1.Grammar;
using Laba1.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Laba1.Windows
{
    public partial class Compiler : Window
    {
        private bool _isFileModified = false;
        private string _currentFilePath = string.Empty;

        private const string DefaultExample = @"Dictionary<int, string> My_dict1 = new Dictionary<int, string> {
    { 1, ""one"" },
    { 2, ""two"" },
    { 3, ""three"" }
};";

        public Compiler()
        {
            InitializeComponent();
            UpdateWindowTitle();
            SetEditorText(DefaultExample);
            ClearOutput();
        }

        private void SetEditorText(string text)
        {
            FileContentViewer.Document.Blocks.Clear();
            FileContentViewer.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private string GetEditorText()
        {
            TextRange textRange = new TextRange(
                FileContentViewer.Document.ContentStart,
                FileContentViewer.Document.ContentEnd);

            return textRange.Text.Trim();
        }

        private void ClearOutput()
        {
            OutputDataGrid.ItemsSource = null;
        }

        private void ShowOutput(List<OutputRow> rows)
        {
            OutputDataGrid.ItemsSource = null;
            OutputDataGrid.ItemsSource = rows;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            SetEditorText(DefaultExample);
            ClearOutput();
            _currentFilePath = string.Empty;
            _isFileModified = false;
            UpdateWindowTitle();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "C# файлы (*.cs)|*.cs|Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                FilterIndex = 1
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    TextRange textRange = new TextRange(
                        FileContentViewer.Document.ContentStart,
                        FileContentViewer.Document.ContentEnd);

                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                    {
                        textRange.Load(fs, DataFormats.Text);
                    }

                    _currentFilePath = openFileDialog.FileName;
                    _isFileModified = false;
                    UpdateWindowTitle();

                    ShowOutput(new List<OutputRow>
                    {
                        new OutputRow { Message = "Файл успешно открыт." }
                    });
                }
                catch (Exception ex)
                {
                    ShowOutput(new List<OutputRow>
                    {
                        new OutputRow { Message = "Ошибка при открытии файла: " + ex.Message }
                    });
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentFilePath))
                SaveAs_Click(sender, e);
            else
                SaveToFile(_currentFilePath);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "C# файлы (*.cs)|*.cs|Текстовые файлы (*.txt)|*.txt",
                DefaultExt = "cs",
                AddExtension = true,
                FilterIndex = 1
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _currentFilePath = saveFileDialog.FileName;
                SaveToFile(_currentFilePath);
            }
        }

        private void SaveToFile(string filePath)
        {
            try
            {
                TextRange textRange = new TextRange(
                    FileContentViewer.Document.ContentStart,
                    FileContentViewer.Document.ContentEnd);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    textRange.Save(fs, DataFormats.Text);
                }

                _isFileModified = false;
                UpdateWindowTitle();

                ShowOutput(new List<OutputRow>
                {
                    new OutputRow { Message = "Файл успешно сохранён: " + filePath }
                });
            }
            catch (Exception ex)
            {
                ShowOutput(new List<OutputRow>
                {
                    new OutputRow { Message = "Ошибка при сохранении файла: " + ex.Message }
                });
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Reference_Click(object sender, RoutedEventArgs e)
        {
            Reference reference = new Reference();
            reference.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

        private void FileContentViewer_TextChanged(object sender, TextChangedEventArgs e)
        {
            _isFileModified = true;
            UpdateWindowTitle();
        }

        private void UpdateWindowTitle()
        {
            string fileName = string.IsNullOrEmpty(_currentFilePath)
                ? "Новый файл"
                : Path.GetFileName(_currentFilePath);

            string modifiedMarker = _isFileModified ? "*" : "";
            Title = $"Компилятор: {fileName}{modifiedMarker}";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        private string GetTokenDescription(string token)
        {
            return token switch
            {
                "DICTIONARY" => "Ключевое слово Dictionary",
                "NEW" => "Ключевое слово new",
                "INT" => "Тип данных int",
                "STRING" => "Тип данных string",
                "BOOL" => "Тип данных bool",
                "IDENTIFIER" => "Идентификатор (имя переменной)",
                "INTEGER_LITERAL" => "Целое число",
                "STRING_LITERAL" => "Строка",
                "COMMA" => "Запятая",
                "SEMI" => "Точка с запятой",
                "LBRACE" => "Открывающая фигурная скобка {",
                "RBRACE" => "Закрывающая фигурная скобка }",
                "LT" => "Символ <",
                "GT" => "Символ >",
                "ASSIGN" => "Оператор присваивания =",
                _ => "Другое"
            };
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string input = GetEditorText();

            if (string.IsNullOrWhiteSpace(input))
            {
                OutputDataGrid.ItemsSource = new List<ErrorRow>
        {
            new ErrorRow
            {
                ErrorType = "Ошибка",
                Message = "Поле ввода пустое.",
                Line = 0,
                Column = 0
            }
        };
                return;
            }

            try
            {
                var inputStream = new AntlrInputStream(input);
                var lexer = new DictionaryDeclarationLexer(inputStream);
                var lexerErrors = new LexerErrorListener();

                lexer.RemoveErrorListeners();
                lexer.AddErrorListener(lexerErrors);

                var tokens = new CommonTokenStream(lexer);
                tokens.Fill();

                var parser = new DictionaryDeclarationParser(tokens);
                var parserErrors = new CollectingErrorListener();

                parser.RemoveErrorListeners();
                parser.AddErrorListener(parserErrors);

                parser.dictionaryDeclaration();

                if (lexerErrors.Errors.Count == 0 && parserErrors.Errors.Count == 0)
                {
                    var tokenTable = new List<TokenInfo>();
                    int counter = 1;

                    foreach (var token in tokens.GetTokens())
                    {
                        if (token.Type == TokenConstants.EOF)
                            continue;

                        string tokenName = lexer.Vocabulary.GetSymbolicName(token.Type) ?? token.Type.ToString();
                        string lexemeText = string.IsNullOrEmpty(token.Text) ? "(пусто)" : token.Text;

                        tokenTable.Add(new TokenInfo
                        {
                            Number = counter++,
                            Lexeme = lexemeText,
                            TokenType = tokenName,
                            Description = GetTokenDescription(tokenName),
                            Line = token.Line,
                            Column = token.Column
                        });
                    }

                    OutputDataGrid.ItemsSource = tokenTable;

                    MessageBox.Show(
                        "Конструкция корректна. В таблице показаны найденные токены и их описание.",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    var errorTable = new List<ErrorRow>();

                    foreach (var err in lexerErrors.Errors)
                    {
                        errorTable.Add(new ErrorRow
                        {
                            ErrorType = "Лексическая ошибка",
                            Message = err.Message,
                            Line = err.Line,
                            Column = err.Column
                        });
                    }

                    foreach (var err in parserErrors.Errors)
                    {
                        errorTable.Add(new ErrorRow
                        {
                            ErrorType = "Синтаксическая ошибка",
                            Message = err.Message,
                            Line = err.Line,
                            Column = err.Column
                        });
                    }

                    OutputDataGrid.ItemsSource = errorTable;

                    MessageBox.Show(
                        "Обнаружены ошибки. Они показаны в таблице вывода.",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                OutputDataGrid.ItemsSource = new List<ErrorRow>
        {
            new ErrorRow
            {
                ErrorType = "Критическая ошибка",
                Message = ex.Message,
                Line = 0,
                Column = 0
            }
        };
            }
        }
    }
}