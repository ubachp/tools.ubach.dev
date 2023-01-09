using System.Text;
using CommunityToolkit.Diagnostics;
using Ubach.Diagnostics;

namespace Ubach.Encoding.Enigma;
public partial class EnigmaMachine
{
    private Rotor _rotor0;
    private Rotor _rotor1;
    private Rotor _rotor2;
    private Rotor _rotor3;
    private Reflector _reflector;
    private readonly EnigmaSettings _settings;
    private Plugboard _plugboard;
    public static readonly Dictionary<Rotors, (string pattern, int firstNotch, int secondNotch)> AvailableRotors = new()
    {
        { Rotors.Blank, ("AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz", -99, -99) },
        { Rotors.I, ("EeKkMmFfLlGgDdQqVvZzNnTtOoWwYyHhXxUuSsPpAaIiBbRrCcJj", 16, -99) },
        { Rotors.II, ("AaJjDdKkSsIiRrUuXxBbLlHhWwTtMmCcQqGgZzNnPpYyFfVvOoEe", 4, -99) },
        { Rotors.III, ("BbDdFfHhJjLlCcPpRrTtXxVvZzNnYyEeIiWwGgAaKkMmUuSsQqOo", 21, -99) },
        { Rotors.IV, ("EeSsOoVvPpZzJjAaYyQqUuIiRrHhXxLlNnFfTtGgKkDdCcMmWwBb", 9, -99) },
        { Rotors.V, ("VvZzBbRrGgIiTtYyUuPpSsDdNnHhLlXxAaWwMmJjQqOoFfEeCcKk", 25, -99) },
        { Rotors.VI, ("JjPpGgVvOoUuMmFfYyQqBbEeNnHhZzRrDdKkAaSsXxLlIiCcTtWw", 25, 12) },
        { Rotors.VII, ("NnZzJjHghrGgRrCcXxMmYySsWwBbOoUuFfAaIiVvLlPpEeKkQqDdTt", 25, 12) },
        { Rotors.VIII, ("FfKkQqHhTtLlXxOoCcBbJjSsPpDdZzRrAaMmEeWwNnIiUuYyGgVv", 25, 12) },
        { Rotors.b, ("LlEeYyJjVvCcNnIiXxWwPpBbQqMmDdRrTtAaKkZzGgFfUuHhOoSs", -99, -99) },
        { Rotors.g, ("FfSsOoKkAaNnUuEeRrHhMmBbTtIiYyCcWwLlQqPpZzXxVvGgJjDd", -99, -99) },
    };
    public readonly Dictionary<Reflectors, string> _reflectors = new()
    {
        { Reflectors.Blank, "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz" },
        { Reflectors.A, "EeJjMmZzAaLlYyXxVvBbWwFfCcRrQqUuOoNnTtSsPpIiKkHhGgDd" },
        { Reflectors.B, "YyRrUuHhQqSsLlDdPpXxNnGgOoKkMmIiEeBbFfZzCcWwVvJjAaTt" },
        { Reflectors.C, "FfVvPpJjIiAaOoYyEeDdRrZzXxWwGgCcTtKkUuQqSsBbNnMmHhLl" },
        { Reflectors.b, "EeNnKkQqAaUuYyWwJjIiCcOoPpBbLlMmDdXxZzVvFfTtHhRrGgSs" },
        { Reflectors.c, "RrDdOoBbJjNnTtKkVvEeHhMmLlFfCcWwZzAaXxGgYyIiPpSsUuQq" },
    };
    private const string _alpha = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz";

    public EnigmaMachine(EnigmaSettings settings)
    {
        Guard.IsNotNull(settings, nameof(settings));
        Guard.IsNotNull(settings.Reflector, nameof(settings.Reflector));
        GuardHelper.IsInitialized(settings.Reel, nameof(settings.Reel));


        _settings = settings;
        _reflector = new(_reflectors[_settings.Reflector]);
        _rotor0 = new(_settings.Reel!.Rotor0);
        _rotor1 = new(_settings.Reel.Rotor1);
        _rotor2 = new(_settings.Reel.Rotor2);
        _rotor3 = new(_settings.Reel.Rotor3);
        _plugboard = CreatePlugboard(_settings.Plugboard!);
    }

    public void ResetSettings()
    {
        _reflector = new(_reflectors[_settings.Reflector]);
        _rotor0 = new (_settings.Reel!.Rotor0);
        _rotor1 = new(_settings.Reel.Rotor1);
        _rotor2 = new(_settings.Reel.Rotor2);
        _rotor3 = new(_settings.Reel.Rotor3);
        _plugboard = CreatePlugboard(_settings.Plugboard!);
    }

    public string Transpose(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
        {
            return string.Empty;
        }

        var plainText = new StringBuilder();
        foreach (var keyValue in cipherText)
        {
            if (keyValue == ' ')
            {
                plainText.Append(' ');
            }
            else
            {
                var lampValue = KeyPress(_alpha.IndexOf(keyValue));
                plainText.Append(_alpha[lampValue]);
            }
        }

        return plainText.ToString();

    }

    private int KeyPress(int keyValue)
    {
        RotateRotors();
        var lampValue = GetLampValue(keyValue);
        return lampValue;
    }

    private int GetLampValue(int keyValue)
    {
        var val = _plugboard.GetIndex(keyValue);
        val = _rotor3.Transpose(val, Rotor.Direction.Foward);
        val = _rotor2.Transpose(val, Rotor.Direction.Foward);
        val = _rotor1.Transpose(val, Rotor.Direction.Foward);
        val = _rotor0.Transpose(val, Rotor.Direction.Foward);
        val = _reflector.GetIndex(val);
        val = _rotor0.Transpose(val, Rotor.Direction.Reverse);
        val = _rotor1.Transpose(val, Rotor.Direction.Reverse);
        val = _rotor2.Transpose(val, Rotor.Direction.Reverse);
        val = _rotor3.Transpose(val, Rotor.Direction.Reverse);
        var lampValue = _plugboard.GetIndex(val);
        return lampValue;
    }
    private void RotateRotors()
    {
        if (_rotor2.IsNotched)
        {
            _rotor1.Rotate();
            _rotor2.Rotate();
        }
        else if (_rotor3.IsNotched)
        {
            _rotor2.Rotate();
        }

        _rotor3.Rotate();
    }

    private Plugboard CreatePlugboard(PlugboardConfiguration configuration)
    {
        _plugboard = new Plugboard();
        if (configuration?.PlugSwaps?.Count > 0)
        {
            foreach (var link in configuration?.PlugSwaps!)
            {
                _plugboard.Swap(_alpha.IndexOf(link.input), _alpha.IndexOf(link.output));
            }
        }

        return _plugboard;
    }
}
