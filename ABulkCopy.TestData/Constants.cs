﻿using Azure.Core;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ABulkCopy.TestData;

public static class Constants
{
    public struct Data
    {
        public const int IdLength = 255;
        public const int MinNameLength = 1;
        public const int NameLength = 255;
        public const int ValueLength = 2047;
        public const int LanguageLength = 20;
        public const int UriLength = 2047;
    }
}