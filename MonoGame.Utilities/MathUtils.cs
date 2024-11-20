using System;

namespace MonoGame.Utilities;

public static class MathUtils
{
    /// <summary>
    /// Computes the modulus of a given integer `x` with respect to a positive integer `N`,
    /// ensuring that the result is always a non-negative integer, even if `x` is negative.
    /// </summary>
    /// <param name="x">The integer value for which the modulus is to be calculated. This value can be positive, negative, or zero.</param>
    /// <param name="N">The modulus base, which should be a positive integer.</param>
    /// <returns>
    /// The result of the modulus operation. The return value is a non-negative integer
    /// between 0 (inclusive) and `N` (exclusive).
    /// </returns>
    /// <remarks>
    /// This method is particularly useful when you need to ensure a non-negative result
    /// from a modulus operation, especially when dealing with negative values of `x`.
    /// 
    /// For example:
    /// - `NegativeMod(-1, 5)` will return `4`, because `-1 % 5` is `-1`, and adding `5` gives `4`.
    /// - `NegativeMod(6, 5)` will return `1`, because `6 % 5` is `1`, which is already non-negative.
    /// 
    /// The formula used in this method is:
    /// (x % N + N) % N
    /// This handles cases where `x` is negative by first adding `N` to the result of `x % N`,
    /// ensuring the intermediate result is non-negative, and then taking modulo `N` again to
    /// wrap around if necessary.
    /// </remarks>
    public static int NegativeMod(int x, int N)
    {
        return (x % N + N) % N;
    }
}
