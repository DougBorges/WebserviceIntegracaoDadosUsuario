using System;
using System.Text;

namespace IntegracaoDadosUsuario.WS {
    public class TextHelper {
        public static String Base64Encode(String plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static String Base64Decode(String base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}