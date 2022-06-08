namespace BioSim;

// class to help handling args
public class ArgsChecker
{
    // list of strings the user cant use
    public List<string> BannendWords { get; }
    // minimum amount of arguments the user must provide
    public int MinArgs { get; set; }

    // check the arguments
    public bool Check(string[] args, out string error)
    {
        error = "";

        if (args.Length < MinArgs)
        {
            error = $"{MinArgs - args.Length} argument(s) are missing";
        }
        foreach (var item in args)
        {
            if (string.IsNullOrEmpty(item))
            {
                error = "the args must not be empty";
            }
            if (BannendWords.Contains(item))
            {
                error = $"the expression '{item}' is preserved";
            }
        }

        if (error != "")
        {
            return false;
        }

        return true;
    }
}
