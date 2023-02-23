using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Web.Script.Serialization;

namespace DynGPT
{
    /// <summary>
    ///     OpenAI inside Dynamo by iRef3at.
    /// </summary>
    public static class OpenAI
    {
        /// <summary>
        ///     Send natural language questions to any OpenAI model and receive accurate responses.
        /// </summary>
        public static string ChatGPT(OpenAIModel Model, string APIKey, string Question, double Temperature,
            int MaxTokens)
        {
            APIKey = APIKey.Replace(" ", string.Empty);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 |
                                                   SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            var apiEndpoint = "https://api.openai.com/v1/completions";
            var request = WebRequest.Create(apiEndpoint);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + APIKey);

            var data = "{" + " \"model\":\"" + Model.ModelName + "\"," + " \"prompt\": \"" + Question + "\"," +
                       " \"max_tokens\": " + MaxTokens + "," + " \"user\": \"" + "1" + "\", " + " \"temperature\": " +
                       Temperature + "}";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(data);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var response = request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            var sJson = streamReader.ReadToEnd();

            var oJavaScriptSerializer = new JavaScriptSerializer();
            var oJson = (Dictionary<string, object>)oJavaScriptSerializer.DeserializeObject(sJson);
            var oChoices = (object[])oJson["choices"];
            var oChoice = (Dictionary<string, object>)oChoices[0];
            var sResponse = (string)oChoice["text"];

            return sResponse;
        }

        /// <summary>
        ///     Open Documentation file, How to use, About Project and Author.
        /// </summary>
        public static void Help()
        {
            try
            {
                var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var relativePath = assemblyPath.Substring(0, assemblyPath.Length - 3);
                var helpPath = relativePath + @"extra\Dyn OpenAI - Docs.pdf";
                Process.Start(helpPath);
            }
            catch (Exception)
            {
                throw new Exception("File Not Found");
            }
        }
    }

    /// <summary>
    ///     Define a specific OpenAI language model to be utilized by the Chat node.
    /// </summary>
    public class OpenAIModel
    {
        private OpenAIModel(string name)
        {
            ModelName = name;
        }

        internal string ModelName { get; set; }

        #region CodeCushman_001

        /// <summary>
        ///     Almost as capable as Davinci Codex, but slightly faster. This speed advantage may make it preferable for real-time
        ///     applications.
        ///     Max Tokens ≤ 2048
        /// </summary>
        public static OpenAIModel CodeCushman_001()
        {
            return new OpenAIModel("code-cushman-001");
        }

        #endregion

        #region CodeDavinci_002

        /// <summary>
        ///     Most capable Codex model.Particularly good at translating natural language to code.In addition to completing code,
        ///     also supports inserting completions within code. Max Tokens ≤ 8000
        /// </summary>
        public static OpenAIModel CodeDavinci_002()
        {
            return new OpenAIModel("code-davinci-002");
        }

        #endregion

        #region TextAda_001

        /// <summary>
        ///     Capable of very simple tasks, usually the fastest model in the GPT-3 series, and lowest cost. Max Tokens ≤ 2048
        /// </summary>
        public static OpenAIModel TextAda_001()
        {
            return new OpenAIModel("text-ada-001");
        }

        #endregion

        #region TextBabbage_001

        /// <summary>
        ///     Capable of straightforward tasks, very fast, and lower cost. Max Tokens ≤ 2048
        /// </summary>
        public static OpenAIModel TextBabbage_001()
        {
            return new OpenAIModel("text-babbage-001");
        }

        #endregion

        #region TextCurie_001

        /// <summary>
        ///     Very capable, but faster and lower cost than Davinci. Max Tokens ≤ 2048
        /// </summary>
        public static OpenAIModel TextCurie_001()
        {
            return new OpenAIModel("text-curie-001");
        }

        #endregion

        #region TextDavinci_002

        /// <summary>
        ///     GPT-2 model. Can do any task the other models can do, second best option, often with higher quality, Also supports
        ///     inserting completions within text. Max Tokens ≤ 4000
        /// </summary>
        public static OpenAIModel TextDavinci_002()
        {
            return new OpenAIModel("text-davinci-002");
        }

        #endregion

        #region TextDavinci_003

        /// <summary>
        ///     Most capable GPT-3 model. Can do any task the other models can do, often with higher quality, longer output and
        ///     better instruction-following. Also supports inserting completions within text. Max Tokens ≤ 4000
        /// </summary>
        public static OpenAIModel TextDavinci_003()
        {
            return new OpenAIModel("text-davinci-003");
        }

        #endregion
    }
}