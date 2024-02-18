// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

public class VoteCounter
{
    public List<KeyValuePair<string, int>> Sort(string[] args)
    {
        string[][] votes =
        {
            new string[] {"alice", "bob", "charlie"},
            new string[] {"bob", "charlie", "dan"},
            new string[] {"alice", "charlie", "dan"},
            new string[] {"alice", "bob", "dan"}
        };

        Dictionary<string, int> points = new Dictionary<string, int>();

        // Calculate points for each candidate
        foreach (string[] vote in votes)
        {
            for (int i = 0; i < vote.Length; i++)
            {
                string candidate = vote[i];

                if (!points.ContainsKey(candidate))
                {
                    points[candidate] = 0;
                }

                if (i == 0)
                {
                    points[candidate] += 3;
                }
                else if (i == 1)
                {
                    points[candidate] += 2;
                }
                else if (i == 2)
                {
                    points[candidate] += 1;
                }
            }
        }

        // Sort candidates by points
        List<KeyValuePair<string, int>> sortedPoints = new List<KeyValuePair<string, int>>(points);
        sortedPoints.Sort((x, y) => y.Value.CompareTo(x.Value));

        // Print results
        Console.WriteLine("Candidates by points:");
        foreach (KeyValuePair<string, int> pair in sortedPoints)
        {
            Console.WriteLine("{0}: {1} points", pair.Key, pair.Value);
        }

        return sortedPoints;
    }
}

///--------in tie, person who reaches first
static void Main2(string[] args)
{
    string[][] votes =
    {
        new string[] {"alice", "bob", "charlie"},
        new string[] {"bob", "charlie", "dan"},
        new string[] {"alice", "charlie", "dan"},
        new string[] {"alice", "bob", "dan"}
    };

    Dictionary<string, int> points = new Dictionary<string, int>();
    Dictionary<string, int> firstWinningPoints = new Dictionary<string, int>();

    // Calculate points and first winning points for each candidate
    foreach (string[] vote in votes)
    {
        for (int i = 0; i < vote.Length; i++)
        {
            string candidate = vote[i];

            if (!points.ContainsKey(candidate))
            {
                points[candidate] = 0;
            }

            if (i == 0)
            {
                points[candidate] += 3;
            }
            else if (i == 1)
            {
                points[candidate] += 2;
            }
            else if (i == 2)
            {
                points[candidate] += 1;
            }

            if (points[candidate] >= 3 && !firstWinningPoints.ContainsKey(candidate))
            {
                firstWinningPoints[candidate] = i + 1;
            }
        }
    }

    // Sort candidates by points and first winning points
    List<KeyValuePair<string, Tuple<int, int>>> sortedPoints = new List<KeyValuePair<string, Tuple<int, int>>>();
    foreach (KeyValuePair<string, int> pair in points)
    {
        int firstWinningPoint = firstWinningPoints.ContainsKey(pair.Key) ? firstWinningPoints[pair.Key] : int.MaxValue;
        sortedPoints.Add(
            new KeyValuePair<string, Tuple<int, int>>(pair.Key, Tuple.Create(pair.Value, firstWinningPoint)));
    }

    sortedPoints.Sort((x, y) =>
    {
        if (y.Value.Item1 != x.Value.Item1)
        {
            return y.Value.Item1.CompareTo(x.Value.Item1);
        }
        else
        {
            return x.Value.Item2.CompareTo(y.Value.Item2);
        }
    });

    // Print results
    Console.WriteLine("Candidates by points:");
    foreach (KeyValuePair<string, Tuple<int, int>> pair in sortedPoints)
    {
        Console.WriteLine("{0}: {1} points (first winning point: {2})", pair.Key, pair.Value.Item1, pair.Value.Item2);
    }
}


///--------in tie, person who has most 3 points
static void Main3(string[] args)
{
    string[][] votes =
    {
        new string[] {"alice", "bob", "charlie"},
        new string[] {"bob", "charlie", "dan"},
        new string[] {"alice", "charlie", "dan"},
        new string[] {"alice", "bob", "dan"}
    };

    Dictionary<string, int> points = new Dictionary<string, int>();
    Dictionary<string, int> numThreePoints = new Dictionary<string, int>();

    // Calculate points and number of 3-point scores for each candidate
    foreach (string[] vote in votes)
    {
        for (int i = 0; i < vote.Length; i++)
        {
            string candidate = vote[i];

            if (!points.ContainsKey(candidate))
            {
                points[candidate] = 0;
                numThreePoints[candidate] = 0;
            }

            if (i == 0)
            {
                points[candidate] += 3;
                numThreePoints[candidate]++;
            }
            else if (i == 1)
            {
                points[candidate] += 2;
            }
            else if (i == 2)
            {
                points[candidate] += 1;
            }
        }
    }

    // Sort candidates by points and number of 3-point scores
    List<KeyValuePair<string, Tuple<int, int>>> sortedPoints = new List<KeyValuePair<string, Tuple<int, int>>>();
    foreach (KeyValuePair<string, int> pair in points)
    {
        int num3Points = numThreePoints.ContainsKey(pair.Key) ? numThreePoints[pair.Key] : 0;
        sortedPoints.Add(new KeyValuePair<string, Tuple<int, int>>(pair.Key, Tuple.Create(pair.Value, num3Points)));
    }

    sortedPoints.Sort((x, y) =>
    {
        if (y.Value.Item1 != x.Value.Item1)
        {
            return y.Value.Item1.CompareTo(x.Value.Item1);
        }
        else
        {
            return y.Value.Item2.CompareTo(x.Value.Item2);
        }
    });

    // Print results
    Console.WriteLine("Candidates by points:");
    foreach (KeyValuePair<string, Tuple<int, int>> pair in sortedPoints)
    {
        Console.WriteLine("{0}: {1} points ({2} three-point scores)", pair.Key, pair.Value.Item1, pair.Value.Item2);
    }
}

///--------in tie, person who has most votes
static void Main4(string[] args)
{
    string[][] votes =
    {
        new string[] {"alice", "bob", "charlie"},
        new string[] {"bob", "charlie", "dan"},
        new string[] {"alice", "charlie", "dan"},
        new string[] {"alice", "bob", "dan"}
    };

    Dictionary<string, int> points = new Dictionary<string, int>();
    Dictionary<string, int> numThreePoints = new Dictionary<string, int>();
    Dictionary<string, int> numVotes = new Dictionary<string, int>();

    // Calculate points, number of 3-point scores, and total number of votes for each candidate
    foreach (string[] vote in votes)
    {
        for (int i = 0; i < vote.Length; i++)
        {
            string candidate = vote[i];

            if (!points.ContainsKey(candidate))
            {
                points[candidate] = 0;
                numThreePoints[candidate] = 0;
                numVotes[candidate] = 0;
            }

            numVotes[candidate]++;

            if (i == 0)
            {
                points[candidate] += 3;
                numThreePoints[candidate]++;
            }
            else if (i == 1)
            {
                points[candidate] += 2;
            }
            else if (i == 2)
            {
                points[candidate] += 1;
            }
        }
    }

    // Sort candidates by points, number of 3-point scores, and total number of votes
    List<KeyValuePair<string, Tuple<int, int, int>>> sortedPoints =
        new List<KeyValuePair<string, Tuple<int, int, int>>>();
    foreach (KeyValuePair<string, int> pair in points)
    {
        int num3Points = numThreePoints.ContainsKey(pair.Key) ? numThreePoints[pair.Key] : 0;
        int numVotesForCandidate = numVotes.ContainsKey(pair.Key) ? numVotes[pair.Key] : 0;
        sortedPoints.Add(new KeyValuePair<string, Tuple<int, int, int>>(pair.Key,
            Tuple.Create(pair.Value, num3Points, numVotesForCandidate)));
    }

    sortedPoints.Sort((x, y) =>
    {
        if (y.Value.Item1 != x.Value.Item1)
        {
            return y.Value.Item1.CompareTo(x.Value.Item1);
        }
        else if (y.Value.Item2 != x.Value.Item2)
        {
            return y.Value.Item2.CompareTo(x.Value.Item2);
        }
        else
        {
            return y.Value.Item3.CompareTo(x.Value.Item3);
        }
    });

    // Print results
    Console.WriteLine("Candidates by points:");
    foreach (KeyValuePair<string, Tuple<int, int, int>> pair in sortedPoints)
    {
        Console.WriteLine("{0}: {1} points ({2} three-point scores, {3} votes)", pair.Key, pair.Value.Item1,
            pair.Value.Item2, pair.Value.Item3);
    }
}