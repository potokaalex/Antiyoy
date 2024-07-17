namespace ClientCode.Services.Progress.Base
{
    public enum SaveLoaderResultType
    {
        Normal,
        Error,

        ErrorFileIsExist,
        ErrorFileIsNotExist,
        ErrorFileIsDamaged,
        ErrorFileNameIsEmptyOrNull
    }
}