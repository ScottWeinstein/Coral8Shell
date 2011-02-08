using System;
using System.IO;
using System.Text;
using Wintellect.Threading.AsyncProgModel;
using System.Collections.Generic;

namespace C8cx
{
    public class AsyncStreamReader : IDisposable
    {
        private Stream stream;
        private Encoding encoding;
        private Decoder decoder;
        private byte[] byteBuffer;
        private char[] charBuffer;
        private int charPos;
        private int charLen;
        private int byteLen;
        internal AsyncStreamReader()
        {
        }

        public event Action<object, string> LineRead;
        public event Action<object> EOFReached;


        public AsyncStreamReader(Stream stream, Encoding encoding)
        {
            LineRead += (_, e) => { };
            EOFReached += (_) => { };

            this.stream = stream;
            this.encoding = encoding;
            decoder = encoding.GetDecoder();
            byteBuffer = new byte[1024];
            charBuffer = new char[encoding.GetMaxCharCount(byteBuffer.Length)];
            byteLen = 0;
        }

        public void Dispose()
        {
            try
            {
                if (stream != null)
                    stream.Close();
            }
            finally
            {
                stream = null;
            }
        }

        private int ReadBuffer()
        {
            charLen = 0;
            charPos = 0;
            byteLen = 0;
            do
            {
                byteLen = stream.Read(byteBuffer, 0, byteBuffer.Length);
                if (byteLen == 0)  // We're at EOF
                    return charLen;

                charLen += decoder.GetChars(byteBuffer, 0, byteLen, charBuffer, charLen);
            } while (charLen == 0);
            return charLen;
        }

        public void ReadLineAsync()
        {
            AsyncEnumerator ae = new AsyncEnumerator();
            ae.BeginExecute(Process(ae), ae.EndExecute, null);
        }

        private IEnumerator<Int32> Process(AsyncEnumerator ae)
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                if (charPos == charLen)
                {
                    do
                    {
                        charLen = 0;
                        charPos = 0;
                        byteLen = 0;

                        stream.BeginRead(byteBuffer, 0, byteBuffer.Length, ae.End(), null);
                        yield return 1;
                        byteLen = stream.EndRead(ae.DequeueAsyncResult());
                        if (byteLen == 0)
                        {
                            if (sb.Length > 0)
                            {
                                LineRead(this, sb.ToString());
                                sb.Length = 0;
                            }
                            EOFReached(this);
                            yield break;
                        }
                        charLen += decoder.GetChars(byteBuffer, 0, byteLen, charBuffer, charLen);
                    } while (charLen ==0);
                }
                int i = charPos;
                do
                {
                    char ch = charBuffer[i];
                    if (ch == '\r' || ch == '\n')
                    {
                        sb.Append(charBuffer, charPos, i - charPos);
                        charPos = i + 1;
                        if (ch == '\r' && (charPos < charLen )) //|| ReadBuffer() > 0)
                        {
                            if (charBuffer[charPos] == '\n')
                            {
                                charPos++;
                                i++;
                            }
                        }
                        LineRead(this, sb.ToString());
                        sb.Length=0;
                    }
                    i++;
                } while (i < charLen);
                i = charLen - charPos;
                sb.Append(charBuffer, charPos, i);
                charPos = charLen;
            }
        }
    }
}
        //private void EndRead(IAsyncResult ar)
        //{
        //    _readlineDel.EndInvoke(ar);
        //    stream.EndRead(ar);
        //}
        ////public string EndReadLine(IAsyncResult asyncResult)
        ////{
        ////}
        //public String ReadLine()
        //{
            
        //}

        //public void EndGetBuffer(IAsyncResult ar)
        //{
        //    byteLen = stream.EndRead(ar);
        //}


//                if (ReadBuffer() == 0)
//                    return null;
//            }
//    }
//}



//if (_checkPreamble)
//                {
//                    int len = stream.Read(byteBuffer, bytePos, byteBuffer.Length - bytePos);
//                    if (len == 0)
//                    {
//                        if (byteLen > 0)
//                            charLen += decoder.GetChars(byteBuffer, 0, byteLen, charBuffer, charLen);

//                        return charLen;
//                    }

//                    byteLen += len;
//                }
//                else
//                {
//_isBlocked = (byteLen < byteBuffer.Length);


//private void CompressBuffer(int n)
//{
//    Debug.Assert(byteLen >= n, "CompressBuffer was called with a number of bytes greater than the current buffer length.  Are two threads using this StreamReader at the same time?");
//    Buffer.BlockCopy(byteBuffer, n, byteBuffer, 0, byteLen - n);
//    byteLen -= n;
//}
//private bool IsPreamble()
//{
//    if (!_checkPreamble)
//        return _checkPreamble;

//    Debug.Assert(bytePos <= _preamble.Length, "_compressPreamble was called with the current bytePos greater than the preamble buffer length.  Are two threads using this StreamReader at the same time?");
//    int len = (byteLen >= (_preamble.Length)) ? (_preamble.Length - bytePos) : (byteLen - bytePos);

//    for (int i = 0; i < len; i++, bytePos++)
//    {
//        if (byteBuffer[bytePos] == _preamble[bytePos])
//            continue;

//        bytePos = 0;
//        _checkPreamble = false;
//        break;
//    }

//    Debug.Assert(bytePos <= _preamble.Length, "possible bug in _compressPreamble.  Are two threads using this StreamReader at the same time?");

//    if (!_checkPreamble)
//        return _checkPreamble;

//    if (bytePos != _preamble.Length)
//        return _checkPreamble;

//    // We have a match 
//    CompressBuffer(_preamble.Length);
//    bytePos = 0;
//    _checkPreamble = false;

//    return _checkPreamble;
//}
