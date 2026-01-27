// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace AncestralVault.Common.Exceptions
{
    public class PlaceParserException : Exception
    {
        public PlaceParserException(string message)
            : base(message)
        {
        }
    }
}
