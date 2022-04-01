using System;
using System.Runtime.InteropServices;

namespace LambdaTheDev.SharpStringUtils.Helpers
{
    // Helper methods for arrays
    public static class ArrayHelpers
    {
        // Method that ensures that array's sizes are alright
        public static void ExpandArrayByPowOfTwo<T>(ref T[] array, int requiredCapacity, int elementsToCopy = 0)
        {
            // If size alr, then do nothing
            int currentLength = array.Length;
            if (currentLength >= requiredCapacity)
                return;

            // Grow using powers of 2 to find new length
            while (currentLength < requiredCapacity)
                currentLength *= 2;
            
            // Allocate new array & copy content, if necessary
            T[] newArray = new T[currentLength];
            
            if(elementsToCopy > 0)
                Array.Copy(array, newArray, elementsToCopy);

            array = newArray;
        }
        
        // Method that zeros unmanaged array
        public static void Zero<T>(T[] array, int toZero = -1)
        {
            if (toZero < -1)
                throw new ArgumentOutOfRangeException(nameof(toZero), "Value toZero cannot be lesser than -1!");
            
            T val = default;
            int toRemove = toZero == -1 ? array.Length : toZero;
            
            for (int i = 0; i < toRemove; i++)
                array[i] = val;
        }
    }
}