using System.Collections.Generic;
using System.Windows.Media;

namespace Simulator_UI
{
    public class KeyWordDetector
    {
        private static readonly Dictionary<string, SolidColorBrush>
            KEYWORD_BRUSHES = new Dictionary<string, SolidColorBrush>
            {
                // Data movement
                { "LOAD",       Brushes.DeepSkyBlue },
                { "LOADIM",     Brushes.DeepSkyBlue },
                { "POP",        Brushes.DeepSkyBlue },
                { "STORE",      Brushes.DeepSkyBlue },
                { "PUSH",       Brushes.DeepSkyBlue },
                { "LOADRIND",   Brushes.DeepSkyBlue },
                { "STOREIND",   Brushes.DeepSkyBlue },
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
                { "RETURN",     Brushes.Salmon },
                // Registers
                 { "R1"         , Brushes.Red   },
                 { "R2"         , Brushes.Red   },
                 { "R3"         , Brushes.Red   },
                 { "R4"         , Brushes.Red   },
                 { "R5"         , Brushes.Red   },
                 { "R6"         , Brushes.Red   },
                 { "R7"         , Brushes.Red   },

                // other
                { "DB"          , Brushes.Yellow },
                { "ORG"         , Brushes.Yellow },
                { "CONST"       , Brushes.Yellow },
                { "//"          , Brushes.LightSeaGreen },
            };

        /// <summary>
        /// Gets the highlight color associated with keyword
        /// </summary>
        /// <param name="text">Target string</param>
        /// <returns>True if keyword, false otherwise</returns>
        public static bool IsKeyword(string text, out SolidColorBrush color)
        {
            if (text.StartsWith("//"))
            {
                color = Brushes.LightSeaGreen;
                return true;
            }

            return KEYWORD_BRUSHES.TryGetValue(text.ToUpper(), out color);
        }
    }
}
