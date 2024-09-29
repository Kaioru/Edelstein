namespace Edelstein.Protocol.Gameplay.Constants;

public static class JobConstants
{
    public static int GetJobLevel(this int job)
    {
        if (job % 100 > 0 && job != 2001)
            return (job / 10 == 43
                    ? (job - 430) / 2
                    : job % 10
                ) + 2;

        return job % 1000 == 0 || job == 2001 ? 0 : 1;
    }

    public static int GetJobRace(this int job)
        => job / 1000;

    public static int GetJobType(this int job)
        => job / 100 % 10;

    public static int GetJobBranch(this int job)
        => job / 10 % 10;

    public static int GetBeginnerJob(this int job)
    {
        if (job.GetJobRace() == JobRace.Third && job.GetJobType() == JobType.Magician) 
            return Job.EvanJr;
        return job.GetJobRace() * 1000;
    }
}
