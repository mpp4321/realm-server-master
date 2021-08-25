namespace RotMG.Common
{
    public enum RegisterStatus
    {
        Success,
        UsernameTaken,
        InvalidUsername,
        InvalidPassword,
        TooManyRegisters
    }
    
    public enum GuildCreateStatus
    {
        Success,
        InvalidName,
        UsedName
    }

    public enum AddGuildMemberStatus
    {
        Success,
        NameNotChosen,
        AlreadyInGuild,
        InAnotherGuild,
        IsAMember,
        GuildFull,
        Error
    }
}
