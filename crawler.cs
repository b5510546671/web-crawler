using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Collections.Generic;

public class MyQueue{
	ArrayList q;
	
	public MyQueue(){
		q = new ArrayList();
	}
	
	public void Enqueue(string url){
		q.Add(url);
	}
	
	public string Dequeue(){
		string res = "";
		if (q.Count > 0){
			res += q[0];
			q.RemoveAt(0);
		}
		return res;
	}
	
	public void ToString(){
		foreach(String str in q)
		{
			Console.WriteLine("{0}", str);
		}
	}

}


public class MyClass
{
	public static ArrayList LinkParser(string page){
		//Console.WriteLine("LinkParser");
		ArrayList list = new ArrayList();
		string html = page.ToLower();
		while(html.IndexOf("<a href=") >= 0){
			int start = html.IndexOf("<a href=\"") + "<a href=\"".Length;
			int end = html.IndexOf("\"", start);
			string link = html.Substring(start, end-start);
			html = html.Substring(end);
//			Console.WriteLine(link);
			list.Add(new Uri(link, UriKind.RelativeOrAbsolute));
		}
		return list;
	}
	public static string GetPage(string url){
		//Console.WriteLine("GetPage");
	
		// WebRequest.DefaultWebProxy = new WebProxy(proxyURL);
		WebRequest req = WebRequest.Create(url);
		((HttpWebRequest)req).UserAgent = "knotCrawler";
		req.Timeout = 1000;
		Console.WriteLine(">> " + url);
		WebResponse resp = req.GetResponse();
		Stream st = resp.GetResponseStream();
		StreamReader sr = new StreamReader(st);
		string page = sr.ReadToEnd();
		sr.Close();
		resp.Close();
		return page;
	}
	
	public static Uri CombineUri(Uri absoluteUri, Uri unknownUri)
	{
		//Console.WriteLine("CombineUri");
		/*
		if(!unknownUri.IsAbsoluteUri){
			return new Uri(absoluteUri, unknownUri);
		}
		return absoluteUri;
		*/
		
		return new Uri(absoluteUri, unknownUri);
	}
	
	public static void Main()
	{	
		// Console.Write("Input URL: ");
		// string link = Console.ReadLine();
		string link = "http://www.ku.ac.th/web2012/index.php";
		string page = GetPage(link);
		ArrayList pageList = LinkParser(page);
		
		Console.WriteLine("1");
		
		MyQueue queue = new MyQueue();
		
		foreach(Uri u in pageList)
		{
			if(!u.IsAbsoluteUri){
				queue.Enqueue(CombineUri(new Uri(link, UriKind.Absolute),u).ToString());
			}
			else{
				queue.Enqueue(u.ToString());
			}
		}
		
		queue.ToString();
		
	
	/*	MyQueue queue = new MyQueue();
		
		Uri absUri = new Uri("https://mike.cpe.ku.ac.th");
		Uri relUri = new Uri("01204553/p.html", UriKind.Relative);
		
		if(!relUri.IsAbsoluteUri){
			Console.WriteLine("{0} is a relative Uri.", relUri);
		}
		
		Uri resolve = new Uri(absUri, relUri);
		
		Console.WriteLine("Resolve.AbsoluteUri: {0}",resolve.AbsoluteUri);
	*/
		
		
		//Console.WriteLine(link);
		Console.ReadLine();
	}
	
	#region Helper methods

	private static void WL(object text, params object[] args)
	{
		Console.WriteLine(text.ToString(), args);	
	}
	
	private static void RL()
	{
		Console.ReadLine();	
	}
	
	private static void Break() 
	{
		System.Diagnostics.Debugger.Break();
	}

	#endregion
}