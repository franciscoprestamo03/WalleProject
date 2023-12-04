using System;
using System.Collections.Generic;

public class NumGenerator
{

    public int Seed { get; }
    public int Index { get; }
    public int Min { get; }
    public int Max { get; }

    public NumGenerator(int seed,int index=0,int min=0,int max=9)
	{
        Seed = seed;
        Index = index;
        Min = min;
        Max = max;
    }

    public IEnumerator<int> GetEnumerator()
    {
        // Create a new instance of Random with the given seed
        Random random = new Random(Seed);

        // Set the seed of the random object based on the index
        int randomNumber;
        for (int i = 0; i < Index; i++)
        {
            randomNumber = random.Next(Min, Max + 1);
        }
        while (true)
        {
            // Generate a random number within the specified range
            randomNumber = random.Next(Min, Max + 1);
            yield return randomNumber;
        }
    }
}

