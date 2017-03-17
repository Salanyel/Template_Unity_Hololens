using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

namespace Template_Hololens
{
    /// <summary>
    /// Class used to detect the speech of the user and add the corresponding behavior
    /// </summary>
    public class SpeechManager : Singleton<SpeechManager>
    {
        KeywordRecognizer keywordRecognizer = null;
        Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

        // Use this for initialization
        void Start()
        {
            keywords.Add("Shutdown Application", () =>
            {
                //Shutdown the application
                Debug.Log("Application will be shutdown");
                this.BroadcastMessage("shutdownApplication");
            });

            // Tell the KeywordRecognizer about our keywords.
            keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

            // Register a callback for the KeywordRecognizer and start recognizing!
            keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
            keywordRecognizer.Start();
        }

        /// <summary>
        /// Add a recgonition of a precise phrase to link it to a behavior
        /// </summary>
        /// <param name="args"></param>
        private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            System.Action keywordAction;
            Debug.Log("Try get value : " + args.text);
            if (keywords.TryGetValue(args.text, out keywordAction))
            {
                keywordAction.Invoke();
            }
        }

        public void shutdownApplication()
        {
            Application.Quit();
        }
    }
}