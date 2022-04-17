﻿namespace FCM.Generators.Output.Types;
internal class CostumeFlow
{
    private string source;

    public string Source
    {
        get { return source; }
        set { source = value; }
    }
    private string target;

    public string Target
    {
        get { return target; }
        set { target = value; }
    }

    private string category;

    public string Category
    {
        get { return category; }
        set { category = value; }
    }
    public CostumeFlow(string source, string target, string category)
    {
        Source = source;
        Target = target;
        Category = category;
    }
    public CostumeFlow(string source, string target)
    {
        Source = source;
        Target = target;
    }
}

