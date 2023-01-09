using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CommunityToolkit.Diagnostics;

namespace Ubach.Diagnostics;
public static class GuardHelper
{
    /// <summary>
    /// Asserts that the input value is initialized.
    /// </summary>
    /// <typeparam name="T">The type of reference value type being tested.</typeparam>
    /// <param name="value">The input value to test.</param>
    /// <param name="name">The name of the input parameter being tested.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void IsInitialized<T>([NotNull] T? value, [CallerArgumentExpression("value")] string name = "")
        where T : class
    {
        Guard.IsNotNull(value, name);
        var propertyInfo = typeof(T).GetProperties();
        foreach (var property in propertyInfo)
        {
            Guard.IsNotNull(property.GetValue(value), property.Name);
        }
    }
}
