namespace SuperTestBase;

using Moq;

public static class TimesExt
{
    public static Times OnceOrNever(bool once)
    {
        if (once)
        {
            return Times.Once();
        }
        else
        {
            return Times.Never();
        }
    }

    public static Times ExactlyOrNever(int exactly)
    {
        if (exactly <= 0)
        {
            return Times.Never();
        }
        else
        {
            return Times.Exactly(exactly);
        }
    }
}
