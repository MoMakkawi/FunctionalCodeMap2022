﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using OpenSoftware.DgmlTools;
using OpenSoftware.DgmlTools.Builders;
using OpenSoftware.DgmlTools.Model;

using FCM.Generators.Output.Types;


namespace FCM.Generators.Output;

internal class DGML
{
    public static List<CostumeFunction> Functions = new();
    public static List<CostumeFlow> Flows = new();
    private static readonly List<CostumeCategory> Categories = new()
    {
        new CostumeCategory("Par", "Green"),
        new CostumeCategory("Alt", "Yellow"),
        new CostumeCategory("Cho", "Blue"),
        new CostumeCategory("Ite", "Red") ,
        new CostumeCategory("Error","Black")
    };

    public static byte[] GetDGMLCodeAsArrayOfBytes()
    {
        //CosmeticImprovements
        Functions.ForEach(f => f.Lable = f.Lable.Trim());

        DgmlBuilder builder = new()
        {
            //convert Types ( I wrote it ) to Orginal Types ( OpenSoftware.DgmlTools.Model )

            NodeBuilders = new List<NodeBuilder> { new NodeBuilder<CostumeFunction>(CustomeFunction2Node) },
            LinkBuilders = new List<LinkBuilder> { new LinkBuilder<CostumeFlow>(CustomeFlow2Link) },
            CategoryBuilders = new List<CategoryBuilder> { new CategoryBuilder<CostumeCategory>(CreatCustomeCategory) }
        };

        DirectedGraph DG = new();

        DG = builder.Build(Functions, Flows, Categories);

        DG.GraphDirection = GraphDirection.LeftToRight;
        DG.Layout = Layout.Sugiyama;

        return ConvertDirectedGraphToByteArray(DG);
    }

    private static byte[] ConvertDirectedGraphToByteArray(DirectedGraph graph)
    {
        XmlSerializer serializer = new(typeof(DirectedGraph));
        MemoryStream memoryStream = new();

        // Serialize DirectedGraph graph to  DGML Code
        // in memoryStream (to put DGML Code in memory as stream instead of a disk or a network connection)
        // as UTF8 (VS need DGML code as byte array in utf8)

        var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
        serializer.Serialize(streamWriter, graph);

        byte[] utf8EncodedXml = memoryStream.ToArray();

        return utf8EncodedXml;
    }

    private static Node CustomeFunction2Node(CostumeFunction fun)
        => new()
        {
            Id = fun.Id,
            Label = fun.Lable,
            Description = fun.Info,
            Group = fun.Group,
            Category = fun.Category
        };
    private static Link CustomeFlow2Link(CostumeFlow flow)
        => new()
        {
            Source = flow.Source,
            Target = flow.Target,
            Category = flow.Category
        };
    private static Category CreatCustomeCategory(CostumeCategory category)
        => new()
        {
            Id = category.Id,
            Background = category.Background
        };
}