
﻿using System;
using System.Collections.Generic;
using System.Text;


﻿namespace Assembler.Parsing
{
    /// <summary>
    /// Enum of the different Types of Tokens
    /// </summary>

    public enum TokenType
    {
        LINE_COMMENT,
        NEW_LINE,
        TAB,
        WHITE_SPACE,
        COLON,
        COMMA,
        IDENTIFIER,
        OPERATOR,
        REGISTER,
        VARIABLE_ASSIGN,
        CONSTANT_ASSIGN,
        ORIGIN
    }
}
