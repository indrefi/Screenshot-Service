using ScreenshotsService.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ScreenshotsService.Services
{
    public class ComputeSHA256: IHashService
    {
        public string GetHash(string inputString)
        {
            var bytes = Encoding.UTF8.GetBytes(inputString);

            using (var sha = SHA256.Create())
            {
                var hashBytes = sha.ComputeHash(bytes);

                return HexStringFromBytes(hashBytes);
            }
        }

        private string HexStringFromBytes(byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var b in bytes)
            {
                var hexString = b.ToString("x2");
                stringBuilder.Append(hexString);
            }

            return stringBuilder.ToString();
        }
    }
}
