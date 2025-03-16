namespace Kekser.UnityCVarConsole
{
    public static class CharMapping
    {
        public static byte GetCP437Byte(char c)
        {
            // Direct mapping for Code Page 437 (DOS/IBM PC OEM US)
            switch (c)
            {
                // Control characters (0-31)
                case '\u0000': return 0;   // NULL
                case '\u0001': return 1;   // START OF HEADING
                case '\u0002': return 2;   // START OF TEXT
                case '\u0003': return 3;   // END OF TEXT
                case '\u0004': return 4;   // END OF TRANSMISSION
                case '\u0005': return 5;   // ENQUIRY
                case '\u0006': return 6;   // ACKNOWLEDGE
                case '\u0007': return 7;   // BELL
                case '\u0008': return 8;   // BACKSPACE
                case '\u0009': return 9;   // HORIZONTAL TAB
                case '\u000A': return 10;  // LINE FEED
                case '\u000B': return 11;  // VERTICAL TAB
                case '\u000C': return 12;  // FORM FEED
                case '\u000D': return 13;  // CARRIAGE RETURN
                case '\u000E': return 14;  // SHIFT OUT
                case '\u000F': return 15;  // SHIFT IN
                case '\u0010': return 16;  // DATA LINK ESCAPE
                case '\u0011': return 17;  // DEVICE CONTROL 1
                case '\u0012': return 18;  // DEVICE CONTROL 2
                case '\u0013': return 19;  // DEVICE CONTROL 3
                case '\u0014': return 20;  // DEVICE CONTROL 4
                case '\u0015': return 21;  // NEGATIVE ACKNOWLEDGE
                case '\u0016': return 22;  // SYNCHRONOUS IDLE
                case '\u0017': return 23;  // END OF TRANSMISSION BLOCK
                case '\u0018': return 24;  // CANCEL
                case '\u0019': return 25;  // END OF MEDIUM
                case '\u001A': return 26;  // SUBSTITUTE
                case '\u001B': return 27;  // ESCAPE
                case '\u001C': return 28;  // FILE SEPARATOR
                case '\u001D': return 29;  // GROUP SEPARATOR
                case '\u001E': return 30;  // RECORD SEPARATOR
                case '\u001F': return 31;  // UNIT SEPARATOR

                // Standard ASCII (32-127)
                case ' ': return 32;
                case '!': return 33;
                case '"': return 34;
                case '#': return 35;
                case '$': return 36;
                case '%': return 37;
                case '&': return 38;
                case '\'': return 39;
                case '(': return 40;
                case ')': return 41;
                case '*': return 42;
                case '+': return 43;
                case ',': return 44;
                case '-': return 45;
                case '.': return 46;
                case '/': return 47;
                case '0': return 48;
                case '1': return 49;
                case '2': return 50;
                case '3': return 51;
                case '4': return 52;
                case '5': return 53;
                case '6': return 54;
                case '7': return 55;
                case '8': return 56;
                case '9': return 57;
                case ':': return 58;
                case ';': return 59;
                case '<': return 60;
                case '=': return 61;
                case '>': return 62;
                case '?': return 63;
                case '@': return 64;
                case 'A': return 65;
                case 'B': return 66;
                case 'C': return 67;
                case 'D': return 68;
                case 'E': return 69;
                case 'F': return 70;
                case 'G': return 71;
                case 'H': return 72;
                case 'I': return 73;
                case 'J': return 74;
                case 'K': return 75;
                case 'L': return 76;
                case 'M': return 77;
                case 'N': return 78;
                case 'O': return 79;
                case 'P': return 80;
                case 'Q': return 81;
                case 'R': return 82;
                case 'S': return 83;
                case 'T': return 84;
                case 'U': return 85;
                case 'V': return 86;
                case 'W': return 87;
                case 'X': return 88;
                case 'Y': return 89;
                case 'Z': return 90;
                case '[': return 91;
                case '\\': return 92;
                case ']': return 93;
                case '^': return 94;
                case '_': return 95;
                case '`': return 96;
                case 'a': return 97;
                case 'b': return 98;
                case 'c': return 99;
                case 'd': return 100;
                case 'e': return 101;
                case 'f': return 102;
                case 'g': return 103;
                case 'h': return 104;
                case 'i': return 105;
                case 'j': return 106;
                case 'k': return 107;
                case 'l': return 108;
                case 'm': return 109;
                case 'n': return 110;
                case 'o': return 111;
                case 'p': return 112;
                case 'q': return 113;
                case 'r': return 114;
                case 's': return 115;
                case 't': return 116;
                case 'u': return 117;
                case 'v': return 118;
                case 'w': return 119;
                case 'x': return 120;
                case 'y': return 121;
                case 'z': return 122;
                case '{': return 123;
                case '|': return 124;
                case '}': return 125;
                case '~': return 126;
                case '\u007F': return 127;  // DELETE

                // Extended ASCII (128-255) - Code Page 437 specific
                case '\u00C7': return 128;  // Ç - LATIN CAPITAL LETTER C WITH CEDILLA
                case '\u00FC': return 129;  // ü - LATIN SMALL LETTER U WITH DIAERESIS
                case '\u00E9': return 130;  // é - LATIN SMALL LETTER E WITH ACUTE
                case '\u00E2': return 131;  // â - LATIN SMALL LETTER A WITH CIRCUMFLEX
                case '\u00E4': return 132;  // ä - LATIN SMALL LETTER A WITH DIAERESIS
                case '\u00E0': return 133;  // à - LATIN SMALL LETTER A WITH GRAVE
                case '\u00E5': return 134;  // å - LATIN SMALL LETTER A WITH RING ABOVE
                case '\u00E7': return 135;  // ç - LATIN SMALL LETTER C WITH CEDILLA
                case '\u00EA': return 136;  // ê - LATIN SMALL LETTER E WITH CIRCUMFLEX
                case '\u00EB': return 137;  // ë - LATIN SMALL LETTER E WITH DIAERESIS
                case '\u00E8': return 138;  // è - LATIN SMALL LETTER E WITH GRAVE
                case '\u00EF': return 139;  // ï - LATIN SMALL LETTER I WITH DIAERESIS
                case '\u00EE': return 140;  // î - LATIN SMALL LETTER I WITH CIRCUMFLEX
                case '\u00EC': return 141;  // ì - LATIN SMALL LETTER I WITH GRAVE
                case '\u00C4': return 142;  // Ä - LATIN CAPITAL LETTER A WITH DIAERESIS
                case '\u00C5': return 143;  // Å - LATIN CAPITAL LETTER A WITH RING ABOVE
                case '\u00C9': return 144;  // É - LATIN CAPITAL LETTER E WITH ACUTE
                case '\u00E6': return 145;  // æ - LATIN SMALL LETTER AE
                case '\u00C6': return 146;  // Æ - LATIN CAPITAL LETTER AE
                case '\u00F4': return 147;  // ô - LATIN SMALL LETTER O WITH CIRCUMFLEX
                case '\u00F6': return 148;  // ö - LATIN SMALL LETTER O WITH DIAERESIS
                case '\u00F2': return 149;  // ò - LATIN SMALL LETTER O WITH GRAVE
                case '\u00FB': return 150;  // û - LATIN SMALL LETTER U WITH CIRCUMFLEX
                case '\u00F9': return 151;  // ù - LATIN SMALL LETTER U WITH GRAVE
                case '\u00FF': return 152;  // ÿ - LATIN SMALL LETTER Y WITH DIAERESIS
                case '\u00D6': return 153;  // Ö - LATIN CAPITAL LETTER O WITH DIAERESIS
                case '\u00DC': return 154;  // Ü - LATIN CAPITAL LETTER U WITH DIAERESIS
                case '\u00A2': return 155;  // ¢ - CENT SIGN
                case '\u00A3': return 156;  // £ - POUND SIGN
                case '\u00A5': return 157;  // ¥ - YEN SIGN
                case '\u20A7': return 158;  // ₧ - PESETA SIGN
                case '\u0192': return 159;  // ƒ - LATIN SMALL LETTER F WITH HOOK
                case '\u00E1': return 160;  // á - LATIN SMALL LETTER A WITH ACUTE
                case '\u00ED': return 161;  // í - LATIN SMALL LETTER I WITH ACUTE
                case '\u00F3': return 162;  // ó - LATIN SMALL LETTER O WITH ACUTE
                case '\u00FA': return 163;  // ú - LATIN SMALL LETTER U WITH ACUTE
                case '\u00F1': return 164;  // ñ - LATIN SMALL LETTER N WITH TILDE
                case '\u00D1': return 165;  // Ñ - LATIN CAPITAL LETTER N WITH TILDE
                case '\u00AA': return 166;  // ª - FEMININE ORDINAL INDICATOR
                case '\u00BA': return 167;  // º - MASCULINE ORDINAL INDICATOR
                case '\u00BF': return 168;  // ¿ - INVERTED QUESTION MARK
                case '\u2310': return 169;  // ⌐ - REVERSED NOT SIGN
                case '\u00AC': return 170;  // ¬ - NOT SIGN
                case '\u00BD': return 171;  // ½ - VULGAR FRACTION ONE HALF
                case '\u00BC': return 172;  // ¼ - VULGAR FRACTION ONE QUARTER
                case '\u00A1': return 173;  // ¡ - INVERTED EXCLAMATION MARK
                case '\u00AB': return 174;  // « - LEFT-POINTING DOUBLE ANGLE QUOTATION MARK
                case '\u00BB': return 175;  // » - RIGHT-POINTING DOUBLE ANGLE QUOTATION MARK
                case '\u2591': return 176;  // ░ - LIGHT SHADE
                case '\u2592': return 177;  // ▒ - MEDIUM SHADE
                case '\u2593': return 178;  // ▓ - DARK SHADE
                case '\u2502': return 179;  // │ - BOX DRAWINGS LIGHT VERTICAL
                case '\u2524': return 180;  // ┤ - BOX DRAWINGS LIGHT VERTICAL AND LEFT
                case '\u2561': return 181;  // ╡ - BOX DRAWINGS VERTICAL SINGLE AND LEFT DOUBLE
                case '\u2562': return 182;  // ╢ - BOX DRAWINGS VERTICAL DOUBLE AND LEFT SINGLE
                case '\u2556': return 183;  // ╖ - BOX DRAWINGS DOWN DOUBLE AND LEFT SINGLE
                case '\u2555': return 184;  // ╕ - BOX DRAWINGS DOWN SINGLE AND LEFT DOUBLE
                case '\u2563': return 185;  // ╣ - BOX DRAWINGS DOUBLE VERTICAL AND LEFT
                case '\u2551': return 186;  // ║ - BOX DRAWINGS DOUBLE VERTICAL
                case '\u2557': return 187;  // ╗ - BOX DRAWINGS DOUBLE DOWN AND LEFT
                case '\u255D': return 188;  // ╝ - BOX DRAWINGS DOUBLE UP AND LEFT
                case '\u255C': return 189;  // ╜ - BOX DRAWINGS UP DOUBLE AND LEFT SINGLE
                case '\u255B': return 190;  // ╛ - BOX DRAWINGS UP SINGLE AND LEFT DOUBLE
                case '\u2510': return 191;  // ┐ - BOX DRAWINGS LIGHT DOWN AND LEFT
                case '\u2514': return 192;  // └ - BOX DRAWINGS LIGHT UP AND RIGHT
                case '\u2534': return 193;  // ┴ - BOX DRAWINGS LIGHT UP AND HORIZONTAL
                case '\u252C': return 194;  // ┬ - BOX DRAWINGS LIGHT DOWN AND HORIZONTAL
                case '\u251C': return 195;  // ├ - BOX DRAWINGS LIGHT VERTICAL AND RIGHT
                case '\u2500': return 196;  // ─ - BOX DRAWINGS LIGHT HORIZONTAL
                case '\u253C': return 197;  // ┼ - BOX DRAWINGS LIGHT VERTICAL AND HORIZONTAL
                case '\u255E': return 198;  // ╞ - BOX DRAWINGS VERTICAL SINGLE AND RIGHT DOUBLE
                case '\u255F': return 199;  // ╟ - BOX DRAWINGS VERTICAL DOUBLE AND RIGHT SINGLE
                case '\u255A': return 200;  // ╚ - BOX DRAWINGS DOUBLE UP AND RIGHT
                case '\u2554': return 201;  // ╔ - BOX DRAWINGS DOUBLE DOWN AND RIGHT
                case '\u2569': return 202;  // ╩ - BOX DRAWINGS DOUBLE UP AND HORIZONTAL
                case '\u2566': return 203;  // ╦ - BOX DRAWINGS DOUBLE DOWN AND HORIZONTAL
                case '\u2560': return 204;  // ╠ - BOX DRAWINGS DOUBLE VERTICAL AND RIGHT
                case '\u2550': return 205;  // ═ - BOX DRAWINGS DOUBLE HORIZONTAL
                case '\u256C': return 206;  // ╬ - BOX DRAWINGS DOUBLE VERTICAL AND HORIZONTAL
                case '\u2567': return 207;  // ╧ - BOX DRAWINGS UP SINGLE AND HORIZONTAL DOUBLE
                case '\u2568': return 208;  // ╨ - BOX DRAWINGS UP DOUBLE AND HORIZONTAL SINGLE
                case '\u2564': return 209;  // ╤ - BOX DRAWINGS DOWN SINGLE AND HORIZONTAL DOUBLE
                case '\u2565': return 210;  // ╥ - BOX DRAWINGS DOWN DOUBLE AND HORIZONTAL SINGLE
                case '\u2559': return 211;  // ╙ - BOX DRAWINGS UP DOUBLE AND RIGHT SINGLE
                case '\u2558': return 212;  // ╘ - BOX DRAWINGS UP SINGLE AND RIGHT DOUBLE
                case '\u2552': return 213;  // ╒ - BOX DRAWINGS DOWN SINGLE AND RIGHT DOUBLE
                case '\u2553': return 214;  // ╓ - BOX DRAWINGS DOWN DOUBLE AND RIGHT SINGLE
                case '\u256B': return 215;  // ╫ - BOX DRAWINGS VERTICAL DOUBLE AND HORIZONTAL SINGLE
                case '\u256A': return 216;  // ╪ - BOX DRAWINGS VERTICAL SINGLE AND HORIZONTAL DOUBLE
                case '\u2518': return 217;  // ┘ - BOX DRAWINGS LIGHT UP AND LEFT
                case '\u250C': return 218;  // ┌ - BOX DRAWINGS LIGHT DOWN AND RIGHT
                case '\u2588': return 219;  // █ - FULL BLOCK
                case '\u2584': return 220;  // ▄ - LOWER HALF BLOCK
                case '\u258C': return 221;  // ▌ - LEFT HALF BLOCK
                case '\u2590': return 222;  // ▐ - RIGHT HALF BLOCK
                case '\u2580': return 223;  // ▀ - UPPER HALF BLOCK
                case '\u03B1': return 224;  // α - GREEK SMALL LETTER ALPHA
                case '\u00DF': return 225;  // ß - LATIN SMALL LETTER SHARP S
                case '\u0393': return 226;  // Γ - GREEK CAPITAL LETTER GAMMA
                case '\u03C0': return 227;  // π - GREEK SMALL LETTER PI
                case '\u03A3': return 228;  // Σ - GREEK CAPITAL LETTER SIGMA
                case '\u03C3': return 229;  // σ - GREEK SMALL LETTER SIGMA
                case '\u00B5': return 230;  // µ - MICRO SIGN
                case '\u03C4': return 231;  // τ - GREEK SMALL LETTER TAU
                case '\u03A6': return 232;  // Φ - GREEK CAPITAL LETTER PHI
                case '\u0398': return 233;  // Θ - GREEK CAPITAL LETTER THETA
                case '\u03A9': return 234;  // Ω - GREEK CAPITAL LETTER OMEGA
                case '\u03B4': return 235;  // δ - GREEK SMALL LETTER DELTA
                case '\u221E': return 236;  // ∞ - INFINITY
                case '\u03C6': return 237;  // φ - GREEK SMALL LETTER PHI
                case '\u03B5': return 238;  // ε - GREEK SMALL LETTER EPSILON
                case '\u2229': return 239;  // ∩ - INTERSECTION
                case '\u2261': return 240;  // ≡ - IDENTICAL TO
                case '\u00B1': return 241;  // ± - PLUS-MINUS SIGN
                case '\u2265': return 242;  // ≥ - GREATER-THAN OR EQUAL TO
                case '\u2264': return 243;  // ≤ - LESS-THAN OR EQUAL TO
                case '\u2320': return 244;  // ⌠ - TOP HALF INTEGRAL
                case '\u2321': return 245;  // ⌡ - BOTTOM HALF INTEGRAL
                case '\u00F7': return 246;  // ÷ - DIVISION SIGN
                case '\u2248': return 247;  // ≈ - ALMOST EQUAL TO
                case '\u00B0': return 248;  // ° - DEGREE SIGN
                case '\u2219': return 249;  // ∙ - BULLET OPERATOR
                case '\u00B7': return 250;  // · - MIDDLE DOT
                case '\u221A': return 251;  // √ - SQUARE ROOT
                case '\u207F': return 252;  // ⁿ - SUPERSCRIPT LATIN SMALL LETTER N
                case '\u00B2': return 253;  // ² - SUPERSCRIPT TWO
                case '\u25A0': return 254;  // ■ - BLACK SQUARE
                case '\u00A0': return 255;  // NBSP - NO-BREAK SPACE

                default:
                    // Handle basic ASCII characters (0-127)
                    if (c < 128)
                        return (byte)c;
                    return (byte)'?'; // Default replacement character for unmapped values
            }
        }
    }
}