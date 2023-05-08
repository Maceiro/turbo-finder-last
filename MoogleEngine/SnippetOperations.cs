namespace MoogleEngine;

public class SnippetOperations
{
public static string MakeSnippet(string s, Dictionary<string, double>query, Dictionary<string, List<int>> positions)
{
 int pos= Operations.BestStart( positions, query);
 int index= ObtainIndex(s, pos);
 string aux= MakeWords(s, index, 10)+"...";
 return aux;
}

 public static string MakeWord(string s, int index)
{ 
    string aux="";
    int i=index;
  while(s[i]!=' ')
   { if(char.IsLetter(s[i])&& s[i]!='á'&& s[i]!='é' && s[i]!='í' && s[i]!='ó' && s[i]!='ú') aux+= s[i] ;
    else
     switch(s[i])
        { case'ó': 
          aux= aux+"o";
          break;
          case'á':
          aux= aux+"a";
          break;
          case'í':
          aux= aux+"i";
          break; 
          case'é':
          aux= aux+"e";
          break; 
          case'ú':
          aux= aux+"u";
          break;
         }  
         i++;
         if(i==s.Length) break;
   }

    return aux;     
}

 public static string MakeWords(string str, int index, int words)
{ 
  int spaces= 0;
  int i= index;
  while(spaces< words && i< str.Length)
  {
    if(str[i]==' ') spaces++;
    i++;
  }
  return str.Substring(index, i-index);
}
 
 public static int ObtainIndex(string s, int pos)
 {
   int spaces= 0; 
   int index= 0;
   while(spaces< pos)
   {
     if(s[index]==' ') spaces++;
     index++;
   }
   return index;
 }

}

