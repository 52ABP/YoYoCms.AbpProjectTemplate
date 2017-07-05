using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace LTM.Common.Net
{
    public class HttpCall
    {
        private static readonly Hashtable XmlNamespaces = new Hashtable(); //缓存xmlNamespace，避免重复调用GetNamespace

        /// <summary>
        ///     需要WebService支持Post调用
        /// </summary>
        public static XmlDocument QueryPostWebService(string url, string methodName, Hashtable pars)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            var data = EncodePars(pars);
            WriteRequestData(request, data);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>
        ///     需要WebService支持Get调用
        /// </summary>
        public static XmlDocument QueryGetWebService(string url, string methodName, Hashtable pars)
        {
            var request = (HttpWebRequest)WebRequest.Create(url + "/" + methodName + "?" + ParsToString(pars));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>
        ///     通用WebService调用(Soap),参数Pars为String类型的参数名、参数值
        /// </summary>
        public static XmlDocument QuerySoapWebService(string url, string methodName, Hashtable pars)
        {
            if (XmlNamespaces.ContainsKey(url))
            {
                return QuerySoapWebService(url, methodName, pars, XmlNamespaces[url].ToString());
            }
            return QuerySoapWebService(url, methodName, pars, GetNamespace(url));
        }

        private static XmlDocument QuerySoapWebService(string url, string methodName, Hashtable pars, string xmlNs)
        {
            XmlNamespaces[url] = xmlNs; //加入缓存，提高效率
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"" + xmlNs + (xmlNs.EndsWith("/") ? "" : "/") + methodName + "\"");
            SetWebRequest(request);
            var data = EncodeParsToSoap(pars, xmlNs, methodName);
            WriteRequestData(request, data);
            XmlDocument doc = new XmlDocument(), doc2 = new XmlDocument();
            doc = ReadXmlResponse(request.GetResponse());

            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            var selectSingleNode = doc.SelectSingleNode("//soap:Body/*/*", mgr);
            if (selectSingleNode != null)
            {
                var retXml = selectSingleNode.InnerXml;
                //doc2.LoadXml("<root>" + RetXml + "</root>");
                retXml = HttpUtility.HtmlDecode(retXml);
                if (retXml != null) doc2.LoadXml(retXml);
            }
            AddDelaration(doc2);
            return doc2;
        }

        private static string GetNamespace(string URL)
        {
            var request = (HttpWebRequest)WebRequest.Create(URL + "?wsdl");
            SetWebRequest(request);
            var response = request.GetResponse();
            var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }

        private static byte[] EncodeParsToSoap(Hashtable Pars, string XmlNs, string MethodName)
        {
            var doc = new XmlDocument();
            doc.LoadXml(
                "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            //XmlElement soapBody = doc.createElement_x_x("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            var soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            //XmlElement soapMethod = doc.createElement_x_x(MethodName);
            var soapMethod = doc.CreateElement(MethodName);
            soapMethod.SetAttribute("xmlns", XmlNs);
            foreach (string k in Pars.Keys)
            {
                //XmlElement soapPar = doc.createElement_x_x(k);
                var soapPar = doc.CreateElement(k);
                soapPar.InnerXml = ObjectToSoapXml(Pars[k]);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            if (doc.DocumentElement != null) doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        private static string ObjectToSoapXml(object o)
        {
            var mySerializer = new XmlSerializer(o.GetType());
            var ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            var doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            return o.ToString();
        }

        /// <summary>
        ///     设置凭证与超时时间
        /// </summary>
        /// <param name="request"></param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            var writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        private static byte[] EncodePars(string Pars)
        {
            return Encoding.UTF8.GetBytes(Pars);
        }

        private static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }

        private static string ParsToString(Hashtable Pars)
        {
            var sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                if (Pars[k] != null)
                {
                    sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
                }
            }
            return sb.ToString();
        }

        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            var retXml = sr.ReadToEnd();
            sr.Close();
            var doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        private static void AddDelaration(XmlDocument doc)
        {
            var decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            if (!doc.InnerXml.StartsWith("<?xml "))
            {
                doc.InsertBefore(decl, doc.DocumentElement);
            }
        }

        #region 动态调用

        //#region InvokeWebService
        ///// < summary>
        ///// 动态调用web服务
        ///// < /summary>
        ///// < param name="url">WSDL服务地址< /param>
        ///// < param name="methodname">方法名< /param>
        ///// < param name="args">参数< /param>
        ///// < returns>< /returns>
        //public static object InvokeWebService(string url, string methodname, object[] args)
        //{
        //    return WebServiceHelper.InvokeWebService(url, null, methodname, args);
        //}

        ///// < summary>
        ///// 动态调用web服务
        ///// < /summary>
        ///// < param name="url">WSDL服务地址< /param>
        ///// < param name="classname">类名< /param>
        ///// < param name="methodname">方法名< /param>
        ///// < param name="args">参数< /param>
        ///// < returns>< /returns>
        //public static object InvokeWebService(string url, string classname, string methodname, object[] args)
        //{
        //    string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
        //    if ((classname == null) || (classname == ""))
        //    {
        //        classname = WebServiceHelper.GetWsClassName(url);
        //    }

        //    try
        //    {
        //        //获取WSDL
        //        WebClient wc = new WebClient();
        //        Stream stream = wc.OpenRead(url + "?wsdl");
        //        ServiceDescription sd = ServiceDescription.Read(stream);
        //        ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
        //        sdi.AddServiceDescription(sd, "", "");
        //        CodeNamespace cn = new CodeNamespace(@namespace);

        //        //生成客户端代理类代码
        //        CodeCompileUnit ccu = new CodeCompileUnit();
        //        ccu.Namespaces.Add(cn);
        //        sdi.Import(cn, ccu);
        //        CSharpCodeProvider icc = new CSharpCodeProvider();

        //        //设定编译参数
        //        CompilerParameters cplist = new CompilerParameters();
        //        cplist.GenerateExecutable = false;
        //        cplist.GenerateInMemory = true;
        //        cplist.ReferencedAssemblies.Add("System.dll");
        //        cplist.ReferencedAssemblies.Add("System.XML.dll");
        //        cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
        //        cplist.ReferencedAssemblies.Add("System.Data.dll");

        //        //编译代理类
        //        CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
        //        if (true == cr.Errors.HasErrors)
        //        {
        //            System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //            foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
        //            {
        //                sb.Append(ce.ToString());
        //                sb.Append(System.Environment.NewLine);
        //            }
        //            throw new Exception(sb.ToString());
        //        }

        //        //生成代理实例，并调用方法
        //        System.Reflection.Assembly assembly = cr.CompiledAssembly;
        //        Type t = assembly.GetType(@namespace + "." + classname, true, true);
        //        object obj = Activator.CreateInstance(t);
        //        System.Reflection.MethodInfo mi = t.GetMethod(methodname);

        //        return mi.Invoke(obj, args);

        //        /*
        //        PropertyInfo propertyInfo = type.GetProperty(propertyname);
        //        return propertyInfo.GetValue(obj, null);
        //        */
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
        //    }
        //}

        //private static string GetWsClassName(string wsUrl)
        //{
        //    string[] parts = wsUrl.Split('/');
        //    string[] pps = parts[parts.Length - 1].Split('.');

        //    return pps[0];
        //}
        //#endregion

        #endregion 动态调用

        #region Tip:使用说明

        //webServices 应该支持Get和Post调用，在web.config应该增加以下代码
        //<webServices>
        //  <protocols>
        //    <add name="HttpGet"/>
        //    <add name="HttpPost"/>
        //  </protocols>
        //</webServices>

        #endregion Tip:使用说明
    }
}