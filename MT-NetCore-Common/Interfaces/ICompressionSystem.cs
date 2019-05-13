using System;
using System.Collections.Generic;
using System.Text;

namespace MT_NetCore_Common.Interfaces
{
    public interface ICompressionSystem
    {
        string Decompress(string input);
        byte[] Decompress(byte[] input);
        byte[] Compress(byte[] input);
       string Compress(string input);
    }
}
