namespace MoogleEngine;


public class Document
{  
   string core;
   string title;
   Dictionary<string, int>tfs;
   Dictionary<string, double>weigth;
   

   public Document(string core, string title)
   { 
     this.title= title;
     this.core= core;
     this.tfs= Separate(core);
     this.weigth= new Dictionary<string, double>();
   }


   public Dictionary<string, int> Tfs {get{ return this.tfs;} }

   public Dictionary<string, double> Weigth {get{ return this.weigth;} }

   public string Title {get{ return this.title;} }

   public string Core {get{ return this.core;} }



   public double Similarity(Document other)
   {  
      if(this.weigth.Count== 0 || other.weigth.Count==0) return -1;
      double top= 0;
      foreach(var pair in this.weigth)
      if(other.weigth.ContainsKey(pair.Key)) top+= pair.Value*other.weigth[pair.Key];

      double norma1= this.Norma();
      double norma2= other.Norma();
      return top/(norma1*norma2);
   }


   public double Norma()
   {
      double raiz= 0; 
       foreach(var pair in this.weigth)
       raiz+= Math.Pow(pair.Value, 2);
        
     return Math.Sqrt(raiz); 
   }




public static Dictionary<string, int> Separate(string s)      // Es utilizado en el metodo constructor del tf, va separando las palabras del core, 
  {                                                            // y calculando la frecuencia de las mismas 
     Dictionary<string, int>vector= new Dictionary<string, int>();
     string aux="";
     string temp="";

     for(int i=0; i< s.Length; i++)
     if(s[i]!=' ') 
     {
        if(char.IsLetter(s[i])&& s[i]!='á'&& s[i]!='é' && s[i]!='í' && s[i]!='ó' && s[i]!='ú' && s[i]!='ñ') aux+= s[i] ;
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
          aux= aux+ "n";
          break;
         }  
     }
     else if( aux.Length!= 0)
     { 
      temp= aux.ToLower();

       if(vector.ContainsKey(temp)) vector[temp]+=1;
       else vector[temp]= 1;

       aux="";
     }
     

     if( aux.Length!= 0)
     {
     temp= aux.ToLower();

     if(vector.ContainsKey(temp)) vector[temp]+=1;
     else vector[temp]=1;
     
     }

     return vector;
  }  



  
 public static Dictionary<string, int> SeparateForQuery(string s)
  { 
     Dictionary<string, int>vector= new Dictionary<string, int>();
     string aux="";
     string temp="";
     int count= 1;
     bool procesing= false;

     for(int i=0; i< s.Length; i++)
     if(s[i]!=' '  &&  s[i]!='~' ) 
     {  
       if(s[i]!= '*') procesing= true;

       if( ( char.IsLetter(s[i]) || ( s[i]=='*' &&  !procesing ) ) && s[i]!='á'&& s[i]!='é' && s[i]!='í' && s[i]!='ó' && s[i]!='ú' && s[i]!='ñ') aux+= s[i] ;
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
          aux= aux+ "n";
          break;
         }  
         
     }

     else
     { 
      temp= aux.ToLower();
      for(int j=0; j< temp.Length; j++)
       if(temp[j]=='*') count+=2;
       else break;

       string t= temp.Substring(count/2);
       if(t.Length!= 0)
      { if(vector.ContainsKey(t)) vector[t]+=count;
       else vector[t]=count;
      }
       aux="";
       count= 1;
       procesing= false;

     }


     temp= aux.ToLower();
     for(int i=0; i< temp.Length; i++)
      if(temp[i]=='*') count+=2;
       else break;
     string te= temp.Substring(count/2);
     if(te.Length!=0)
     if(vector.ContainsKey(te)) vector[te]+=count;
     else vector[te]=count;

     return vector;
  }  



 public static void GetIdf(Document[]doc, Dictionary<string, double>idf)
    {
      for(int i=0; i< doc.Length; i++)
        foreach(var pair  in doc[i].tfs)
         {
            if(idf.ContainsKey(pair.Key)) idf[pair.Key]+=1;
            else idf[pair.Key]= 1;
         }

       foreach(var pair in idf)
      idf[pair.Key]= Math.Log10((double)doc.Length/idf[pair.Key]);


     for(int i=0; i< doc.Length; i++)
     {
       int max= int.MinValue;
       foreach( var pair in doc[i].tfs)
       if( pair.Value> max) max= pair.Value;

       double realmax= (double)max; 

      foreach(KeyValuePair<string, int> pair in doc[i].tfs)
       doc[i].weigth[pair.Key]= ( (double)pair.Value/realmax ) * idf[pair.Key];
       
     }

    }
    


   public Document(Dictionary<string, double>idf, string query)
      {
        Dictionary<string, int> temp= SeparateForQuery(query);
        Dictionary<string, double> vector= new Dictionary<string, double>();
        int max=0;
        var sugestions= new List< KeyValuePair<string, int>>();

        foreach(var pair in temp)
        {
          if( idf.ContainsKey(pair.Key) ) max= ( pair.Value> max ) ? pair.Value : max;
          else 
          {
            string aux= SugestionUpdate.Sugestion( pair.Key, idf );
            if(aux.Length!= 0) sugestions.Add( new KeyValuePair<string, int>( aux, pair.Value ) );
          } 

        }
      
         for( int i=0; i< sugestions.Count; i++)
          {
           if( temp.ContainsKey( sugestions[i].Key) ) temp[ sugestions[i].Key]+= sugestions[i].Value  ;
           else temp[ sugestions[i].Key ]= sugestions[i].Value ;

             max= ( temp[ sugestions[i].Key ]> max ) ? temp[ sugestions[i].Key ] : max;
             
          }


        foreach(var pair in temp)
        if(idf.ContainsKey(pair.Key))
         { vector[pair.Key]= (0.5+ (1-0.5)* ( (double)(pair.Value)/max))* idf[pair.Key];
           Console.WriteLine( idf[pair.Key] );
         }

        this.title= query;
        this.core= query;
        this.tfs= temp;
        this.weigth= vector;

      }


      public static double[] Sorting(Document[]doc, Dictionary<string, double>idf, string query)
     {
        Document vector= new Document(idf, query);
       double[]scores= new double[doc.Length];

       for(int i=0; i< doc.Length; i++)
       scores[i]= vector.Similarity(doc[i]);

       Array.Sort(scores, doc);

       return scores;
     }
     
}
