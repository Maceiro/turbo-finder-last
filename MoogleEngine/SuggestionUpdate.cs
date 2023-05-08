
namespace MoogleEngine;


public class SugestionUpdate
 {

   public static string Sugestion( string query, Dictionary<string, double> dicc )
   { 
     int length= query.Length;
     string result= "";

     foreach(var pair in dicc  )
    {
      if( Math.Abs( pair.Key.Length- length ) < 3 )
     {
      int current= Levenshtein(  query, pair.Key);
      if( current< 3) result= pair.Key;
      if( current==1) break;

     }

   }
  
   if( result.Length> 0) return result;
  
   foreach( var pair in dicc)
  { 
     if( Math.Abs( pair.Key.Length- length)< 4) 
    if( Levenshtein( pair.Key, query )== 3) result= pair.Key;
  }

  return result;

 }



 public static bool Split(string s, List<string>str, Dictionary<string, double>dicc)    //Fragmenta una palabra en un conjunto de palabras que se encuentren 
 {                                                                                      //en el diccionario
   for(int i=0; i<=s.Length; i++)
     if(dicc.ContainsKey(s.Substring(0,i))) 
     { 
       str.Add(s.Substring(0,i));
       if(i==s.Length) return true;  //Caso base

       if(Split(s.Substring(i), str, dicc)) return true;
       str.RemoveAt(str.Count-1);
     }

     return false; 
 }

    public static List<string> SuggestionUpdate( string query, Dictionary<string, double> dicc )
    {
       var list= new List<string>();
       if( Split( query, list, dicc ) )  return list;
   

      int length= query.Length;
     string result= "";

     foreach(var pair in dicc  )
    {
      if( Math.Abs( pair.Key.Length- length ) < 3 );
     {
      int current= Levenshtein(  query, pair.Key);
      if( current< 3) result= query;
      if( current==1) break;

     }

   }
   if( result.Length> 0)
   {  list.Add(result);
      return list;
   }
  
   foreach( var pair in dicc)
  { 
     if( Math.Abs( pair.Key.Length- length)< 4) 
    if( Levenshtein( pair.Key, query )== 3) result= pair.Key;
  }
   
   list.Add(result );
  return list  ;

    }

   public static int Levenshtein( string s1, string s2)
   {
     int n1= s1.Length;
     int n2= s2.Length;

     int[,]distances= new int[n1 +1, n2 +1 ] ;

     for( int i=0; i<= n1; i++)
      distances[i, 0]= i;    

      for( int i=0; i<= n2; i++)
      distances[0, i]= i;    

     for( int i=1; i<= n1; i++ )
      for( int j=1; j<= n2; j++)
      {
        int cost= ( s1[i-1]== s2[j-1] ) ? 0 : 1;
        distances[i,j]= Min( distances[i, j-1] +1,  distances[i-1, j] +1 , distances[i-1, j-1] + cost  );
      }

      return distances[ n1, n2];

   }

   
   public static int Min( int p, int q, int r  )
   {
     return  ( p< q ) ? ( ( p< r ) ? p : r ) : ( ( q< r )? q : r  ) ;

   }






 }