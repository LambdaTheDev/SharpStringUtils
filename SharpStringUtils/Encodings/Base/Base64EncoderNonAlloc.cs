/*
 * BSD 2-Clause License
Copyright (c) 2017, Ramon Smits
All rights reserved.
Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */




// PREVIOUSLY: Exyll
namespace LambdaTheDev.SharpStringUtils.Encodings.Base
{
    public class Base64EncoderNonAlloc : BaseEncoderNonAlloc
    {
        const string CharacterSetBase = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public readonly char PlusChar;
        public readonly char SlashChar;

        public static readonly Base64EncoderNonAlloc Default = new Base64EncoderNonAlloc('+', '/', true);
        public static readonly Base64EncoderNonAlloc UrlEncoding = new Base64EncoderNonAlloc('-', '_', false);
     
        // I removed fields that I won't use
        // public static readonly Base64Encoder DefaultNoPadding = new Base64Encoder('+', '/', false);
        // public static readonly Base64Encoder XmlEncoding = new Base64Encoder('_', ':', false);
        // public static readonly Base64Encoder RegExEncoding = new Base64Encoder('!', '-', false);
        // public static readonly Base64Encoder FileEncoding = new Base64Encoder('+', '-', false);

        public Base64EncoderNonAlloc(char plusChar, char slashChar, bool paddingEnabled)
            : base((CharacterSetBase + plusChar + slashChar).ToCharArray(), paddingEnabled)
        {
            PlusChar = plusChar;
            SlashChar = slashChar;
        }
    }
}