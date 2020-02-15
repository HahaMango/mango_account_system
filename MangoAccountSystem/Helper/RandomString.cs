using System;

namespace MangoAccountSystem.Helper
{
    public static class RandomString
    {
        /// <summary>
        /// 生产bit位随机字符串（字符可能相同）
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static string NextString(int bits)
        {
            string result="";
            Random random = new Random();
            int r = random.Next(0, 62);
            for(int i = 0; i < bits; i++)
            {
                char c;
                if (r <= 10)
                {
                    c = (char)(r + 48);
                }
                else if(r>10 && r<= 36)
                {
                    c = (char)(r + 65);
                }
                else
                {
                    c = (char)(r + 97);
                }
                result = result + c;
            }

            return result;
        }
    }
}
