// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("3WtU2ps761sIL8Gpk6DHhX7vIg2Myh+BTnYle6iNzW5tAk6F+dUbXF5Z+/CjieZffGeduY6uKJ2RB2C7sTI8MwOxMjkxsTIyM55VWzLlZWuWX328pTC2Js1NUPtH/YY903PBkQOxMhEDPjU6GbV7tcQ+MjIyNjMwQ7TNDO9ClSWw35Q+Fa+51j1AnHHtIe54sQfYy4Th7dWn1qcz2ggZPRKbyJX6No1dLPrvbs+PJapym9eEDGUkvSxnQW5JOxkO88QnR1p3VkoPsc2j3C0Fsgv8rcXmbCsPI6biqs5ab2bo8Zz9xxXtMMOQ3ar4+eoiWneuTw8mPoC+RiwcBOtx73s2duZ9wYIgEHFUxWcggJ9zwrkFg/pVkvuYHQJw8OrFNDEwMjMy");
        private static int[] order = new int[] { 12,2,8,8,13,12,10,12,10,10,13,13,13,13,14 };
        private static int key = 51;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
