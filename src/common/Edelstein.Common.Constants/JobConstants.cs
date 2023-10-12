namespace Edelstein.Common.Constants;

public static class JobConstants
{
    public static int GetJobLevel(int job)
    {
        if (job % 100 > 0 && job != 2001)
            return (job / 10 == 43
                    ? (job - 430) / 2
                    : job % 10
                ) + 2;
        return job % 1000 == 0 || job == 2001 ? 0 : 1;
    }
    
    public static int GetJobRace(int job)
        => job / 1000;

    public static int GetJobType(int job)
        => job / 100 % 10;

    public static int GetJobBranch(int job)
        => job / 10 % 10;

    public static int GetBeginnerJob(int job)
    {
        if (GetJobRace(job) == JobRace.Third && GetJobType(job) == JobType.Magician)
            return Job.EvanJr;
        return GetJobRace(job) * 1000;
    }
    
    public static bool IsExtendSPJob(int job)
        => GetJobRace(job) == JobRace.Resistance ||
           GetJobRace(job) == JobRace.Third && GetJobType(job) == JobType.Magician ||
           job == Job.EvanJr;
}
