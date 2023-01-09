using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubach.Encoding.Jwt;
public class JsonWebToken
{
    public JsonWebToken(string jwt)
    {
        var parts = jwt.Split('.');
        if (parts.Length != 3)
        {
            throw new ArgumentException($"{nameof(jwt)} is malformed. It don't have 3 parts");
        }

        Header = parts[0];
        Payload= parts[1];
        Signature = parts[2];
    }

    public string Header { get; set; }
    public string Payload { get; set; }
    public string Signature { get; set; }
}
