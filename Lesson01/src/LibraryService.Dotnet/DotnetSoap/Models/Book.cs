using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace DotnetSoap.Models;

[DataContract]
public class Book
{
    [DataMember]
    public string Id { get; set; }

    [DataMember]
    public string Title { get; set; }

    [DataMember]
    public string Category { get; set; }

    [DataMember]
    public Author[] Authors { get; set; }

    [DataMember]
    public string PublicationDate { get; set; }

    [DataMember]
    public string Lang { get; set; }

    [DataMember]
    public int Pages { get; set; }

    [DataMember]
    public int AgeLimit { get; set; }
}
