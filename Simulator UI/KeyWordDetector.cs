using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Simulator_UI
{
    public class KeyWordDetector
    {
        private static readonly Dictionary<string, SolidColorBrush>
            KEYWORD_BRUSHES = new Dictionary<string, SolidColorBrush>
            {
                // Data movement
                // { OP_CODE, INSTRUCTION_FORMAT, NUM_OF_PARAMS }
                { "LOAD",       Brushes.Blue },
                { "LOADIM",     Brushes.Blue },
                { "POP",        Brushes.Blue },
                { "STORE",      Brushes.Blue },
                { "PUSH",       Brushes.Blue },
                { "LOADRIND",   Brushes.Blue },
                { "STOREIND",   Brushes.Blue },
                // Arithmetic Operations
                { "ADD",        Brushes.Green },
                { "SUB",        Brushes.Green },
                { "ADDIM",      Brushes.Green },
                { "SUBIM",      Brushes.Green },
                //Logic operations
                { "AND",        Brushes.Aqua },
                { "OR",         Brushes.Aqua },
                { "XOR",        Brushes.Aqua },
                { "NOT",        Brushes.Aqua },
                { "NEG",        Brushes.Aqua },
                { "SHIFTR",     Brushes.Aqua },
                { "SHIFTL",     Brushes.Aqua },
                { "ROTAR",      Brushes.Aqua },
                { "ROTAL",      Brushes.Aqua },
                // Flow Control
                { "JMPRIND",    Brushes.Salmon },
                { "JMPADDR",    Brushes.Salmon },
                { "JCONDRIN",   Brushes.Salmon },
                { "JCONDADDR",  Brushes.Salmon },
                { "LOOP",       Brushes.Salmon },
                { "GRT",        Brushes.Salmon },
                { "GRTEQ",      Brushes.Salmon },
                { "EQ",         Brushes.Salmon },
                { "NEQ",        Brushes.Salmon },
                { "NOP",        Brushes.Salmon },
                { "CALL",       Brushes.Salmon },
                { "RETURN",     Brushes.Salmon }
            };
        private RichTextBox _rb;
        public KeyWordDetector(RichTextBox rb)
        {
            _rb = rb;
        }
        public void AnalizeContent()
        {
            //foreach (string word in KEYWORD_BRUSHES.Keys)
            //{

            //    Detect(word);
            //}
            Detect("ADD");
        }

        public void Detect(string word)
        {
            ////FlowDocument doc = rb.Document;
            //TextRange textRange = new TextRange(_rb.Document.ContentStart, _rb.Document.ContentEnd);

            //if (textRange.Text.Length < word.Length)
            //    return;
            //if (textRange.Text.Contains(word))
            //{
            //    int index = -1;
            //    //int selectStart = this.Rchtxt.SelectionStart;
            //    TextPointer position = _rb.Document.ContentStart;

            //    while ((index = textRange.Text.IndexOf(word, (index + 1))) != -1)
            //    {
            //        System.Windows.MessageBox.Show(textRange.Text + "Found");
            //        TextPointer start = position.GetPositionAtOffset(index);
            //        TextPointer end = start.GetPositionAtOffset(word.Length);

            //        textRange.Select((start), end);
            //        _rb.SelectionBrush = KEYWORD_BRUSHES[word];
            //        textRange.Select(position, position.GetPositionAtOffset(0));
            //        _rb.SelectionBrush = Brushes.Black;
            //        return;
            //    }
            //}

            _rb.SelectAll();
            _rb.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);


            Regex reg = new Regex(word, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            TextPointer position = _rb.Document.ContentStart;
            List<TextRange> ranges = new List<TextRange>();
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string text = position.GetTextInRun(LogicalDirection.Forward);
                    var matchs = reg.Matches(text);

                    for (int i = 0; i < matchs.Count; i++)
                    {

                        TextPointer start = position.GetPositionAtOffset(matchs[i].Index);
                        TextPointer end = start.GetPositionAtOffset(word.Length);

                        TextRange textrange = new TextRange(start, end);
                        ranges.Add(textrange);
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }


            foreach (TextRange range in ranges)
            {
                range.ApplyPropertyValue(TextElement.ForegroundProperty, KEYWORD_BRUSHES[word]);
            }

        }
    }
}
