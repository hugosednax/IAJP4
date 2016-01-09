using System;
using System.Collections.Generic;
using System.Linq;
public class BaComp : IEqualityComparer<byte[]>
{
    public bool Equals(byte[] left, byte[] right)
    {
        // Handle case where one or both is null (equal only if both are null).

        if ((left == null) || (right == null))
            return (left == right);

        // Otherwise compare array sequences of two non-null array refs.

        return left.SequenceEqual(right);
    }

    public int GetHashCode(byte[] key)
    {
        // Complain bitterly if null reference.

        if (key == null)
            throw new ArgumentNullException();

        // Otherwise just sum bytes in array (one option, there are others).

        int rc = 0;
        foreach (byte b in key)
            rc += b;
        return rc;
    }
}