namespace Ubach.Encoding.Enigma;

internal class Plugboard
{
    public string _alpha = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
    public string _pattern = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

    public void Swap(int a, int b)
    {
        _pattern = a != b && _alpha[a] == _pattern[a] && _alpha[b] == _pattern[b]
            ? _pattern.Replace(_alpha[a], '~').Replace(_pattern[b], _alpha[a]).Replace('~', _pattern[b])
            : throw new Exception();
    }

    public int GetIndex(int x)
    {
        return _pattern.IndexOf(_alpha[x]);
    }
}
