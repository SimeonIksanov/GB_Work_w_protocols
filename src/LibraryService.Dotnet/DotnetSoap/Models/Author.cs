using System.Runtime.Serialization;
using System.ServiceModel;

namespace DotnetSoap.Models;

[DataContract]
public class Author
{
    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Lang { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Lang})";
    }
}
