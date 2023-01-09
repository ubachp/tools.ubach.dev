namespace Ubach.Encoding.Enigma;

public partial class Rotor
{
    private readonly string _pattern;
    private readonly string _alpha = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";
    private readonly int _notch1;
    private readonly int _notch2;
    private int _reelPosition;
    private int _rsl;

    public Rotor(Rotor rotor)
    {
        _pattern = rotor._pattern;
        _notch1= rotor._notch1;
        _notch2= rotor._notch2;
        _reelPosition = rotor._reelPosition;
        _rsl = rotor._rsl;
    }

    public Rotor((string pattern, int firstNotch, int secondNotch) rotor, int ringPos, int startPos)
    {
        _pattern = rotor.pattern;
        _notch1 = rotor.firstNotch;
        _notch2 = rotor.secondNotch;
        _reelPosition = startPos;
        _rsl = ringPos;
    }

    public bool IsNotched
    {
        get { return (_reelPosition == _notch1 || _reelPosition == _notch2); }
    }

    public void Rotate()
    {
        _reelPosition = (_reelPosition + 1) % _alpha.Length;
    }

    public int Transpose(int x, Direction dir)
    {
        x = (x + _reelPosition - _rsl + _alpha.Length) % _alpha.Length;
        var t = GetIndex(x, dir) - _reelPosition + _rsl + _alpha.Length;
        return t % _alpha.Length;
    }

    private int GetIndex(int x, Direction dir)
    {
        return dir == Direction.Foward ? _alpha.IndexOf(_pattern[x]) : _pattern.IndexOf(_alpha[x]);
    }
    
}
