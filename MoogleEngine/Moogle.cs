namespace MoogleEngine;

  public class Moogle
{
  static Document[]doc;
  static Dictionary<string, double>idf;
  static bool procesed= false;

   public static void GetDocuments()
    { 
       if(procesed) return;
      string[]files = Directory.GetFiles(@"../Content"); 
       string[]titles= new string[files.Length];
        for(int i=0; i< files.Length; i++)
        {
        titles[i]= files[i].Remove(0, "../Content".Length+1);   
        }
      // for(int i=0; i< files.Length; i++)
       //  Console.WriteLine(titles[i]);
      
      string[]core= new string[files.Length];
      for(int i=0; i< files.Length; i++)
      core[i]= File.ReadAllText(files[i]);

      doc= new Document[core.Length];
      for(int i=0; i< doc.Length; i++)
      doc[i]= new Document(core[i], titles[i]);
      

       idf= new Dictionary<string, double>();
       Document.GetIdf(doc, idf);

       procesed= true;

    }
 
  public static SearchResult  Query( string query) 
    {
      GetDocuments();

     double[]scores= Document.Sorting(doc, idf, query);      //Obtener los scores de los documentos ordenados de menor a mayor
    // Inverse<double>(scores, 0, scores.Length-1);            //Ordenar los documentos y scores de mayor a menor
    // Inverse<Document>(doc, 0, doc.Length-1);

      var temp= new List<SearchItem>();
     Document vector= new Document(idf, query);
     
     Dictionary<string, List<int>>[]positions= new Dictionary<string, List<int>>[scores.Length];   //Array con las posiciones de las palabras del query en 
     for( int i=0; i< positions.Length; i++)                                                        //cada documento
     {
       positions[i]= Operations.Positions( doc[i].Core, vector.Weigth);
     }

     List<string> list= Operations.Operation(query, positions, scores);      //Obtener las palabras con prefijo ^
     
     double[]aux= new double[scores.Length];
     for( int i=0; i< aux.Length; i++)
      aux[i] = scores[i];
     
     Array.Sort(scores, doc );  
     Array.Sort(aux, positions) ;                                             //Si en la busqueda estaba el caracter ~ los scores pudieron haberse modificado
     Inverse<double>(scores, 0, scores.Length-1);
     Inverse<Document>(doc, 0, doc.Length-1);
     Inverse<Dictionary<string, List<int>>>(positions, 0, positions.Length-1 );

     float[]floatscores= new float[scores.Length];
     for(int i=0; i< scores.Length; i++)
     floatscores[i]= (float)scores[i];
 

     int count= 0;
     for(int i=0; i< doc.Length; i++)
     { 
       if( Operations.ContainsAllWordsIterative(list, doc[i].Weigth )  &&  scores[i]!= 0  )
       {
        temp.Add( new SearchItem(doc[i].Title, SnippetOperations.MakeSnippet(doc[i].Core, vector.Weigth, positions[i]), floatscores[i]) )  ;
        count++;
       }
       if(count==7) break;
       
     }

     SearchItem[]solution= temp.ToArray();       //Creando el array que se le pasara al constructor SearchResult
     
     return new SearchResult(solution, SugestionUpdate.Sugestion(query, idf));
           
    }

   public static void Inverse<T>(T[]a, int ini, int last)
 {
  while(ini< last)
  {
    T temp= a[ini];
    a[ini]= a[last];
    a[last]= temp;
    ini++;
    last--;
  }
 }

}
 
 