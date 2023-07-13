namespace Checkers
{
    public interface IReplay
    {
        string GetNextLine();

        void PlayLine(string recordLine);
    }

    public interface IRecord
    {
        void WriteLine(string recordLine);
    }
}
