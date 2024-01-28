using System.Linq;
using System.Xml.Linq;
using UnityEditor;

namespace DefaultNamespace
{
    public class AdditionalFilesPatcher : AssetPostprocessor
    {
        public static string OnGeneratedCSProject(string path, string content)
        {
            if (!path.EndsWith("Assembly-CSharp.csproj")) return content;
       
            var myFiles = AssetDatabase.GetAllAssetPaths().Where(file => file.EndsWith(".csv"));
       
            var xDoc = XDocument.Parse(content);
            var nsMsbuild = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";
            var project = xDoc.Element(nsMsbuild + "Project");
 
            if (project == null) return content;
       
            var itemGroup = new XElement(nsMsbuild + "ItemGroup");
            project.Add(itemGroup);
       
            foreach (var myFile in myFiles)
            {
                var include = new XAttribute("Include", myFile);
                var item = new XElement(nsMsbuild + "AdditionalFiles", include);
           
                itemGroup.Add(item);
            }
       
            content = xDoc.ToString();
       
            return content;
        }
    }
}