// See https://aka.ms/new-console-template for more information

int[][] MergeIntervals(int[][] intervals) {
    Array.Sort(intervals, (a, b) => a[0].CompareTo(b[0])); // sort by start time
    List<int[]> merged = new List<int[]> { intervals[0] };
    foreach (int[] interval in intervals.Skip(1)) {
        if (interval[0] <= merged[^1][1]) {  // overlap
            merged[^1][1] = Math.Max(merged[^1][1], interval[1]);  // merge
        } else {
            merged.Add(interval);  // non-overlapping
        }
    }
    return merged.ToArray();
}