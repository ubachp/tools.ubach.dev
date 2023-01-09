namespace Ubach.Encoding.Enigma;

internal class Reflector
{
    private readonly string _alpha = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
    private readonly string _pattern;

    public Reflector(string pattern)
    {
        _pattern = pattern;
    }

    public int GetIndex(int x)
    {
        return _pattern.IndexOf(_alpha[x]);
    }
}
