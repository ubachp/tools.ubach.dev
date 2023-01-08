using FluentAssertions;

namespace Ubach.Encoding.Enigma.Tests;

[TestClass]
public class EnigmaMachineTests
{
    [TestMethod]
    public void Four_Rotor_M4_Navy_Enigma()
    {
        var textToEncrypt = "This message need to be encrypted";
        var settings = new EnigmaSettings
        {
            Reflector = Reflectors.B,
            Reel = new ReelConfiguration
            {
                Rotor0 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.b], 0, 0),
                Rotor1 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.I], 0, 23),
                Rotor2 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.II], 1, 24),
                Rotor3 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.III], 2, 25),
            },
            Plugboard = null,
        };

        var sut = new EnigmaMachine(settings);

        var encrypted = sut.Transpose(textToEncrypt);
        sut.ResetSettings();
        var decrypted = sut.Transpose(encrypted);

        decrypted.Should().Be(textToEncrypt);
    }

    [TestMethod]
    public void Three_Rotor_M3_Wehrmacht_Enigma_With_Plug()
    {
        var textToEncrypt = "This message need to be encrypted";
        var settings = new EnigmaSettings
        {
            Reflector = Reflectors.B,
            Reel = new ReelConfiguration
            {
                Rotor0 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.Blank], 0, 0),
                Rotor1 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.I], 0, 23),
                Rotor2 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.II], 1, 24),
                Rotor3 = new Rotor(EnigmaMachine.AvailableRotors[Rotors.III], 2, 25),
            },
            Plugboard = new PlugboardConfiguration
            {
                PlugSwaps = new List<(char input, char output)>
                {
                    {('A', 'B')},
                    {('C', 'D')},
                    {('E', 'F')},
                }
                //"AB-CD-EF",
            }
        };

        var sut = new EnigmaMachine(settings);

        var encrypted = sut.Transpose(textToEncrypt);
        sut.ResetSettings();
        var decrypted = sut.Transpose(encrypted);

        decrypted.Should().Be(textToEncrypt);
    }
}