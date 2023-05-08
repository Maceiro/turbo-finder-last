namespace MoogleEngine;


public class Operations
{
 public static KeyValuePair<string, int> MakeWord(string s, int index)
{ 
    string aux="";
    int i=index;
    int trash= 0;

  while(s[i]!=' ' && s[i]!= '~')
   { if((char.IsLetter(s[i]) || s[i]=='^') && s[i]!='á'&& s[i]!='é' && s[i]!='í' && s[i]!='ó' && s[i]!='ú' && s[i]!='ñ') aux+= s[i] ;
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
          case'ñ':
          aux= aux+"n";
          break;
         }  
      if(  !(char.IsLetter(s[i]) || s[i]=='^')  ) trash+=1;
         i++;
         if(i==s.Length) break;
   }

    return new KeyValuePair<string, int> ( aux, trash );     
}

  public static KeyValuePair<string, int> MakeWordForPositions(string s, int index)
  {
    string aux="";
    int i=index;
    int trash= 0;
   while(s[i]!=' ')
   { if((char.IsLetter(s[i]) ) && s[i]!='á'&& s[i]!='é' && s[i]!='í' && s[i]!='ó' && s[i]!='ú') aux+= s[i] ;
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
         if( !char.IsLetter(s[i])  ) trash+=1;
         i++;
         if(i==s.Length) break;
   }
    string temp= aux.ToLower();
    return new KeyValuePair<string, int>( temp, trash )  ;     
  }



 public static Dictionary<string, List<int>> Positions(string core, Dictionary<string, double>query)
 {
   var positions= new Dictionary<string, List<int>>();
   int index= 0;
   int count= 0;

   while(index< core.Length)
   {
     var pair= MakeWordForPositions(core, index);
     string aux= pair.Key ;
     
     if(query.ContainsKey(aux))
     {
        if(positions.ContainsKey(aux)) positions[aux].Add(count);
        else 
        { var list= new List<int>();
          list.Add(count); 
          positions[aux]= list;
        }
     }
     index= index+ aux.Length+ 1+ pair.Value;
     count++;

   }
   return positions;

 }

 public static int Distance(string s1, string s2, Dictionary<string, List<int>> positions)
 {
   if(!(positions.ContainsKey(s1) && positions.ContainsKey(s2))) return -1;

   int min= int.MaxValue;
   for(int i=0; i< positions[s1].Count; i++) 
   { 
     int j= 0; 
     int current= int.MaxValue;
     while( (current> 0) &&  (j< positions[s2].Count) )
     {
        current= positions[s1][i]- positions[s2][j];
       if( Math.Abs(current) < min) min= Math.Abs(current);
       j++;
     }

   }

   return min;
 }

 public static int BestStart( Dictionary<string, List<int>> positions, Dictionary<string, double> idf)
 {
   double bestvalue= 0; 
   int bestword= -1;
   foreach(var pair in positions)
   {
     for(int i=0; i< pair.Value.Count; i++)
     {
      double current= ObtainValue( pair.Value[i], positions, idf, 10);
      if(current> bestvalue) 
      {
         bestvalue= current;
         bestword= pair.Value[i];
      }

     }

   }
   return bestword;
 }

 public static double ObtainValue( int n, Dictionary<string, List<int>> positions, Dictionary<string, double> idf, int length)
 { 
    double val= 0;
   foreach(var pair in positions)
   {
     int index= GetFirst( n, pair.Value);
     if(index !=-1)
     {
      while( (index< pair.Value.Count) && (pair.Value[index]- n <= length)) 
      {   
         val+= idf[pair.Key];
         index++;
      }
     }

   }
   return val;
 }

 public static int GetFirst(int n, List<int>a)
 {
   for(int i=0; i< a.Count; i++)
     if(a[i] >= n) return i;
  
  return -1;
 }


 public static int GetFirstRecursivo(int n, List<int>a, int ini, int last )
 {
   if( (ini== last) && a[ini]> n) return ini;
   if( ini> last ) return -1;

   int mid= ( ini + last)/2;
   if(mid== n) return mid;
   else if(mid < n)  return GetFirstRecursivo( n, a, mid+1, last );
   else return GetFirstRecursivo( n, a, ini, mid);

 }



   public static List<string> Operation(string s, Dictionary<string, List<int>>[]positions, double[]scores)
    {
      List<string>list= new List<string>();
      int index= 0;
      while(index< s.Length)
      {  
        var pair= MakeWord(s, index);
       string  aux= pair.Key;
        if( aux.Length>1  &&  aux[0]=='^' ) list.Add(aux.Substring(1)) ;
        index+= aux.Length + pair.Value + 1;

        if(index< s.Length && s[index-1]=='~')
        { 
          var paircouple= MakeWord(s, index);
          string couple= paircouple.Key;
          index= index + paircouple.Value + couple.Length+1;
  
          if(  couple.Length>1 &&  couple[0]=='^' ) list.Add(couple.Substring(1));
         
         if( aux.Length> 0 && couple.Length> 0)
         {
         string auxtemp= ( aux[0]=='^' ) ? ( ( aux.Length== 1 )? "" : aux.Substring(1) ) : aux;
         string coupletemp= ( couple[0]=='^' ) ? ( ( couple.Length== 1 )? "" : couple.Substring(1) ) : couple;
          if( auxtemp.Length!=0  && coupletemp.Length!=0 )  ChangeScore( auxtemp, coupletemp, positions, scores );
         }

        }
      
      }
       

        var result= Normalizer( list );
       //  for( int i=0; i< result.Count; i++)
        //  Console.WriteLine( resutl[i]));

         return result;


       

    }

   public static bool ContainsAllWords( List<string> words, Dictionary<string, double> weigth, int index)
    {
      if(index== words.Count) return true;
      
      if(weigth.ContainsKey(words[index])) return ContainsAllWords(words, weigth, index+1);
      else return false;

    }

   
    public static bool ContainsAllWordsIterative( List<string> words, Dictionary<string, double> weigth )
    {
      for( int i=0; i< words.Count; i++)
       if( !weigth.ContainsKey( words[i])) return false;

       return true;

    }






   public static void ChangeScore( string s1, string s2, Dictionary<string, List<int>>[] positions, double[]scores)
   {
     
     double[]distance = new double[scores.Length];
     for(int i=0; i< scores.Length; i++)
     distance[i]= (double)Distance(s1, s2, positions[i]);

     //for(int i=0; i< distance.Length; i++)
     //Console.WriteLine(distance[i]);     

     double max= Major(distance);
     if(max== -1) return;

     for(int i=0; i< distance.Length; i++)
     {
      if(distance[i]!=-1) distance[i]=  Math.Log10( max/ distance[i] + 10);
      else distance[i]= 1;
     }

     //for(int i=0; i< distance.Length; i++)
     //Console.WriteLine(distance[i]);

     for( int i= 0; i< scores.Length; i++)
     scores[i]= scores[i]* distance[i];

   }

   public static double Major( double[]a)
   {
      double major= a[0];
      for(int i=0; i< a.Length; i++)
      if( a[i]> major) major= a[i];

      return major;
   }


    public static List<string> Normalizer(  List<string> list)
    { 
      var result= new List<string>();
      for( int i=0; i< list.Count; i++)
      {
        string aux= list[i];
        string temp= "";

        for( int j= 0; j< aux.Length; j++)
        if( aux[j]!='^') temp+= aux[j];

        result.Add(temp.ToLower());

      }

      return result;


    }


}